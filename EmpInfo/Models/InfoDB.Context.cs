﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ICAuditEntities : DbContext
    {
        public ICAuditEntities()
            : base("name=ICAuditEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ei_users> ei_users { get; set; }
        public DbSet<ei_event_log> ei_event_log { get; set; }
        public DbSet<vw_ei_users> vw_ei_users { get; set; }
        public DbSet<ei_authority> ei_authority { get; set; }
        public DbSet<ei_groupAuthority> ei_groupAuthority { get; set; }
        public DbSet<ei_groups> ei_groups { get; set; }
        public DbSet<ei_groupUser> ei_groupUser { get; set; }
        public DbSet<dn_dishes> dn_dishes { get; set; }
        public DbSet<dn_order> dn_order { get; set; }
        public DbSet<dn_orderEntry> dn_orderEntry { get; set; }
        public DbSet<dn_desks> dn_desks { get; set; }
        public DbSet<dn_shoppingCar> dn_shoppingCar { get; set; }
        public DbSet<all_maxNum> all_maxNum { get; set; }
        public DbSet<dn_discountInfo> dn_discountInfo { get; set; }
        public DbSet<dn_discountInfoUsers> dn_discountInfoUsers { get; set; }
        public DbSet<dn_points> dn_points { get; set; }
        public DbSet<dn_pointsForDish> dn_pointsForDish { get; set; }
        public DbSet<dn_pointsRecord> dn_pointsRecord { get; set; }
        public DbSet<dn_birthdayMealLog> dn_birthdayMealLog { get; set; }
        public DbSet<ei_specialUsers> ei_specialUsers { get; set; }
        public DbSet<dn_items> dn_items { get; set; }
        public DbSet<dn_Restaurent> dn_Restaurent { get; set; }
        public DbSet<ei_resVisitLog> ei_resVisitLog { get; set; }
        public DbSet<vw_ei_simple_users> vw_ei_simple_users { get; set; }
        public DbSet<ei_deliveryInfo> ei_deliveryInfo { get; set; }
        public DbSet<ei_pushMsg> ei_pushMsg { get; set; }
        public DbSet<ei_emp_portrait> ei_emp_portrait { get; set; }
        public DbSet<ei_PushResponse> ei_PushResponse { get; set; }
    
        public virtual ObjectResult<GetHREmpInfo_Result> GetHREmpInfo(string card_no)
        {
            var card_noParameter = card_no != null ?
                new ObjectParameter("card_no", card_no) :
                new ObjectParameter("card_no", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetHREmpInfo_Result>("GetHREmpInfo", card_noParameter);
        }
    
        public virtual ObjectResult<string> GetDormChargeMonth()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetDormChargeMonth");
        }
    
        public virtual ObjectResult<GetEmpDormInfo_Result> GetEmpDormInfo(string card_no)
        {
            var card_noParameter = card_no != null ?
                new ObjectParameter("card_no", card_no) :
                new ObjectParameter("card_no", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetEmpDormInfo_Result>("GetEmpDormInfo", card_noParameter);
        }
    
        public virtual ObjectResult<GetDormFeeByMonth_Result> GetDormFeeByMonth(string yearMonth, string salaryNo)
        {
            var yearMonthParameter = yearMonth != null ?
                new ObjectParameter("yearMonth", yearMonth) :
                new ObjectParameter("yearMonth", typeof(string));
    
            var salaryNoParameter = salaryNo != null ?
                new ObjectParameter("salaryNo", salaryNo) :
                new ObjectParameter("salaryNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDormFeeByMonth_Result>("GetDormFeeByMonth", yearMonthParameter, salaryNoParameter);
        }
    
        public virtual ObjectResult<ValidateDormStatus_Result> ValidateDormStatus(string account)
        {
            var accountParameter = account != null ?
                new ObjectParameter("account", account) :
                new ObjectParameter("account", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidateDormStatus_Result>("ValidateDormStatus", accountParameter);
        }
    
        public virtual ObjectResult<string> GetHREmpStatus(string card_no)
        {
            var card_noParameter = card_no != null ?
                new ObjectParameter("card_no", card_no) :
                new ObjectParameter("card_no", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetHREmpStatus", card_noParameter);
        }
    
        public virtual ObjectResult<GetPOAccount_Result> GetPOAccount(string po_number)
        {
            var po_numberParameter = po_number != null ?
                new ObjectParameter("po_number", po_number) :
                new ObjectParameter("po_number", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPOAccount_Result>("GetPOAccount", po_numberParameter);
        }
    
        public virtual int InsertIntoYFEmp(string email, string mobilephone, string shortphone, string username, string cardno)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var mobilephoneParameter = mobilephone != null ?
                new ObjectParameter("mobilephone", mobilephone) :
                new ObjectParameter("mobilephone", typeof(string));
    
            var shortphoneParameter = shortphone != null ?
                new ObjectParameter("shortphone", shortphone) :
                new ObjectParameter("shortphone", typeof(string));
    
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var cardnoParameter = cardno != null ?
                new ObjectParameter("cardno", cardno) :
                new ObjectParameter("cardno", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertIntoYFEmp", emailParameter, mobilephoneParameter, shortphoneParameter, usernameParameter, cardnoParameter);
        }
    
        public virtual ObjectResult<GetSalaryDetail_Result> GetSalaryDetail(Nullable<int> salary_no, Nullable<System.DateTime> begin_date, Nullable<System.DateTime> end_date)
        {
            var salary_noParameter = salary_no.HasValue ?
                new ObjectParameter("salary_no", salary_no) :
                new ObjectParameter("salary_no", typeof(int));
    
            var begin_dateParameter = begin_date.HasValue ?
                new ObjectParameter("begin_date", begin_date) :
                new ObjectParameter("begin_date", typeof(System.DateTime));
    
            var end_dateParameter = end_date.HasValue ?
                new ObjectParameter("end_date", end_date) :
                new ObjectParameter("end_date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalaryDetail_Result>("GetSalaryDetail", salary_noParameter, begin_dateParameter, end_dateParameter);
        }
    
        public virtual ObjectResult<GetSalaryInfo_Result> GetSalaryInfo(Nullable<int> salary_no)
        {
            var salary_noParameter = salary_no.HasValue ?
                new ObjectParameter("salary_no", salary_no) :
                new ObjectParameter("salary_no", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalaryInfo_Result>("GetSalaryInfo", salary_noParameter);
        }
    
        public virtual ObjectResult<GetSalarySummary_Result> GetSalarySummary(Nullable<int> salary_no, Nullable<System.DateTime> begin_date, Nullable<System.DateTime> end_date)
        {
            var salary_noParameter = salary_no.HasValue ?
                new ObjectParameter("salary_no", salary_no) :
                new ObjectParameter("salary_no", typeof(int));
    
            var begin_dateParameter = begin_date.HasValue ?
                new ObjectParameter("begin_date", begin_date) :
                new ObjectParameter("begin_date", typeof(System.DateTime));
    
            var end_dateParameter = end_date.HasValue ?
                new ObjectParameter("end_date", end_date) :
                new ObjectParameter("end_date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSalarySummary_Result>("GetSalarySummary", salary_noParameter, begin_dateParameter, end_dateParameter);
        }
    
        public virtual ObjectResult<string> GetSalaryMonths(Nullable<int> salary_no)
        {
            var salary_noParameter = salary_no.HasValue ?
                new ObjectParameter("salary_no", salary_no) :
                new ObjectParameter("salary_no", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("GetSalaryMonths", salary_noParameter);
        }
    }
}
