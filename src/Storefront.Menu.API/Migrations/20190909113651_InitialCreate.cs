using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Storefront.Menu.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "menu");

            migrationBuilder.CreateTable(
                name: "item_group",
                schema: "menu",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<long>(nullable: false),
                    title = table.Column<string>(maxLength: 50, nullable: false),
                    picture_file_id = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item_group", x => new { x.tenant_id, x.id });
                });

            migrationBuilder.CreateTable(
                name: "item",
                schema: "menu",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<long>(nullable: false),
                    item_group_id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    description = table.Column<string>(maxLength: 255, nullable: true),
                    picture_file_id = table.Column<string>(maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    is_available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_item", x => new { x.tenant_id, x.id });
                    table.ForeignKey(
                        name: "fk_item__item_group",
                        columns: x => new { x.tenant_id, x.item_group_id },
                        principalSchema: "menu",
                        principalTable: "item_group",
                        principalColumns: new[] { "tenant_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "option_group",
                schema: "menu",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<long>(nullable: false),
                    item_group_id = table.Column<long>(nullable: false),
                    title = table.Column<string>(maxLength: 50, nullable: true),
                    is_multichoice = table.Column<bool>(nullable: false),
                    is_required = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_option_group", x => new { x.tenant_id, x.id });
                    table.ForeignKey(
                        name: "fk_option_group__item_group",
                        columns: x => new { x.tenant_id, x.item_group_id },
                        principalSchema: "menu",
                        principalTable: "item_group",
                        principalColumns: new[] { "tenant_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "option",
                schema: "menu",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tenant_id = table.Column<long>(nullable: false),
                    option_group_id = table.Column<long>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    is_available = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_option", x => new { x.tenant_id, x.id });
                    table.ForeignKey(
                        name: "fk_option_group__option",
                        columns: x => new { x.tenant_id, x.option_group_id },
                        principalSchema: "menu",
                        principalTable: "option_group",
                        principalColumns: new[] { "tenant_id", "id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_item_tenant_id_item_group_id",
                schema: "menu",
                table: "item",
                columns: new[] { "tenant_id", "item_group_id" });

            migrationBuilder.CreateIndex(
                name: "ix_option_tenant_id_option_group_id",
                schema: "menu",
                table: "option",
                columns: new[] { "tenant_id", "option_group_id" });

            migrationBuilder.CreateIndex(
                name: "ix_option_group_tenant_id_item_group_id",
                schema: "menu",
                table: "option_group",
                columns: new[] { "tenant_id", "item_group_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "item",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "option",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "option_group",
                schema: "menu");

            migrationBuilder.DropTable(
                name: "item_group",
                schema: "menu");
        }
    }
}
