using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Interfaces
{
    public interface IAdminiastradorServico
    {
        Administrador? Login(LoginDTOController loginDTO);
        Administrador Cadastrar(Administrador administrador);
        Administrador ObterPorEmail(string email);
        Administrador ObterPorId(int id);
        List<Administrador> ObterTodos(int? pagina);
        Administrador Atualizar(int id, AdministradorDTO administradorDTO);
        bool Deletar(int id);
    }
}