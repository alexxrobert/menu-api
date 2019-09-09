using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storefront.Menu.API.Models.DataModel.OptionGroups
{
    public static class OptionGroupMap
    {
        public static void Configure(this EntityTypeBuilder<OptionGroup> optionGroup)
        {
            optionGroup.ToTable("option_group");

            optionGroup.HasKey(p => new
            {
                p.TenantId,
                p.Id
            })
            .HasName("pk_option_group");

            optionGroup.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            optionGroup.Property(p => p.TenantId)
                .HasColumnName("tenant_id");

            optionGroup.Property(p => p.ItemGroupId)
                .HasColumnName("item_group_id")
                .IsRequired();

            optionGroup.Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(50);

            optionGroup.Property(p => p.IsMultichoice)
                .HasColumnName("is_multichoice")
                .IsRequired();

            optionGroup.Property(p => p.IsRequired)
                .HasColumnName("is_required")
                .IsRequired();

            optionGroup.HasMany(p => p.Options)
                .WithOne(p => p.OptionGroup)
                .HasForeignKey(p => new
                {
                    p.TenantId,
                    p.OptionGroupId
                })
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_option_group__option");
        }
    }
}
