using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace BarcodeDrugChecker.ValidationRules
{
    public class StringToInt : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string input = value.ToString();
            if (input.Length <= 0 || string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) 
            {
                return new ValidationResult(false, "Please Enter Patient HN.");
            }
            else
            {

                Regex regex = new Regex("[^0-9]+");
                if(!regex.IsMatch(input))
                    return new ValidationResult(true, null);
                else
                    return new ValidationResult(false, "Please enter a valid integer value.");
            }
            /*
            if (input.Length <= 0 || string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, "Please Enter Patient HN.");
            }
            else if (int.TryParse(input, out _))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Please enter a valid integer value.");
            }
            */
        }
    }
}
