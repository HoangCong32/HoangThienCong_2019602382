﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebBanSach.Data.EF.Configurations;
using WebBanSach.Data.EF.Extensions;
using WebBanSach.Data.Entities;
using WebBanSach.Data.Interfaces;

namespace WebBanSach.Data.EF
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<SystemConfig> SystemConfigs { get; set; }
		public DbSet<Function> Functions { get; set; }

		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppRole> AppRoles { get; set; }

		public DbSet<Bill> Bills { set; get; }
		public DbSet<BillDetail> BillDetails { set; get; }
		public DbSet<Contact> Contacts { set; get; }
		public DbSet<Feedback> Feedbacks { set; get; }
		public DbSet<Footer> Footers { set; get; }
		public DbSet<Page> Pages { set; get; }
		public DbSet<Product> Products { set; get; }
		public DbSet<ProductCategory> ProductCategories { set; get; }
		public DbSet<ProductImage> ProductImages { set; get; }
		public DbSet<ProductQuantity> ProductQuantities { set; get; }
		public DbSet<ProductTag> ProductTags { set; get; }

		public DbSet<Tag> Tags { set; get; }

		public DbSet<Permission> Permissions { get; set; }
		public DbSet<WholePrice> WholePrices { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{

			#region Identity Config

			builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
				.HasKey(x => x.Id);

			builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

			builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
				.HasKey(x => new { x.RoleId, x.UserId });

			builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
			   .HasKey(x => new { x.UserId });

			#endregion Identity Config

			builder.AddConfiguration(new TagConfiguration());
			builder.AddConfiguration(new ContactDetailConfiguration());
			builder.AddConfiguration(new FooterConfiguration());
			builder.AddConfiguration(new PageConfiguration());
			builder.AddConfiguration(new FooterConfiguration());
			builder.AddConfiguration(new ProductTagConfiguration());
			builder.AddConfiguration(new SystemConfigConfiguration());

			//base.OnModelCreating(builder);
		}

		public override int SaveChanges()
		{
			var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

			foreach (EntityEntry item in modified)
			{
				var changedOrAddedItem = item.Entity as IDateTracking;
				if (changedOrAddedItem != null)
				{
					if (item.State == EntityState.Added)
					{
						changedOrAddedItem.DateCreated = DateTime.Now;
					}
                    
					else
					{
                        changedOrAddedItem.DateCreated = DateTime.Now;
                        changedOrAddedItem.DateModified = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
                
            }
			return base.SaveChanges();
		}
	}

	public class IDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
	{
		public AppDbContext CreateDbContext(string[] args)
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json").Build();
			var builder = new DbContextOptionsBuilder<AppDbContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			builder.UseSqlServer(connectionString);
			return new AppDbContext(builder.Options);
		}
	}
}
