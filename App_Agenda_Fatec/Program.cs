﻿using Microsoft.EntityFrameworkCore;

using App_Agenda_Fatec.Models;

using App_Agenda_Fatec.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// "Enganando" a aplicação para utilizar os recursos de scaffolding do EntityFramework.

builder.Services.AddDbContext<PseudoDatabaseContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("PseudoDatabaseContext") ?? throw new InvalidOperationException("Connection string 'PseudoDatabaseContext' not found."));

});

// Definindo os valores dos atributos de configuração do arquivo de contexto do MongoDB.

MongoDBContext.Connection_String = builder.Configuration.GetSection("MongoDBConnection:ConnectionString").Value;

MongoDBContext.Database_Name = builder.Configuration.GetSection("MongoDBConnection:DatabaseName").Value;

MongoDBContext.Is_Ssl = Convert.ToBoolean(builder.Configuration.GetSection("MongoDBConnection:IsSsl").Value);

// Configurando os recursos de autenticação (Login).

builder.Services.AddIdentity<AppUser, AppRole>().AddMongoDbStores<AppUser, AppRole, Guid>(MongoDBContext.Connection_String, MongoDBContext.Database_Name);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");

}

app.UseStaticFiles();

app.UseRouting();

// Habilita os recursos de autenticação de usuários.

app.UseAuthentication();

// Habilita os recursos de verificação de nível de acesso (Autorizações) do usuário autenticado.

app.UseAuthorization();

// Especifica a "Action" que deve ser executada assim que a aplicação entra em execução.

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicia a execução da aplicação.

app.Run();