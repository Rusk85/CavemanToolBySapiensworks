using System;
using CavemanTools.Model.Validation;

namespace CavemanTools.Model
{
    /// <summary>
    /// This exception is used to set errors to ModelState
    /// </summary>
    public class ValidationErrorsException:Exception
    {
        public ValidationErrorsException(string key,string message)
        {
            Errors= new DefaultValidationWrapper();
            Errors.AddError(key,message);
        }
        public ValidationErrorsException():this(new DefaultValidationWrapper())
        {
            
        }
        public ValidationErrorsException(IValidationDictionary errors)
        {
            if (errors == null) throw new ArgumentNullException("errors");
            Errors = errors;
        }

        /// <summary>
        /// Gets errors dictionary
        /// </summary>
        public IValidationDictionary Errors { get; private set; }
    }
}