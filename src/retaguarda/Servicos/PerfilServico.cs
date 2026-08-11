using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class PerfilServico : ServicoBase<Perfil, PerfilDto>, IPerfilServico
    {
        private readonly IPerfilRepositorio _repositorioConcrete;

        public PerfilServico(IPerfilRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override PerfilDto ToDto(Perfil e)
        {
            return new PerfilDto
            {
                Id = e.Id,
                Nome = e.Nome,
                AdministradorDoSistema = e.AdministradorDoSistema,
                Ativo = e.Ativo
            };
        }

        protected override Perfil FromDto(PerfilDto dto)
        {
            return new Perfil
            {
                Nome = dto.Nome,
                AdministradorDoSistema = dto.AdministradorDoSistema,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Perfil entity, PerfilDto dto)
        {
            entity.Nome = dto.Nome;
            entity.AdministradorDoSistema = dto.AdministradorDoSistema;
            entity.Ativo = dto.Ativo;
        }
    }
}
