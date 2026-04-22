using ERPSystem.Domain.Entities.Identity;
using ERPSystem.Domain.Enums;
using ERPSystem.Infrastructure;
using ERPSystem.Infrastructure.IocExtension;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDatabaseExtension(builder.Configuration);
builder.Services.UseService(builder.Configuration);
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ERPSystem.Application.Common.Behaviors.ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.Migrate();

    if (!context.Set<Role>().Any())
    {
        context.Set<Role>().AddRange(
            new Role { Id = Guid.NewGuid(),Name = UserRoles.Manager.ToString(), 
                NormalizedName = UserRoles.Manager.ToString().ToUpper()
                } , new Role
            {
                Id = Guid.NewGuid(),Name = UserRoles.Employee.ToString(), 
                NormalizedName = UserRoles.Employee.ToString().ToUpper()
            });
        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers();
app.Run();
