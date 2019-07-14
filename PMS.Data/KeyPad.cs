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
    
    public partial class KeyPad
    {
        public KeyPad()
        {
            this.KeyPad_Object = new HashSet<KeyPad_Object>();
        }
    
        public int Id { get; set; }
        public string KeyPadName { get; set; }
        public string Description { get; set; }
        public Nullable<int> EquipmentId { get; set; }
        public Nullable<int> FloorId { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> UseTypeId { get; set; }
    
        public virtual Floor Floor { get; set; }
        public virtual ICollection<KeyPad_Object> KeyPad_Object { get; set; }
    }
}
