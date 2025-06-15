using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaDeLogin.Data;
using SistemaDeLogin.Repositories.Cliente;
using SistemaDeLogin.Repositories.Servicos;
using SistemaDeLogin.Repositories.Pecas;
using SistemaDeLogin.Services.Pecas;
using SistemaDeLogin.Services.Cliente;
using SistemaDeLogin.Services.Servico;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Configurar a porta que o Render libera
var port = Environment.GetEnvironmentVariable("PORT");

if (port != null)
{
    var url = $"http://0.0.0.0:{port}";
    builder.WebHost.UseUrls(url);
}

// 🔥 Serviços MVC
builder.Services.AddControllersWithViews();

// 🔥 Configurar banco SQLite
builder.Services.AddDbContext<SistemaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔥 Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SistemaContext>()
    .AddDefaultTokenProviders();

// 🔥 Injeção de dependência - Clientes
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// 🔥 Injeção de dependência - Serviços
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();
builder.Services.AddScoped<IServicoService, ServicoService>();

// 🔥 Injeção de dependência - Peças
builder.Services.AddScoped<IPecasRepository, PecasRepository>();
builder.Services.AddScoped<IPecasService, PecasService>();

//Configurar o IdentityOptions
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Conta/AcessoNegado";
    options.LoginPath = "/Conta/Login";
});

var app = builder.Build();

// 🔥 Migração automática + Roles + Usuário Admin
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<SistemaContext>();
    db.Database.Migrate();

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        var roleExists = await roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // 🔥 Criar um usuário Admin inicial
    var adminEmail = "admin@gmail.com";
    var adminPassword = "Admin@7221";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdmin = new IdentityUser
        {
            Email = adminEmail,
            UserName = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(newAdmin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }

    // 🔥 Resetar senha de um usuário específico (opcional)
    var user = await userManager.FindByEmailAsync("abcd@gmail.com");
    if (user != null)
    {
        await userManager.RemovePasswordAsync(user);
        await userManager.AddPasswordAsync(user, "Abcd@1234");
    }
}

// 🔥 Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 🔥 Roteamento
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
