using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace gp_backend.EF.MySql.Data.Configurations
{
    public class RoleDoctorConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var adminRole = new IdentityRole
            {
                Id = "ad4c4ecd-1e91-4180-be16-45df72e44436",
                Name = "Doc",
                NormalizedName = "DOC"
            };

            builder.HasData(adminRole);
        }
    }
}
