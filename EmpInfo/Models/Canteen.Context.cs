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
    
    public partial class CanteenEntities : DbContext
    {
        public CanteenEntities()
            : base("name=CanteenEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<ljq20160323_001_Result> ljq20160323_001(string empNo, string begDay, string endDay)
        {
            var empNoParameter = empNo != null ?
                new ObjectParameter("EmpNo", empNo) :
                new ObjectParameter("EmpNo", typeof(string));
    
            var begDayParameter = begDay != null ?
                new ObjectParameter("BegDay", begDay) :
                new ObjectParameter("BegDay", typeof(string));
    
            var endDayParameter = endDay != null ?
                new ObjectParameter("EndDay", endDay) :
                new ObjectParameter("EndDay", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ljq20160323_001_Result>("ljq20160323_001", empNoParameter, begDayParameter, endDayParameter);
        }
    
        public virtual ObjectResult<ljq20160323_002_Result> ljq20160323_002(string empNo)
        {
            var empNoParameter = empNo != null ?
                new ObjectParameter("EmpNo", empNo) :
                new ObjectParameter("EmpNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ljq20160323_002_Result>("ljq20160323_002", empNoParameter);
        }
    
        public virtual ObjectResult<ljq20160323_003_Result> ljq20160323_003(string empNo, string begDay, string endDay)
        {
            var empNoParameter = empNo != null ?
                new ObjectParameter("EmpNo", empNo) :
                new ObjectParameter("EmpNo", typeof(string));
    
            var begDayParameter = begDay != null ?
                new ObjectParameter("BegDay", begDay) :
                new ObjectParameter("BegDay", typeof(string));
    
            var endDayParameter = endDay != null ?
                new ObjectParameter("EndDay", endDay) :
                new ObjectParameter("EndDay", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ljq20160323_003_Result>("ljq20160323_003", empNoParameter, begDayParameter, endDayParameter);
        }
    
        public virtual int ljq20161019(string empNo, string status)
        {
            var empNoParameter = empNo != null ?
                new ObjectParameter("EmpNo", empNo) :
                new ObjectParameter("EmpNo", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ljq20161019", empNoParameter, statusParameter);
        }
    
        public virtual ObjectResult<laijq20161105_001_Result> laijq20161105_001()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<laijq20161105_001_Result>("laijq20161105_001");
        }
    
        public virtual ObjectResult<laijq20161105_002_Result> laijq20161105_002()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<laijq20161105_002_Result>("laijq20161105_002");
        }
    }
}
