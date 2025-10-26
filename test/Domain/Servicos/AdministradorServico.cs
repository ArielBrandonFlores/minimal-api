using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Xml.Linq;

namespace Test.Domain.Entidades;

[DoNotParallelize]
[TestClass]
public class AdministradorServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbContexto(configuration);
    }


    [TestMethod]
    public void TestandoSalvarAdministrador()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
        context.Database.ExecuteSqlRaw("DELETE FROM Administradores;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Administradores AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
        context.SaveChanges();

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Cadastrar(adm);

        // Assert
        Assert.AreEqual(1, administradorServico.ObterTodos(1).Count());
    }

    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
        context.Database.ExecuteSqlRaw("DELETE FROM Administradores;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Administradores AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
        context.SaveChanges();

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Cadastrar(adm);
        var admDoBanco = administradorServico.ObterPorId(adm.Id);

        // Assert
        Assert.AreEqual(1, admDoBanco.Id);
    }
}