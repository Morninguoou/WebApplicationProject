namespace marian_onsite.Models
{
    public class StudyGroupDatabaseSettings : IStudyGroupDatabaseSettings
    {
        public string StudyGroupsCollectionName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }

    public interface IStudyGroupDatabaseSettings
    {
        string StudyGroupsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
