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
    
    public partial class flow_template
    {
        public flow_template()
        {
            this.flow_apply = new HashSet<flow_apply>();
            this.flow_templateEntry = new HashSet<flow_templateEntry>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public bool enable { get; set; }
        public Nullable<System.DateTime> effective_date { get; set; }
        public Nullable<System.DateTime> expire_date { get; set; }
        public string bill_type { get; set; }
        public string summary { get; set; }
        public Nullable<System.DateTime> create_date { get; set; }
        public string creater { get; set; }
    
        public virtual ICollection<flow_apply> flow_apply { get; set; }
        public virtual ICollection<flow_templateEntry> flow_templateEntry { get; set; }
    }
}
