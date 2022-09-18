using Eonup.EF.Model.Model;
namespace Eonup.EF.Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class NorthwindContext : DbContext
	{
		public NorthwindContext()
			: base("name=Northwind")
		{
		}

		public virtual DbSet<Category> Categories { get; set; }
		public virtual DbSet<CustomerDemographic> CustomerDemographics { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<dtproperty> dtproperties { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
		public virtual DbSet<Order_Detail> Order_Details { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Region> Regions { get; set; }
		public virtual DbSet<Shipper> Shippers { get; set; }
		public virtual DbSet<Supplier> Suppliers { get; set; }
		public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
		public virtual DbSet<T_User> T_User { get; set; }
		public virtual DbSet<Territory> Territories { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CustomerDemographic>()
				.Property(e => e.CustomerTypeID)
				.IsFixedLength();

			modelBuilder.Entity<CustomerDemographic>()
				.HasMany(e => e.Customers)
				.WithMany(e => e.CustomerDemographics)
				.Map(m => m.ToTable("CustomerCustomerDemo").MapLeftKey("CustomerTypeID").MapRightKey("CustomerID"));

			modelBuilder.Entity<Customer>()
				.Property(e => e.CustomerID)
				.IsFixedLength();

			modelBuilder.Entity<dtproperty>()
				.Property(e => e.property)
				.IsUnicode(false);

			modelBuilder.Entity<dtproperty>()
				.Property(e => e.value)
				.IsUnicode(false);

			modelBuilder.Entity<Employee>()
				.HasMany(e => e.Employees1)
				.WithOptional(e => e.Employee1)
				.HasForeignKey(e => e.ReportsTo);

			modelBuilder.Entity<Employee>()
				.HasMany(e => e.Territories)
				.WithMany(e => e.Employees)
				.Map(m => m.ToTable("EmployeeTerritories").MapLeftKey("EmployeeID").MapRightKey("TerritoryID"));

			modelBuilder.Entity<Order_Detail>()
				.Property(e => e.UnitPrice)
				.HasPrecision(19, 4);

			modelBuilder.Entity<Order>()
				.Property(e => e.CustomerID)
				.IsFixedLength();

			modelBuilder.Entity<Order>()
				.Property(e => e.Freight)
				.HasPrecision(19, 4);

			modelBuilder.Entity<Order>()
				.HasMany(e => e.Order_Details)
				.WithRequired(e => e.Order)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Product>()
				.Property(e => e.UnitPrice)
				.HasPrecision(19, 4);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.Order_Details)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Region>()
				.Property(e => e.RegionDescription)
				.IsFixedLength();

			modelBuilder.Entity<Region>()
				.HasMany(e => e.Territories)
				.WithRequired(e => e.Region)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Shipper>()
				.HasMany(e => e.Orders)
				.WithOptional(e => e.Shipper)
				.HasForeignKey(e => e.ShipVia);

			modelBuilder.Entity<T_User>()
				.Property(e => e.UserName)
				.IsUnicode(false);

			modelBuilder.Entity<T_User>()
				.Property(e => e.Remark)
				.IsUnicode(false);

			modelBuilder.Entity<T_User>()
				.Property(e => e.Pwd)
				.IsUnicode(false);

			modelBuilder.Entity<T_User>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<Territory>()
				.Property(e => e.TerritoryDescription)
				.IsFixedLength();
		}
	}
}
