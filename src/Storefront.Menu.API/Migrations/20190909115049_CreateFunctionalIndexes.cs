using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using Storefront.Menu.API.Models.DataModel;

namespace Storefront.Menu.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class CreateFunctionalIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                migrationBuilder.Sql($@"
                    CREATE INDEX idx_item_group_name
                    ON {ApiDbContext.Schema}.item_group (public.ci_ai(title));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_item_name
                    ON {ApiDbContext.Schema}.item (public.ci_ai(name));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_option_group_name
                    ON {ApiDbContext.Schema}.option_group (public.ci_ai(title));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_option_name
                    ON {ApiDbContext.Schema}.option (public.ci_ai(name));
                ");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_item_group_name;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_item_name;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_option_group_name;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_option_name;");
            }
        }
    }
}
