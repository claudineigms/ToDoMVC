using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoMVC.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public UserModel Manager { get; set; }
        public string Name { get; set; }
        public bool Check { get; set; }
    }
}