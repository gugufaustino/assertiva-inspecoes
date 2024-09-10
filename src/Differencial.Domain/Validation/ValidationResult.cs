namespace Differencial.Domain.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }
        public string EntityName { get; private set; }

        public ValidationResult(bool isValid, string errorMessage,
            string entityName, string propertyName)
        {
            this.IsValid = isValid;
            this.ErrorMessage = errorMessage;
            this.PropertyName = propertyName;
            this.EntityName = entityName;
        }
         
    }
}