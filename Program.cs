using System;
using System.IO;
using FhirResourceValidation.Validation;
using FluentValidation;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;

namespace FhirResourceValidation
{
  public static class Program
  {
    public static void Main()
    {
      var inputHealthPlan = FileReader.ReadFhirInputFile("careplan.json");

      var fjp = new FhirJsonParser();
      var resource = fjp.Parse<CarePlan>(inputHealthPlan);

      foreach (var item in resource.Goal)
      {
        if (resource.FindContainedResource(item) is not Goal goal) continue;
        ValidateUsingFluentValidation(goal, new GoalValidator());
        ValidateUsingFhirProfile(goal);
      }
    }

    private static void ValidateUsingFhirProfile(Base goal)
    {
      Console.WriteLine("======================================");
      Console.WriteLine("Validate Goals via FHIR Profile");
      var rootDir = Directory.GetCurrentDirectory();
      var profileDirectory = Path.Combine(rootDir, "Profiles");
      // This using CreateValidationSource seems flaky on n0man's mac - manually maintaining specification.zip in Profiles
      // var coreSource = new CachedResolver(ZipSource.CreateValidationSource());
      var coreSource = new CachedResolver(
          new ZipSource(Path.Combine(profileDirectory, "specification.zip"))
      );
      var cachedResolver = new CachedResolver(new DirectorySource(profileDirectory,
          new DirectorySourceSettings { IncludeSubDirectories = true }));
      var combinedSource = new MultiResolver(cachedResolver, coreSource);
      var settings = new ValidationSettings
      {
        EnableXsdValidation = true,
        GenerateSnapshot = true,
        Trace = false,
        ResourceResolver = combinedSource,
        ResolveExternalReferences = true,
        SkipConstraintValidation = false
      };
      var validator = new Validator(settings);

      var outcome = validator.Validate(goal);
      WriteOutcome(outcome);
    }

    private static void WriteOutcome(OperationOutcome outcome)
    {
      Console.WriteLine($"Validation of resource {(outcome.Success ? "is successful" : "has failed:")}");
      if (!outcome.Success)
      {
        outcome.Issue.ForEach(i => Console.WriteLine($"  {i}"));
      }
    }

    private static void ValidateUsingFluentValidation<T>(T resource,
      IValidator<T> validator)
    {
      Console.WriteLine("======================================");
      Console.WriteLine("Validate Goals via C# FluentValidation");
      var validationResult = validator.Validate(resource);

      Console.WriteLine($"Validation of resource {(validationResult.IsValid ? "is successful" : "has failed:")}");

      if (validationResult.IsValid) return;
      foreach (var failure in validationResult.Errors)
      {
        Console.WriteLine($"Validation Issue: {failure.PropertyName}: {failure.ErrorMessage}");
      }
    }
  }
}