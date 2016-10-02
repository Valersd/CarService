using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarService.Models
{
    public class User : IdentityUser
    {
        private ICollection<RepairDocument> _createdRepairDocuments;
        private ICollection<RepairDocument> _servedRepairDocuments;

        public User()
        {
            _createdRepairDocuments = new HashSet<RepairDocument>();
            _servedRepairDocuments = new HashSet<RepairDocument>();
        }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        //[Required]
        [MaxLength(15)]
        public override string PhoneNumber { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        public virtual ICollection<RepairDocument> CreatedRepairDocuments
        {
            get { return _createdRepairDocuments; }
            set { _createdRepairDocuments = value; }
        }

        public virtual ICollection<RepairDocument> ServedRepairDocuments
        {
            get { return _servedRepairDocuments; }
            set { _servedRepairDocuments = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
