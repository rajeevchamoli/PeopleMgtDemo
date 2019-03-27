namespace PeopleMgt
{
    /// <summary>
    /// place to keep common constants.
    /// </summary>
    public static class MessageConstants
    {
        public const string InputObjectNullOrInvalidIdErrorMsg = "{0} object is null or id mismatch!!";
        public const string GenericServerSideErrorMsg = "Internal server error";
        public const string InValidModelStateErrorMsg = "Invalid model object";
        public const string FirstNameMinLengthErrorMsg = "Required min Length for FirstName is 2";
        public const string FirstNameMaxLengthErrorMsg = "Allowed max Length for FirstName is 25";
        public const string LastNameMinLengthErrorMsg = "Required min Length for LastName is 2";
        public const string LastNameMaxLengthErrorMsg = "Allowed max Length for LastName is 25";
        public const string AgeRangeErrorMsg = "Valid Range for age is 0 to 120";
        public const string AddressMaxLengthErrorMsg = "Allowed max Length for Address is 100";
        public const string EmailLengthErrorMsg = "Allowed max Length for Email is 50";
        public const string InterestsMaxLengthErrorMsg = "Allowed max Length for Interests is 100";
        public const string PictureMaxSizeErrorMsg = "Allowed max szie for picture is 200KB";
       

    }

    public static class ConfigurationConstants
    {
        public const int MAX_PAGE_SIZE = 20;
        public const int DEFAULT_PAGE_SIZE = 5;
    }
}
