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
    
    public partial class ei_ieApply
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_name { get; set; }
        public string applier_number { get; set; }
        public Nullable<System.DateTime> apply_time { get; set; }
        public string bus_name { get; set; }
        public string dep_name { get; set; }
        public string project_name { get; set; }
        public string project_no { get; set; }
        public string project_type { get; set; }
        public string project_property { get; set; }
        public string proposer { get; set; }
        public string group_leader { get; set; }
        public string group_member { get; set; }
        public string before_flow { get; set; }
        public string before_people { get; set; }
        public string before_production_setup { get; set; }
        public string before_position_beat { get; set; }
        public string improve_method { get; set; }
        public Nullable<decimal> equitment_price { get; set; }
        public Nullable<int> equitment_qty { get; set; }
        public Nullable<int> after_save_people_sum { get; set; }
        public Nullable<decimal> after_save_money { get; set; }
        public Nullable<decimal> after_investment_money { get; set; }
        public Nullable<System.DateTime> period1_from { get; set; }
        public Nullable<System.DateTime> period1_to { get; set; }
        public string period1_content { get; set; }
        public string period1_man { get; set; }
        public Nullable<System.DateTime> period2_from { get; set; }
        public Nullable<System.DateTime> period2_to { get; set; }
        public string period2_content { get; set; }
        public string period2_man { get; set; }
        public Nullable<System.DateTime> period3_from { get; set; }
        public Nullable<System.DateTime> period3_to { get; set; }
        public string period3_content { get; set; }
        public string period3_man { get; set; }
        public bool has_attachment { get; set; }
    }
}