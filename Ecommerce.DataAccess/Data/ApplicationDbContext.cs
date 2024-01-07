using Ecommerce.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) { }
        public DbSet<Users> Userss { get; set; }
        public DbSet<UserPersonalAddresse> UserPersonalAddresses { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<SubCategory> SubCategorys { get; set; }
        public DbSet<ThirdCategory> ThirdCategorys { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSpecs> ProductSpecss { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ProductWishlist> ProductWishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderShipping> OrderShippings { get; set; }

    }
}
