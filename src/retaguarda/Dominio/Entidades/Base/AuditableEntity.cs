using System;

namespace Retaguarda.Dominio.Entidades.Base
{
    public abstract class AuditableEntity
    {
        public long Id { get; set; }
        public DateTime DataInsercao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAlteracao { get; set; }
    }
}
