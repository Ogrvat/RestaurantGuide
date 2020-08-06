using Microsoft.EntityFrameworkCore;

namespace RestaurantGuide.Models
{
    /// <summary>
    /// app context
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Коллекция городов
        /// </summary>
        public DbSet<City> Cities { get; set; }
        /// <summary>
        /// Коллекция ресторанов
        /// </summary>
        public DbSet<Restaurant> Restaurants { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();   
        }
    }
}
