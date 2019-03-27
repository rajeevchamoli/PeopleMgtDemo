using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace PeopleMgt
{
    public class Utility
    {
        public static object ExtractValidationErrorMsg(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errors = modelState.Values.Where(val => val.ValidationState == ModelValidationState.Invalid).
                    SelectMany(err => err.Errors.Select(e => e.ErrorMessage)).ToList();
                errors.Insert(0, MessageConstants.InValidModelStateErrorMsg);

                return errors;
            }

            return string.Empty;
        }

        public static string NormalizeSortPropertyNameForUserEntity(string columnName)
        {
            // defualt property
            string propertyName = "FirstName";

            switch (columnName.ToLower())
            {
                case "lastname":
                    propertyName = "LastName";
                    break;
                case "email":
                    propertyName = "Email";
                    break;
                case "address":
                    propertyName = "Address";
                    break;
                case "interests":
                    propertyName = "Interests";
                    break;
            }

            return propertyName;
        }

    }
}
