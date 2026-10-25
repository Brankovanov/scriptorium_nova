using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Scriptorium_Nova.Web.Common.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MustContainLetterAttribute : ValidationAttribute, IClientModelValidator
{
    public MustContainLetterAttribute()
    {
        ErrorMessage = "{0} must contain at least one letter.";
    }

    public override bool IsValid(object? value)
    {
        // Empty values are handled by [Required], so they are not reported twice.
        if (value is not string text || string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        return text.Any(char.IsLetter);
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.TryAdd("data-val", "true");
        context.Attributes.TryAdd(
            "data-val-mustcontainletter",
            FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
    }
}