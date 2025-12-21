// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Validator for SearchSoftwareRequirementsRequest.
/// </summary>
public class SearchSoftwareRequirementsRequestValidator : AbstractValidator<SearchSoftwareRequirementsRequest>
{
    public SearchSoftwareRequirementsRequestValidator()
    {
        RuleFor(x => x.Query)
            .NotNull()
            .NotEmpty()
            .WithMessage("Search query is required")
            .MaximumLength(1000)
            .WithMessage("Search query must not exceed 1000 characters");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be greater than or equal to 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be at least 1")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100");

        RuleFor(x => x.MinSimilarity)
            .InclusiveBetween(0f, 1f)
            .WithMessage("Minimum similarity must be between 0 and 1");
    }
}
