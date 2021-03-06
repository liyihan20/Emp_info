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
    
    public partial class ei_hhApply
    {
        public ei_hhApply()
        {
            this.ei_hhReturnDetail = new HashSet<ei_hhReturnDetail>();
            this.ei_hhApplyEntry = new HashSet<ei_hhApplyEntry>();
        }
    
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public System.DateTime apply_time { get; set; }
        public string agency_name { get; set; }
        public string customer_no { get; set; }
        public string customer_name { get; set; }
        public string return_dep { get; set; }
        public string quality_manager_no { get; set; }
        public string quality_manager_name { get; set; }
        public System.DateTime return_date { get; set; }
        public string return_reason { get; set; }
        public string return_addr { get; set; }
        public bool cargo_has_return { get; set; }
        public string company { get; set; }
        public bool has_attachment { get; set; }
        public Nullable<System.DateTime> out_time { get; set; }
        public string out_guard { get; set; }
        public string out_status { get; set; }
        public string print_time { get; set; }
        public string notify_clerk_name { get; set; }
        public string notify_clerk_no { get; set; }
        public string charge_customers { get; set; }
    
        public virtual ICollection<ei_hhReturnDetail> ei_hhReturnDetail { get; set; }
        public virtual ICollection<ei_hhApplyEntry> ei_hhApplyEntry { get; set; }
    }
}
