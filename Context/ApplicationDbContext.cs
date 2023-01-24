using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoMVC.Models;

namespace ToDoMVC.Data;

public class ApplicationDbContext : IdentityDbContext<UserModel,IdentityRole<int>,int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Tarefa> Tasks { get; set; }
}
