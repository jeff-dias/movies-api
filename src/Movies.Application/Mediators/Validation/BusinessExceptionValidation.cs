namespace Movies.Application.Mediators.Validation
{
    public class BusinessExceptionValidation : Exception
    {
        public BusinessExceptionValidation(string error) : base(error)
        { }

        public static void When(bool hasError, string errorMessage)
        {
            if (hasError)
                throw new BusinessExceptionValidation(errorMessage);
        }
    }
}
