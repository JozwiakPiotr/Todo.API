using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.API.Extensions;

namespace Todo.API.Entities
{
    public class TodoTask
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Completed { get; set; }
        public DateTime? Deadline { get; private set; }
        public Guid UserId { get; set; }

        public TodoTask(Guid id, string title, string description, Guid userId)
        {
            SetId(id);
            SetTitle(title);
            SetDescription(description);
            SetUserId(userId);
            CreatedAt = DateTime.UtcNow;
            Completed = null;
            Deadline = null;
        }

        public TodoTask(Guid id, string title, string description, Guid userId, DateTime? deadline)
            : this(id, title, description, userId)
        {
            SetDeadline(deadline);
        }

        public void Complete()
        {
            if (Completed != null)
                return;

            Completed = DateTime.UtcNow;
        }

        public void SetId(Guid id)
            => Id = id != Guid.Empty ? id : throw new ArgumentException();

        public void SetTitle(string title)
            => Title = !title.IsEmpty() ? title : throw new ArgumentException();

        public void SetDescription(string description)
            => Description = !description.IsEmpty() ? description : throw new ArgumentException();

        public void SetUserId(Guid id)
            => UserId = id != Guid.Empty ? id : throw new ArgumentException();

        public void SetDeadline(DateTime? deadline)
            => Deadline = deadline > DateTime.UtcNow || deadline is null ? deadline : throw new ArgumentException();
    }
}