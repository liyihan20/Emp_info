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
    
    public partial class ei_deliveryInfo
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<System.DateTime> register_date { get; set; }
        public string delivery_no { get; set; }
        public string deivery_cop { get; set; }
        public string delivery_stuff { get; set; }
        public Nullable<System.DateTime> drop_date { get; set; }
        public Nullable<System.DateTime> fetch_date { get; set; }
        public string dorm_area { get; set; }
        public string dorm_number { get; set; }
        public string status { get; set; }
        public Nullable<bool> is_finish { get; set; }
        public Nullable<int> packs { get; set; }
    
        public virtual ei_users ei_users { get; set; }
    }
}