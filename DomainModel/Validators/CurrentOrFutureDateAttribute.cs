// <copyright file="CurrentOrFutureDateAttribute.cs" company="Transilvania University of Brasov">
// Copyright © 2024 Bosnea Marian-Daniel. All rights reserved.
// </copyright>

namespace DomainModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Validation attribute that ensures the date value is either the current date or a future date.
    /// </summary>
    /// <remarks>
    /// This attribute can be applied to a <see cref="DateTime"/> property to enforce that the date
    /// is not in the past. If the date is earlier than the current date and time, the validation will fail.
    /// </remarks>
    public class CurrentOrFutureDateAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value is valid, ensuring the date is not in the past.
        /// </summary>
        /// <param name="value">The value of the object being validated, which should be of type <see cref="DateTime"/>.</param>
        /// <returns>
        /// <c>true</c> if the date is valid (i.e., it is the current date or in the future); otherwise, <c>false</c>.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value is DateTime dateValue)
            {
                return dateValue >= DateTime.Now;
            }

            return false;
        }

        /// <summary>
        /// Formats the error message to be displayed when validation fails.
        /// </summary>
        /// <param name="name">The name of the property being validated.</param>
        /// <returns>
        /// A string representing the error message that states the value cannot be earlier than the current date.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            return $"The {name} cannot be earlier than the current date.";
        }
    }
}
