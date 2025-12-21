// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tag.Models.Tag;

public class UpdateTagRequestValidator: AbstractValidator<UpdateTagRequest>
{
    public UpdateTagRequestValidator(){
        RuleFor(x => x.TagId).NotNull();
        RuleFor(x => x.Name).NotNull().NotEmpty();
        RuleFor(x => x.Description).NotNull().NotEmpty();

    }

}

