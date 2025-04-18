﻿using FluentValidation;
using HomeApi.Contracts.Models.Devices;

namespace HomeApi.Contracts.Validation;

public class EditDeviceRequestValidator: AbstractValidator<EditDeviceRequest>
{
    public EditDeviceRequestValidator()
    {
        RuleFor(x => x.NewName).NotEmpty()
            .WithMessage("Please choose a valid name");
        RuleFor(x => x.NewRoom).NotEmpty()
            .Must(BeSupported)
            .WithMessage($"Please choose one of the following locations: {string.Join(", ", Values.ValidRooms)}");
    }

    private bool BeSupported(string location)
    {
        return Values.ValidRooms.Any(x => x ==location);
    }
}