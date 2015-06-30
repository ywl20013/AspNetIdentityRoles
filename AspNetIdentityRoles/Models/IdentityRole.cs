using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AspNetIdentityRoles.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }


        public ApplicationRole(string name, string category, string description)
            : base(name)
        {
            this.Category = category;
            this.Description = description;
        }

        [Display(Name = "类别")]
        public string Category { get; set; }

        [Display(Name = "描述")]
        public virtual string Description { get; set; }
    }
}