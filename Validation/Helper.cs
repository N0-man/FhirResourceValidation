namespace FhirResourceValidation.Validation;

public static class Helper
{
    public static T Convert<T>(dynamic element) => (T)element;

    public static bool ValidateRange(decimal? value, decimal min, decimal max) =>
        value >= min && value <= max;
}