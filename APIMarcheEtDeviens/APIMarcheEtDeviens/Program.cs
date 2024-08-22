using APIMarcheEtDeviens.Data;
using APIMarcheEtDeviens.Repository;
using Microsoft.EntityFrameworkCore;
using APIMarcheEtDeviens.Services;
using AutoMapper;
using APIMarcheEtDeviens.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IController<Guid, ParticipantDTO>, ParticipantService>();
builder.Services.AddScoped<IController<Guid, MediaDto>, MediaService>();
builder.Services.AddScoped<IController<int, RoleDto>, RoleService>();
builder.Services.AddScoped<IController<Guid, RandonneeDto>, RandonneeService>();
builder.Services.AddScoped<IController<Guid, RandonneurDTO>, RandonneurService>();
builder.Services.AddScoped<IController<Guid, PenseeDto>, PenseeService>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultValue"));
});

builder.Services.AddAuthentication();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
var config = new MapperConfiguration(cfg => {
    cfg.AddProfile<AutomapperProfile>();
});


// ajout de l'autorisation pour le JWT 
builder.Services.AddAuthentication();


// Ajoutez les services CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Utilisez la politique CORS configurée
app.UseCors("AllowAngularApp");



app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();