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
    
    public partial class GetAPHistoryQty_Result
    {
        public string account { get; set; }
        public string dep_name { get; set; }
        public string bus_name { get; set; }
        public decimal qty { get; set; }
        public Nullable<decimal> transit_qty { get; set; }
        public Nullable<System.DateTime> po_date { get; set; }
        public string FBillNo { get; set; }
    }
}
