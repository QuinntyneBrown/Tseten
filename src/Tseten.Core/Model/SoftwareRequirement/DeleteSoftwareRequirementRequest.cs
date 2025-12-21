// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Tseten.Core.Model.SoftwareRequirement;

public class DeleteSoftwareRequirementRequest: IRequest<DeleteSoftwareRequirementResponse>
{
    public string SoftwareRequirementId { get; set; }
}

