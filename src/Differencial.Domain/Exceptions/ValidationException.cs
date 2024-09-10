using Differencial.Domain.Validation;
using System;
using System.Collections.Generic;

namespace Differencial.Domain.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public List<ValidationResult> ValidationResults { get; private set; }

        public ValidationException(string message, List<ValidationResult> listValidationResults)
            : base(message)
        {
            this.ValidationResults = listValidationResults;
        }

        public ValidationException(string message)
            : base(message)
        {
            this.ValidationResults = new List<ValidationResult>();
        }
    }
}