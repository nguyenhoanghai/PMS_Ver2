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
    
    public partial class Floor
    {
        public Floor()
        {
            this.Chuyens = new HashSet<Chuyen>();
            this.KeyPads = new HashSet<KeyPad>();
        }
    
        public int IdFloor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
    
        public virtual ICollection<Chuyen> Chuyens { get; set; }
        public virtual ICollection<KeyPad> KeyPads { get; set; }
    }
}
