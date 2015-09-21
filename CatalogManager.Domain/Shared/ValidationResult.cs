using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatalogManager.Domain
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; private set; } 
        public ValidationResult()
        {
            Errors = new List<string>();
        }
        public void AddError(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Errors.Add(message);
            }
        }
        public static ValidationResult operator +(ValidationResult first, ValidationResult second)
        {
            var result = new ValidationResult { IsValid = first.IsValid && second.IsValid };
            result.Errors.AddRange(first.Errors);
            result.Errors.AddRange(second.Errors);
            return result;
        }
    }
}
