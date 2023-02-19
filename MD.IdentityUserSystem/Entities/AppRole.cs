using Microsoft.AspNetCore.Identity;
using System;

namespace MD.IdentityUserSystem.Entities
{
    public class AppRole:IdentityRole<int>
    {
        public DateTime CreatedDate { get; set; }
    }
}
