namespace Retaguarda.DTO.Dtos
{
    /// <summary>
    /// DTO para representar a associação de um usuário a um setor de atuação.
    /// Inclui informações de unidade associada e marcação como padrão.
    /// </summary>
    public class SetorAtuacaoDto
    {
        public long SetorId { get; set; }
        public string SetorNome { get; set; } = string.Empty;
        public long? UnidadeId { get; set; }
        public string UnidadeNome { get; set; } = string.Empty;
        public bool Padrao { get; set; }
        public bool HabilitarPermissoesNegativas { get; set; }
    }
}
