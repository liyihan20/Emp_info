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
    
    public partial class ei_department
    {
        public ei_department()
        {
            this.ei_departmentAuditNode = new HashSet<ei_departmentAuditNode>();
        }
    
        public int id { get; set; }
        public string FNumber { get; set; }
        public string FName { get; set; }
        public string FAdmin { get; set; }
        public string FParent { get; set; }
        public Nullable<bool> FIsDeleted { get; set; }
        public Nullable<bool> FIsForbit { get; set; }
        public string FCreator { get; set; }
        public Nullable<System.DateTime> FCreateDate { get; set; }
        public Nullable<bool> FIsAuditNode { get; set; }
    
        public virtual ICollection<ei_departmentAuditNode> ei_departmentAuditNode { get; set; }
    }
}
