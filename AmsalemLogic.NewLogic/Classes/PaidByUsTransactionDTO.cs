using AmsalemLogic.NewLogic.Entity_Framework;
using AmsalemLogic.VBClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmsalemLogic.NewLogic.Classes
{
    public class PaidByUsTransactionDTO
    {
        public int Id { get; set; }
        public string OriginalCurrencyCode { get; set; }
        public Nullable<decimal> BillingAmount { get; set; }
        public string BillingCurrencyCode { get; set; }
        public string SupplierAccountNum { get; set; }
        public string SupplierName { get; set; }
        public string BackOfficeCompany { get; set; }
        public string TripNumber { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string Item { get; set; }
        public string CustomerName { get; set; }
        public Nullable<decimal> OriginalAmount { get; set; }
        public Nullable<int> ProductType { get; set; }
        public Nullable<int> ProductIdentifier { get; set; }

        public List<PaidByUsCreditCardTransactionLog> PaidByUsCreditCardTransactionLog { get; set; }


        public PaidByUsTransactionDTO()
        {
        }


        public PaidByUsTransactionDTO(AmsalemLogic.NewLogic.Entity_Framework.PaidByUsTransaction PaidByUsTransaction)
        {
            this.BackOfficeCompany = PaidByUsTransaction.BackOfficeCompany;
            this.BillingAmount = PaidByUsTransaction.BillingAmount;
            this.BillingCurrencyCode = PaidByUsTransaction.BillingCurrencyCode;
            this.CreatedBy = PaidByUsTransaction.CreatedBy;
            this.CreatedDateTime = PaidByUsTransaction.CreatedDateTime;
            this.CustomerName = PaidByUsTransaction.CustomerName;
            this.Id = PaidByUsTransaction.Id;
            this.Item = PaidByUsTransaction.Item;
            this.ModifiedBy = PaidByUsTransaction.ModifiedBy;
            this.ModifiedDateTime = PaidByUsTransaction.ModifiedDateTime;
            this.OriginalAmount = PaidByUsTransaction.OriginalAmount;
            this.OriginalCurrencyCode = PaidByUsTransaction.OriginalCurrencyCode;
            this.Status = PaidByUsTransaction.Status;
            this.SupplierAccountNum = PaidByUsTransaction.SupplierAccountNum;
            this.SupplierName = PaidByUsTransaction.SupplierName;
            this.TripNumber = PaidByUsTransaction.TripNumber; 
            //to do get from DB
            this.ModifiedByName = "Orgad";
            this.CreatedByName = "Orgad";
        }

        public ResultOfOperation UpdateInDataBase(ClassUsers user)
        {
            var result = new ResultOfOperation(false);
            try
            {
                using (var db = new Entities())
                {
                    var DBRow = db.PaidByUsTransaction.FirstOrDefault(x => x.Id == this.Id);
                    if (DBRow == null) //create new
                    {
                        DBRow = new Entity_Framework.PaidByUsTransaction();
                        db.PaidByUsTransaction.Add(DBRow);
                        
                        DBRow.CreatedBy = user.WorkerClockId;
                        DBRow.CreatedDateTime = DateTime.Now;
                        var log = PaidByUsCreditCardTransactionLog.First();
                        db.PaidByUsCreditCardTransactionLog.Add(log);
                        log.PaidByUsTransaction = DBRow;
                        DBRow.OriginalAmount = this.OriginalAmount;
                        DBRow.CustomerName = this.CustomerName;
                        DBRow.BackOfficeCompany = this.BackOfficeCompany;
                        DBRow.BillingAmount = this.BillingAmount;
                        DBRow.BillingCurrencyCode = this.BillingCurrencyCode;
                        DBRow.Id = this.Id;
                        DBRow.Item = this.Item;
                        DBRow.ModifiedBy = this.ModifiedBy;
                        DBRow.ModifiedDateTime = this.ModifiedDateTime;
                        DBRow.OriginalCurrencyCode = this.OriginalCurrencyCode;
                        DBRow.Status = this.Status;
                        DBRow.SupplierAccountNum = this.SupplierAccountNum;
                        DBRow.SupplierName = this.SupplierName;
                        DBRow.TripNumber = this.TripNumber; 

                    }
                    else //update
                    {
                        DBRow.ModifiedBy = user.WorkerClockId;
                        DBRow.ModifiedDateTime = DateTime.Now;
                        foreach (var log in PaidByUsCreditCardTransactionLog)
                        {
                            var logDBRow = db.PaidByUsCreditCardTransactionLog.FirstOrDefault(x => x.id == log.id);
                            if (logDBRow != null)
                            {
                                logDBRow.Active = log.Active;
                            }
                        }
                    }
                    DBRow.Status = this.Status;
                    db.SaveChanges();
                    this.Id = DBRow.Id;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        internal static PaidByUsTransactionDTO Load(int id)
        {
            PaidByUsTransactionDTO result = null;
            using (var context = new Entities())
            {
                var dbRow = context.PaidByUsTransaction.FirstOrDefault(x => x.Id == id);
                if (dbRow != null)
                result = new PaidByUsTransactionDTO(dbRow);
            }
            return result;
        }

    }
}
