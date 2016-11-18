using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amsalem.Types
{
    public enum EBackOfficeType
    {
        NONE,
        DPC,
        AX,
        All
    }

    public static class EBackOfficeTypeHelper
    {
        public static EBackOfficeType Parse(string value)
        {
            var val = string.IsNullOrEmpty(value) ? "DPC" : value;
            return (EBackOfficeType)Enum.Parse(typeof(EBackOfficeType), val);
        }
    }

    public enum EPtcCode
    {
        INF = 2,
        YTH = 31,
        SRC = 120,
        CNN = 12,
        ADT = 60,
        NONE = 0

        //'If CInt(YearOld) <= 120 Then TypeOfPass = "SRC"
        //'If CInt(YearOld) <= 60 Then TypeOfPass = "ADT" 'PFA
        //'If CInt(YearOld) <= 31 Then TypeOfPass = "YTH"
        //'If CInt(YearOld) < 12 Then TypeOfPass = "CNN"
        //'If CInt(YearOld) < 2 Then TypeOfPass = "INF" 'INS
    }
    public enum EPhoneType
    {
        Business_Phone = 1,
        Business_Fax = 2,
        Home_Phone = 3,
        Home_Fax = 4,
        Mobile = 5,
        Telex = 6,
        Other_Phone = 7,
    }

    public enum EPNRState
    {
        Active,
        Passive
    }
}
