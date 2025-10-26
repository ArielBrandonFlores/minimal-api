using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.DTOs;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Infraestrutura.Db;


namespace minimal_api.Dominio.Servicos
{
    public class AdministradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;


        public AdministradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public void Atualizar(int id, AdministradorDTO administradorDTO)
        {
            var administrador = _contexto.Administradores.Where(a => a.Id == id).FirstOrDefault();

            if (administrador == null)
            {
                throw new Exception("Administrador não encontrado");
            }

            administrador.Email = administradorDTO.Email;
            administrador.Senha = administradorDTO.Senha;
            administrador.Perfil = administradorDTO.Perfil;

            _contexto.Administradores.Update(administrador);
            _contexto.SaveChanges();

        }

        public Administrador Cadastrar(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();
            return administrador;
        }

        public void Deletar(int id)
        {
            var administrador = _contexto.Administradores.Where(a => a.Id == id).FirstOrDefault();
            _contexto.Administradores.Remove(administrador);
            _contexto.SaveChanges();
        }

        public Administrador? Login(LoginDTOController loginDTO)
        {
            return _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        }


        public Administrador ObterPorId(int id)
        {
            return _contexto.Administradores.Where(a => a.Id == id).FirstOrDefault();
        }

        public List<Administrador> ObterTodos(int? pagina)
        {
            var query = _contexto.Administradores.AsQueryable();

            int itensPorPagina = 5;

            if (pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

            }

            return query.ToList();
        }
    }
}