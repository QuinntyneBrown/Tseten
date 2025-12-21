// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Couchbase.Lite;
using Couchbase.Lite.Query;
using Tseten.Core.Model.SoftwareRequirement;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tseten.SoftwareRequirements.Api;

public class SoftwareRequirementsRepository: ISoftwareRequirementsRepository
{
    private readonly ILogger<SoftwareRequirementsRepository> _logger;
    private readonly Database _database;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    public SoftwareRequirementsRepository(ILogger<SoftwareRequirementsRepository> logger, Database database)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(database);

        _logger = logger;
        _database = database;
    }

    public void Create(SoftwareRequirement softwareRequirement)
    {
        var mutableDocument = new MutableDocument(softwareRequirement.SoftwareRequirementId, JsonSerializer.Serialize(softwareRequirement, _options));

        _database.GetDefaultCollection().Save(mutableDocument);
    }

    public void Delete(string softwareRequirementId)
    {
        throw new NotImplementedException();
    }

    public List<SoftwareRequirement> Get()
    {
        var result = new List<SoftwareRequirement>();

        var query = QueryBuilder.Select(SelectResult.All())
            .From(DataSource.Collection(_database.GetDefaultCollection()));

        foreach(var item in query.Execute()) {

            var json = item.ToJSON();

            var softwareRequirement = JsonSerializer.Deserialize<Dictionary<string,SoftwareRequirement>>(json);

            result.Add(softwareRequirement!.Single().Value);
        }

        return result;
    }

    public SoftwareRequirement GetById(string softwareRequirementId)
    {
        throw new NotImplementedException();
    }

    public void Update(SoftwareRequirement softwareRequirement)
    {
        throw new NotImplementedException();
    }
}

