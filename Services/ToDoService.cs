using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoApi.Infrastructure.Interfaces;
using ToDoApi.Infrastructure.Settings;
using ToDoApi.Models;

namespace ToDoApi.Services
{
    public class ToDoService : ITodoService
    {
        private readonly MongoDbSettings settings;
        private readonly IMongoCollection <ToDoModel> context;
        public ToDoService(IOptions<MongoDbSettings> options)
        {
            settings=options.Value;
            var client=new MongoClient(settings.ConnectionString);
            var database=client.GetDatabase(settings.DbName);
            context=database.GetCollection<ToDoModel>(settings.CollectionName);
        }
        public async Task CreateToDo(ToDoModel model)
        {
            await context.InsertOneAsync(model);
        }

        public async Task DeleteToDo(string id)
        {
            await context.DeleteOneAsync(to=>to.Id == id);
        }

        public async Task<List<ToDoModel>> GetAllTodo()
        {
          return await context.Find(todo=>true).ToListAsync();
        }

        public async Task<ToDoModel> GetOneById(string id)
        {
            return await context.Find(todo=>todo.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ToDoModel> GetOneByActivity(string activity)
        {
           return await context.Find(todo=>todo.Activity == activity).FirstOrDefaultAsync();
        }

        public async Task UpdateToDo(string id, ToDoModel model)
        {
            await context.ReplaceOneAsync(todo=>todo.Id == id,model);
        }
    }
}