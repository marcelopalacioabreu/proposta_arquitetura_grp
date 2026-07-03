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
        public long? OrganizacaoSetorId { get; set; }
        // Active flag (ativo / inativo)
        public bool Ativo { get; set; } = true;
    }
}
