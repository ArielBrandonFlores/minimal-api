using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.DTOs;

namespace minimal_api.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTOController loginDTO);
        Administrador Cadastrar(Administrador administrador);
        Administrador ObterPorId(int id);
        List<Administrador> ObterTodos(int? pagina);
        void Atualizar(int id, AdministradorDTO administradorDTO);
        void Deletar(int id);
    }
}