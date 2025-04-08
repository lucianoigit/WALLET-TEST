using DOMAIN.Movements;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DOMAIN.Wallets;

namespace INFRAESTRUCTURE.Settings;

internal sealed class MovementSettings : IEntityTypeConfiguration<Movement>
{
    public void Configure(EntityTypeBuilder<Movement> builder)
    {
        builder.ToTable("Movements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd(); // ID autoincremental

        builder.Property(m => m.WalletId)
            .IsRequired();

        builder.Property(m => m.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.Type)
            .IsRequired()
            .HasConversion<string>(); // Enum como string

        builder.Property(m => m.Date)
            .IsRequired();

        builder.HasOne<Wallet>() // relación uno a muchos
            .WithMany(w => w.Movements)
            .HasForeignKey(m => m.WalletId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
