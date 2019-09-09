using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Storefront.Menu.API.Models.DataModel.Options
{
    public static class OptionMap
    {
        public static void Configure(this EntityTypeBuilder<Option> option)
        {
            option.ToTable("option");

            option.HasKey(p => new
            {
                p.TenantId,
                p.Id
            })
            .HasName("pk_option");

            option.Property(p => p.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            option.Property(p => p.TenantId)
                .HasColumnName("tenant_id");

            option.Property(p => p.OptionGroupId)
                .HasColumnName("option_group_id")
                .IsRequired();

            option.Property(p => p.Name)
                .HasColumnName("name")
                .HasMaxLength(50)
                .IsRequired();

            option.Property(p => p.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            option.Property(p => p.IsAvailable)
                .HasColumnName("is_available")
                .IsRequired();
        }
    }
}
