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
    
    public partial class ei_apApplyEntry
    {
        public int id { get; set; }
        public Nullable<int> ap_id { get; set; }
        public string item_no { get; set; }
        public string item_name { get; set; }
        public string item_modual { get; set; }
        public Nullable<int> item_id { get; set; }
        public string brand { get; set; }
        public Nullable<decimal> qty { get; set; }
        public string unit_name { get; set; }
        public Nullable<System.DateTime> latest_arrive_date { get; set; }
        public string using_speed { get; set; }
        public string can_use_span { get; set; }
        public string order_period { get; set; }
        public string usage { get; set; }
        public Nullable<int> entry_no { get; set; }
        public Nullable<decimal> real_qty { get; set; }
    
        public virtual ei_apApply ei_apApply { get; set; }
    }
}
