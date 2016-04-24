using System;
using System.ComponentModel.DataAnnotations;

namespace Dewey.Temporal.Annotations
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public string NotBefore { get; set; }
        public string NotAfter { get; set; }

        public DateRangeAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTime = value as DateTime?;
            if (dateTime == null) {
                return new ValidationResult(validationContext.MemberName + " is not a DateTime object.");
            }

            if (NotBefore != null) {
                var beforePinfo = validationContext.ObjectInstance.GetType().GetProperty(NotBefore);
                var beforeDateTime = (DateTime?)beforePinfo.GetValue(validationContext.ObjectInstance);

                if (beforeDateTime == null || beforeDateTime == DateTime.MinValue) {
                    return null;
                }

                if (dateTime <= beforeDateTime) {
                    return new ValidationResult(validationContext.DisplayName + " cannot be set before " + NotBefore);
                }

                return null;
            }

            if (NotAfter != null) {
                var afterPinfo = validationContext.ObjectInstance.GetType().GetProperty(NotAfter);
                var afterDateTime = (DateTime?)afterPinfo.GetValue(validationContext.ObjectInstance);

                if (afterDateTime == null || afterDateTime == DateTime.MinValue) {
                    return null;
                }

                if (dateTime >= afterDateTime) {
                    return new ValidationResult(validationContext.DisplayName + " cannot be set after " + NotAfter);
                }

                return null;
            }

            return null;
        }
    }
}