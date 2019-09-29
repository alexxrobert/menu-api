using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StorefrontCommunity.Menu.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class CreateIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_option_group_title",
                schema: "menu",
                table: "option_group",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "idx_option_name",
                schema: "menu",
                table: "option",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_item_group_title",
                schema: "menu",
                table: "item_group",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "idx_item_name",
                schema: "menu",
                table: "item",
                column: "name");

            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                migrationBuilder.Sql($@"
                    CREATE INDEX idx_item_group_name_ci_ai
                    ON menu.item_group (public.ci_ai(title));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_item_name_ci_ai
                    ON menu.item (public.ci_ai(name));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_option_group_name_ci_ai
                    ON menu.option_group (public.ci_ai(title));
                ");

                migrationBuilder.Sql($@"
                    CREATE INDEX idx_option_name_ci_ai
                    ON menu.option (public.ci_ai(name));
                ");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_option_group_title",
                schema: "menu",
                table: "option_group");

            migrationBuilder.DropIndex(
                name: "idx_option_name",
                schema: "menu",
                table: "option");

            migrationBuilder.DropIndex(
                name: "idx_item_group_title",
                schema: "menu",
                table: "item_group");

            migrationBuilder.DropIndex(
                name: "idx_item_name",
                schema: "menu",
                table: "item");

            if (migrationBuilder.ActiveProvider == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_item_group_name_ci_ai;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_item_name_ci_ai;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_option_group_name_ci_ai;");
                migrationBuilder.Sql(@"DROP INDEX IF EXISTS idx_option_name_ci_ai;");
            }
        }
    }
}
