// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;

namespace Tseten.Models.SoftwareRequirement;

/// <summary>
/// Request to import/sync all software requirements into the vector database.
/// </summary>
public class ImportEmbeddingsRequest : IRequest<ImportEmbeddingsResponse>
{
}
