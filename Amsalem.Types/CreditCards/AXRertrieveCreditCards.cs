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

            string listManagerBanks = "25,26"; //an example - mockup
            string AxDBStatement = ";WITH cte AS(                                                                                    ";
            AxDBStatement += "  SELECT *,ROW_NUMBER() OVER (PARTITION BY CREDITCARDNO ORDER BY RECID asc) AS rn                      ";
            AxDBStatement += "  FROM                                                                                                 ";
            AxDBStatement += "  (select EXPIRYDATE, CREDITCARDNO, CVV, RECID, COMPANYID, AMS_BANKNUMBER ,EMPLID, DATAAREAID, AMS_EmpGovernmentId,status         ";
            AxDBStatement += "  from [ELI_AMSALEMCREDITCARD]                                                                         ";
            AxDBStatement += "  where DATAAREAID = @AxCompany                                                                               ";
            if (!string.IsNullOrEmpty(listManagerBanks))
            {
                AxDBStatement += "   and AMS_BANKNUMBER in ('25','26')           ";
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
            toAdd.Status = row.Field<int>("STATUS");
            toAdd.RecId = row.Field<long>("RECID");

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
            string AxDBStatement ="select EXPIRYDATE, STATUS, CREDITCARDNO, CVV, RECID, COMPANYID, AMS_BANKNUMBER ,EMPLID, DATAAREAID, AMS_EmpGovernmentId";
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
            var cardsList = toReturn.OrderByDescending(x => x.CreditCardExpirationDate).ToList();
            return cardsList;
        }
    }
}
