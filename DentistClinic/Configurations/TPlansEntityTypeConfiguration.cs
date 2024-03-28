using DentistClinic.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentistClinic.Configurations
{
    public class TPlansEntityTypeConfiguration : IEntityTypeConfiguration<Tplans>
    {
        public void Configure(EntityTypeBuilder<Tplans> builder)
        {

        }
    }
}
