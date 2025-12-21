// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tseten.Core;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
        RuleFor(x => x.NewPassword)
            .Must((cmd, newPassword) => newPassword != cmd.OldPassword)
            .WithMessage("New password must be different from old password");
        RuleFor(x => x.ConfirmationPassword).NotEmpty()
            .Equal(x => x.NewPassword)
            .WithMessage("Password confirmation must match new password");
    }
}
