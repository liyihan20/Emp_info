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
    
    public partial class ei_xcDepTarget
    {
        public int id { get; set; }
        public string year_month { get; set; }
        public string dep_name { get; set; }
        public string manager { get; set; }
        public string manager_no { get; set; }
        public int month_target { get; set; }
        public System.DateTime create_date { get; set; }
        public System.DateTime update_date { get; set; }
        public string create_user { get; set; }
        public string update_user { get; set; }
        public string company { get; set; }
    }
}
