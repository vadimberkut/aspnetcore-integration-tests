using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserProfileManager.Entity.ValidationAttributes
{
    class OnlyLettersAttribute : ValidationAttribute
    {
        public OnlyLettersAttribute()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            if(!(value is string))
            {
                throw new ArgumentException("Can be applied only to string properties!");
            }
            var regex = new Regex("^[a-zA-Z ]*$");
            if (regex.IsMatch(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(this.GetErrorMessage(validationContext));
            }
        }

        private string GetErrorMessage(ValidationContext validationContext)
        {
            if (!String.IsNullOrEmpty(this.ErrorMessage))
            {
                return this.ErrorMessage;
            }
            return $"{validationContext.DisplayName} should consist of: A-Za-z and space";
        }
    }
}
