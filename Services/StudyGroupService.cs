using Microsoft.Extensions.Options;
using MongoDB.Driver;
using marian_onsite.Models;


namespace marian_onsite.Services;

public class StudyGroupService
{
    private readonly IMongoCollection<StudyGroup> _studygroupsCollection;

    public StudyGroupService(
        IOptions<StudyGroupDatabaseSettings> studyGroupDatabaseSettings)
    {
        var mongoClient = new MongoClient(studyGroupDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(studyGroupDatabaseSettings.Value.DatabaseName);

        _studygroupsCollection = mongoDatabase.GetCollection<StudyGroup>(studyGroupDatabaseSettings.Value.StudyGroupsCollectionName);
    }

    public async Task<List<StudyGroup>> GetAsync() =>
       await _studygroupsCollection.Find(studygroup => true).ToListAsync();

    public async Task<StudyGroup?> GetAsync(string id) =>
        await _studygroupsCollection.Find<StudyGroup>(studygroup => studygroup.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(StudyGroup newStudyGroup)
    {
        await _studygroupsCollection.InsertOneAsync(newStudyGroup);
    }

    public async Task UpdateAsync(string id, StudyGroup updatedStudyGroup)
    {
        await _studygroupsCollection.ReplaceOneAsync(studygroup => studygroup.Id == id, updatedStudyGroup);
    }

    public async Task DeleteAsync(string id)
    {
        await _studygroupsCollection.DeleteOneAsync(studygroup => studygroup.Id == id);
    }

    public async Task<List<StudyGroup>> FindByUserIdAsync(string userId)
    {
        return await _studygroupsCollection.Find(studygroup => studygroup.Creator == userId).ToListAsync();
    }
}

