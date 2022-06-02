using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.API.Extensions;

namespace Todo.API.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public User(Guid id, string email)
        {
            SetId(id);
            SetEmail(email);
        }

        public void SetId(Guid id)
            => Id = id != Guid.Empty ? id : throw new ArgumentException();

        public void SetEmail(string email)
            => Email = !email.IsEmpty() ? email : throw new ArgumentException();

        public void SetPasswordHash(string hash)
            => PasswordHash = !hash.IsEmpty() ? hash : throw new ArgumentException();
    }
}