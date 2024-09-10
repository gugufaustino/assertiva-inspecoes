using Differencial.Domain.Contracts.Validation;

namespace Differencial.Domain.Validation
{
    public class ServicesValidation : IServicesValidation
    {

        protected ValidationResultsManager ValidationResultsManager { get; set; }

        public ServicesValidation()
        {
            this.ValidationResultsManager = new ValidationResultsManager();
        }

        

        protected virtual void ThrowBusinessError()
        {
            this.ValidationResultsManager.ThrowException();
        }

        public virtual void AddValidationResultNotValid(string errorMessage, string propertyName)
        {
            this.ValidationResultsManager.AddValidationResultNotValid(errorMessage, propertyName: propertyName);
        }

        protected virtual void ClearValidationResults()
        {
            this.ValidationResultsManager.ClearValidationResults();
        }

        
    }
}