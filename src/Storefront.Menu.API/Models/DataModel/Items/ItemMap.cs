using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storefront.Menu.API.Models.DataModel.Items
{
    public static class ItemMap
    {
        public static void Configure(this EntityTypeBuilder<Item> item)
        {
            item.ToTable("item");

            item.HasKey(p => new
            {
                p.TenantId,
                p.Id
            })
            .HasName("pk_item");

            item.HasIndex(p => p.Name)
                .HasName("idx_item_name");

            item.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            item.Property(p => p.TenantId)
                .HasColumnName("tenant_id");

            item.Property(p => p.ItemGroupId)
                .HasColumnName("item_group_id")
                .IsRequired();

            item.Property(p => p.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            item.Property(p => p.IsAvailable)
                .HasColumnName("is_available")
                .IsRequired();

            item.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            item.Property(p => p.Description)
                .HasColumnName("description")
                .HasMaxLength(255);

            item.Property(p => p.PictureFileId)
                .HasColumnName("picture_file_id")
                .HasMaxLength(50);
        }
    }
}
