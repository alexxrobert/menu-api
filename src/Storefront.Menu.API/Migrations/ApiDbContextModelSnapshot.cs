﻿// <auto-generated />
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Storefront.Menu.API.Models.DataModel;

namespace Storefront.Menu.API.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("menu")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.ItemGroups.ItemGroup", b =>
                {
                    b.Property<long>("TenantId")
                        .HasColumnName("tenant_id");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("PictureFileId")
                        .HasColumnName("picture_file_id")
                        .HasMaxLength(50);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasMaxLength(50);

                    b.HasKey("TenantId", "Id")
                        .HasName("pk_item_group");

                    b.HasIndex("Title")
                        .HasName("idx_item_group_title");

                    b.ToTable("item_group");
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.Items.Item", b =>
                {
                    b.Property<long>("TenantId")
                        .HasColumnName("tenant_id");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasMaxLength(255);

                    b.Property<bool>("IsAvailable")
                        .HasColumnName("is_available");

                    b.Property<long>("ItemGroupId")
                        .HasColumnName("item_group_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(50);

                    b.Property<string>("PictureFileId")
                        .HasColumnName("picture_file_id")
                        .HasMaxLength(50);

                    b.Property<decimal>("Price")
                        .HasColumnName("price")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("TenantId", "Id")
                        .HasName("pk_item");

                    b.HasIndex("Name")
                        .HasName("idx_item_name");

                    b.HasIndex("TenantId", "ItemGroupId");

                    b.ToTable("item");
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.OptionGroups.OptionGroup", b =>
                {
                    b.Property<long>("TenantId")
                        .HasColumnName("tenant_id");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("IsMultichoice")
                        .HasColumnName("is_multichoice");

                    b.Property<bool>("IsRequired")
                        .HasColumnName("is_required");

                    b.Property<long>("ItemGroupId")
                        .HasColumnName("item_group_id");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasMaxLength(50);

                    b.HasKey("TenantId", "Id")
                        .HasName("pk_option_group");

                    b.HasIndex("Title")
                        .HasName("idx_option_group_title");

                    b.HasIndex("TenantId", "ItemGroupId");

                    b.ToTable("option_group");
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.Options.Option", b =>
                {
                    b.Property<long>("TenantId")
                        .HasColumnName("tenant_id");

                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Description");

                    b.Property<bool>("IsAvailable")
                        .HasColumnName("is_available");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(50);

                    b.Property<long>("OptionGroupId")
                        .HasColumnName("option_group_id");

                    b.Property<decimal>("Price")
                        .HasColumnName("price")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("TenantId", "Id")
                        .HasName("pk_option");

                    b.HasIndex("Name")
                        .HasName("idx_option_name");

                    b.HasIndex("TenantId", "OptionGroupId");

                    b.ToTable("option");
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.Items.Item", b =>
                {
                    b.HasOne("Storefront.Menu.API.Models.DataModel.ItemGroups.ItemGroup", "ItemGroup")
                        .WithMany("Items")
                        .HasForeignKey("TenantId", "ItemGroupId")
                        .HasConstraintName("fk_item__item_group")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.OptionGroups.OptionGroup", b =>
                {
                    b.HasOne("Storefront.Menu.API.Models.DataModel.ItemGroups.ItemGroup", "ItemGroup")
                        .WithMany("OptionGroups")
                        .HasForeignKey("TenantId", "ItemGroupId")
                        .HasConstraintName("fk_option_group__item_group")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Storefront.Menu.API.Models.DataModel.Options.Option", b =>
                {
                    b.HasOne("Storefront.Menu.API.Models.DataModel.OptionGroups.OptionGroup", "OptionGroup")
                        .WithMany("Options")
                        .HasForeignKey("TenantId", "OptionGroupId")
                        .HasConstraintName("fk_option_group__option")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
