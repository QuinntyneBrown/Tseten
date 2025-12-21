// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tseten.Core.Model.SoftwareRequirement;

public class AcceptanceCriteriaValidator : AbstractValidator<AcceptanceCriteria>
{
    public AcceptanceCriteriaValidator()
    {
        RuleFor(x => x.Given).NotNull().NotEmpty().WithMessage("Given is required");
        RuleFor(x => x.When).NotNull().NotEmpty().WithMessage("When is required");
        RuleFor(x => x.Then).NotNull().NotEmpty().WithMessage("Then is required");
        RuleFor(x => x.Status).IsInEnum().WithMessage("Status must be a valid value");
        RuleFor(x => x.Priority).IsInEnum().WithMessage("Priority must be a valid value");
    }
}
