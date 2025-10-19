using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Servicos;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;

#region Builder

var builder = WebApplication.CreateBuilder(args);
var Key = builder.Configuration["Jwt"];

if (string.IsNullOrEmpty(Key))
    Key = "Essa_é_minha_chave_secreta";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        // ValidateAudience = jwtSettings.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key)),
        ValidateAudience = false,
        ValidateIssuer = false,
    };
});

builder.Services.AddAuthorization();


builder.Services.AddScoped<IAdminiastradorServico, AdministradorServicos>();
builder.Services.AddScoped<IVeiculosServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("Mysql"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Mysql")));
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administrador

string GerarToken(Administrador administrador)
{
    if (string.IsNullOrEmpty(Key))
        return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil),
        new Claim(ClaimTypes.Role, administrador.Perfil),

    };

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}


app.MapPost("/Administradores/login", ([FromBody] LoginDTOController loginDTO, IAdminiastradorServico adminiastradorServico) =>
{
    var adm = adminiastradorServico.Login(loginDTO);
    if (adm != null)
    {
        string token = GerarToken(adm);
        return Results.Ok(new AdministradorLogado
        {
            Email = adm.Email,
            Perfil = adm.Perfil,
            Token = token
        });
    }
    else
        return Results.Unauthorized();
}).WithTags("Administrador");
app.MapPost("/Administradores", ([FromBody] AdministradorDTO administradorDTO, IAdminiastradorServico adminiastradorServico) =>
{
    var validacao = new ErrosDeValidacao();
    {
        if (string.IsNullOrEmpty(administradorDTO.Email))
            validacao.Mensagem.Add("O campo Email é obrigatório.");
        if (string.IsNullOrEmpty(administradorDTO.Senha))
            validacao.Mensagem.Add("O campo Senha é obrigatório.");
        if (string.IsNullOrEmpty(administradorDTO.Perfil))
            validacao.Mensagem.Add("O campo Perfil é obrigatório.");
        if (validacao.Mensagem.Count > 0)
            return Results.BadRequest(validacao);
    }
    var administrador = new Administrador
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil
    };
    adminiastradorServico.Cadastrar(administrador);
    return Results.Created($"/Administradores/{administrador.Id}", (new AdministradorModelView
    {
        Id = administrador.Id,
        email = administrador.Email,
        Perfil = administrador.Perfil
    }));
}).RequireAuthorization().WithTags("Administrador");

app.MapGet("/Administradores", ([FromQuery] int? pagina, IAdminiastradorServico adminiastradorServico) =>
{
    var viewer = new List<AdministradorModelView>();
    var administradores = adminiastradorServico.ObterTodos(pagina);

    viewer = administradores.Select(adm => new AdministradorModelView
    {
        Id = adm.Id,
        email = adm.Email,
        Perfil = adm.Perfil
    }).ToList();

    return Results.Ok(viewer);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administrador");

app.MapGet("/Administradores/{id}", ([FromRoute] int id, IAdminiastradorServico adminiastradorServico) =>
{
    var administrador = adminiastradorServico.ObterPorId(id);
    if (administrador == null)
        return Results.NotFound();
    return Results.Ok(new AdministradorModelView
    {
        Id = administrador.Id,
        email = administrador.Email,
        Perfil = administrador.Perfil
    });
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administrador");

app.MapPut("/Administradores/{id}", ([FromRoute] int id, AdministradorDTO administradorDTO, IAdminiastradorServico adminiastradorServico) =>
{
    var administrador = adminiastradorServico.ObterPorId(id);
    if (administrador == null)
        return Results.NotFound();

    var validacao = new ErrosDeValidacao();
    {
        if (string.IsNullOrEmpty(administradorDTO.Email))
            validacao.Mensagem.Add("O campo Email é obrigatório.");
        if (string.IsNullOrEmpty(administradorDTO.Senha))
            validacao.Mensagem.Add("O campo Senha é obrigatório.");
        if (string.IsNullOrEmpty(administradorDTO.Perfil))
            validacao.Mensagem.Add("O campo Perfil é obrigatório.");
        if (validacao.Mensagem.Count > 0)
            return Results.BadRequest(validacao);
    }

    administrador.Email = administradorDTO.Email;
    administrador.Senha = administradorDTO.Senha;
    administrador.Perfil = administradorDTO.Perfil;
    adminiastradorServico.Atualizar(id, administradorDTO);
    return Results.Ok(administrador);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administrador");

app.MapDelete("/Administradores/{id}", ([FromRoute] int id, IAdminiastradorServico adminiastradorServico) =>
{
    var administrador = adminiastradorServico.ObterPorId(id);
    if (administrador == null)
        return Results.NotFound();
    adminiastradorServico.Deletar(id);
    return Results.NoContent();
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Administrador");

#endregion

#region Veículos

ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao();
    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagem.Add("O campo Nome é obrigatório.");
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagem.Add("O campo Marca é obrigatório.");
    if (veiculoDTO.Ano < 1900)
        validacao.Mensagem.Add($"O campo Ano deve estar entre 1900 e {DateTime.Now.Year}.");
    return validacao;
}

app.MapPost("/Veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculoServico) =>
{
    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagem.Count > 0)
        return Results.BadRequest(validacao);
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };
    veiculoServico.Incluir(veiculo);
    return Results.Created($"/Veiculos/{veiculo.Id}", veiculo);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm , Editor" })
.WithTags("Veiculos");

app.MapGet("/Veiculos", ([FromQuery] int? pagina, IVeiculosServico veiculosServico) =>
{
    var veiculos = veiculosServico.Todos(pagina);

    return Results.Ok(veiculos);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm , Editor" })
.WithTags("Veiculos");

app.MapGet("/Veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscarPorId(id);
    if (veiculo == null)
        return Results.NotFound();
    return Results.Ok(veiculo);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm , Editor" })
.WithTags("Veiculos");

app.MapPut("/Veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscarPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagem.Count > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;
    veiculosServico.Atualizar(veiculo);
    return Results.Ok(veiculo);
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Veiculos");

app.MapDelete("/Veiculos/{id}", ([FromRoute] int id, IVeiculosServico veiculosServico) =>
{
    var veiculo = veiculosServico.BuscarPorId(id);
    if (veiculo == null)
        return Results.NotFound();
    veiculosServico.Deletar(veiculo);
    return Results.NoContent();
}).RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
.WithTags("Veiculos");

#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
