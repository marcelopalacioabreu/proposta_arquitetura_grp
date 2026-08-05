using System;

namespace Retaguarda.Dominio.Entidades.Base
{
    public abstract class MultilocatarioEntidade
    {
        public long Id { get; set; }
        public Guid IdentificadorUnico { get; set; } = Guid.NewGuid();
        public string IdentificadorUnicoAmigavel { get; set; } = "";
        public DateTime DataInsercao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; } = DateTime.UtcNow;
        public long? OrganizacaoId { get; set; }
        public long? OrganizacaoUnidadeId { get; set; }
        public long? SetorId { get; set; }
        public bool Ativo { get; set; } = true;
        public long? UsuarioInsercaoId { get; set; }
        public long? UsuarioAlteracaoId { get; set; }
        public long Versao { get; set; } = 1;
    }
}
