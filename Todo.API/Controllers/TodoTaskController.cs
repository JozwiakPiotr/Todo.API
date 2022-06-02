using Microsoft.AspNetCore.Mvc;
using Todo.API.Commands;
using Todo.API.DTO;
using Todo.API.Queries;
using Todo.API.Services;
using Todo.API.Common;

namespace Todo.API.Controllers
{
    [ApiController]
    [Route("api/user/{userId}/todotask")]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITaskService _todoTaskService;

        public TodoTaskController(ITaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTaskDTO>> GetTodoTaskById([FromRoute] Guid id)
        {
            var result = await _todoTaskService.GetTodoTaskById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ListedResult<TodoTaskDTO>>> GetTodoTasks([FromQuery] GetTasks query)
        {
            var result = await _todoTaskService.GeTodoTasks(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddTodoTask([FromBody] AddTodoTask command)
        {
            var id = await _todoTaskService.AddTodoTask(command);
            return Created($"api/user/{command.UserId}/todotask/{id}", null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTodoTask([FromBody] UpdateTodoTask command)
        {
            await _todoTaskService.UpdateTodoTask(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoTask([FromRoute] Guid id)
        {
            await _todoTaskService.DeleteTodoTask(new DeleteTodoTask(id));
            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> CompleteTask([FromRoute] Guid id)
        {
            await _todoTaskService.Complete(id);
            return Ok();
        }
    }
}