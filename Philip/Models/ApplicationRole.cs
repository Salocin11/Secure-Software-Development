using System;
using Microsoft.AspNetCore.Identity;
namespace Philip.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}