using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Amsalem.Types.Misc.DAL;

namespace Amsalem.Types.CreditCards
{
    public class AXRertrieveCreditCards : RetrieveCreditCardBase, IRetrieveCardCard
    {
        public List<CreditCard> RetrievePaidByUsCreditCardsByManagerClockId(int AgentClockId, bool filterd, string dataAreaID, EBackOfficeType backOffice, bool withVAN = false)
        {
            string source = "AmsLogic_DBConnectionString";

            List<SqlParameter> SqlParameters = new List<SqlParameter>();
            string selectStatement = "SELECT LiveInAx FROM AxCompanys_Tbl where AxCompanyName = @dataAreaID";
            SqlParameters.Add(new SqlParameter("dataAreaID", dataAreaID));
            var ext = new DataBaseAccess();
            var LiveInAx = ext.OpenConnectionGetResults(source, selectStatement, SqlParameters.ToArray());

            bool isLive = (from DataRow row in LiveInAx.Rows select (bool)row.ItemArray[0]).FirstOrDefault();
            bool isTestEnvironemt = (System.Configuration.ConfigurationManager.AppSettings["Environment"].ToUpper() == "TEST");
            if (!isLive && !isTestEnvironemt)
            {
                return new List<CreditCard>();
            }

            string listManagerBanks = string.Empty;
            if (filterd)
            {
                List<SqlParameter> AmsLogicDBSqlParameters = new List<SqlParameter>();
                string AmsLogicDBStatement = "SELECT [bankNumber] ";
                AmsLogicDBStatement += "FROM [AmsLogic_DB].[dbo].[CreditCardBanks] CC ";
                AmsLogicDBStatement += "where  CC.managerClockId = @AgentClockId ";
                AmsLogicDBStatement += "or CC.managerClockId in ";
                AmsLogicDBStatement += "(select ClockIdIs ";
                AmsLogicDBStatement += "from Relations R,Employee_Account EC ";
                AmsLogicDBStatement += "where R.RelationTypeNum = 1 AND R.ClockIdIs = ec.AmsalemWorkerClockId ";
                AmsLogicDBStatement += "and R.ClockIdOf = @AgentClockId) ";
                AmsLogicDBSqlParameters.Add(new SqlParameter("AgentClockId", AgentClockId));

                var ManagerBanksDB = ext.OpenConnectionGetResults(source, AmsLogicDBStatement, AmsLogicDBSqlParameters.ToArray());

                var ManagerBanks = (from DataRow row in ManagerBanksDB.Rows select row.ItemArray[0]).ToList();
                listManagerBanks = string.Join(",", ManagerBanks);
            }
            string AxDBStatement = ";WITH cte AS(                                                                                    ";
            AxDBStatement += "  SELECT *,ROW_NUMBER() OVER (PARTITION BY CREDITCARDNO ORDER BY RECID asc) AS rn                      ";
            AxDBStatement += "  FROM                                                                                                 ";
            AxDBStatement += "  (select EXPIRYDATE, CREDITCARDNO, CVV, RECID, COMPANYID, AMS_BANKNUMBER ,EMPLID, DATAAREAID, AMS_EmpGovernmentId         ";
            AxDBStatement += "  from [ELI_AMSALEMCREDITCARD]                                                                         ";
            AxDBStatement += "  where DATAAREAID = @AxCompany                                                                               ";
            if (!string.IsNullOrEmpty(listManagerBanks))
            {
                AxDBStatement += "   and AMS_BANKNUMBER in (select value from  [Shared].[dbo].fn_Split(@listManagerBanks,','))           ";
            }
            AxDBStatement += "   ) d)                                                                                                ";
            AxDBStatement += "	 SELECT * from cte                                                                                   ";
            AxDBStatement += "	 where rn=1                                                                                          ";

            List<SqlParameter> AxDBSqlParameters = new List<SqlParameter>();
            AxDBSqlParameters.Add(new SqlParameter("listManagerBanks", listManagerBanks));

            var AxDBresults = DataBaseAccess.PerformQueryToCompany((dataAreaID == "%"), AxDBStatement, AxDBSqlParameters, dataAreaID);
            var toReturn = new List<CreditCard>();
            if (AxDBresults != null)
            {
                foreach (DataRow row in AxDBresults.Rows)
                {
                    var toAdd = ParseSinglePaidByUsCard(row);
                    toReturn.Add(toAdd);
                };
            }
            return toReturn;
        }

