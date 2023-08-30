using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SBLApps.Enums;
using SBLApps.Models;
using SBLApps.Services;
using System.Text;
using System.Threading;

namespace SBLApps.Controllers
{
    [Authorize]
    public class BlacklistingMemoMainController : Controller
    {
        #region Fields
        // Inject IConfiguration into your class constructor
        private readonly IConfiguration _configuration;
        private readonly DbHelper _dbHelper;
        private readonly CBSHelperService _cbsHelper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly CommonService _commonService;
        #endregion

        #region Ctor
        public BlacklistingMemoMainController(IConfiguration configuration, DbHelper dbHelper, CBSHelperService cbsHelperService, IWebHostEnvironment hostingEnvironment, CommonService commonService)
        {
            _configuration = configuration;
            _dbHelper = dbHelper;
            _cbsHelper = cbsHelperService;
            _hostingEnvironment = hostingEnvironment;
            _commonService = commonService;
        }
        #endregion

        #region Methods

        #region Get
        public async Task<IActionResult> BlacklistingMemoMain()
        {
            List<User> userList = _dbHelper.GetAllActiveUserList();

            #region Make users list capable of binding into dropdown
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;
            #endregion

            BlacklistingMemoMain bmmModel = new BlacklistingMemoMain()
            {
                ReferenceNumber = $"SBL-{DateTime.Now.ToString("yyMdHmsfff")}{_dbHelper.GetLatestMemoId() + 1}",//$"SBL-{DateTime.Now.ToString("yyMdHmsfff")}{new Random().Next(1, 99)}{_dbHelper.GetLatestMemoId() + 1}",
                CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName"),
                DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName"),
                BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName"),
                DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName"),
                RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName"),
                MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName"),
                OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => x.OperationID == (int)OperationEnum.Initiate), "OperationID", "OperationName")
            };
            return View(bmmModel);
        }

        public async Task<IActionResult> BlacklistingMemoMainView(long memoMainId)
        {
            List<long> relatedMemoIds = (await _dbHelper.GetAllMemoRequestOperations()).Where(m => m.OperationBy?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant() || m.RequestComingFrom?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())
                                        .Select(m => m.RequestID)
                                        .Distinct()
                                        .ToList();

            if (IsCCACUser(User.Identity.Name))
            {
                List<long> extraRelatedMemoIdForCCACUsers = (await _dbHelper.GetAllMemoData()).Where(m => (m.RequestStatusId == (int)RequestStatusEnum.Completed))
                                        .Select(m => m.MemoId)
                                        .Distinct()
                                        .ToList();

                // Merge the lists while keeping only distinct values
                relatedMemoIds = relatedMemoIds.Union(extraRelatedMemoIdForCCACUsers).ToList();
            }

            if (!relatedMemoIds.Contains(memoMainId))
            {
                return RedirectToAction("BlacklistingMemoMainList");
            }

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;

            BlacklistingMemoMain bmmModel = await _dbHelper.GetMemoDataById(memoMainId);
            bmmModel.BlacklistingDocumentDetails = await _dbHelper.GetBlacklistingDetailOfDocuments(memoMainId);
            bmmModel.StringifiedBlacklistingDocumentDetails = JsonConvert.SerializeObject(bmmModel.BlacklistingDocumentDetails);
            bmmModel.StringifiedBlacklistingMemoDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfAccounts(memoMainId));
            bmmModel.StringifiedBlacklistingOtherPartyDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfOtherParties(memoMainId));
            bmmModel.CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName", bmmModel.CustomerTypeId);
            bmmModel.DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName");
            bmmModel.BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName", bmmModel.BranchId);
            bmmModel.DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName", bmmModel.DepartmentId);
            bmmModel.RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName", bmmModel.RequestStatusId);
            bmmModel.MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName", bmmModel.MemoTypeId);
            bmmModel.OperationList = new SelectList(await _dbHelper.GetAllOperations(), "OperationID", "OperationName");
            if (bmmModel.MemoId > 0)
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoMainId).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve corresponding OperationNames and NextAuthorityNames
                var operations = await _dbHelper.GetAllOperations();
                var users = _dbHelper.GetAllActiveUserList();

                foreach (var memoRequestOperation in memoRequestOperations)
                {
                    memoRequestOperation.OperationName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationName;
                    memoRequestOperation.OperationCompletedName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationCompletedName;
                    memoRequestOperation.RequestedByName = users.FirstOrDefault(u => u.Username?.ToLowerInvariant() == memoRequestOperation.RequestComingFrom?.ToLowerInvariant())?.Name;
                }
                if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Complete && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject)
                {
                    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == bmmModel.NextAuthority?.ToLowerInvariant())?.Name ?? "CCAC", OperationCompletedName = "Awaiting" });
                }

                bmmModel.MemoRequestOperations = memoRequestOperations;
            }
            return View("BlacklistingMemoMainView", bmmModel);
        }

        public async Task<IActionResult> BlacklistingMemoMainEdit(long memoMainId)
        {
            List<long> relatedMemoIds = (await _dbHelper.GetAllMemoRequestOperations()).Where(m => m.OperationBy?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant() || m.RequestComingFrom?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())
                                        .Select(m => m.RequestID)
                                        .Distinct()
                                        .ToList();

            if (!relatedMemoIds.Contains(memoMainId))
            {
                return RedirectToAction("BlacklistingMemoMainList");
            }

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;

            BlacklistingMemoMain bmmModel = await _dbHelper.GetMemoDataById(memoMainId);
            var authorityManager = GetOperationalAuthorityList(new LoggedInUserState()
            {
                IsRequester = (User.Identity.Name == bmmModel.Initiator),
                IsCurrentAuthority = (User.Identity.Name?.ToLowerInvariant() == bmmModel.NextAuthority?.ToLowerInvariant()),
                IsFinalApprover = (User.Identity.Name?.ToLowerInvariant() == bmmModel.FinalApproverSAMName?.ToLowerInvariant()),
                RequestedStatusId = bmmModel.RequestStatusId,
                Username = User.Identity.Name ?? "",
                IsCCACUser = IsCCACUser(User.Identity.Name)
            });
            if (authorityManager.Mode == "VIEW")
            {
                return RedirectToAction("BlacklistingMemoMainView", new { memoMainId = memoMainId });
            }

            bmmModel.BlacklistingDocumentDetails = await _dbHelper.GetBlacklistingDetailOfDocuments(memoMainId);
            bmmModel.StringifiedBlacklistingDocumentDetails = JsonConvert.SerializeObject(bmmModel.BlacklistingDocumentDetails);
            bmmModel.StringifiedBlacklistingMemoDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfAccounts(memoMainId));
            bmmModel.StringifiedBlacklistingOtherPartyDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfOtherParties(memoMainId));
            bmmModel.CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName", bmmModel.CustomerTypeId);
            bmmModel.DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName");
            bmmModel.BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName", bmmModel.BranchId);
            bmmModel.DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName", bmmModel.DepartmentId);
            bmmModel.RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName", bmmModel.RequestStatusId);
            bmmModel.MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName", bmmModel.MemoTypeId);
            bmmModel.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            if (bmmModel.MemoId > 0)
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoMainId).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve corresponding OperationNames and NextAuthorityNames
                var operations = await _dbHelper.GetAllOperations();
                var users = _dbHelper.GetAllActiveUserList();

                foreach (var memoRequestOperation in memoRequestOperations)
                {
                    memoRequestOperation.OperationName = operations?.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationName ?? "";
                    memoRequestOperation.OperationCompletedName = operations?.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationCompletedName ?? "";
                    memoRequestOperation.RequestedByName = users?.FirstOrDefault(u => u.Username?.ToLowerInvariant() == memoRequestOperation.RequestComingFrom?.ToLowerInvariant())?.Name ?? "";
                }

                if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Complete && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject)
                {
                    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == bmmModel.NextAuthority?.ToLowerInvariant())?.Name ?? "CCAC", OperationCompletedName = "Awaiting" });
                }

                bmmModel.MemoRequestOperations = memoRequestOperations;
            }
            return View("BlacklistingMemoMain", bmmModel);
        }

        public async Task<IActionResult> BlacklistingMemoMainOrchestrate(long memoMainId)
        {
            List<long> relatedMemoIds = (await _dbHelper.GetAllMemoRequestOperations()).Where(m => m.OperationBy?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant() || m.RequestComingFrom?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())
                                        .Select(m => m.RequestID)
                                        .Distinct()
                                        .ToList();

            if (!relatedMemoIds.Contains(memoMainId))
            {
                return RedirectToAction("BlacklistingMemoMainList");
            }

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;
            MemoRequestOperation mroModel = new MemoRequestOperation();
            mroModel.Request = await _dbHelper.GetMemoDataById(memoMainId);

            var authorityManager = GetOperationalAuthorityList(new LoggedInUserState()
            {
                IsRequester = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.Initiator?.ToLowerInvariant()),
                IsCurrentAuthority = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.NextAuthority?.ToLowerInvariant()),
                IsFinalApprover = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.FinalApproverSAMName?.ToLowerInvariant()),
                RequestedStatusId = mroModel.Request.RequestStatusId,
                Username = User.Identity.Name,
                IsCCACUser = IsCCACUser(User.Identity.Name)
            });
            if (authorityManager.Mode == "VIEW")
            {
                return RedirectToAction("BlacklistingMemoMainView", new { memoMainId = memoMainId });
            }

            mroModel.RequestID = memoMainId;
            mroModel.Request.BlacklistingDocumentDetails = await _dbHelper.GetBlacklistingDetailOfDocuments(memoMainId);
            mroModel.Request.StringifiedBlacklistingDocumentDetails = JsonConvert.SerializeObject(mroModel.Request.BlacklistingDocumentDetails);
            mroModel.Request.StringifiedBlacklistingMemoDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfAccounts(memoMainId));
            mroModel.Request.StringifiedBlacklistingOtherPartyDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfOtherParties(memoMainId));
            mroModel.Request.CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName", mroModel.Request.CustomerTypeId);
            mroModel.Request.DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName");
            mroModel.Request.BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName", mroModel.Request.BranchId);
            mroModel.Request.DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName", mroModel.Request.DepartmentId);
            mroModel.Request.RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName", mroModel.Request.RequestStatusId);
            mroModel.Request.MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName", mroModel.Request.MemoTypeId);
            mroModel.Request.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            mroModel.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            if (mroModel.RequestID > 0)
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoMainId).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve corresponding OperationNames and NextAuthorityNames
                var operations = await _dbHelper.GetAllOperations();
                var users = _dbHelper.GetAllActiveUserList();

                foreach (var memoRequestOperation in memoRequestOperations)
                {
                    memoRequestOperation.OperationName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationName;
                    memoRequestOperation.OperationCompletedName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationCompletedName;
                    memoRequestOperation.RequestedByName = users.FirstOrDefault(u => u.Username?.ToLowerInvariant() == memoRequestOperation.RequestComingFrom?.ToLowerInvariant())?.Name;
                }

                //if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Approve && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject && memoRequestOperations.Last().OperationID != (int)OperationEnum.Note)
                //{
                //    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == mroModel.Request.NextAuthority?.ToLowerInvariant())?.Name ?? "", OperationCompletedName = "Awaiting" });
                //}
                if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Complete && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject)
                {
                    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == mroModel.Request.NextAuthority?.ToLowerInvariant())?.Name ?? "CCAC", OperationCompletedName = "Awaiting" });
                }

                mroModel.Request.MemoRequestOperations = memoRequestOperations;
            }
            return View(mroModel);
        }

        public async Task<IActionResult> BlacklistingMemoMainToCCAC(long memoMainId)
        {
            var user = _dbHelper.GetAllActiveUserList().Where(x => (x.Username?.Trim().ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())).FirstOrDefault();
            if (user != null && !_dbHelper.GetAllUserRoleIdsOfUser(user.UserId).Contains((int)UserRoleEnum.CCAC))
            {
                return RedirectToAction("BlacklistingMemoMainList");
            }

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;
            MemoRequestOperation mroModel = new MemoRequestOperation();
            mroModel.Request = await _dbHelper.GetMemoDataById(memoMainId);

            var authorityManager = GetOperationalAuthorityList(new LoggedInUserState()
            {
                IsRequester = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.Initiator?.ToLowerInvariant()),
                IsCurrentAuthority = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.NextAuthority?.ToLowerInvariant()),
                IsFinalApprover = (User.Identity.Name?.ToLowerInvariant() == mroModel.Request.FinalApproverSAMName?.ToLowerInvariant()),
                RequestedStatusId = mroModel.Request.RequestStatusId,
                Username = User.Identity.Name,
                IsCCACUser = IsCCACUser(User.Identity.Name)
            });
            if (authorityManager.Mode == "VIEW")
            {
                return RedirectToAction("BlacklistingMemoMainView", new { memoMainId = memoMainId });
            }

            mroModel.RequestID = memoMainId;
            mroModel.Request.BlacklistingDocumentDetails = await _dbHelper.GetBlacklistingDetailOfDocuments(memoMainId);
            mroModel.Request.StringifiedBlacklistingDocumentDetails = JsonConvert.SerializeObject(mroModel.Request.BlacklistingDocumentDetails);
            mroModel.Request.StringifiedBlacklistingMemoDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfAccounts(memoMainId));
            mroModel.Request.StringifiedBlacklistingOtherPartyDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfOtherParties(memoMainId));
            mroModel.Request.CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName", mroModel.Request.CustomerTypeId);
            mroModel.Request.DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName");
            mroModel.Request.BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName", mroModel.Request.BranchId);
            mroModel.Request.DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName", mroModel.Request.DepartmentId);
            mroModel.Request.RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName", mroModel.Request.RequestStatusId);
            mroModel.Request.MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName", mroModel.Request.MemoTypeId);
            mroModel.Request.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            mroModel.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            if (mroModel.RequestID > 0)
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoMainId).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve corresponding OperationNames and NextAuthorityNames
                var operations = await _dbHelper.GetAllOperations();
                var users = _dbHelper.GetAllActiveUserList();

                foreach (var memoRequestOperation in memoRequestOperations)
                {
                    memoRequestOperation.OperationName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationName;
                    memoRequestOperation.OperationCompletedName = operations.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationCompletedName;
                    memoRequestOperation.RequestedByName = users.FirstOrDefault(u => u.Username?.ToLowerInvariant() == memoRequestOperation.RequestComingFrom?.ToLowerInvariant())?.Name;
                }

                #region Check the request is taken or not
                if ((memoRequestOperations.Last().OperationID == (int)OperationEnum.Approve || memoRequestOperations.Last().OperationID == (int)OperationEnum.Note) && string.IsNullOrEmpty(memoRequestOperations.Last().OperationBy))
                {
                    ViewData["EnableTaking"] = true;
                }
                else if ((memoRequestOperations.Last().OperationID == (int)OperationEnum.Approve || memoRequestOperations.Last().OperationID == (int)OperationEnum.Note) && memoRequestOperations?.Last()?.OperationBy?.Trim().ToLowerInvariant() == User?.Identity?.Name?.Trim().ToLowerInvariant())
                {
                    ViewData["EnableTaking"] = false;
                }
                #endregion

                //if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Approve && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject && memoRequestOperations.Last().OperationID != (int)OperationEnum.Note)
                if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Complete && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject)
                {
                    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == mroModel.Request.NextAuthority?.ToLowerInvariant())?.Name ?? "CCAC", OperationCompletedName = "Awaiting" });
                }

                mroModel.Request.MemoRequestOperations = memoRequestOperations;
            }
            return View(mroModel);
        }

        public JsonResult GetAccountDetailFromAccountNumber(string accountNumber)
        {
            AccountDetail accountDetail = new AccountDetail();

            accountDetail = _cbsHelper.GetAccountDetailFromAccountNumber(accountNumber);

            return Json(accountDetail);
        }

        public JsonResult GetAccountDetailFromAccountNumberFromLocalDB(string accountNumber, long memoId)
        {
            AccountDetail accountDetail = new AccountDetail();

            accountDetail = _dbHelper.GetAccountDetailFromAccountNumberFromLocalDB(accountNumber, memoId);

            return Json(accountDetail);
        }

        public List<OtherAccount> GetAllAccountDetailsRelatedToTheCIF(string cif)
        {
            List<OtherAccount> otherAccounts = new List<OtherAccount>();

            otherAccounts = _cbsHelper.GetAllAccountDetailsRelatedToTheCIF(cif);

            return otherAccounts;
        }

        public List<OtherAccount> GetAllAccountDetailsRelatedToTheCIFFromLocalDB(string cif, long memoId)
        {
            List<OtherAccount> otherAccounts = new List<OtherAccount>();

            otherAccounts = _dbHelper.GetAllAccountDetailsRelatedToTheCIFFromLocalDB(cif, memoId);

            return otherAccounts;
        }

        public List<LinkedEntitiesDetail> GetLinkedEntitiesDetailFromAccountNumber(string accountNumber)
        {
            List<LinkedEntitiesDetail> leDetail = new List<LinkedEntitiesDetail>();

            leDetail = _cbsHelper.GetLinkedEntitiesDetailFromAccountNumber(accountNumber);

            return leDetail;
        }

        public List<LinkedEntitiesDetail> GetLinkedEntitiesDetailFromAccountNumberFromLocalDB(string accountNumber, long memoId)
        {
            List<LinkedEntitiesDetail> leDetail = new List<LinkedEntitiesDetail>();

            leDetail = _dbHelper.GetLinkedEntitiesDetailFromAccountNumberFromLocalDB(accountNumber, memoId);

            return leDetail;
        }

        public async Task<IActionResult> BlacklistingMemoMainList()
        {
           // return View("Test");
            List<long> relatedMemoIds = (await _dbHelper.GetAllMemoRequestOperations()).Where(m => m.OperationBy?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant() || m.RequestComingFrom?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())
                                        .Select(m => m.RequestID)
                                        .Distinct()
                                        .ToList();

            if (IsCCACUser(User.Identity.Name))
            {
                List<long> extraRelatedMemoIdForCCACUsers = (await _dbHelper.GetAllMemoData()).Where(m => (m.RequestStatusId == (int)RequestStatusEnum.Approved || m.RequestStatusId == (int)RequestStatusEnum.Noted || m.RequestStatusId == (int)RequestStatusEnum.Completed))
                                        .Select(m => m.MemoId)
                                        .Distinct()
                                        .ToList();

                // Merge the lists while keeping only distinct values
                relatedMemoIds = relatedMemoIds.Union(extraRelatedMemoIdForCCACUsers).ToList();
            }

            List<BlacklistingMemoMain> list = (await _dbHelper.GetAllMemoData()).Where(x => relatedMemoIds.Contains(x.MemoId)).ToList();

            List<MemoRequestOperation> memoRequestOperations = await _dbHelper.GetAllMemoRequestOperations();

            foreach (var item in list)
            {
                // Find the last OperatedBy value for the corresponding MemoId in memoRequestOperations
                string lastOperatedBy = memoRequestOperations
                    .Where(m => m.RequestID == item.MemoId).OrderBy(m=>m.MemoRequestOperationId)
                    .Select(m => m.OperationBy)
                    .LastOrDefault();

                // Fill the NextAuthority field in the BlacklistingMemoMain object
                item.NextAuthority = lastOperatedBy ?? "-";
            }

            List<CustomerType> custTypeList = await _dbHelper.GetAllCustomerTypes();
            List<RequestStatus> requestStatusList = await _dbHelper.GetAllRequestStatuses();
            foreach (var item in list)
            {
                var customerType = custTypeList
                    .Where(c => c.CustomerTypeId == item.CustomerTypeId).FirstOrDefault();

                if (customerType != null)
                {
                    item.CustomerTypeName = customerType.CustomerTypeName;
                }

                var requestStatus = requestStatusList
                    .Where(c => c.RequestStatusId == item.RequestStatusId).FirstOrDefault();

                if (requestStatus != null)
                {
                    item.RequestStatusName = requestStatus.RequestStatusName;
                }

                item.AuthorityManager = GetOperationalAuthorityList(new LoggedInUserState()
                {
                    IsRequester = (User.Identity.Name?.ToLowerInvariant() == item.Initiator?.ToLowerInvariant()),
                    IsCurrentAuthority = (User.Identity.Name?.ToLowerInvariant() == item.NextAuthority?.ToLowerInvariant()),
                    IsFinalApprover = (User.Identity.Name?.ToLowerInvariant() == item.FinalApproverSAMName?.ToLowerInvariant()),
                    RequestedStatusId = item.RequestStatusId,
                    Username = User.Identity.Name,
                    IsCCACUser = IsCCACUser(User.Identity.Name)
                });

                List<User> userList = _dbHelper.GetAllActiveUserList();
                
                item.FinalApproverFullName = userList?.FirstOrDefault(x => x.Username.ToLowerInvariant() == item.FinalApproverSAMName.ToLowerInvariant())?.Name ?? "";
            }

           
            return View(list);
        }

        private bool IsCCACUser(string username)
        {
            bool isCCACUser = false;
            if (!string.IsNullOrEmpty(username))
            {
                var user = _dbHelper.GetAllActiveUserList().Where(x => (x.Username?.Trim().ToLowerInvariant() == username.ToLowerInvariant())).FirstOrDefault();
                if (user != null && _dbHelper.GetAllUserRoleIdsOfUser(user.UserId).Contains((int)UserRoleEnum.CCAC))
                {
                    isCCACUser = true;
                }
            }
            return isCCACUser;
        }

        public AuthorityManager GetOperationalAuthorityList(LoggedInUserState user)
        {
            AuthorityManager authMgr = new AuthorityManager();
            authMgr.LstOperationsAvailable = new List<int>() { };
            if (user != null && user.IsCCACUser && (user.RequestedStatusId == (int)RequestStatusEnum.Approved || user.RequestedStatusId == (int)RequestStatusEnum.Noted))
            {
                switch (user.RequestedStatusId)
                {
                    case (int)RequestStatusEnum.Approved:
                    case (int)RequestStatusEnum.Noted:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Complete, (int)OperationEnum.Return, (int)OperationEnum.Reject });
                        break;
                    default:
                        authMgr.Mode = "VIEW";
                        break;
                }
                return authMgr;
            }
            else if (user != null && user.IsRequester && user.IsCurrentAuthority && user.RequestedStatusId == (int)RequestStatusEnum.Returned)
            {
                switch (user.RequestedStatusId)
                {
                    case (int)RequestStatusEnum.Returned:
                        authMgr.Mode = "EDIT";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Reinitiate });
                        break;
                    default:
                        authMgr.Mode = "VIEW";
                        break;
                }
                return authMgr;
            }
            else if (user != null && !user.IsRequester && user.IsCurrentAuthority && !user.IsFinalApprover)
            {
                switch (user.RequestedStatusId)
                {
                    case (int)RequestStatusEnum.Initiated:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer });
                        break;
                    case (int)RequestStatusEnum.Recommended:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer });
                        break;
                    case (int)RequestStatusEnum.ForRefer:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.ReferralApproved });
                        break;
                    case (int)RequestStatusEnum.ForReview:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.RequestReviewed });
                        break;
                    case (int)RequestStatusEnum.Referred:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer });
                        break;
                    case (int)RequestStatusEnum.Reviewed:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer });
                        break;
                    default:
                        authMgr.Mode = "VIEW";
                        break;
                }
            }
            else if (user != null && !user.IsRequester && user.IsCurrentAuthority && user.IsFinalApprover)
            {
                switch (user.RequestedStatusId)
                {
                    case (int)RequestStatusEnum.Initiated:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer, (int)OperationEnum.Approve, (int)OperationEnum.Note });
                        break;
                    case (int)RequestStatusEnum.Recommended:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer, (int)OperationEnum.Approve, (int)OperationEnum.Note });
                        break;
                    case (int)RequestStatusEnum.ForRefer:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.ReferralApproved });
                        break;
                    case (int)RequestStatusEnum.ForReview:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.RequestReviewed });
                        break;
                    case (int)RequestStatusEnum.Referred:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer, (int)OperationEnum.Approve, (int)OperationEnum.Note });
                        break;
                    case (int)RequestStatusEnum.Reviewed:
                        authMgr.Mode = "FLOW";
                        authMgr.LstOperationsAvailable.AddRange(new int[] { (int)OperationEnum.Recommend, (int)OperationEnum.Return, (int)OperationEnum.Reject, (int)OperationEnum.ForwardForReview, (int)OperationEnum.Refer, (int)OperationEnum.Approve, (int)OperationEnum.Note });
                        break;
                    default:
                        authMgr.Mode = "VIEW";
                        break;
                }
            }
            else
            {
                authMgr.Mode = "VIEW";
            }

            return authMgr;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredOperationByList(int operationID, int memoID)
        {
            // Perform filtering logic based on the operationID parameter
            // Retrieve the filtered list of OperationBy options from your data source or any other means
            BlacklistingMemoMain bmmModel = await _dbHelper.GetMemoDataById(memoID);

            // Assuming you have the filtered list as a collection of SelectListItem
            var filteredOperationByList = new List<User>(); // Implement this method according to your data retrieval logic

            switch (operationID)
            {
                case (int)OperationEnum.Return:
                    filteredOperationByList = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == bmmModel.Initiator?.ToLowerInvariant()).ToList();
                    break;
                case (int)OperationEnum.Reject:
                    filteredOperationByList = new List<User>();
                    break;
                case (int)OperationEnum.ReferralApproved:
                    var requestLastHandledBy = (await _dbHelper.GetAllMemoRequestOperations()).Where(x => x.RequestID == memoID).OrderByDescending(x => x.MemoRequestOperationId)?.FirstOrDefault()?.RequestComingFrom;
                    filteredOperationByList = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == requestLastHandledBy?.ToLowerInvariant()).ToList();
                    break;
                case (int)OperationEnum.Approve:
                    filteredOperationByList = new List<User>();
                    break;
                case (int)OperationEnum.Note:
                    filteredOperationByList = new List<User>();
                    break;
                default:
                    filteredOperationByList = _dbHelper.GetAllActiveUserList().Where(x => (x.Username?.ToLowerInvariant() != bmmModel.Initiator?.ToLowerInvariant() && x.Username?.ToLowerInvariant() != User.Identity.Name?.ToLowerInvariant())).ToList();
                    break;
            }

            IEnumerable<SelectListItem> usersSelectList = filteredOperationByList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            return Json(filteredOperationByList);
        }

        //forward part.
        public async Task<IActionResult> GetMemoForwardPartial(int memoMainId)
        {
            // Fetch the necessary data based on memoMainId
            BlacklistingMemoMain memoData =  await _dbHelper.GetMemoDataById(memoMainId);

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;
            return PartialView("_BlacklistingMemoMainForward", memoData);
        }
        public async Task<IActionResult> BlacklistingMemoMainForward(long memoMainId)
        {
            List<long> relatedMemoIds = (await _dbHelper.GetAllMemoRequestOperations()).Where(m => m.OperationBy?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant() || m.RequestComingFrom?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant())
                                        .Select(m => m.RequestID)
                                        .Distinct()
                                        .ToList();

            if (!relatedMemoIds.Contains(memoMainId))
            {
                return RedirectToAction("BlacklistingMemoMainList");
            }

            List<User> userList = _dbHelper.GetAllActiveUserList();
            IEnumerable<SelectListItem> usersSelectList = userList.Select(u => new SelectListItem
            {
                Value = u.Username,
                Text = u.Name
            });
            ViewData["users"] = usersSelectList;

            BlacklistingMemoMain bmmModel = await _dbHelper.GetMemoDataById(memoMainId);

            return View("BlacklistingMemoMainForward", bmmModel);

           /* var authorityManager = GetOperationalAuthorityList(new LoggedInUserState()
            {
                IsRequester = (User.Identity.Name == bmmModel.Initiator),
                IsCurrentAuthority = (User.Identity.Name?.ToLowerInvariant() == bmmModel.NextAuthority?.ToLowerInvariant()),
                IsFinalApprover = (User.Identity.Name?.ToLowerInvariant() == bmmModel.FinalApproverSAMName?.ToLowerInvariant()),
                RequestedStatusId = bmmModel.RequestStatusId,
                Username = User.Identity.Name ?? "",
                IsCCACUser = IsCCACUser(User.Identity.Name)
            });
            if (authorityManager.Mode == "VIEW")
            {
                return RedirectToAction("BlacklistingMemoMainView", new { memoMainId = memoMainId });
            }

            bmmModel.BlacklistingDocumentDetails = await _dbHelper.GetBlacklistingDetailOfDocuments(memoMainId);
            bmmModel.StringifiedBlacklistingDocumentDetails = JsonConvert.SerializeObject(bmmModel.BlacklistingDocumentDetails);
            bmmModel.StringifiedBlacklistingMemoDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfAccounts(memoMainId));
            bmmModel.StringifiedBlacklistingOtherPartyDetails = JsonConvert.SerializeObject(await _dbHelper.GetBlacklistingDetailOfOtherParties(memoMainId));
            bmmModel.CustomerTypeList = new SelectList(await _dbHelper.GetAllCustomerTypes(), "CustomerTypeId", "CustomerTypeName", bmmModel.CustomerTypeId);
            bmmModel.DocumentTypeList = new SelectList(await _dbHelper.GetAllDocumentTypes(), "DocumentTypeId", "DocumentTypeName");
            bmmModel.BranchList = new SelectList(await _dbHelper.GetAllBranches(), "BranchId", "BranchName", bmmModel.BranchId);
            bmmModel.DepartmentList = new SelectList(await _dbHelper.GetAllDepartments(), "DepartmentId", "DepartmentName", bmmModel.DepartmentId);
            bmmModel.RequestStatusList = new SelectList(await _dbHelper.GetAllRequestStatuses(), "RequestStatusId", "RequestStatusName", bmmModel.RequestStatusId);
            bmmModel.MemoTypeList = new SelectList(await _dbHelper.GetAllMemoTypes(), "MemoTypeId", "MemoTypeName", bmmModel.MemoTypeId);
            bmmModel.OperationList = new SelectList((await _dbHelper.GetAllOperations()).Where(x => authorityManager.LstOperationsAvailable.Contains(Convert.ToInt32(x.OperationID))).ToList(), "OperationID", "OperationName");
            if (bmmModel.MemoId > 0)
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoMainId).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve corresponding OperationNames and NextAuthorityNames
                var operations = await _dbHelper.GetAllOperations();
                var users = _dbHelper.GetAllActiveUserList();

                foreach (var memoRequestOperation in memoRequestOperations)
                {
                    memoRequestOperation.OperationName = operations?.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationName ?? "";
                    memoRequestOperation.OperationCompletedName = operations?.FirstOrDefault(o => o.OperationID == memoRequestOperation.OperationID)?.OperationCompletedName ?? "";
                    memoRequestOperation.RequestedByName = users?.FirstOrDefault(u => u.Username?.ToLowerInvariant() == memoRequestOperation.RequestComingFrom?.ToLowerInvariant())?.Name ?? "";
                }

                if (memoRequestOperations.Last().OperationID != (int)OperationEnum.Complete && memoRequestOperations.Last().OperationID != (int)OperationEnum.Reject)
                {
                    memoRequestOperations.Add(new MemoRequestOperation() { RequestedByName = users?.FirstOrDefault(x => x.Username?.ToLowerInvariant() == bmmModel.NextAuthority?.ToLowerInvariant())?.Name ?? "CCAC", OperationCompletedName = "Awaiting" });
                }

                bmmModel.MemoRequestOperations = memoRequestOperations;
            }
            return View("BlacklistingMemoMainForward", bmmModel);*/
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> BlacklistingMemoMain(BlacklistingMemoMain memoModel)
        {
            int operationRows = 0;
            try
            {
                memoModel.MemoTypeId = (int)MemoTypeEnum.Blacklisting;
                if (!string.IsNullOrEmpty(memoModel.StringifiedOtherAccounts))
                {
                    memoModel.OtherAccounts = JsonConvert.DeserializeObject<List<OtherAccount>>(memoModel.StringifiedOtherAccounts);
                }
                if (!string.IsNullOrEmpty(memoModel.StringifiedLinkedEntitiesDetails))
                {
                    memoModel.LinkedEntitiesDetails = JsonConvert.DeserializeObject<List<LinkedEntitiesDetail>>(memoModel.StringifiedLinkedEntitiesDetails);
                }
                if (!string.IsNullOrEmpty(memoModel.StringifiedBlacklistingMemoDetails))
                {
                    memoModel.BlacklistingMemoDetails = JsonConvert.DeserializeObject<List<BlacklistingMemoDetail>>(memoModel.StringifiedBlacklistingMemoDetails);
                }
                if (!string.IsNullOrEmpty(memoModel.StringifiedBlacklistingOtherPartyDetails))
                {
                    memoModel.BlacklistingOtherPartyDetails = JsonConvert.DeserializeObject<List<BlacklistingOtherPartyDetail>>(memoModel.StringifiedBlacklistingOtherPartyDetails);
                }

                #region Set Request Status
                if (memoModel.LatestOperationId == (int)OperationEnum.Initiate || memoModel.LatestOperationId == (int)OperationEnum.Reinitiate)
                {
                    memoModel.RequestStatusId = (int)RequestStatusEnum.Initiated;
                }
                #endregion

                if (memoModel.MemoId == 0) // For case of New Memo
                {
                    operationRows = await _dbHelper.SaveMemo(memoModel);
                }
                else // For Returned and Reinitiated case
                {
                    operationRows = await _dbHelper.EditMemo(memoModel);
                }

                if (operationRows > 0)
                {
                    MemoRequestOperation memoRequestOperation = new MemoRequestOperation() { OperationID = memoModel.LatestOperationId, RequestID = memoModel.MemoId, OperationBy = memoModel.NextAuthority, RequestComingFrom = User.Identity.Name, OperationRemarks = memoModel.MemoRequirementRemarks, Timestamp = DateTime.Now };
                    operationRows = await _dbHelper.SaveMemoRequestOperation(memoRequestOperation);
                }

                // Process the uploaded files
                if (memoModel.BlacklistingDocumentDetails != null && memoModel.BlacklistingDocumentDetails.Count > 0 && memoModel.MemoId > 0 && operationRows > 0)
                {
                    List<BlacklistingDocumentDetail> documentDetails = new List<BlacklistingDocumentDetail>();

                    // Create the directory if it does not exist
                    var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/blacklisting/{memoModel.MemoId.ToString()}");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    int index = 0;
                    foreach (var documentRow in memoModel.BlacklistingDocumentDetails)
                    {
                        var file = documentRow.DocumentHolder;
                        var documentType = documentRow.DocumentTypeId;
                        if (file != null && file.Length > 0)
                        {
                            // Generate a unique file name
                            var fileName = $"{memoModel.MemoId.ToString()}_{documentType.ToString()}_{Guid.NewGuid().ToString()}_{file.FileName}";

                            // Define the file path where the file will be saved
                            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads\\blacklisting\\{memoModel.MemoId.ToString()}", fileName);

                            // Save the file to the server
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            documentDetails.Add(new BlacklistingDocumentDetail() { DocumentDetailId = documentRow.DocumentDetailId, MemoId = memoModel.MemoId, DocumentFullPath = Path.Combine($"uploads\\blacklisting\\{memoModel.MemoId.ToString()}", fileName), DocumentTypeId = documentType });
                            // Optionally, you can store the file path or other information in your database
                            // Add your logic here to save the file path or other details in the database
                        }
                        index++;
                    }

                    memoModel.BlacklistingDocumentDetails = documentDetails;
                }

                operationRows = await _dbHelper.EditMemo(memoModel);

                if (operationRows > 0)
                {
                    string defaultSender = _configuration?.GetValue<string>("Smtp:Username") ?? "";
                    string redirectUrl = _configuration?.GetValue<string>("RedirectURI:Orchestrate") ?? "";
                    var assignee = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == memoModel.NextAuthority?.ToLowerInvariant()).FirstOrDefault();
                    string assigneeEmail = assignee?.Email ?? "";
                    string assigneeFullName = assignee?.Name ?? "";

                    var assigner = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant()).FirstOrDefault();
                    string assignerEmail = assigner?.Email ?? "";
                    string assignerFullName = assigner?.Name ?? "";
                    memoModel.RequestStatusId = (int)RequestStatusEnum.Initiated;
                    EmailDetail emailDetail = new EmailDetail() { MemoId = memoModel.MemoId, OperationType = memoModel.LatestOperationId, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoModel?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoModel.MemoId}" };
                    _commonService.SendEmail(emailDetail);
                }

                // Set TempData for success notification
                TempData["NotificationType"] = "success";
                TempData["NotificationMessage"] = "Data submitted successfully.";

                if (memoModel.MemoId != 0)
                {
                    return RedirectToAction("BlacklistingMemoMainEdit", new { memoMainId = memoModel.MemoId });
                }

                return RedirectToAction("BlacklistingMemoMain");
            }
            catch (Exception ex)
            {
                // Set TempData for success notification
                TempData["NotificationType"] = "error";
                TempData["NotificationMessage"] = $"Error Occured. {ex.Message}";
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> BlacklistingMemoMainOrchestrate(MemoRequestOperation memoRequestOperationModel, string? buttonType)
        {
            if (!string.IsNullOrEmpty(buttonType) && buttonType == "Take Request")
            {
                // Retrieve MemoRequestOperations based on RequestID
                var memoRequestOperations = (await _dbHelper.GetAllMemoRequestOperations())
                    .Where(x => x.RequestID == memoRequestOperationModel.RequestID).OrderBy(x => x.MemoRequestOperationId)
                    .ToList();

                // Retrieve Blacklisting Main Record based on RequestID
                var memoRequest = (await _dbHelper.GetMemoDataById(memoRequestOperationModel.RequestID));

                var lastOperationRecorded = memoRequestOperations.Last();

                if ((lastOperationRecorded.OperationID == (int)OperationEnum.Approve || lastOperationRecorded.OperationID == (int)OperationEnum.Note) && string.IsNullOrEmpty(lastOperationRecorded.OperationBy))
                {
                    memoRequest.NextAuthority = User.Identity.Name;
                    await _dbHelper.EditMemo(memoRequest);

                    lastOperationRecorded.OperationBy = User.Identity.Name;
                    int result = await _dbHelper.UpdateMemoRequestOperation(lastOperationRecorded);

                    if (result > 0)
                    {
                        // Set TempData for success notification
                        TempData["NotificationType"] = "success";
                        TempData["NotificationMessage"] = "Request Taken.";
                    }
                    else
                    {
                        // Set TempData for success notification
                        TempData["NotificationType"] = "error";
                        TempData["NotificationMessage"] = "Taking request failed.";
                    }
                }

                return RedirectToAction("BlacklistingMemoMainToCCAC", new { memoMainId = memoRequestOperationModel.RequestID });
            }

            int operationRows = 0;
            try
            {
                memoRequestOperationModel.Timestamp = DateTime.Now;
                memoRequestOperationModel.RequestComingFrom = User.Identity.Name;

                // Process the uploaded files
                if (memoRequestOperationModel.DocumentHolder != null)
                {
                    // Create the directory if it does not exist
                    var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads/blacklisting/CCAC/{memoRequestOperationModel?.RequestID.ToString()}");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    var file = memoRequestOperationModel?.DocumentHolder;
                    if (file != null && file.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = $"{memoRequestOperationModel?.RequestID.ToString()}_{Guid.NewGuid().ToString()}_{file.FileName}";

                        // Define the file path where the file will be saved
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, $"uploads\\blacklisting\\CCAC\\{memoRequestOperationModel?.RequestID.ToString()}", fileName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        memoRequestOperationModel.BlacklistDocumentFullPath = Path.Combine($"uploads\\blacklisting\\CCAC\\{memoRequestOperationModel?.RequestID.ToString()}", fileName);
                        // Optionally, you can store the file path or other information in your database
                        // Add your logic here to save the file path or other details in the database
                    }
                }

                if (memoRequestOperationModel.OperationBy == null && memoRequestOperationModel.OperationID == (int)OperationEnum.Return)
                {
                    memoRequestOperationModel.OperationBy = (await _dbHelper.GetAllMemoRequestOperations()).Where(x => (x.RequestID == memoRequestOperationModel.RequestID && x.OperationID == (int)OperationEnum.Initiate)).FirstOrDefault()?.RequestComingFrom;
                }
                operationRows = await _dbHelper.SaveMemoRequestOperation(memoRequestOperationModel);
                var emailDetail = new EmailDetail();
                if (operationRows > 0)
                {
                    BlacklistingMemoMain memoMain = await _dbHelper.GetMemoDataById(memoRequestOperationModel.RequestID);//new BlacklistingMemoMain() { MemoId = memoRequestOperationModel.RequestID };
                    memoMain.LatestOperationId = memoRequestOperationModel.OperationID;
                    memoMain.NextAuthority = memoRequestOperationModel.OperationBy;
                    string defaultSender = _configuration?.GetValue<string>("Smtp:Username") ?? "";
                    var assignee = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == memoMain.NextAuthority?.ToLowerInvariant()).FirstOrDefault();
                    string assigneeEmail = assignee?.Email ?? "";
                    string assigneeFullName = assignee?.Name ?? "";

                    var assigner = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == memoRequestOperationModel.RequestComingFrom?.ToLowerInvariant()).FirstOrDefault();
                    string assignerEmail = assigner?.Email ?? "";
                    string assignerFullName = assigner?.Name ?? "";

                    var initiator = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == memoMain.Initiator?.ToLowerInvariant()).FirstOrDefault();
                    string initiatorEmail = initiator?.Email ?? "";
                    string initiatorFullName = initiator?.Name ?? "";

                    string redirectUrl = _configuration?.GetValue<string>("RedirectURI:Orchestrate") ?? "";
                    switch (memoRequestOperationModel.OperationID)
                    {
                        case (int)OperationEnum.Initiate:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Initiated;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Reinitiate:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Initiated;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Recommend:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Recommended;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Return:
                            redirectUrl = _configuration?.GetValue<string>("RedirectURI:Edit") ?? "";
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Returned;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.ForwardForReview:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.ForReview;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.RequestReviewed:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Reviewed;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Refer:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.ForRefer;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.ReferralApproved:
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Referred;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Note:
                            redirectUrl = _configuration?.GetValue<string>("RedirectURI:ToCCAC") ?? "";
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Noted;
                            //memoMain.NextAuthority = memoRequestOperationModel.RequestComingFrom;
                            //emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = initiatorEmail, AssigneeFullName = initiatorFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };

                            var ccacGroupEmailNoted = _configuration?.GetValue<string>("EmailAddresses:CCAC") ?? "";
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = ccacGroupEmailNoted, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Approve:
                            redirectUrl = _configuration?.GetValue<string>("RedirectURI:ToCCAC") ?? "";
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Approved;
                            //memoMain.NextAuthority = memoRequestOperationModel.RequestComingFrom;
                            //emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = initiatorEmail, AssigneeFullName = initiatorFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };

                            var ccacGroupEmailApproved = _configuration?.GetValue<string>("EmailAddresses:CCAC") ?? "";
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = ccacGroupEmailApproved, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Reject:
                            redirectUrl = _configuration?.GetValue<string>("RedirectURI:View") ?? "";
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Rejected;
                            memoMain.NextAuthority = memoRequestOperationModel.RequestComingFrom;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = initiatorEmail, AssigneeFullName = initiatorFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        case (int)OperationEnum.Complete:
                            redirectUrl = _configuration?.GetValue<string>("RedirectURI:View") ?? "";
                            memoMain.RequestStatusId = (int)RequestStatusEnum.Completed;
                            memoMain.NextAuthority = memoRequestOperationModel.RequestComingFrom;
                            emailDetail = new EmailDetail() { MemoId = memoRequestOperationModel.RequestID, OperationType = memoRequestOperationModel.OperationID, EmailSender = defaultSender, AssigneeEmail = initiatorEmail, AssigneeFullName = initiatorFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoMain?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoRequestOperationModel.RequestID}" };
                            break;
                        default:
                            break;
                    }
                    operationRows = await _dbHelper.EditMemoRequestStatusDetails(memoMain);

                    _commonService.SendEmail(emailDetail);
                }

                // Set TempData for success notification
                TempData["NotificationType"] = "success";
                TempData["NotificationMessage"] = "Data submitted successfully.";

                return RedirectToAction("BlacklistingMemoMainList");
            }
            catch (Exception ex)
            {
                // Set TempData for success notification
                TempData["NotificationType"] = "error";
                TempData["NotificationMessage"] = $"Error Occured. {ex.Message}";
                throw;
            }
        }

        //forward part.
        [HttpPost]
        public async Task<IActionResult> ForwardBlacklistingMemoMain(BlacklistingMemoMain memoModel)
        {
            int operationRows = 0;
            try
            {
                    operationRows = await _dbHelper.EditMemo(memoModel);

                if (operationRows > 0)
                {
                    MemoRequestOperation memoRequestOperation = new MemoRequestOperation() { OperationID = memoModel.LatestOperationId, RequestID = memoModel.MemoId, OperationBy = User.Identity.Name, RequestComingFrom = memoModel.ForwardToUser, OperationRemarks = memoModel.ForwardRemarks, Timestamp = DateTime.Now };
                    operationRows = await _dbHelper.SaveMemoRequestOperation(memoRequestOperation);
                }

                if (operationRows > 0)
                {
                    string defaultSender = _configuration?.GetValue<string>("Smtp:Username") ?? "";
                    string redirectUrl = _configuration?.GetValue<string>("RedirectURI:Orchestrate") ?? "";
                    var assignee = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == memoModel.NextAuthority?.ToLowerInvariant()).FirstOrDefault();
                    string assigneeEmail = assignee?.Email ?? "";
                    string assigneeFullName = assignee?.Name ?? "";

                    var assigner = _dbHelper.GetAllActiveUserList().Where(x => x.Username?.ToLowerInvariant() == User.Identity.Name?.ToLowerInvariant()).FirstOrDefault();
                    string assignerEmail = assigner?.Email ?? "";
                    string assignerFullName = assigner?.Name ?? "";
                    memoModel.RequestStatusId = (int)RequestStatusEnum.Initiated;
                    EmailDetail emailDetail = new EmailDetail() { MemoId = memoModel.MemoId, OperationType = memoModel.LatestOperationId, EmailSender = defaultSender, AssigneeEmail = assigneeEmail, AssigneeFullName = assigneeFullName, AssignerFullName = assignerFullName, MemoReferenceNumber = memoModel?.ReferenceNumber ?? "", RedirectUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/{redirectUrl}?memoMainId={memoModel.MemoId}" };
                    _commonService.SendEmail(emailDetail);
                }

                // Set TempData for success notification
                TempData["NotificationType"] = "success";
                TempData["NotificationMessage"] = "Data Forwarded Successfully.";

                if (memoModel.MemoId != 0)
                {
                    return RedirectToAction("BlacklistingMemoMainEdit", new { memoMainId = memoModel.MemoId });
                }

                return RedirectToAction("BlacklistingMemoMain");
            }
            catch (Exception ex)
            {
                // Set TempData for success notification
                TempData["NotificationType"] = "error";
                TempData["NotificationMessage"] = $"Error Occured. {ex.Message}";
                throw;
            }
        }
    }
}
#endregion
#endregion
