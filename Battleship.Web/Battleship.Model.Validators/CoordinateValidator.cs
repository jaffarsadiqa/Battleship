using FluentValidation;
using Battleship.Model;

namespace Battleship.Model.Validators
{
    public class CoordinateValidator : AbstractValidator<Coordinate>
    {
        public CoordinateValidator()
        {
            RuleFor(x=> x).Must(co=> CustomValidators.ValidateCoordinate(co)).WithMessage("Coordinate placed is outside of range,must be with in row - 1 to 10 and column - 1 to 10");
        }
    }
}
