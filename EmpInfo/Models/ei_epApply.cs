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
    
    public partial class ei_epApply
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public string applier_phone { get; set; }
        public string bus_dep_name { get; set; }
        public string produce_dep_name { get; set; }
        public string produce_dep_addr { get; set; }
        public string produce_dep_charger_no { get; set; }
        public string produce_dep_charger_name { get; set; }
        public string equitment_name { get; set; }
        public string equitment_modual { get; set; }
        public string property_number { get; set; }
        public string equitment_supplier { get; set; }
        public string equitment_dep_charger_no { get; set; }
        public string equitment_dep_charger_name { get; set; }
        public Nullable<int> emergency_level { get; set; }
        public string faulty_situation { get; set; }
        public Nullable<System.DateTime> report_time { get; set; }
        public Nullable<System.DateTime> accept_time { get; set; }
        public string accept_user_no { get; set; }
        public string accept_user_name { get; set; }
        public string faulty_type { get; set; }
        public string faulty_reason { get; set; }
        public string repair_method { get; set; }
        public string exchange_parts { get; set; }
        public string repair_result { get; set; }
        public string other_repairers { get; set; }
        public string confirm_user_no { get; set; }
        public string confirm_user_name { get; set; }
        public Nullable<System.DateTime> confirm_time { get; set; }
        public string evaluation_content { get; set; }
        public Nullable<System.DateTime> evaluation_time { get; set; }
        public Nullable<int> evaluation_score { get; set; }
        public string transfer_to_repairer { get; set; }
        public Nullable<System.DateTime> confirm_register_time { get; set; }
        public string equitment_dep_name { get; set; }
        public string property_type { get; set; }
        public Nullable<bool> confirm_later_flag { get; set; }
        public string confirm_later_reason { get; set; }
        public Nullable<int> difficulty_score { get; set; }
        public Nullable<System.DateTime> grade_time { get; set; }
    }
}
