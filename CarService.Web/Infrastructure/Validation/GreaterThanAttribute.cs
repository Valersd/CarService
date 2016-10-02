using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace CarService.Web.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    public class GreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        private const string ErrorMessageWithEquality = "{0} must be greater than or equal to the {1}";
        private const string ErrorMessageWithoutEquality = "{0} must be greater than the {1}";
        private bool _allowEquality;
        private string _otherPropertyDisplayName;

        public GreaterThanAttribute(string otherProperty, bool allowEquality)
            :base(ErrorMessageWithEquality)
        {
            this.OtherProperty = otherProperty;
            this.AllowEquality = allowEquality;
        }

        public string OtherProperty { get; set; }
        public bool AllowEquality
        {
            get { return _allowEquality; }
            set 
            {
                this._allowEquality = value;
                this.ErrorMessage = this.AllowEquality ? ErrorMessageWithEquality : ErrorMessageWithoutEquality;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessage, name, _otherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherProperty == null)
            {
                return new ValidationResult("There is no property " + this.OtherProperty);
            }
            _otherPropertyDisplayName = this.OtherProperty;
            var attributes = otherProperty.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes != null && attributes.Length == 1)
            {
                _otherPropertyDisplayName = ((DisplayAttribute)attributes[0]).Name.ToLower();
            }
            
            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance);

            if (otherPropertyValue == null)
            {
                return ValidationResult.Success;
            }

            decimal parseOther;
            decimal parseCurrent;

            if (!decimal.TryParse(value.ToString(), out parseCurrent))
            {
                return new ValidationResult(validationContext.DisplayName + " is not a number");
            }

            if (!decimal.TryParse(otherPropertyValue.ToString(), out parseOther))
            {
                return new ValidationResult(this.OtherProperty + " is not a number");
            }

            if (AllowEquality)
            {
                if (parseCurrent >= parseOther)
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                if (parseCurrent > parseOther)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            PropertyInfo otherProperty = metadata.ContainerType.GetProperty(this.OtherProperty);
            var otherPropertyDisplayAttribute = otherProperty.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            _otherPropertyDisplayName = this.OtherProperty.ToLower();
            if (otherPropertyDisplayAttribute != null)
            {
                _otherPropertyDisplayName = (otherPropertyDisplayAttribute as DisplayAttribute).Name.ToLower();
            }

            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationType = "pricegreaterthan";
            rule.ValidationParameters.Add("otherpropertyname", this.OtherProperty);
            rule.ValidationParameters.Add("allowequality", this.AllowEquality);
            yield return rule;
        }
    }
}