namespace FhirResourceValidation.Validation;

using FluentValidation;
using Hl7.Fhir.Model;

public class Gm002Validator : AbstractValidator<Goal.TargetComponent>
{
    public Gm002Validator()
    {
        RuleFor(target => target)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(target =>
                target.Due.GetType() == typeof(Date) &&
                Helper.Convert<Date>(target.Due).Value != null);
    }
}