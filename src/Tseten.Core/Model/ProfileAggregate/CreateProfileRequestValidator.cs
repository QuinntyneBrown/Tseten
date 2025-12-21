// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tseten.Core;

public class CreateProfileRequestValidator : AbstractValidator<CreateProfileRequest>
{
    public CreateProfileRequestValidator()
    {
        RuleFor(x => x.InvitationToken).NotEmpty().WithMessage("Invitation token is required");
        RuleFor(x => x.Firstname).NotEmpty().WithMessage("First name is required");
        RuleFor(x => x.Lastname).NotEmpty().WithMessage("Last name is required");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.PasswordConfirmation).NotEmpty()
            .Equal(x => x.Password).WithMessage("Password confirmation must match password");
    }
}
