using Todo.API.Middleware;
using Todo.API.Settings;
using Todo.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Todo.API.Services;
using Todo.API;
using Microsoft.AspNetCore.Identity;
using Todo.API.Entities;
using Todo.API.Queries;
using FluentValidation;
using Todo.API.Queries.Validators;
using Todo.API.Commands;
using Todo.API.Commands.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAutoMapper(typeof(TodoMapperProfile).Assembly);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<IValidator<GetTasks>, GetTasksValidator>();
builder.Services.AddScoped<IValidator<RegisterUser>, RegisterUserValidator>();

builder.Services.AddDbContext<TodoDbContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("Todo")));
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:JwtIssuer"],
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtKey"]))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();