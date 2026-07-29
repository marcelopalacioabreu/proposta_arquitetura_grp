using System;

namespace Retaguarda.Dominio.Entidades.Base
{
    public abstract class MultilocatarioEntidade
    {
        public long Id { get; set; }
        public DateTime DataInsercao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; }

        // Multi-tenant / multi-sector identifiers
        public long? OrganizacaoId { get; set; }
        // New multi-tenant identifiers: unidade and setor
        public long? OrganizacaoUnidadeId { get; set; }
        public long? SetorId { get; set; }
        // Keep legacy OrganizacaoSetorId for compatibility
        public long? OrganizacaoSetorId { get; set; }
        // Active flag (ativo / inativo)
        public bool Ativo { get; set; } = true;
    }
}
