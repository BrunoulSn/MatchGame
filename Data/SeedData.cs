using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BFF_GameMatch.Models;
using MyBffProject.Models;

namespace MyBffProject.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            // Se já houver dados, não faz nada
            if (await db.Users.AnyAsync().ConfigureAwait(false)) return;

            // Usuários de exemplo
            var u1 = new User
            {
                Id = "user-1",
                Name = "Alice Silva",
                Email = "alice@example.com",
                CPF = "11111111111",
                Phone = "(11) 90000-0001",
                Password = "password"
            };

            var u2 = new User
            {
                Id = "user-2",
                Name = "Bruno Souza",
                Email = "bruno@example.com",
                CPF = "22222222222",
                Phone = "(11) 90000-0002",
                Password = "password"
            };

            var u3 = new User
            {
                Id = "user-3",
                Name = "Carla Pereira",
                Email = "carla@example.com",
                CPF = "33333333333",
                Phone = "(11) 90000-0003",
                Password = "password"
            };

            await db.Users.AddRangeAsync(new[] { u1, u2, u3 }).ConfigureAwait(false);
            await db.SaveChangesAsync().ConfigureAwait(false);

            // Times de exemplo
            var t1 = new MyBffProject.Models.Team
            {
                Name = "Futebol FC",
                Description = "Time casual de futebol",
                SportType = "Futebol",
                Owner = u1,
                Members = new List<User> { u1, u2 },
                Address = "Praça Central, 100",
                Photo = null,
                CreatedAt = DateTime.UtcNow
            };

            var t2 = new MyBffProject.Models.Team
            {
                Name = "Basquete All Stars",
                Description = "Treinos aos sábados",
                SportType = "Basquete",
                Owner = u2,
                Members = new List<User> { u2, u3 },
                Address = "Rua das Flores, 200",
                Photo = null,
                CreatedAt = DateTime.UtcNow
            };

            var t3 = new MyBffProject.Models.Team
            {
                Name = "Corrida Solta",
                Description = "Grupo de corrida matinal",
                SportType = "Corrida",
                Owner = u3,
                Members = new List<User> { u1, u3 },
                Address = "Parque Central",
                Photo = null,
                CreatedAt = DateTime.UtcNow
            };

            await db.Teams.AddRangeAsync(new[] { t1, t2, t3 }).ConfigureAwait(false);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
