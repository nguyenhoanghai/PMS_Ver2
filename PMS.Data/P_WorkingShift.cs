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
    
    public partial class P_WorkingShift
    {
        public P_WorkingShift()
        {
            this.P_LineWorkingShift = new HashSet<P_LineWorkingShift>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public System.TimeSpan TimeStart { get; set; }
        public System.TimeSpan TimeEnd { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<P_LineWorkingShift> P_LineWorkingShift { get; set; }
    }
}