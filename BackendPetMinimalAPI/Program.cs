using BackendPetMinimalAPI.date;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UserDb>();
    db.Database.EnsureCreated();
}

app.MapPost("/users", async ([FromBody] User user, IUserRepository repository) =>
    {
        await repository.InsertUserAsync(user);
        await repository.SaveAsync();
        return Results.Created($"/users/{user.Id}", user);
    })
    .Accepts<User>("application/json")
    .Produces<User>(StatusCodes.Status201Created)
    .WithName("CreateUser")
    .WithTags("Creators");
    
app.MapGet("/users", async (IUserRepository repository) => 
        Results.Ok(await repository.GetUsersAsync()))
    .Produces<List<User>>(StatusCodes.Status200OK)
    .WithName("GetAllUser")
    .WithTags("Getters");

app.MapGet("/users/{id}", async (int id, IUserRepository repository) =>
    await repository.GetUserAsync(id) is User user
        ? Results.Ok(user)
        : Results.NotFound())
    .Produces<User>(StatusCodes.Status200OK)
    .WithName("GetUserById")
    .WithTags("Getters");

app.MapGet("/users/search/username/{query}", async (string query, IUserRepository repository) =>
        await repository.GetUsersAsync(query) is IEnumerable<User> users
            ? Results.Ok(users)
            : Results.NotFound(Array.Empty<User>()))
    .Produces<List<User>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("SearchUser")
    .WithTags("Getters");

app.MapPut("/users", async ([FromBody] User user, IUserRepository repository) =>
    {
        await repository.UpdateUserAsync(user);
        await repository.SaveAsync();
        return Results.NoContent();
    })
    .Accepts<User>("application/json")
    .WithName("UpdateUser")
    .WithTags("Updaters");

app.MapDelete("/users/{id}", async (int id, IUserRepository repository) =>
    {
        await repository.DeleteUserAsync(id);
        await repository.SaveAsync();
        return Results.NoContent();
    })
    .WithName("DeleteUser")
    .WithTags("Deleters");


app.UseHttpsRedirection();

app.Run();
