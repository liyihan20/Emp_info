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
    
    public partial class ei_etApplyEntry
    {
        public int id { get; set; }
        public Nullable<int> et_id { get; set; }
        public string order_number { get; set; }
        public string item_name { get; set; }
        public string item_modual { get; set; }
        public Nullable<decimal> qty { get; set; }
    
        public virtual ei_etApply ei_etApply { get; set; }
    }
}
