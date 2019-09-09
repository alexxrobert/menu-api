using Microsoft.EntityFrameworkCore;
using Storefront.Menu.API.Models.DataModel.ItemGroups;
using Storefront.Menu.API.Models.DataModel.Items;
using Storefront.Menu.API.Models.DataModel.OptionGroups;
using Storefront.Menu.API.Models.DataModel.Options;

namespace Storefront.Menu.API.Models.DataModel
{
    public sealed class ApiDbContext : DbContext
    {
        public const string Schema = "menu";

        public ApiDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ItemGroup> ItemGroups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Option> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<ItemGroup>().Configure();
            modelBuilder.Entity<Item>().Configure();
            modelBuilder.Entity<OptionGroup>().Configure();
            modelBuilder.Entity<Option>().Configure();
        }

        [DbFunction("public.ci_ai")]
        public static string Normalize(string text) => text;
    }
}
