using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amsalem.Types.BackOffice
{
    public class BackOfficeCompany
    {
        public int Code { get; set; }

        public string Company { get; set; }

        public string MainCurrency { get; set; }

        public string SecondaryCurrency { get; set; }

        public string ConfigurationName { get; set; }

        public int PermissionGroup { get; set; }

        public bool IsLive { get; set; }

        public string CustomerInMore { get; set; }

        public DateTime LiveDate { get; set; }

        public EServerType ServerType { get; set; }

        public EBackOfficeType BackOfficeType { get; set; }

        public const string ANY_COMPANY_STRING = "Any Company";
        
        public BackOfficeCompany()
        {

        }

        public BackOfficeCompany(DataRow rowFromALTable)
        {
            this.Code = rowFromALTable.Field<int>("AxCompanyCode");
            this.Company = rowFromALTable.Field<string>("AxCompanyName").ToUpper();
            this.ConfigurationName = rowFromALTable.Field<string>("ConfigurationName");
            this.PermissionGroup = rowFromALTable.Field<int>("PermissionGroup");
            this.IsLive = rowFromALTable.Field<bool>("LiveInAx");
            this.CustomerInMore = rowFromALTable.Field<string>("CustomerInMore");
            object date = rowFromALTable["LiveDate"];
            this.ServerType = (EServerType) rowFromALTable.Field<int>("ServerType");
            switch (this.ServerType)
            {
                case EServerType.STANDARD:
                    this.BackOfficeType = EBackOfficeType.AX;
                    break;
                case EServerType.INDIAN:
                    this.BackOfficeType = EBackOfficeType.AX;
                    break;
                case EServerType.TURKEY:
                    this.BackOfficeType = EBackOfficeType.AX;
                    break;
                case EServerType.DPC:
                    this.BackOfficeType = EBackOfficeType.DPC;
                    break;
                default:
                    throw new Exception("What the hell?");
            }
            this.LiveDate = (date == System.DBNull.Value ?
                              new DateTime(1970, 1, 1) :
                              rowFromALTable.Field<DateTime>("LiveDate"));            
        }
    }
}