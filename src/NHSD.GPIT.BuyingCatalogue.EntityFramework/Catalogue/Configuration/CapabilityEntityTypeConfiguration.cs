﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Configuration
{
    internal sealed class CapabilityEntityTypeConfiguration : IEntityTypeConfiguration<Capability>
    {
        public void Configure(EntityTypeBuilder<Capability> builder)
        {
            builder.ToTable("Capability");

            builder.HasKey(c => c.Id)
                .IsClustered(false);

            builder.HasIndex(c => c.CapabilityRef, "IX_CapabilityCapabilityRef")
                .IsClustered();

            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.CapabilityRef)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(c => c.EffectiveDate).HasColumnType("date");
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.PreviousVersion).HasMaxLength(10);
            builder.Property(c => c.SourceUrl).HasMaxLength(1000);
            builder.Property(c => c.Status)
                .HasConversion<int>()
                .HasColumnName("StatusId");

            builder.Property(c => c.Version)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasOne(c => c.Category)
                .WithMany(cc => cc.Capabilities)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Capability_CapabilityCategory");
        }
    }
}