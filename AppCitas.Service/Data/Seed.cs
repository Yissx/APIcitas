﻿using AppCitas.Service.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace AppCitas.Service.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;
        var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
        //var users = JsonSerializer.Deserialize<List<AppUser>>(userData); no funciona al 100
        var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);

        foreach(var user in users)
        {
            using var hmac = new HMACSHA512(); //Va a desechar lo que necesite para crear la variable después de
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}
