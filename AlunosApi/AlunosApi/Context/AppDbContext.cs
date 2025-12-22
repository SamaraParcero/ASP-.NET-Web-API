using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().HasData(
                new Aluno
                {
                    Id = 1,
                    Nome = "Samara",
                    Email = "samara@gmail.com",
                    Idade = 20
                },
                new Aluno
                {
                    Id = 2,
                    Nome = "pedro",
                    Email = "pedro@gmail.com",
                    Idade = 18
                }
                );
        }
    }
}
