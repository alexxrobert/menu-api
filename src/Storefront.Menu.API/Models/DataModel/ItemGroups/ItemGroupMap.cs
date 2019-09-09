using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storefront.Menu.API.Models.DataModel.ItemGroups
{
    public static class ItemGroupMap
    {
        public static void Configure(this EntityTypeBuilder<ItemGroup> ItemGroup)
        {
            ItemGroup.ToTable("item_group");

            ItemGroup.HasKey(p => new
            {
                p.TenantId,
                p.Id
            })
            .HasName("pk_item_group");

            ItemGroup.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            ItemGroup.Property(p => p.TenantId)
                .HasColumnName("tenant_id");

            ItemGroup.Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(50)
                .IsRequired();

            ItemGroup.Property(p => p.PictureFileId)
                .HasColumnName("picture_file_id")
                .HasMaxLength(50);

            ItemGroup.HasMany(p => p.Items)
                .WithOne(p => p.ItemGroup)
                .HasForeignKey(p => new
                {
                    p.TenantId,
                    p.ItemGroupId
                })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_item__item_group");

            ItemGroup.HasMany(p => p.OptionGroups)
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
