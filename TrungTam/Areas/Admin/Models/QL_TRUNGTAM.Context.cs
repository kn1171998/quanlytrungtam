﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrungTam.Areas.Admin.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QL_TRUNGTAM1Entities : DbContext
    {
        public QL_TRUNGTAM1Entities()
            : base("name=QL_TRUNGTAM1Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BANG_GIA_HOC_PHI> BANG_GIA_HOC_PHI { get; set; }
        public virtual DbSet<BANG_LUONG> BANG_LUONG { get; set; }
        public virtual DbSet<BUOI_HOC> BUOI_HOC { get; set; }
        public virtual DbSet<CHI_TIEU_NGOAI> CHI_TIEU_NGOAI { get; set; }
        public virtual DbSet<CONG_NO> CONG_NO { get; set; }
        public virtual DbSet<CT_BUOIHOC> CT_BUOIHOC { get; set; }
        public virtual DbSet<CT_CONG_NO> CT_CONG_NO { get; set; }
        public virtual DbSet<CT_HOADON> CT_HOADON { get; set; }
        public virtual DbSet<CT_HOADON_NGOAIGIO> CT_HOADON_NGOAIGIO { get; set; }
        public virtual DbSet<CT_LOP_HOC> CT_LOP_HOC { get; set; }
        public virtual DbSet<DANH_GIA_CUOI_THANG> DANH_GIA_CUOI_THANG { get; set; }
        public virtual DbSet<GHI_DANH> GHI_DANH { get; set; }
        public virtual DbSet<GIA_SU> GIA_SU { get; set; }
        public virtual DbSet<GIAO_VIEN> GIAO_VIEN { get; set; }
        public virtual DbSet<HOA_DON> HOA_DON { get; set; }
        public virtual DbSet<HOC_SINH> HOC_SINH { get; set; }
        public virtual DbSet<KHOI_LOP> KHOI_LOP { get; set; }
        public virtual DbSet<KHUYEN_MAI> KHUYEN_MAI { get; set; }
        public virtual DbSet<LOAI_LOP> LOAI_LOP { get; set; }
        public virtual DbSet<LOP_HOC> LOP_HOC { get; set; }
        public virtual DbSet<MON_HOC> MON_HOC { get; set; }
        public virtual DbSet<NGOAI_GIO> NGOAI_GIO { get; set; }
        public virtual DbSet<TAI_KHOAN> TAI_KHOAN { get; set; }
        public virtual DbSet<TAILIEU> TAILIEUx { get; set; }
        public virtual DbSet<THOI_KHOA_BIEU> THOI_KHOA_BIEU { get; set; }
        public virtual DbSet<TRANG_CHU> TRANG_CHU { get; set; }
    }
}
