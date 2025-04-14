using FluentValidation;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;

namespace HomeApi.Contracts.Validation;

public class EditRoomRequestValidator : AbstractValidator<EditRoomRequest>
{
    public EditRoomRequestValidator()
    {
        RuleFor(x => x.NewName).NotEmpty();
        RuleFor(x => x.Area).NotEmpty();
        RuleFor(x => x.GasConnected).NotEmpty().WithMessage("Please choose a valid GasConnected");
        RuleFor(x => x.Voltage).NotEmpty().WithMessage("Please choose a valid ValidVoltage");
    }
}