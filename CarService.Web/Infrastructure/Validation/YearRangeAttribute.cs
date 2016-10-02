using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarService.Web.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class YearRangeAttribute:ValidationAttribute, IClientValidatable
    {
        private const string ErrorMessageCustom = "{0} should be between {1} and {2}";
        public YearRangeAttribute(int year)
            : base(ErrorMessageCustom)
        {
            this.ErrorMessage = ErrorMessageCustom;
            this.Year = year;
        }

        public int Year { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            int year;
            if (!int.TryParse(value.ToString(), out year))
            {
                return new ValidationResult(validationContext.DisplayName + " is not a number");
            }
            if (year >= this.Year && year <= DateTime.Now.Year)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessage, name, this.Year, DateTime.Now.Year);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "yearrange"
            };
            rule.ValidationParameters.Add("min", this.Year);
            rule.ValidationParameters.Add("max", DateTime.Now.Year);
            yield return rule;
        }
    }
}