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
    
    public partial class GetAPPriceHistory_Result
    {
        public string account { get; set; }
        public Nullable<System.DateTime> po_date { get; set; }
        public string bill_no { get; set; }
        public string supplier_name { get; set; }
        public decimal price { get; set; }
        public decimal tax_rate { get; set; }
    }
}