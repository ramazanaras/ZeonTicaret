using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ZeonTicaret.WebUI.Models.Mapping
{
    public class aspnet_MembershipMap : EntityTypeConfiguration<aspnet_Membership>
    {
        public aspnet_MembershipMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PasswordSalt)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.MobilePIN)
                .HasMaxLength(16);

            this.Property(t => t.Email)
                .HasMaxLength(256);

            this.Property(t => t.LoweredEmail)
                .HasMaxLength(256);

            this.Property(t => t.PasswordQuestion)
                .HasMaxLength(256);

            this.Property(t => t.PasswordAnswer)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("aspnet_Membership");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.PasswordFormat).HasColumnName("PasswordFormat");
            this.Property(t => t.PasswordSalt).HasColumnName("PasswordSalt");
            this.Property(t => t.MobilePIN).HasColumnName("MobilePIN");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.LoweredEmail).HasColumnName("LoweredEmail");
            this.Property(t => t.PasswordQuestion).HasColumnName("PasswordQuestion");
            this.Property(t => t.PasswordAnswer).HasColumnName("PasswordAnswer");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.IsLockedOut).HasColumnName("IsLockedOut");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastPasswordChangedDate).HasColumnName("LastPasswordChangedDate");
            this.Property(t => t.LastLockoutDate).HasColumnName("LastLockoutDate");
            this.Property(t => t.FailedPasswordAttemptCount).HasColumnName("FailedPasswordAttemptCount");
            this.Property(t => t.FailedPasswordAttemptWindowStart).HasColumnName("FailedPasswordAttemptWindowStart");
            this.Property(t => t.FailedPasswordAnswerAttemptCount).HasColumnName("FailedPasswordAnswerAttemptCount");
            this.Property(t => t.FailedPasswordAnswerAttemptWindowStart).HasColumnName("FailedPasswordAnswerAttemptWindowStart");
            this.Property(t => t.Comment).HasColumnName("Comment");

            // Relationships
            this.HasRequired(t => t.aspnet_Applications)
                .WithMany(t => t.aspnet_Membership)
                .HasForeignKey(d => d.ApplicationId);
            this.HasRequired(t => t.aspnet_Users)
                .WithOptional(t => t.aspnet_Membership);

        }
    }
}
