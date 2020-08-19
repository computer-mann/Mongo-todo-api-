namespace ToDoApi.Infrastructure.Settings
{
    public class MongoDbSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
    }
}