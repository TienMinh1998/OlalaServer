
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using APIProject.Domain.Models;

namespace APIProject.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<MemberPointHistory> MemberPointHistories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<OrderComplainImage> OrderComplainImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductStorage> ProductStorages { get; set; }
        public DbSet<ProductStorageHistory> ProductStorageHistories { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<ReceiveAddress> ReceiveAddresses { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<StorageExport> StorageExports { get; set; }
        public DbSet<StorageExportDetail> StorageExportDetails { get; set; }
        public DbSet<StorageImport> StorageImports { get; set; }
        public DbSet<StorageImportDetail> StorageImportDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get;set;}
        public DbSet<RolePermission> RolePermissions { get;set;}

    }
}