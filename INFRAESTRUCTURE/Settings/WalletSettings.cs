using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DOMAIN.Wallets;

namespace INFRAESTRUCTURE.Settings
{
    internal sealed class WalletSettings : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            ConfigureWallet(builder);
        }

        private static void ConfigureWallet(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Id)
                .ValueGeneratedOnAdd(); 

            builder.Property(w => w.DocumentId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(w => w.Balance)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(w => w.CreatedAt)
                .IsRequired();

            builder.Property(w => w.UpdatedAt)
                .IsRequired(false); 

            builder.HasMany(w => w.Movements)
                .WithOne()
                .HasForeignKey("WalletId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
