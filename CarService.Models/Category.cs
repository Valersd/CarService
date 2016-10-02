using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class Category
    {
        private ICollection<ReplacementPart> _replacementParts;

        public Category()
        {
            _replacementParts = new HashSet<ReplacementPart>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Index(IsUnique=true)]
        public string Name { get; set; }

        public virtual ICollection<ReplacementPart> ReplacementParts
        {
            get { return _replacementParts; }
            set { _replacementParts = value; }
        }
    }
}
