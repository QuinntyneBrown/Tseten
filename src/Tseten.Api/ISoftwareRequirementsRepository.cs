// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Models.SoftwareRequirement;

namespace Tseten.Api;

public interface ISoftwareRequirementsRepository
{
    void Create(SoftwareRequirement softwareRequirement);

    void Update(SoftwareRequirement softwareRequirement);

    void Delete(string softwareRequirementId);

    SoftwareRequirement? GetById(string softwareRequirementId);

    List<SoftwareRequirement> Get();
}

