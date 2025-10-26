using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.DTOs;

namespace Test.Mocks;

public class AdministradorServicoMock : IAdministradorServico
{
    private static List<Administrador> administradores = new List<Administrador>(){
        new Administrador{
            Id = 1,
            Email = "adm@teste.com",
            Senha = "123456",
            Perfil = "Adm"
        },
        new Administrador{
            Id = 2,
            Email = "editor@teste.com",
            Senha = "123456",
            Perfil = "Editor"
        }
    };

    public Administrador? ObterPorId(int id)
    {
        return administradores.Find(a => a.Id == id);
    }

    public Administrador Cadastrar(Administrador administrador)
    {
        administrador.Id = administradores.Count() + 1;
        administradores.Add(administrador);

        return administrador;
    }

    public Administrador? Login(LoginDTOController loginDTO)
    {
        return administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
    }

    public List<Administrador> ObterTodos(int? pagina)
    {
        return administradores;
    }
    public void Deletar(int id)
    {
        var administrador = administradores.Find(a => a.Id == id);
        administradores.Remove(administrador);
    }

    public void Atualizar(int id, AdministradorDTO administradorDTO)
    {
        var administrador = administradores.Find(a => a.Id == id);

        if (administrador == null)
        {
            throw new Exception("Administrador não encontrado");
        }

        administrador.Email = administradorDTO.Email;
        administrador.Senha = administradorDTO.Senha;
        administrador.Perfil = administradorDTO.Perfil;
    }
}