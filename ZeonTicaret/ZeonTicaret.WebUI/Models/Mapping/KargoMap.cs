using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ZeonTicaret.WebUI.Models.Mapping
{
    public class KargoMap : EntityTypeConfiguration<Kargo>
    {
        public KargoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SirketAdi)
                .HasMaxLength(150);

            this.Property(t => t.Adres)
                .HasMaxLength(500);

            this.Property(t => t.Telefon)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.WebSite)
                .HasMaxLength(50);

            this.Property(t => t.EMail)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Kargo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SirketAdi).HasColumnName("SirketAdi");
            this.Property(t => t.Adres).HasColumnName("Adres");
            this.Property(t => t.Telefon).HasColumnName("Telefon");
            this.Property(t => t.WebSite).HasColumnName("WebSite");
            this.Property(t => t.EMail).HasColumnName("EMail");
        }
    }
}
