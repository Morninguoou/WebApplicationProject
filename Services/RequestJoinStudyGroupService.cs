using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using marian_onsite.Models;


namespace marian_onsite.Services;

public class RequestJoinStudyGroupService
{
    private readonly IMongoCollection<RequestJoinStudyGroup> _requestJoinStudyGroupCollection;

    public RequestJoinStudyGroupService(IOptions<RequestJoinStudyGroupDatabaseSettings> requestJoinStudyGroupDatabaseSettings)
    {
        var mongoClient = new MongoClient(requestJoinStudyGroupDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(requestJoinStudyGroupDatabaseSettings.Value.DatabaseName);

        _requestJoinStudyGroupCollection = mongoDatabase.GetCollection<RequestJoinStudyGroup>(requestJoinStudyGroupDatabaseSettings.Value.RequestJoinStudyGroupCollectionName);
    }

    public async Task<List<RequestJoinStudyGroup>> GetAsync() =>
        await _requestJoinStudyGroupCollection.Find(requestJoinStudyGroup => true).ToListAsync();


    public async Task<RequestJoinStudyGroup?> GetAsync(string id)
    {
        return await _requestJoinStudyGroupCollection.Find(requestJoinStudyGroup => requestJoinStudyGroup.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<RequestJoinStudyGroup>> GetByStudyGroupIdAsync(string studyGroupId)
    {
        return await _requestJoinStudyGroupCollection.Find(requestJoinStudyGroup => requestJoinStudyGroup.StudyGroupId == studyGroupId).ToListAsync();
    }

    public async Task<List<RequestJoinStudyGroup>> GetByUserIdAsync(string userId)
    {
        return await _requestJoinStudyGroupCollection.Find(requestJoinStudyGroup => requestJoinStudyGroup.UserId == userId).ToListAsync();
    }

    public async Task CreateAsync(RequestJoinStudyGroup newRequestJoinStudyGroup)
    {
        await _requestJoinStudyGroupCollection.InsertOneAsync(newRequestJoinStudyGroup);
    }

    public async Task UpdateAsync(string id, RequestJoinStudyGroup updatedRequestJoinStudyGroup)
    {
        await _requestJoinStudyGroupCollection.ReplaceOneAsync(requestJoinStudyGroup => requestJoinStudyGroup.Id == id, updatedRequestJoinStudyGroup);
    }


    public async Task DeleteAsync(string id)
    {
        await _requestJoinStudyGroupCollection.DeleteOneAsync(requestJoinStudyGroup => requestJoinStudyGroup.Id == id);
    }

    public async Task DeleteByStudyGroupIdAsync(string studyGroupId)
    {
        await _requestJoinStudyGroupCollection.DeleteManyAsync(requestJoinStudyGroup => requestJoinStudyGroup.StudyGroupId == studyGroupId);
    }
}