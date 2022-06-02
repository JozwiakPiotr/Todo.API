using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.API.DTO;
using Todo.API.Entities;

namespace Todo.API
{
    public class TodoMapperProfile : Profile
    {
        public TodoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<TodoTask, TodoTaskDTO>();
        }
    }
}