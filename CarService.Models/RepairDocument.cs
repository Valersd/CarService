using System;
using System.Linq;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace CarService.Models
{
    public class RepairDocument
    {
        private ICollection<DocumentsParts> _documentsParts;

        public RepairDocument()
        {
            _documentsParts = new HashSet<DocumentsParts>();
            //CreatedOn = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime? FinishedOn { get; set; }

        [MaxLength(3000)]
        public string RepairDescription { get; set; }

        public decimal? TotalPrice { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; }

        public string CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public string MechanicId { get; set; }

        public virtual User Mechanic { get; set; }

        public virtual ICollection<DocumentsParts> DocumentsParts
        {
            get { return _documentsParts; }
            set { _documentsParts = value; }
        }

    }
}
