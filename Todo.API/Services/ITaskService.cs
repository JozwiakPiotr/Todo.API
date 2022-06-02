using Todo.API.Commands;
using Todo.API.DTO;
using Todo.API.Queries;
using Todo.API.Common;

namespace Todo.API.Services
{
    public interface ITaskService
    {
        Task<Guid> AddTodoTask(AddTodoTask command);

        Task<TodoTaskDTO> GetTodoTaskById(Guid id);

        Task<ListedResult<TodoTaskDTO>> GeTodoTasks(GetTasks query);

        Task UpdateTodoTask(UpdateTodoTask command);

        Task DeleteTodoTask(DeleteTodoTask command);

        Task Complete(Guid id);
    }
}