using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<CategoryModel> Categories { get; set; }
        public virtual DbSet<ProductModel> Products { get; set; }
        public virtual DbSet<CartModel> Carts { get; set; }
        public virtual DbSet<OrderModel> Orders { get; set; }
        public virtual DbSet<PrescriptionModel> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserModel>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.user_id)
                    .HasColumnName("user_id")
                    .HasColumnType("int");
                entity.Property(e => e.user_email)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(255);
                entity.Property(e => e.user_email)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(255);
                entity.Property(e => e.user_password)
                    .IsRequired()
                    .HasColumnName("user_password")
                    .HasMaxLength(255);
                entity.Property(e => e.user_name)
                    .HasColumnName("user_name")
                    .HasMaxLength(255);
                entity.Property(e => e.user_role)
                    .IsRequired()
                    .HasColumnName("user_role")
                    .HasMaxLength(255);
                entity.Property(e => e.user_phone)
                    .HasColumnName("user_phone")
                    .HasMaxLength(255);
                entity.Property(e => e.user_dob)
                    .HasColumnName("user_dob")
                    .HasMaxLength(255);
                entity.Property(e => e.user_address)
                    .HasColumnName("user_address")
                    .HasMaxLength(255);
            });

            builder.Entity<CartModel>(entity =>
            {
                entity.ToTable("carts");

                entity.Property(e => e.cart_id)
                    .HasColumnName("cart_id")
                    .HasColumnType("int");
                entity.Property(e => e.cart_customer)
                    .HasColumnName("cart_customer")
                    .HasColumnType("int");
                entity.Property(e => e.cart_order)
                    .HasColumnName("cart_order")
                    .HasColumnType("int");
                entity.Property(e => e.cart_product)
                    .HasColumnName("cart_product")
                    .HasColumnType("int");
                entity.Property(e => e.cart_product_count)
                    .HasColumnName("cart_product_count")
                    .HasColumnType("int");
            });

            builder.Entity<CategoryModel>(entity =>
            {
                entity.ToTable("categories");

                entity.Property(e => e.category_id)
                    .HasColumnName("category_id")
                    .HasColumnType("int");
                entity.Property(e => e.category_name)
                    .HasColumnName("category_name")
                    .HasMaxLength(255);
                entity.Property(e => e.category_image)
                    .HasColumnName("category_image")
                    .HasMaxLength(255);
            });

            builder.Entity<OrderModel>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.order_id)
                    .HasColumnName("order_id")
                    .HasColumnType("int");
                entity.Property(e => e.order_date)
                    .HasColumnName("order_date")
                    .HasColumnType("date");
                entity.Property(e => e.order_customer)
                    .HasColumnName("category_image")
                    .HasColumnType("int");
                entity.Property(e => e.order_address)
                    .HasColumnName("order_address")
                    .HasMaxLength(255);
                entity.Property(e => e.order_status)
                    .HasColumnName("order_status")
                    .HasMaxLength(255);
            });

            builder.Entity<PrescriptionModel>(entity =>
            {
                entity.ToTable("priscriptions");

                entity.Property(e => e.prescription_id)
                    .HasColumnName("prescription_id")
                    .HasColumnType("int");
                entity.Property(e => e.prescription_customer)
                    .HasColumnName("prescription_customer")
                    .HasColumnType("int");
                entity.Property(e => e.prescription_image)
                    .HasColumnName("prescription_image")
                    .HasMaxLength(255);
                entity.Property(e => e.prescription_date)
                    .HasColumnName("prescription_date")
                    .HasColumnType("date");
                entity.Property(e => e.prescription_order)
                    .HasColumnName("prescription_order")
                    .HasColumnType("int");
            });

            builder.Entity<ProductModel>(entity =>
            {
                entity.ToTable("products");

                entity.Property(e => e.product_id)
                    .HasColumnName("product_id")
                    .HasColumnType("int");
                entity.Property(e => e.product_name)
                    .HasColumnName("product_name")
                    .HasMaxLength(255);
                entity.Property(e => e.product_image)
                    .HasColumnName("product_image")
                    .HasMaxLength(255);
                entity.Property(e => e.product_price)
                    .HasColumnName("product_price")
                    .HasColumnType("float");
                entity.Property(e => e.product_description)
                    .HasColumnName("product_description");
                entity.Property(e => e.product_category)
                    .HasColumnName("product_category")
                    .HasColumnType("int");
            });
        }
    }
}
