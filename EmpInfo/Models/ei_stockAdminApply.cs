//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmpInfo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ei_stockAdminApply
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public Nullable<System.DateTime> apply_time { get; set; }
        public string k3_account_num { get; set; }
        public string k3_account_name { get; set; }
        public string k3_stock_num { get; set; }
        public string k3_stock_name { get; set; }
        public string stock_auditor_num { get; set; }
        public string stock_auditor_name { get; set; }
        public string comment { get; set; }
    }
}
