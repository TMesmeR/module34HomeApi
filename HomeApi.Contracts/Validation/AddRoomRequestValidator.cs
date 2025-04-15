using FluentValidation;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation;

public class AddRoomRequestValidator:AbstractValidator<AddRoomRequest>
{
    public AddRoomRequestValidator()
    {
        RuleFor(x => x.Area).NotEmpty().WithMessage("Please choose a valid area");
        RuleFor (x => x.Name).NotEmpty().WithMessage("Please choose a valid name");
        RuleFor(x => x.Voltage).NotEmpty().WithMessage("Please choose a valid voltage")
            .InclusiveBetween(120,220).WithMessage("Please choose a valid voltage");
        RuleFor(x => x.GasConnected).NotEmpty().WithMessage("Please choose a valid GetConnects");
    }
}