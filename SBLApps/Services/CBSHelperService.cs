using Oracle.ManagedDataAccess.Client;
using SBLApps.Models;
using Serilog;
using System.Data;

namespace SBLApps.Services
{
    public class CBSHelperService
    {
        string connectionStringCBS = string.Empty;

        public IConfiguration Configuration { get; }

        public CBSHelperService(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionStringCBS = Configuration["ConnectionStrings:CBSConnection"];
        }

        public AccountDetail GetAccountDetailFromAccountNumber(string accountNumber)
        {
            AccountDetail accountDetail = new AccountDetail();
            string commandText = $"select * from VW_LON_CUST where ACCOUNTNUMBER=:ACCOUNTNUMBER";
            try
            {
                OracleConnection conn = new OracleConnection(connectionStringCBS);

                using (OracleCommand cmd = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("ACCOUNTNUMBER", accountNumber);

                        conn.Open();
                        // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                        // IDataReader is closed.
                        OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                accountDetail = new AccountDetail()
                                {
                                    Cif = (reader["CIF_ID"] != null) ? reader["CIF_ID"].ToString() : "",
                                    AccountNumber = (reader["ACCOUNTNUMBER"] != null) ? reader["ACCOUNTNUMBER"].ToString() : "",
                                    AccountHolderName = (reader["ACCT_NAME"] != null) ? reader["ACCT_NAME"].ToString() : "",
                                    NameOfRORM = (reader["RO"] != null) ? reader["RO"].ToString() : "",
                                    CustomerTypeId = (reader["TYPEOFCUSTOMER"] != null) ? (reader["TYPEOFCUSTOMER"].ToString() == "INDIVIDUAL" ? "1" : reader["TYPEOFCUSTOMER"].ToString() == "CORPORATE" ? "2" : "") : "",
                                    TotalLoanOutstanding = (reader["TOTALLOANOUTSTANING"] != null) ? reader["TOTALLOANOUTSTANING"].ToString() : "",
                                    IsLoanCustomer = (reader["ISACHOLLONCUST"] != null) ? reader["ISACHOLLONCUST"].ToString() : ""
                                };
                            }
                        }

                        return accountDetail;
                    }
                    catch (OracleException oraEx)
                    {
                        Log.Error("Error on Getting Data from CBS: OracleException with AccountNumber" + accountNumber + " " + oraEx);
                        return accountDetail;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error on Exception Getting Data from CBS: Exception" + ex);
                        return accountDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error on Exception Loan Types from CBS: Exception" + ex);
                return accountDetail;
            }
        }

        public List<OtherAccount> GetAllAccountDetailsRelatedToTheCIF(string cif)
        {
            OtherAccount otherAccount = null;
            List<OtherAccount> otherAccounts = new List<OtherAccount>();
            string commandText = $"SELECT * FROM VW_LON_CUST_OTH_DATA where CIF_ID=:CIF_ID";
            try
            {
                OracleConnection conn = new OracleConnection(connectionStringCBS);

                using (OracleCommand cmd = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("CIF_ID", cif);
                        conn.Open();
                        OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                otherAccount = new OtherAccount()
                                {
                                    CIF = (reader["CIF_ID"].ToString() != null) ? reader["CIF_ID"].ToString() : "",
                                    AccountNumber = (reader["ACCOUNTNUMBER"].ToString() != null) ? reader["ACCOUNTNUMBER"].ToString() : "",
                                    AccountScheme = (reader["ACCOUNTSCHEME"] != null) ? reader["ACCOUNTSCHEME"].ToString() : "",
                                    Balance = (reader["BALANCE"] != null) ? reader["BALANCE"].ToString() : "",
                                    AccountStatus = (reader["ACCOUNTSTATUS"] != null) ? reader["ACCOUNTSTATUS"].ToString() : "",
                                    FreezeStatus = (reader["FREEZESTATUS"] != null) ? reader["FREEZESTATUS"].ToString() : ""
                                };
                                otherAccounts.Add(otherAccount);
                            }
                        }

                        return otherAccounts;
                    }
                    catch (OracleException oraEx)
                    {
                        Log.Error("Error from CBS: OracleException with " + oraEx);
                        return otherAccounts;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error on Exception from CBS: Exception" + ex);
                        return otherAccounts;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error on Exception from CBS: Exception" + ex);
                return otherAccounts;
            }
        }

        public List<LinkedEntitiesDetail> GetLinkedEntitiesDetailFromAccountNumber(string accountNumber)
        {
            List<LinkedEntitiesDetail> leDetail = new List<LinkedEntitiesDetail>();
            string commandText = $"SELECT * FROM VW_LINK_ENTITIES WHERE MAINACCOUNT=:MAINACCOUNT";
            try
            {
                OracleConnection conn = new OracleConnection(connectionStringCBS);

                using (OracleCommand cmd = new OracleCommand(commandText, conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("MAINACCOUNT", accountNumber);

                        conn.Open();
                        // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                        // IDataReader is closed.
                        OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                var linkedEntity = new LinkedEntitiesDetail()
                                {
                                    Cif = (reader["CIF_ID"] != null) ? reader["CIF_ID"].ToString() : "",
                                    MainAccountNumber = (reader["MAINACCOUNT"] != null) ? reader["MAINACCOUNT"].ToString() : "",
                                    AccountNumber = (reader["ACCOUNTNUMBER"] != null) ? reader["ACCOUNTNUMBER"].ToString() : "",
                                    AccountName = (reader["ACCOUNT_NAME"] != null) ? reader["ACCOUNT_NAME"].ToString() : "",
                                    Balance = (reader["BALANCE"] != null) ? reader["BALANCE"].ToString() : "",
                                    AccountStatus = (reader["ACCOUNTSTATUS"] != null) ? reader["ACCOUNTSTATUS"].ToString() : "",
                                    FreezeStatus = (reader["FREEZESTATUS"] != null) ? reader["FREEZESTATUS"].ToString() : ""
                                };

                                leDetail.Add(linkedEntity);
                            }
                        }

                        return leDetail;
                    }
                    catch (OracleException oraEx)
                    {
                        Log.Error("Error on Getting Data from CBS: OracleException with AccountNumber" + accountNumber + " " + oraEx);
                        return leDetail;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error on Exception Getting Data from CBS: Exception" + ex);
                        return leDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error on Exception Loan Types from CBS: Exception" + ex);
                return leDetail;
            }
        }
    }
}