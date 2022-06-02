﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.API.Commands
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}