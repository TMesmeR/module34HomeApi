using System.Buffers;
using System.Linq;
using System.Collections.Generic;
using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validation;

public class AddDeviceRequestValidation : AbstractValidator<AddDeviceRequest>
{
    public AddDeviceRequestValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Manufacturer).NotEmpty().WithMessage("Manufacturer is required");
        RuleFor(x => x.Model).NotEmpty().WithMessage("Model is required");
        RuleFor(x => x.SerialNumber).NotEmpty().WithMessage("Serial number is required");
        RuleFor(x => x.CurrentVolts).NotEmpty().InclusiveBetween(120, 220);
        RuleFor(x => x.GasUsage).NotNull();
        RuleFor(x => x.RoomLocation).NotEmpty().Must(BeSupported)
            .WithMessage($"Please choose one of the following locations: {string.Join(", ", Values.ValidRooms)}");
    }

    private bool BeSupported(string location)
    {
        return Values.ValidRooms.Any(e => e==location);
    }
}