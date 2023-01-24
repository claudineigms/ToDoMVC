using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ToDoMVC.Models
{
    public class UserModel : IdentityUser<int>
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        [DataType(DataType.Date)]
        public string DataNascimento { get; set; }
        public string CPF { get; set;}

        [NotMapped]
        [DataType(DataType.Password)]
        public string Senha{ get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        public string ConfirmaSenha{ get; set; }
        [NotMapped]
        public bool ManterConectado{ get; set; }
    }

}