﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataBase
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HTJKEntities : DbContext
    {
        public HTJKEntities()
            : base("name=HTJKEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<A_Menu> A_Menu { get; set; }
        public DbSet<A_News> A_News { get; set; }
        public DbSet<A_News_File> A_News_File { get; set; }
        public DbSet<A_NewsType> A_NewsType { get; set; }
        public DbSet<A_Role> A_Role { get; set; }
        public DbSet<A_User> A_User { get; set; }
        public DbSet<WorkLogs> WorkLogs { get; set; }
        public DbSet<Address_Info> Address_Info { get; set; }
        public DbSet<MemberInfo> MemberInfo { get; set; }
        public DbSet<OrderInfo> OrderInfo { get; set; }
        public DbSet<OrderProductsInfo> OrderProductsInfo { get; set; }
        public DbSet<WX_Order_Commission_Logs> WX_Order_Commission_Logs { get; set; }
        public DbSet<WX_Order_FR_Logs> WX_Order_FR_Logs { get; set; }
        public DbSet<WXReturnInfo> WXReturnInfo { get; set; }
    }
}
