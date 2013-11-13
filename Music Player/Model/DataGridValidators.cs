using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Music_Player.Model
{
    public class StringTagValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            string testedValue = value as string;
            if (testedValue == null)
                return ValidationResult.ValidResult;
            if (testedValue.IndexOf('\"') > -1 || testedValue.IndexOf('\"') > -1 || testedValue.IndexOf('\"') > -1 || testedValue.IndexOf('\"') > -1)
            {
                return new ValidationResult(false,
                    "String tags cannot contain \" signs");
            }
            return ValidationResult.ValidResult;
        }
    }
    public class YearValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            int testedValue;
            if (value.ToString().Equals(""))
                value = "0";
            testedValue = Int32.Parse(value.ToString());
            if (testedValue > DateTime.Now.Year || testedValue<1900)
            {
                return new ValidationResult(false,
                    "Year tag has to be between 1900 and current year");
            }
            return ValidationResult.ValidResult;
        }
    }
    public class RangeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string ErrorMessage { get; set; }
        public override ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            int testedValue;
            if (value.ToString().Equals(""))
                value = "0";
            testedValue = Int32.Parse(value.ToString());
            if (testedValue > Max || testedValue < Min)
            {
                return new ValidationResult(false,
                    ErrorMessage);
            }
            return ValidationResult.ValidResult;
        }
    }
}
