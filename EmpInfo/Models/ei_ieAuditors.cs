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
    
    public partial class ei_ieAuditors
    {
        public int id { get; set; }
        public string bus_name { get; set; }
        public string dep_names { get; set; }
        public string bus_minister_no { get; set; }
        public string bus_minister_name { get; set; }
        public string ie_leader_no { get; set; }
        public string ie_leader_name { get; set; }
        public Nullable<bool> is_forbit { get; set; }
    }
}