using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarService.Models
{
    public class DocumentsParts
    {
        public DocumentsParts()
        {
            this.Quantity = 1;
        }

        [Key]
        [Column(Order=0)]
        public int DocumentId { get; set; }

        [Key]
        [Column(Order=1)]
        public int PartId { get; set; }

        public virtual RepairDocument Document { get; set; }

        public virtual ReplacementPart Part { get; set; }

        public int Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
