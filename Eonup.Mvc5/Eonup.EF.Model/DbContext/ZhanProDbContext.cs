using Eonup.EF.Model.Model;
namespace Eonup.EF.Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class ZhanProDbContext : DbContext
	{
		public ZhanProDbContext()
			: base("name=ZhanPro")
		{
		}

		public virtual DbSet<Bus_Bank> Bus_Bank { get; set; }
		public virtual DbSet<Bus_follow> Bus_follow { get; set; }
		public virtual DbSet<Company> Company { get; set; }
		public virtual DbSet<temp_tab1> temp_tab1 { get; set; }
		public virtual DbSet<User> User { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Bus_Bank>()
				.Property(e => e.Deposit)
				.HasPrecision(12, 2);

			modelBuilder.Entity<User>()
				.Property(e => e.Account)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Password)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Email)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.Mobile)
				.IsUnicode(false);
		}
	}
}
