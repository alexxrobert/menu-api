using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storefront.Menu.API.Models.DataModel.ItemGroups
{
    public static class ItemGroupMap
    {
        public static void Configure(this EntityTypeBuilder<ItemGroup> itemGroup)
        {
            itemGroup.ToTable("item_group");

            itemGroup.HasKey(p => new
            {
                p.TenantId,
                p.Id
            })
            .HasName("pk_item_group");

            itemGroup.HasIndex(p => p.Title)
                .HasName("idx_item_group_title");

            itemGroup.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            itemGroup.Property(p => p.TenantId)
                .HasColumnName("tenant_id");

            itemGroup.Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(50)
                .IsRequired();

            itemGroup.Property(p => p.PictureFileId)
                .HasColumnName("picture_file_id")
                .HasMaxLength(50);

            itemGroup.HasMany(p => p.Items)
                .WithOne(p => p.ItemGroup)
                .HasForeignKey(p => new
                {
                    p.TenantId,
                    p.ItemGroupId
                })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_item__item_group");

            itemGroup.HasMany(p => p.OptionGroups)
                .WithOne(p => p.ItemGroup)
                .HasForeignKey(p => new{
                    p.TenantId,
                    p.ItemGroupId
                })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_option_group__item_group");
        }
    }
}
