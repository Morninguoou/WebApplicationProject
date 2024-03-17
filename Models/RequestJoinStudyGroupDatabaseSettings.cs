namespace marian_onsite.Models
{
    public class RequestJoinStudyGroupDatabaseSettings : IRequestJoinStudyGroupDatabaseSettings
    {
        public string RequestJoinStudyGroupCollectionName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public interface IRequestJoinStudyGroupDatabaseSettings
    {
        string RequestJoinStudyGroupCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}