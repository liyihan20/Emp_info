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
    
    public partial class ei_sjApply
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public Nullable<System.DateTime> apply_time { get; set; }
        public string salary_type { get; set; }
        public string switch_type { get; set; }
        public string card_number { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public string education { get; set; }
        public string account { get; set; }
        public string out_dep_name { get; set; }
        public Nullable<int> out_dep_id { get; set; }
        public string out_dep_position { get; set; }
        public Nullable<System.DateTime> out_time { get; set; }
        public string out_clerk_name { get; set; }
        public string out_clerk_num { get; set; }
        public string out_manager_name { get; set; }
        public string out_manager_num { get; set; }
        public string out_minister_name { get; set; }
        public string out_minister_num { get; set; }
        public string in_dep_name { get; set; }
        public Nullable<int> in_dep_id { get; set; }
        public string in_dep_position { get; set; }
        public Nullable<System.DateTime> in_time { get; set; }
        public string in_clerk_name { get; set; }
        public string in_clerk_num { get; set; }
        public string in_manager_name { get; set; }
        public string in_manager_num { get; set; }
        public string in_minister_name { get; set; }
        public string in_minister_num { get; set; }
        public string comment { get; set; }
    }
}
