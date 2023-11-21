namespace Chirp.Core;
using FluentValidation;

public record CheepDto(string Text, string Author, DateTime TimeStamp);
public record CreateCheepDto(string Text, string Author);

public class CreateCheepValidator : AbstractValidator<CreateCheepDto>
{
    public CreateCheepValidator()
    {
        RuleFor(x => x.Text).NotEmpty().MaximumLength(160);
    }
}