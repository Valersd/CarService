using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class Car
    {
        private ICollection<RepairDocument> _repairDocuments;

        public Car()
        {
            _repairDocuments = new HashSet<RepairDocument>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Index(IsUnique=true)]
        public string RegNumber { get; set; }

        [Required]
        [MaxLength(20)]
        [Index(IsUnique=true)]
        public string ChassisNumber { get; set; }

        [Required]
        [MaxLength(20)]
        [Index(IsUnique=true)]
        public string EngineNumber { get; set; }

        [MaxLength(20)]
        public string Vendor { get; set; }

        [MaxLength(20)]
        public string Model { get; set; }

        //[RegularExpression(@"^\[0-9]{4}$")]
        public int? Year { get; set; }

        [MaxLength(20)]
        public string Color { get; set; }

        //[RegularExpression(@"^[0-9]{2,4}$")]
        public int? EngineCapacity { get; set; }

        [MaxLength(3000)]
        public string Description { get; set; }

        [MaxLength(30)]
        public string OwnerName { get; set; }

        //[RegularExpression(@"^[0-9]{4,15}$")]
        [MaxLength(15)]
        public string OwnerPhone { get; set; }

        public virtual ICollection<RepairDocument> RepairDocuments
        {
            get { return _repairDocuments; }
            set { _repairDocuments = value; }
        }
    }
}
