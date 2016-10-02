using CarService.Web.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.ViewModels
{
    public abstract class RepairDocumentBase
    {
        protected IList<PartInRepairDocumentDetails> _usedParts;
        protected IEnumerable<SelectListItem> _mechanic;
        protected ICollection<CategoryIndex> _categories;

        protected RepairDocumentBase()
        {
            _mechanic = new HashSet<SelectListItem>();
            _categories = new HashSet<CategoryIndex>();
            _usedParts = new List<PartInRepairDocumentDetails>();
        }


        [Required]
        [Display(Name = "Mechanic")]
        public virtual string MechanicId { get; set; }

        public virtual string CarRegNumber { get; set; }

        [Display(Name = "Repair description")]
        [StringLength(3000, ErrorMessage = "{0} should be no more than {1} symbols")]
        [DataType(DataType.MultilineText)]
        public virtual string RepairDescription { get; set; }

        [Display(Name = "Price of parts")]
        [UIHint("_DecimalReadonly")]
        public virtual decimal PartsPrice { get; set; }

        [Display(Name = "Total price")]
        [DataType(DataType.Currency)]
        [GreaterThan("PartsPrice", true)]
        [Range(0.01, 10000000, ErrorMessage="Price should be positive")]
        public virtual decimal? TotalPrice { get; set; }

        [Display(Name = "Current replacement parts")]
        public virtual IList<PartInRepairDocumentDetails> UsedParts
        {
            get { return _usedParts; }
            set { _usedParts = value; }
        }

        public virtual ICollection<CategoryIndex> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        public virtual IEnumerable<SelectListItem> Mechanics
        {
            get { return _mechanic; }
            set { _mechanic = value; }
        }
    }
}