using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Todo.API.Commands;
using Todo.API.DTO;
using Todo.API.Entities;
using Todo.API.Exceptions;
using Todo.API.Infrastructure;
using Todo.API.Queries;
using Todo.API.Common;
using System.Linq;

namespace Todo.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly TodoDbContext _dbContext;
        private readonly IMapper _mapper;

        public TaskService(TodoDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> AddTodoTask(AddTodoTask command)
        {
            var newTodoTask = new TodoTask(Guid.NewGuid(), command.Title, command.Description, command.UserId, command.DeadLine);
            _dbContext.TodoTasks.Add(newTodoTask);
            await _dbContext.SaveChangesAsync();

            return newTodoTask.Id;
        }

        public async Task Complete(Guid id)
        {
            var todoTask = await _dbContext.TodoTasks.FirstOrDefaultAsync(t => t.Id == id) ??
                throw new NotFoundException();

            todoTask.Complete();

            _dbContext.TodoTasks.Update(todoTask);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTodoTask(DeleteTodoTask command)
        {
            var todoTask = await _dbContext.TodoTasks.FirstOrDefaultAsync(x => x.Id == command.TodoTaskId) ??
                throw new NotFoundException();

            _dbContext.TodoTasks.Remove(todoTask);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ListedResult<TodoTaskDTO>> GeTodoTasks(GetTasks query)
        {
            var timePredicates = new Dictionary<TimePeriod, Expression<Func<TodoTask, bool>>>
            {
                {TimePeriod.Month, x => x.CreatedAt.Month == query.PoitingDate.Month && x.CreatedAt.Year == query.PoitingDate.Year }
            };

            var categoryPredicates = new Dictionary<string, Expression<Func<TodoTask, bool>>>
            {
                {"completed", x => x.Completed != null},
                {"notCompleted", x => x.Completed == null},
                {"beforeDeadline", x => x.Completed == null ? x.Deadline >= DateTime.Now : x.Deadline <= x.Completed},
                {"afterDeadline", x => x.Completed == null ? x.Deadline < DateTime.Now : x.Deadline > x.Completed}
            };

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == query.UserId) ??
                throw new NotFoundException();

            var todoTasks = _dbContext.TodoTasks.Where(x => x.UserId == query.UserId);

            todoTasks = todoTasks.Where(timePredicates[query.TimePeriod]);

            if (query.Categories.Any())
            {
                var categories = todoTasks.Where(categoryPredicates[query.Categories[0]]);

                for (int i = 1; i < query.Categories.Count; i++)
                {
                    categories = categories.Union(todoTasks.Where(categoryPredicates[query.Categories[i]]));
                }

                todoTasks = categories;
            }

            todoTasks.OrderBy(x => x.CreatedAt);

            var result = await todoTasks.ToListAsync();

            var dto = _mapper.Map<List<TodoTaskDTO>>(result);

            return new ListedResult<TodoTaskDTO>(dto, query.TimePeriod, query.PoitingDate);
        }

        public async Task<TodoTaskDTO> GetTodoTaskById(Guid id)
        {
            var todoTask = await _dbContext.TodoTasks.FirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException();

            var dto = _mapper.Map<TodoTaskDTO>(todoTask);
            return dto;
        }

        public async Task UpdateTodoTask(UpdateTodoTask command)
        {
            var todoTask = await _dbContext.TodoTasks.FirstOrDefaultAsync(x => x.Id == command.Id) ??
                throw new NotFoundException();

            todoTask.SetTitle(command.Title);
            todoTask.SetDescription(command.Description);
            todoTask.SetDeadline(command.DeadLine);

            _dbContext.TodoTasks.Update(todoTask);
            await _dbContext.SaveChangesAsync();
        }
    }
}