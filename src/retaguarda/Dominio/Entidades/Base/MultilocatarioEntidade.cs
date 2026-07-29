using System;

namespace Retaguarda.Dominio.Entidades.Base
{
    public abstract class MultilocatarioEntidade
    {
        public long Id { get; set; }
        public DateTime DataInsercao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; }
        public long? OrganizacaoId { get; set; }
        public long? OrganizacaoUnidadeId { get; set; }
        public long? SetorId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
