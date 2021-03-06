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
    
    public partial class ei_xcApply
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public string applier_phone { get; set; }
        public System.DateTime apply_time { get; set; }
        public string company { get; set; }
        public string dep_name { get; set; }
        public string addr { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string product_model { get; set; }
        public string product_no { get; set; }
        public decimal qty { get; set; }
        public string unit_name { get; set; }
        public string order_require_capacity { get; set; }
        public string dep_highest_capacity { get; set; }
        public string need_outsourcing_capacity { get; set; }
        public string outsourcing_cycle { get; set; }
        public System.DateTime estimate_finish_time { get; set; }
        public string comment { get; set; }
        public string supplier_name { get; set; }
        public Nullable<decimal> unit_price { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public string check_out_comment { get; set; }
        public string check_in_comment { get; set; }
        public string planner_auditor { get; set; }
        public string planner_auditor_num { get; set; }
        public string dep_charger { get; set; }
        public string dep_charger_num { get; set; }
        public string buyer_auditor { get; set; }
        public string buyer_auditor_num { get; set; }
        public int current_month_target { get; set; }
        public Nullable<int> current_month_total { get; set; }
        public string stock_out_comment { get; set; }
        public string stock_in_comment { get; set; }
        public Nullable<System.DateTime> take_back_time { get; set; }
        public string print_time { get; set; }
        public string out_status { get; set; }
        public string out_guard { get; set; }
        public Nullable<System.DateTime> out_time { get; set; }
        public Nullable<System.DateTime> po_date { get; set; }
        public string mat_group { get; set; }
        public string mat_group_num { get; set; }
        public string bus_po_no { get; set; }
        public string bus_stock_no { get; set; }
        public string k3_account { get; set; }
        public Nullable<int> bus_po_entry { get; set; }
        public bool need_ceo_confirm { get; set; }
    }
}