        public CreditCard RetrievePaidByUsSingleCreditCard(string CreditCardNumber, string dataAreaID, EBackOfficeType backOffice)
        {
            List<SqlParameter> SqlParameters = new List<SqlParameter>();

            SqlParameters.Add(new SqlParameter("dataAreaID", dataAreaID));

            string AxDBStatement = ";WITH cte AS(                                                                                                ";
            AxDBStatement += "  SELECT *,ROW_NUMBER() OVER (PARTITION BY CREDITCARDNO ORDER BY RECID asc) AS rn                                  ";
            AxDBStatement += "  FROM                                                                                                             ";
            AxDBStatement += "  (select EXPIRYDATE, CREDITCARDNO, CVV, RECID, COMPANYID, AMS_BANKNUMBER ,EMPLID, DATAAREAID, AMS_EmpGovernmentId ";
            AxDBStatement += "  from [ELI_AMSALEMCREDITCARD]                                                                                     ";
            AxDBStatement += "  where DATAAREAID = @AxCompany                                                                                    ";
            AxDBStatement += "   and CREDITCARDNO = @CardNumber) ";

            List<SqlParameter> AxDBSqlParameters = new List<SqlParameter>();
            AxDBSqlParameters.Add(new SqlParameter("CardNumber", CreditCardNumber));

            var AxDBresults = DataBaseAccess.PerformQueryToCompany(false, AxDBStatement, AxDBSqlParameters, dataAreaID);
            CreditCard toReturn = null;
            if (AxDBresults != null && AxDBresults.Rows.Count > 0)
            {
                toReturn = this.ParseSinglePaidByUsCard(AxDBresults.Rows[0]);
            }
            return toReturn;
        }

        private CreditCard ParseSinglePaidByUsCard(DataRow row)
        {
            var toAdd = new CreditCard();
            var AMS_EmpGovernmentId = row.Field<string>("AMS_EmpGovernmentId");
            toAdd.CreditCardOwner = PaymentMethodCreditCardOwner.OurCC;
            toAdd.CreditCardInternalIdentifier = row.Field<string>("CREDITCARDNO");
            toAdd.OwnerName = string.IsNullOrEmpty(row.Field<string>("EMPLID")) ? "No name" : row.Field<string>("EMPLID");
            toAdd.CreditCardIdentifier = row.Field<string>("CVV");
            toAdd.CustomerID = (string.IsNullOrEmpty(AMS_EmpGovernmentId) ? row.Field<long>("RECID").ToString() : AMS_EmpGovernmentId);
            toAdd.CreditCardNumber = row.Field<string>("CREDITCARDNO");
            toAdd.CreditCardType = row.Field<string>("COMPANYID");
            if (toAdd.CreditCardType != null && toAdd.CreditCardType.Length > 2)
            {
                toAdd.CreditCardType = toAdd.CreditCardType.Substring(toAdd.CreditCardType.Length - 2);
            }
            toAdd.BankNumber = row.Field<int>("AMS_BANKNUMBER");
            toAdd.AxCompany = row.Field<string>("DATAAREAID").ToUpper();
            toAdd.BackOffice = EBackOfficeType.AX;
            var date = row.Field<string>("EXPIRYDATE");
            var split = date.Split('/');
            DateTime newDate = new DateTime(int.Parse(split[1]) + 2000, int.Parse(split[0]), 1);
            var nextMonth = newDate.AddMonths(1);
            var firstDayOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            toAdd.CreditCardExpirationDate = firstDayOfNextMonth.AddDays(-1);
            return toAdd;
        }

        public List<CreditCard> RertieveAccountCreditCards(List<string> Accounts, string dataAreaID)
        {
            var cc = new List<CreditCard>();
            return cc;
        }

        public List<CreditCard> GetAllPaidByUsCreditCards()
        { 
            string AxDBStatement ="select EXPIRYDATE, CREDITCARDNO, CVV, RECID, COMPANYID, AMS_BANKNUMBER ,EMPLID, DATAAREAID, AMS_EmpGovernmentId";
            AxDBStatement += "  from [ELI_AMSALEMCREDITCARD]                                                                         ";
            AxDBStatement += "  where DATAAREAID = @AxCompany                                                                        ";

            List<SqlParameter> AxDBSqlParameters = new List<SqlParameter>();
        
            var AxDBresults = DataBaseAccess.PerformQueryToCompany (true,AxDBStatement, AxDBSqlParameters, "%");
            var toReturn = new List<CreditCard>();
            if (AxDBresults != null)
            {
                foreach (DataRow row in AxDBresults.Rows)
                {
                    var toAdd = ParseSinglePaidByUsCard(row);
                    toReturn.Add(toAdd);
                };
            }
            return toReturn;
        }
    }
}
