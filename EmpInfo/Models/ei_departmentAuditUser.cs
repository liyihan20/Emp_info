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
    
    public partial class ei_departmentAuditUser
    {
        public int id { get; set; }
        public Nullable<int> FDepartmentNodeId { get; set; }
        public string FAuditorNumber { get; set; }
        public Nullable<System.DateTime> FBeginTime { get; set; }
        public Nullable<System.DateTime> FEndTime { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    
        public virtual ei_departmentAuditNode ei_departmentAuditNode { get; set; }
    }
}
