# FHIR RESOURCE VALIDATION

Sample code to demonstrate various approach for validating FHIR resource

Options

1. StructureDefinition 
   [FHIR profiles](https://www.hl7.org/fhir/validation.html)
2. Typed validation rules using
   [FluentValidation](https://docs.fluentvalidation.net/en/latest/)

#### Note

`validator_cli.jar` can be downloaded from [official FHIR validator](https://github.com/hapifhir/org.hl7.fhir.core/releases/latest/download/validator_cli.jar). Local StructureDefinition profiles can be
validated using
`java -jar validator_cli.jar Profiles/StructureDefinition-goal.json`
