// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentValidation;

namespace Tag.Models.Tag;

public class DeleteTagRequestValidator: AbstractValidator<DeleteTagRequest>
{
    public DeleteTagRequestValidator(){
        RuleFor(x => x.TagId).NotNull();

    }

}

