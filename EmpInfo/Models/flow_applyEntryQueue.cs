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
    
    public partial class flow_applyEntryQueue
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public Nullable<int> step { get; set; }
        public string step_name { get; set; }
        public string auditors { get; set; }
        public Nullable<bool> countersign { get; set; }
        public Nullable<int> flow_template_entry_id { get; set; }
    }
}
