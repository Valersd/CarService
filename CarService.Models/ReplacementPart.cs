using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class ReplacementPart
    {
        private ICollection<DocumentsParts> _documentsParts;
        public ReplacementPart()
        {
            _documentsParts = new HashSet<DocumentsParts>();
            IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Index(IsUnique = true)]
        public string CatalogNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<DocumentsParts> DocumentsParts
        {
            get { return _documentsParts; }
            set { _documentsParts = value; }
        }
    }
}
