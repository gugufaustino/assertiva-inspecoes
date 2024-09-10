using Differencial.Domain.Exceptions;
using System.Collections.Generic;

namespace Differencial.Domain.Validation
{
    public class ValidationResultsManager
    {
        private List<ValidationResult> ValidationResults { get; set; }

        public bool HasError { get { return ValidationResults.Count > 0; } }

        public ValidationResultsManager()
        {
            this.ValidationResults = new List<ValidationResult>();
        }

        public virtual void ThrowException()
        {
            ThrowException(string.Empty);
        }
        public virtual void ThrowException(string validationMessageHeader)
        {
            if (this.ValidationResults.Count == 0)
                return;

            string validationMessage = validationMessageHeader;

            List<ValidationResult> errors = new List<ValidationResult>();

            foreach (ValidationResult vr in this.ValidationResults)
            {
                if (vr.IsValid)
                    continue;

                errors.Add(vr);

                if (validationMessage.Length > 0)
                    validationMessage += "\n";

                if (!string.IsNullOrEmpty(vr.EntityName))
                    validationMessage += vr.EntityName + " - ";
                if (!string.IsNullOrEmpty(vr.PropertyName))
                    validationMessage += vr.PropertyName + " - ";

                validationMessage += vr.ErrorMessage;
            }

            if (errors.Count > 0)
                throw new ValidationException(validationMessage, errors);
        }

        public virtual void AddValidationResultNotValid(string errorMessage, string entityName = "", string propertyName = "")
        {
            this.ValidationResults.Add(new ValidationResult(false, errorMessage, entityName, propertyName));
        }

        public virtual void ClearValidationResults()
        {
            this.ValidationResults = new List<ValidationResult>();
        }
    }
}