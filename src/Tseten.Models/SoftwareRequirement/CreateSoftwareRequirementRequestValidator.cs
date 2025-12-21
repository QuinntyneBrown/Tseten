// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tseten.Models.SoftwareRequirement;

public class CreateSoftwareRequirementRequestValidator: AbstractValidator<CreateSoftwareRequirementRequest>
{
    public CreateSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.Description).NotNull().NotEmpty();
        RuleForEach(x => x.AcceptanceCriteria).SetValidator(new AcceptanceCriteriaValidator());
    }
}

