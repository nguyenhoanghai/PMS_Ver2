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
    
    public partial class P_PlayVideoShedule
    {
        public P_PlayVideoShedule()
        {
            this.P_PlayVideoSheduleDetail = new HashSet<P_PlayVideoSheduleDetail>();
        }
    
        public int Id { get; set; }
        public int LineId { get; set; }
        public System.TimeSpan TimeStart { get; set; }
        public System.TimeSpan TimeEnd { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Chuyen Chuyen { get; set; }
        public virtual ICollection<P_PlayVideoSheduleDetail> P_PlayVideoSheduleDetail { get; set; }
    }
}
