using OnlineStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
    public class OnlineStoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        public OnlineStoreContext()
           : base("OnlineStoreDbContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)//category zorunludur
                .WithMany(c => c.Products)//product nesnesinin içindeki category zorunludur 1-* ilişkisi
                .HasForeignKey(p => p.CategoryId);//forenkey

            modelBuilder.Entity<User>()//many to many relation
                .HasMany(u => u.AllowedCategories)
                .WithMany(c => c.AssignedUsers)
                .Map(mapping =>
                {
                    mapping.MapLeftKey("UserId");
                    mapping.MapRightKey("CategoryId");
                    mapping.ToTable("UserCategory");
                });
        }
    }
}
