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
    
    public partial class ei_groupAuthority
    {
        public int id { get; set; }
        public Nullable<int> group_id { get; set; }
        public Nullable<int> authority_id { get; set; }
    
        public virtual ei_authority ei_authority { get; set; }
        public virtual ei_groups ei_groups { get; set; }
    }
}
