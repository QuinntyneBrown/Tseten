// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace SpecSync.Models.SoftwareRequirement;

public class UpdateSoftwareRequirementRequestValidator: AbstractValidator<UpdateSoftwareRequirementRequest>
{
    public UpdateSoftwareRequirementRequestValidator()
    {
        RuleFor(x => x.SoftwareRequirementId).NotNull().NotEmpty();
        RuleFor(x => x.ParentSoftwareRequirementId).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();
        RuleForEach(x => x.AcceptanceCriteria).SetValidator(new AcceptanceCriteriaValidator());
    }
}

