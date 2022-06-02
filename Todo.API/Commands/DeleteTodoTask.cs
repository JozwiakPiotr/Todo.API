using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.API.Commands
{
    public class DeleteTodoTask
    {
        public DeleteTodoTask(Guid id)
        {
            TodoTaskId = id;
        }

        public Guid TodoTaskId { get; set; }
    }
}