//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class P_DepartmentDailyLabour
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int DepartmentId { get; set; }
        public int LDCurrent { get; set; }
        public int LDOff { get; set; }
        public int LDVacation { get; set; }
        public int LDPregnant { get; set; }
        public int LDNew { get; set; }
        public System.DateTime CreatedAt { get; set; }
    
        public virtual P_Department P_Department { get; set; }
    }
}