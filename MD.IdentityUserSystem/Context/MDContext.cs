using MD.IdentityUserSystem.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MD.IdentityUserSystem.Context
{
    public class MDContext:IdentityDbContext<AppUser,AppRole,int>
    {
        public MDContext(DbContextOptions<MDContext> options):base(options)
        {

        }
    }
}
