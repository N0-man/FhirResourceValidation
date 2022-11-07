namespace FhirResourceValidation.Validation;

using FluentValidation;
using Hl7.Fhir.Model;

public class Gm001Validator : AbstractValidator<Goal.TargetComponent>
{
    public Gm001Validator()
    {
        RuleFor(target => target)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(target =>
                target.Due.GetType() == typeof(Duration) &&
                Helper.ValidateRange(
                    Helper.Convert<Duration>(target.Due).Value,
                    1,
                    12))
            .Must(target =>
                target.Detail.GetType() == typeof(Quantity) &&
                Helper.ValidateRange(
                    Helper.Convert<Quantity>(target.Detail).Value,
                    1,
                    200));
    }
}