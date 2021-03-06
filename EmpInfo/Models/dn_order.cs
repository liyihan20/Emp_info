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
    
    public partial class dn_order
    {
        public dn_order()
        {
            this.dn_orderEntry = new HashSet<dn_orderEntry>();
            this.dn_shoppingCar = new HashSet<dn_shoppingCar>();
        }
    
        public int id { get; set; }
        public string order_no { get; set; }
        public string user_name { get; set; }
        public string user_no { get; set; }
        public Nullable<bool> is_in_room { get; set; }
        public Nullable<bool> is_delivery { get; set; }
        public Nullable<int> people_num { get; set; }
        public Nullable<System.DateTime> arrive_day { get; set; }
        public string arrive_time { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string user_comment { get; set; }
        public string payment_type { get; set; }
        public Nullable<System.DateTime> payment_time { get; set; }
        public string auditor_name { get; set; }
        public string auditor_no { get; set; }
        public Nullable<bool> audit_result { get; set; }
        public string audit_comment { get; set; }
        public Nullable<System.DateTime> audit_time { get; set; }
        public string desk_num { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public Nullable<decimal> real_price { get; set; }
        public Nullable<int> discount_rate { get; set; }
        public string book_card_num { get; set; }
        public string delivery_addr { get; set; }
        public Nullable<bool> cancelled { get; set; }
        public string cancell_reason { get; set; }
        public string status { get; set; }
        public string recipient { get; set; }
        public string recipient_phone { get; set; }
        public Nullable<int> end_flag { get; set; }
        public string order_phone { get; set; }
        public string benefit_info { get; set; }
        public string res_no { get; set; }
        public string desk_name { get; set; }
    
        public virtual ICollection<dn_orderEntry> dn_orderEntry { get; set; }
        public virtual ICollection<dn_shoppingCar> dn_shoppingCar { get; set; }
    }
}
