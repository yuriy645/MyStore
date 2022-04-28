using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MyStore.Models.DBEntities;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;


#nullable disable

namespace MyStore
{
    public partial class MyStoreContext : DbContext
    {
        public MyStoreContext()
        {
        }

        public MyStoreContext(DbContextOptions<MyStoreContext> options)
            : base(options)
        {
        }

        

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryCharacteristic> CategoryCharacteristics { get; set; }
        public virtual DbSet<Characteristic> Characteristics { get; set; }
        public virtual DbSet<CharacteristicValue> CharacteristicValues { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<ColoredProduct> ColoredProducts { get; set; }
        public virtual DbSet<DeliveryType> DeliveryTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductValue> ProductValues { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Value> Values { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                //var connectionString = ConfigurationManager.ConnectionStrings["MyStoreConnectionString"].ConnectionString;

                optionsBuilder.UseSqlServer(
                    // "Server=(localdb)\\MSSQLLocaldb; database=MyStoreVline; Trusted_Connection=true"
                    //"workstation id=MyStoreVline.mssql.somee.com;packet size=4096;user id=markela_SQLLogin_1;pwd=ib7yramxze;data source=MyStoreVline.mssql.somee.com;persist security info=False;initial catalog=MyStoreVline"
                    "workstation id=MyStoreVline.mssql.somee.com;packet size=4096;user id=markela_SQLLogin_1;pwd=ib7yramxze;data source=MyStoreVline.mssql.somee.com;persist security info=False;initial catalog=MyStoreVline"

                    //connectionString

                    );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // 6
            modelBuilder.Entity<CategoryCharacteristic>(entity =>
            {
                entity.HasKey(e => e.CategoryCharacteristicsId)
                    .HasName("PK_CategoryCharacteristics_CharacteristicValuesId");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryCharacteristics)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryCharacteristics_Categories");

                entity.HasOne(d => d.Characteristic)
                    .WithMany(p => p.CategoryCharacteristics)
                    .HasForeignKey(d => d.CharacteristicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryCharacteristics_Characteristics");

                entity.Property(e => e.OrdinationNumber)//
                    .IsRequired();
            });

            // 7
            modelBuilder.Entity<CharacteristicValue>(entity =>
            {
                entity.HasKey(e => e.CharacteristicValuesId)
                    .HasName("PK_CharacteristicValues_CharacteristicValuesId");

                //entity.HasOne(d => d.CharacteristicValues)**
                entity.HasOne(d => d.CategoryCharacteristic)//**
                    .WithMany(m => m.CharacteristicValues)
                    .HasForeignKey(d => d.CategoryCharacteristicsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CharacteristicValues_CategoryCharacteristics");

                entity.HasOne(d => d.Value)
                    .WithMany(m => m.CharacteristicValues)
                    .HasForeignKey(d => d.ValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    //если в главной табл Values удалится строка (ValueId, ValueName),
                    //то здесь FK ValueId станет NULL (но он не может стать NULL ).
                    //можно было бы поставить ClientCascade, но я сейчас ставлю цель
                    //вручную работать с каждой таблицей, не пользуясь автоматическими удалениями.
                    .HasConstraintName("FK_CharacteristicValues_Values");
            });

            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.HasIndex(e => e.CharacteristicName)
                    .IsUnique();

                entity.Property(e => e.CharacteristicName).HasMaxLength(50);
            });

            

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.Apartament).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.DeliveryMeth).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.House).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Npnumber).HasColumnName("NPNumber");

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.SecondName).HasMaxLength(50);

                entity.Property(e => e.Street).HasMaxLength(50);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId)
                    .HasName("PK_Colors_ColorId");//

                entity.Property(e => e.ColorName)
                    .HasMaxLength(50)
                    .HasColumnName("ColorName");//
                
                    
            });

            modelBuilder.Entity<ColoredProduct>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ColorId });

                entity.HasIndex(e => e.ProductCode)
                    .IsUnique();

                entity.Property(e => e.ImgResized)
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.ProductCode).HasMaxLength(200);

                entity.Property(e => e.ProductName).HasMaxLength(200);

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.ColoredProducts)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ColoredProducts_Colors");
               
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ColoredProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ColoredProducts_Products");
            });

            modelBuilder.Entity<DeliveryType>(entity =>
            {
                entity.Property(e => e.DeliveryTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("DeliveryTypeName");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SecondName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PassHash)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => e.ImageId)
                   .HasName("PK_Images_ImageId");

                //entity.Property(e => e.ImageBody)
                //    //.HasMaxLength(1)
                //    .HasColumnName("ImageBody")
                //    .IsFixedLength(true);

                entity.HasOne(d => d.ColoredProduct)
                    .WithMany(d => d.Images)
                    .HasForeignKey(d => new { d.ProductId, d.ColorId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Images_ColoredProducts");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK_Orders_OrderId");

                entity.Property(e => e.AdminComment).HasMaxLength(50);

                entity.Property(e => e.Comment).HasMaxLength(50);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Clients");

                entity.HasOne(d => d.DeliveryType)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_DeliveryTypes");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Employees");

                //entity.HasOne(d => d.ColoredProduct)
                //    .WithMany()
                //    .HasForeignKey(d => new { d.ProductId, d.ColorId })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Orders_ColoredProducts");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Categories");

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Sections");
            });

            modelBuilder.Entity<ProductValue>(entity =>
            {
                entity.HasKey(e => e.ProductValuesId)
                    .HasName("PK_ProductValues_ProductValuesId");

                //entity.HasOne(d => d.CharacteristicValues)**
                entity.HasOne(d => d.CharacteristicValue)//**
                    .WithMany(p => p.ProductValues)
                    .HasForeignKey(d => d.CharacteristicValuesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductValues_CharacteristicValues");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductValues)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductValues_Products");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ColorId, e.OrderId });

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_Orders");

                entity.HasOne(d => d.ColoredProduct)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => new { d.ProductId, d.ColorId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Purchases_ColoredProducts");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Value>(entity =>
            {
                entity.HasKey(e => e.ValueId)
                    .HasName("PK_Values_ValueId");

                entity.HasIndex(e => e.ValueName)
                    .IsUnique();

                entity.Property(e => e.ValueName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("ValueName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
