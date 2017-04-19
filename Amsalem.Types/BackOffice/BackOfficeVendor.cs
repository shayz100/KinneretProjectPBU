
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.Serialization;
namespace Amsalem.Types.BackOffice
{
    [Serializable(), DataContract]
    public class BackOfficeVendor
    {
        [DataMember]
        public string VendorID { get; set; }

        [DataMember]
        public string VendorGroup { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NameAlias { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string label { get; set; }

        public BackOfficeVendor()
        {
        }

        public static BackOfficeVendor FromAXRow(DataRow a)
        {
            var result = new BackOfficeVendor();
            result.VendorID = a.Field<string>("ACCOUNTNUM");
            result.VendorGroup = a.Table.Columns.Contains("VENDGROUP") ? a.Field<string>("VENDGROUP") : string.Empty;
            result.Name = a.Field<string>("NAME");
            result.NameAlias = a.Field<string>("NAMEALIAS");
            result.Address = a.Field<string>("ADDRESS");
            result.Currency = a.Field<string>("CURRENCY");
            result.Phone = a.Field<string>("PHONE");
            result.label = string.Format("{0}, {1} {2} ({3})", result.Name, result.Address, result.NameAlias, result.VendorID);
            return result;
        }



    }

}

