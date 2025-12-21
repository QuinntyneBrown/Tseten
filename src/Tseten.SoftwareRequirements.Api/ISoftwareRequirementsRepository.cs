// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Tseten.Core.Model.SoftwareRequirement;

namespace Tseten.SoftwareRequirements.Api;

public interface ISoftwareRequirementsRepository
{
    void Create(SoftwareRequirement softwareRequirement);

    void Update(SoftwareRequirement softwareRequirement);

    void Delete(string softwareRequirementId);

    SoftwareRequirement GetById(string softwareRequirementId);

    List<SoftwareRequirement> Get();
}

