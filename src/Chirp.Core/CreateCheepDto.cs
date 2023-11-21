namespace Chirp.Core;
using FluentValidation;

public record CreateCheepDto(string Text, string Author);

public class CreateCheepValidator : AbstractValidator<CreateCheepDto>
{

}