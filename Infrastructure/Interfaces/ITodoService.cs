using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models;

namespace ToDoApi.Infrastructure.Interfaces
{
    public interface ITodoService
    {
         Task CreateToDo(ToDoModel model);
        Task DeleteToDo(string id);
        Task UpdateToDo(string id,ToDoModel model);
        Task<List<ToDoModel>> GetAllTodo();
        Task<ToDoModel> GetOneById(string id);
        Task<ToDoModel> GetOneByActivity(string activity);
    }
}