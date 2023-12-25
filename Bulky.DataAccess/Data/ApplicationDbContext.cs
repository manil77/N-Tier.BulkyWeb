using Bulky.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = 1,
                     Title = "Game of Thrones",
                     Description = "Book about war, politics and fantasy",
                     ISBN = "HGAVBJAUGS75H",
                     Author = "George R. Martin",
                     ListPrice = 1000,
                     Price50 = 950,
                     Price100 = 900,
                     CategoryId=1,
                     ImageUrl=""
                 },

                new Product
                {
                    Id = 2,
                    Title = "How I Met Your Mother",
                    Description = "Romantic slice of life story a guy named Ted Mosby",
                    ISBN = "GHAUXGMAK875",
                    Author = "Manil Maharjan",
                    ListPrice = 500,
                    Price50 = 450,
                    Price100 = 400,
                    CategoryId = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "House of Dragon",
                    Description = "Strories of Pre events of Game of thrones",
                    ISBN = "HJAHJX1237AK",
                    Author = "George R. Martin",
                    ListPrice = 700,
                    Price50 = 650,
                    Price100 = 600,
                    CategoryId = 3,
                    ImageUrl = ""
                }
                );
        }
    }
}