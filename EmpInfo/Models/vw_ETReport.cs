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
    
    public partial class vw_ETReport
    {
        public int id { get; set; }
        public string sys_no { get; set; }
        public string applier_num { get; set; }
        public string applier_name { get; set; }
        public string applier_phone { get; set; }
        public System.DateTime apply_time { get; set; }
        public string company { get; set; }
        public string market_name { get; set; }
        public string customer_number { get; set; }
        public string customer_name { get; set; }
        public string bus_dep { get; set; }
        public System.DateTime out_time { get; set; }
        public string transfer_style { get; set; }
        public string demand { get; set; }
        public string gross_weight { get; set; }
        public string pack_num { get; set; }
        public string box_size { get; set; }
        public string cardboard_size { get; set; }
        public string reason { get; set; }
        public string responsibility { get; set; }
        public string addr { get; set; }
        public bool has_attachment { get; set; }
        public string dilivery_company { get; set; }
        public Nullable<decimal> normal_fee { get; set; }
        public Nullable<decimal> apply_fee { get; set; }
        public Nullable<decimal> different_fee { get; set; }
        public int entry_id { get; set; }
        public string order_number { get; set; }
        public string item_name { get; set; }
        public string item_modual { get; set; }
        public Nullable<decimal> qty { get; set; }
        public string market_auditor { get; set; }
        public string logistics_editor { get; set; }
        public string operation_auditor { get; set; }
        public string logistics_auditor { get; set; }
        public string busPlanner_auditor { get; set; }
    }
}
