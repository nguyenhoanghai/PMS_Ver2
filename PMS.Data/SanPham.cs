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
    
    public partial class SanPham
    {
        public SanPham()
        {
            this.MapIdSanPhamNgays = new HashSet<MapIdSanPhamNgay>();
            this.P_AssignCompletion = new HashSet<P_AssignCompletion>();
            this.TheoDoiNgays = new HashSet<TheoDoiNgay>();
            this.Chuyen_SanPham = new HashSet<Chuyen_SanPham>();
        }
    
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string DinhNghia { get; set; }
        public Nullable<int> Floor { get; set; }
        public double DonGia { get; set; }
        public double DonGiaCM { get; set; }
        public double ProductionTime { get; set; }
        public bool IsDelete { get; set; }
        public string MaKhachHang { get; set; }
        public double DonGiaCat { get; set; }
    
        public virtual ICollection<MapIdSanPhamNgay> MapIdSanPhamNgays { get; set; }
        public virtual ICollection<P_AssignCompletion> P_AssignCompletion { get; set; }
        public virtual ICollection<TheoDoiNgay> TheoDoiNgays { get; set; }
        public virtual ICollection<Chuyen_SanPham> Chuyen_SanPham { get; set; }
    }
}