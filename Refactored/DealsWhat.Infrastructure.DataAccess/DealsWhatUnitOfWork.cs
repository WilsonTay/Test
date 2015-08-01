using DealsWhat.Domain.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DealsWhat.Infrastructure.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DealsWhatUnitOfWork : IdentityDbContext<ApplicationUser>, IUnitOfWork
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DealsWhat.Infrastructure.DataAccess.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public DealsWhatUnitOfWork()
            : base("name=Model1", false)
        {
            Database.SetInitializer<DealsWhatUnitOfWork>(new CreateDatabaseIfNotExists<DealsWhatUnitOfWork>());

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>().Ignore(a => a.Id);

            // modelBuilder.Entity<ApplicationUser>().Ignore(a => a.ContactAddress);
            // modelBuilder.Entity<ApplicationUser>().Ignore(a => a.Email);

            modelBuilder.Entity<CartItemModel>().HasKey(a => a.Key)
               .HasMany<DealAttributeModel>(a => a.AttributeValues)
               .WithMany();

            modelBuilder.Entity<ApplicationUser>().HasKey(a => a.Key)
                .HasMany<CartItemModel>(a => a.CartItems)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<OrderModel>(a => a.Orders)
                .WithRequired();

            modelBuilder.Entity<OrderModel>().HasKey(a => a.Key)
                .HasMany<OrderlineModel>(a => a.Orderlines)
                .WithRequired();

            modelBuilder.Entity<OrderModel>().HasOptional(a => a.BillingAddress);

            modelBuilder.Entity<OrderlineModel>().HasKey(a => a.Key)
                 .HasMany<DealAttributeModel>(a => a.AttributeValues)
                 .WithMany(); ;

            modelBuilder.Entity<ApplicationUser>().HasKey(a => a.Key)
                .HasOptional<AddressModel>(a => a.ContactAddress);

            modelBuilder.Entity<ApplicationUser>().HasKey(a => a.Key)
                .HasOptional<AddressModel>(a => a.BillingAddress);

            modelBuilder.Entity<DealCategoryModel>().HasKey(a => a.Key)
                .HasMany<DealModel>(a => a.Deals).WithRequired();


            modelBuilder.Entity<DealModel>().HasKey(a => a.Key)
                .HasMany<DealOptionModel>(a => a.Options).WithRequired();

            modelBuilder.Entity<DealModel>()
                .HasMany<DealImageModel>(a => a.Images).WithRequired();

            modelBuilder.Entity<DealOptionModel>().HasKey(a => a.Key)
          .HasMany<DealAttributeModel>(a => a.Attributes).WithRequired();

            modelBuilder.Entity<DealAttributeModel>().HasKey(a => a.Key);

            modelBuilder.Entity<DealImageModel>().HasKey(a => a.Key);

            modelBuilder.Entity<AddressModel>().HasKey(a => a.Key);

            //modelBuilder.Entity<ApplicationUser>().HasKey(a => a.Key);
            //modelBuilder.Entity<Cart>()
            //       .HasMany<DealAttribute>(b => b.DealAttributes)
            //       .WithMany(r => r.Carts)
            //       .Map(cs =>
            //       {
            //           cs.ToTable("CartDealAttributes");
            //       });
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<DealModel> Deals { get; set; }
        public DbSet<DealCategoryModel> DealCategories { get; set; }
        //public DbSet<Merchant> Merchants { get; set; }
        public DbSet<DealImageModel> DealImages { get; set; }
        //public DbSet<DealComment> DealComments { get; set; }
        public DbSet<CartItemModel> Carts { get; set; }
        public DbSet<DealOptionModel> DealOptions { get; set; }
        public DbSet<DealAttributeModel> DealAttributes { get; set; }

        public DbSet<AddressModel> Addresses { get; set; }

        public DbSet<OrderModel> Orders { get; set; }

        public DbSet<OrderlineModel> Orderlines { get; set; }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            base.Entry<TEntity>(entity).State = EntityState.Modified;
            base.Set<TEntity>().Attach(entity);

        }
        public static DealsWhatUnitOfWork Create()
        {
            return new DealsWhatUnitOfWork();
        }

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}