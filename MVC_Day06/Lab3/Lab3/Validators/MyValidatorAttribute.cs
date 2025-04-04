using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lab3.Validators
{
    public class MyValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        public int MinimumAge { get; set; }

        public MyValidatorAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;
        }

        public override bool IsValid(object value)
        {
            if (value is int age)
            {
                return age >= MinimumAge;
            }
            return false; // Invalid if value is not an integer
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be at least {MinimumAge} years old.";
        }

        // ➤ Adds client-side validation rules
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-myvalidator", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
            context.Attributes.Add("data-val-myvalidator-minage", MinimumAge.ToString());
        }
    }
}
