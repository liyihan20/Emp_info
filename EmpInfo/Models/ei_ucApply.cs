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
    
    public partial class ei_ucApply
    {
        public ei_ucApply()
        {
            this.ei_ucApplyEntry = new HashSet<ei_ucApplyEntry>();
        }
    
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public Nullable<System.DateTime> apply_time { get; set; }
        public string market_name { get; set; }
        public string customer_number { get; set; }
        public string customer_name { get; set; }
        public string company { get; set; }
        public string bus_dep { get; set; }
        public Nullable<System.DateTime> arrive_time { get; set; }
        public string reason { get; set; }
        public string delivery_company { get; set; }
        public Nullable<bool> has_attachment { get; set; }
        public string delivery_addr { get; set; }
    
        public virtual ICollection<ei_ucApplyEntry> ei_ucApplyEntry { get; set; }
    }
}