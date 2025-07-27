using System.ComponentModel.DataAnnotations;

public class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is string cpf && CpfValidator.IsValid(cpf);
    }
}