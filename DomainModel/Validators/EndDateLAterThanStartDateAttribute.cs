// <copyright file="EndDateLAterThanStartDateAttribute.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>
namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validation attribute that ensures the end date is later than the start date.
    /// </summary>
    public class EndDateLaterThanStartDateAttribute : ValidationAttribute
    {
        private readonly string startDatePropertyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndDateLaterThanStartDateAttribute"/> class.
        /// </summary>
        /// <param name="startDatePropertyName">The name of the start date property to compare against.</param>
        public EndDateLaterThanStartDateAttribute(string startDatePropertyName)
        {
            this.startDatePropertyName = startDatePropertyName;
        }

        /// <summary>
        /// Validates whether the end date is later than the start date.
        /// </summary>
        /// <param name="value">The value of the end date being validated.</param>
        /// <param name="validationContext">The context in which the validation is performed.</param>
        /// <returns>
        /// <see cref="ValidationResult"/> indicating success if the end date is valid, otherwise an error message.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(this.startDatePropertyName);
            if (startDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {this.startDatePropertyName}");
            }

            var startDateValue = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance);

            if (value is DateTime endDateValue)
            {
                if (endDateValue > startDateValue)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("End date must be later than start date.");
                }
            }

            return new ValidationResult("Invalid date value.");
        }
    }
}