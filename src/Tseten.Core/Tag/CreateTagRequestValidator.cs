// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tag.Models.Tag;

public class CreateTagRequestValidator: AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator(){
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();

    }

}

