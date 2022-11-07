using System.Linq;

namespace FhirResourceValidation.Validation;

using System.Collections.Generic;
using FluentValidation;
using Hl7.Fhir.Model;

public class GoalValidator : AbstractValidator<Goal>
{
    private const string G1 = "G1";
    private const string G2 = "G2";
    private const string Gm001 = "GM001";
    private const string Gm002 = "GM002";

    private static readonly Dictionary<string, AbstractValidator<Goal.TargetComponent>> MeasureValidators = new()
    {
        { Gm001, new Gm001Validator() },
        { Gm002, new Gm002Validator() },
    };

    private static readonly Dictionary<string, List<string>> GoalValidators = new()
    {
        { G1, new List<string> { Gm001, Gm002 } },
        { G2, new List<string> { Gm002 } }
    };

    //convert to extension function
    private static string Code(Goal.TargetComponent target) => target.Measure.Coding[0].Code;
    private static string Code(Goal goal) => goal.Category[0].Coding[0].Code;

    public GoalValidator()
    {
        RuleFor(goal => goal)
            .Must(goal => goal.Category.Count == 1);
        RuleFor(goal => goal.Target)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must((goal, target) => VerifyMeasures(Code(goal), target))
            .WithMessage($"Goal have incorrect goal measures")
            .Must(target => target.All(t => 
                MeasureValidators[Code(t)].Validate(t).IsValid
            ))
            .WithMessage($"Goal Measures have incorrect data");
    }

    private static bool VerifyMeasures(string goalCode, IEnumerable<Goal.TargetComponent> target) =>
        target.All(t =>
            t.Measure.Coding.Count == 1 && GoalValidators[goalCode].Contains(Code(t))
        );
}