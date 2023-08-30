using SBLApps.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Configuration;
using System.Linq;
using System.Reflection;
using SBLApps.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace SBLApps.Services
{
    public class DbHelper
    {
        private MemoAppDbContext? memoappContext;
        private IConfiguration Configuration { get; }
        public DbHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<T> GetDataByTable<T>(string tableName)
        {
            List<T> x = new List<T>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var dataJson = memoappContext.GetType().GetProperty(tableName).GetValue(memoappContext).ToJson();
                return JsonConvert.DeserializeObject<List<T>>(dataJson);
            }
        }

        public async Task<int> SaveMemo(BlacklistingMemoMain memo)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.BlacklistingMemoMains.Add(memo);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }
        public async Task<int> EditMemo(BlacklistingMemoMain memo)
        {
            try
            {
                using ( memoappContext = new MemoAppDbContext(Configuration))
                {
                    var accountsToRemove = memoappContext.OtherAccounts.Where(x => x.MemoId == memo.MemoId);
                    memoappContext.OtherAccounts.RemoveRange(accountsToRemove);
                    var linkedEntitiesToRemove = memoappContext.LinkedEntitiesDetails.Where(x => x.MemoId == memo.MemoId);
                    memoappContext.LinkedEntitiesDetails.RemoveRange(linkedEntitiesToRemove);
                    memoappContext.BlacklistingMemoMains.Update(memo);
                    int result = await memoappContext.SaveChangesAsync();
                    return result;
                }
            }
            catch (DbUpdateException dbException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> EditMemoRequestStatusDetails(BlacklistingMemoMain memo)
        {
            using (var memoappContext = new MemoAppDbContext(Configuration))
            {
                // Retrieve the entities that you want to update
                var entityToUpdate = memoappContext.BlacklistingMemoMains.Find(memo.MemoId);
                if (entityToUpdate == null)
                    return 0;
                entityToUpdate.LatestOperationId = memo.LatestOperationId;
                entityToUpdate.RequestStatusId = memo.RequestStatusId;
                entityToUpdate.NextAuthority = memo.NextAuthority;
                memoappContext.BlacklistingMemoMains.Update(entityToUpdate);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }
        public async Task<int> SaveMemoRequestOperation(MemoRequestOperation memoRequestOperation)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.MemoRequestOperations.Add(memoRequestOperation);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }

        public async Task<int> UpdateMemoRequestOperation(MemoRequestOperation memoRequestOperation)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.MemoRequestOperations.Update(memoRequestOperation);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }

        public long GetLatestMemoId()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                long latestMemoId = memoappContext.BlacklistingMemoMains.OrderByDescending(x => x.MemoId).FirstOrDefault()?.MemoId ?? 0;

                return latestMemoId;
            }
        }

        public async Task<List<BlacklistingMemoMain>> GetAllMemoData()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.BlacklistingMemoMains.ToListAsync();

                return data;
            }
        }

        public async Task<BlacklistingMemoMain> GetMemoDataById(long memoMainId)
        {
            BlacklistingMemoMain memo = new BlacklistingMemoMain();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memo = await memoappContext.BlacklistingMemoMains.Where(x => x.MemoId == memoMainId).FirstOrDefaultAsync() ?? new BlacklistingMemoMain();
                memo.MemoRequirementRemarks="";

                return memo;
            }
        }

        public async Task<List<BlacklistingMemoDetail>> GetBlacklistingDetailOfAccounts(long memoMainId)
        {
            List<BlacklistingMemoDetail> blacklistingDetail = new List<BlacklistingMemoDetail>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                blacklistingDetail = await memoappContext.BlacklistingMemoDetails.Where(x => x.MemoId == memoMainId).ToListAsync();

                return blacklistingDetail;
            }
        }

        public async Task<List<BlacklistingDocumentDetail>> GetBlacklistingDetailOfDocuments(long memoMainId)
        {
            List<BlacklistingDocumentDetail> blacklistingDetail = new List<BlacklistingDocumentDetail>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                blacklistingDetail = await memoappContext.BlacklistingDocumentDetails.Where(x => x.MemoId == memoMainId).ToListAsync();

                return blacklistingDetail;
            }
        }

        public async Task<List<BlacklistingOtherPartyDetail>> GetBlacklistingDetailOfOtherParties(long memoMainId)
        {
            List<BlacklistingOtherPartyDetail> blacklistingDetail = new List<BlacklistingOtherPartyDetail>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                blacklistingDetail = await memoappContext.BlacklistingOtherPartyDetails.Where(x => x.MemoId == memoMainId).ToListAsync();

                return blacklistingDetail;
            }
        }

        public async Task<List<CustomerType>> GetAllCustomerTypes()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.CustomerTypes.ToListAsync();
                return data;
            }
        }

        public async Task<List<Branch>> GetAllBranches()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.Branches.ToListAsync();
                return data;
            }
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.Departments.ToListAsync();
                return data;
            }
        }

        public async Task<List<RequestStatus>> GetAllRequestStatuses()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.RequestStatuses.ToListAsync();
                return data;
            }
        }

        public async Task<List<DocumentType>> GetAllDocumentTypes()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.DocumentTypes.ToListAsync();
                return data;
            }
        }

        public async Task<List<MemoType>> GetAllMemoTypes()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.MemoTypes.ToListAsync();
                return data;
            }
        }

        public async Task<List<Operation>> GetAllOperations()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.Operations.ToListAsync();
                return data;
            }
        }

        public async Task<List<UserRole>> GetAllUserRoles()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.UserRoles.ToListAsync();
                return data;
            }
        }

        public async Task<List<MemoRequestOperation>> GetAllMemoRequestOperations()
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                var data = await memoappContext.MemoRequestOperations.ToListAsync();

                return data;
            }
        }

        public User GetUserRoleByUserName(string userName)
        {
            List<User> x = new List<User>();
            User user = new User();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                user = memoappContext.Users.FirstOrDefault(x => x.Username == userName && x.IsActive == true);
                if (user != null)
                {
                    user.UserRoleIds = memoappContext.UserRoleMappers.Where(x => x.UserId == user.UserId).Select(x => x.UserRoleId).ToList();
                }
                return user;
            }
        }

        public List<User> GetAllActiveUserList()
        {
            List<User> users = new List<User>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                users = memoappContext.Users.Where(x => x.IsActive).ToList();
                return users;
            }
        }

        public async Task<int> SaveUser(User user)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.Users.Add(user);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }
        public async Task<int> EditUser(User user)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.Users.Update(user);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }

        public List<User> GetAllUsersList()
        {
            List<User> users = new List<User>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                users = memoappContext.Users.ToList();
                return users;
            }
        }

        public List<int> GetAllUserRoleIdsOfUser(int userId)
        {
            List<int> userRoleIds = new List<int>();
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                userRoleIds = memoappContext.UserRoleMappers.Where(x => x.UserId == userId).Select(x => x.UserRoleId).ToList();
                return userRoleIds;
            }
        }

        public async Task<int> SaveUserRoleMapper(UserRoleMapper userRoleMapper)
        {
            using (memoappContext = new MemoAppDbContext(Configuration))
            {
                memoappContext.UserRoleMappers.Add(userRoleMapper);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }

        public async Task<int> DeleteUserRoleMappers(int userId)
        {
            using (var memoappContext = new MemoAppDbContext(Configuration))
            {
                var userRoleMappers = memoappContext.UserRoleMappers.Where(x => x.UserId == userId).ToList();
                memoappContext.UserRoleMappers.RemoveRange(userRoleMappers);
                int result = await memoappContext.SaveChangesAsync();
                return result;
            }
        }

        public AccountDetail GetAccountDetailFromAccountNumberFromLocalDB(string accountNumber, long memoId)
        {
            using (var memoappContext = new MemoAppDbContext(Configuration))
            {
                var mainData = memoappContext.BlacklistingMemoMains.Where(x => x.AccountNumber == accountNumber && x.MemoId == memoId).FirstOrDefault();

                var accountDetail = new AccountDetail()
                {
                    AccountHolderName = mainData?.AccountHolderName,
                    AccountNumber = mainData?.AccountNumber,
                    Cif = mainData?.CIF,
                    CustomerTypeId = mainData?.CustomerTypeId.ToString(),
                    IsLoanCustomer = mainData?.IsLoanCustomer == true ? "Y" : "N",
                    NameOfRORM = mainData?.NameOfRORM,
                    TotalLoanOutstanding = mainData?.TotalLoanOutstanding?.ToString()
                };

                return accountDetail;
            }
        }

        public List<OtherAccount> GetAllAccountDetailsRelatedToTheCIFFromLocalDB(string cif, long memoId)
        {
            using (var memoappContext = new MemoAppDbContext(Configuration))
            {
                var mainData = memoappContext.OtherAccounts.Where(x => x.CIF == cif && x.MemoId == memoId)?.ToList();

                return mainData ?? new List<OtherAccount>();
            }
        }

        public List<LinkedEntitiesDetail> GetLinkedEntitiesDetailFromAccountNumberFromLocalDB(string accountNumber, long memoId)
        {
            using (var memoappContext = new MemoAppDbContext(Configuration))
            {
                var mainData = memoappContext.LinkedEntitiesDetails.Where(x => x.MainAccountNumber == accountNumber && x.MemoId == memoId)?.ToList();

                return mainData ?? new List<LinkedEntitiesDetail>();
            }
        }
    }
}
