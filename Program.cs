using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Builder;
using SistemaDeLogin.Data;
using Microsoft.AspNetCore.Identity;
using SistemaDeLogin.Repositories.Cliente;
using SistemaDeLogin.Repositories.Servicos;
using SistemaDeLogin.Repositories.Pecas;
using SistemaDeLogin.Services.Pecas;
using SistemaDeLogin.Services.Cliente;
using SistemaDeLogin.Services.Servico;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//Add Context
builder.Services.AddDbContext<SistemaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


//Add Injeções Clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

//Add Injeções Servicos
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IServicoService, ServicoService>();

//Add Injeções Peças
builder.Services.AddScoped<IPecasRepository, PecasRepository>();
builder.Services.AddScoped<IPecasService, PecasService>();

//Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SistemaContext>()
    .AddDefaultTokenProviders();


var app = builder.Build();

//Add Roles (Admin, User)

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        var roleExits = await roleManager.RoleExistsAsync(role);

        if (!roleExits)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    //Criar um Admin Incial
    var email = "admin@gmail.com";
    var password = "Admin@7221";

    var adminUser = await userManager.FindByEmailAsync(email);
    if (adminUser == null)
    {
        var newAdmin = new IdentityUser
        {
            Email = email,
            UserName = email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(newAdmin, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }

    var user = await userManager.FindByEmailAsync("abcd@gmail.com");
    if (user != null)
    {
        await userManager.RemovePasswordAsync(user);
        await userManager.AddPasswordAsync(user, "Abcd@1234");
    }
}


    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
