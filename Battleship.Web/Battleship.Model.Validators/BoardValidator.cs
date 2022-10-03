using FluentValidation;
using Battleship.Model;

namespace Battleship.Model.Validators
{
    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(x=> x.Name).NotEmpty().NotNull().WithMessage("Name canoot be null or empty");
            RuleFor(x => x.Name).Must(name => CustomValidators.ValidateSpecialCharacter(name)).WithMessage("Name cannot have special characters");
        }
    }
}
