//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmsalemLogic.NewLogic.Entity_Framework
{
    using System;
    using System.Collections.Generic;
    
    public partial class PaidByUsCreditCardTransactionLog
    {
        public int id { get; set; }
        public Nullable<bool> CreditCardRecId { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<int> PaidByUsTransactionId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
    
        public virtual PaidByUsTransaction PaidByUsTransaction { get; set; }
    }
}
