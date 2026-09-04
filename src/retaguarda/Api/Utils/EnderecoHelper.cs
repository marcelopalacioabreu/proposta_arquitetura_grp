using System.Linq;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.DTO.Dtos;
using Retaguarda.Persistencia;

namespace Retaguarda.Api.Utils
{
    /// <summary>
    /// Helpers para gravar e carregar endereços associados a qualquer entidade.
    /// </summary>
    internal static class EnderecoHelper
    {
        // Resolve a hierarquia completa do CEP e cria/atualiza o Endereco
        internal static Endereco ObterOuCriarEndereco(IApplicationDbContext db, long cepId, string? complemento)
        {
            var cep = db.EnderecoCEPs
                .Include(c => c.Logradouro)
                    .ThenInclude(l => l.Bairro)
                        .ThenInclude(b => b.Municipio)
                            .ThenInclude(m => m.Uf)
                                .ThenInclude(u => u.Pais)
                .FirstOrDefault(c => c.Id == cepId);

            var end = new Endereco
            {
                CepId = cepId,
                Complemento = complemento ?? string.Empty,
                LogradouroId = cep?.Logradouro?.Id,
                BairroId = cep?.Logradouro?.Bairro?.Id,
                MunicipioId = cep?.Logradouro?.Bairro?.Municipio?.Id,
                UfId = cep?.Logradouro?.Bairro?.Municipio?.Uf?.Id,
                PaisId = cep?.Logradouro?.Bairro?.Municipio?.Uf?.Pais?.Id,
            };
            db.Enderecos.Add(end);
            db.SaveChanges();
            return end;
        }

        internal static void SalvarEnderecosPessoa(IApplicationDbContext db, long pessoaId, EnderecoSubcadastroDto[] enderecos)
        {
            db.PessoaEnderecos.RemoveRange(db.PessoaEnderecos.Where(x => x.PessoaId == pessoaId));
            db.SaveChanges();
            foreach (var e in enderecos.Where(e => e.CepId > 0))
            {
                var end = e.EnderecoId > 0 ? db.Enderecos.Find(e.EnderecoId!.Value) : null;
                if (end != null) end.Complemento = e.Complemento ?? string.Empty;
                else end = ObterOuCriarEndereco(db, e.CepId!.Value, e.Complemento);
                db.PessoaEnderecos.Add(new PessoaEndereco { PessoaId = pessoaId, EnderecoId = end.Id, EnderecoTipoId = e.TipoId, EnderecoPrincipal = e.Principal });
            }
            db.SaveChanges();
        }

        internal static void SalvarEnderecosOrganizacao(IApplicationDbContext db, long orgId, EnderecoSubcadastroDto[] enderecos)
        {
            db.OrganizacaoEnderecos.RemoveRange(db.OrganizacaoEnderecos.Where(x => x.OrganizacaoId == orgId));
            db.SaveChanges();
            foreach (var e in enderecos.Where(e => e.CepId > 0))
            {
                var end = e.EnderecoId > 0 ? db.Enderecos.Find(e.EnderecoId!.Value) : null;
                if (end != null) end.Complemento = e.Complemento ?? string.Empty;
                else end = ObterOuCriarEndereco(db, e.CepId!.Value, e.Complemento);
                db.OrganizacaoEnderecos.Add(new OrganizacaoEndereco { OrganizacaoId = orgId, EnderecoId = end.Id, EnderecoTipoId = e.TipoId, EnderecoPrincipal = e.Principal });
            }
            db.SaveChanges();
        }

        internal static void SalvarEnderecosUnidade(IApplicationDbContext db, long unidadeId, EnderecoSubcadastroDto[] enderecos)
        {
            db.OrganizacaoUnidadeEnderecos.RemoveRange(db.OrganizacaoUnidadeEnderecos.Where(x => x.OrganizacaoUnidadeId == unidadeId));
            db.SaveChanges();
            foreach (var e in enderecos.Where(e => e.CepId > 0))
            {
                var end = e.EnderecoId > 0 ? db.Enderecos.Find(e.EnderecoId!.Value) : null;
                if (end != null) end.Complemento = e.Complemento ?? string.Empty;
                else end = ObterOuCriarEndereco(db, e.CepId!.Value, e.Complemento);
                db.OrganizacaoUnidadeEnderecos.Add(new OrganizacaoUnidadeEndereco { OrganizacaoUnidadeId = unidadeId, EnderecoId = end.Id, EnderecoTipoId = e.TipoId, EnderecoPrincipal = e.Principal });
            }
            db.SaveChanges();
        }

        internal static EnderecoSubcadastroDto[] CarregarEnderecosPessoa(IApplicationDbContext db, long pessoaId) =>
            db.PessoaEnderecos.Where(x => x.PessoaId == pessoaId)
              .Select(x => new EnderecoSubcadastroDto { Id = x.Id, EnderecoId = x.EnderecoId, CepId = x.Endereco != null ? x.Endereco.CepId : (long?)null, Complemento = x.Endereco != null ? x.Endereco.Complemento : null, TipoId = x.EnderecoTipoId, Principal = x.EnderecoPrincipal })
              .ToArray();

        internal static EnderecoSubcadastroDto[] CarregarEnderecosOrganizacao(IApplicationDbContext db, long orgId) =>
            db.OrganizacaoEnderecos.Where(x => x.OrganizacaoId == orgId)
              .Select(x => new EnderecoSubcadastroDto { Id = x.Id, EnderecoId = x.EnderecoId, CepId = x.Endereco != null ? x.Endereco.CepId : (long?)null, Complemento = x.Endereco != null ? x.Endereco.Complemento : null, TipoId = x.EnderecoTipoId, Principal = x.EnderecoPrincipal })
              .ToArray();

        internal static EnderecoSubcadastroDto[] CarregarEnderecosUnidade(IApplicationDbContext db, long unidadeId) =>
            db.OrganizacaoUnidadeEnderecos.Where(x => x.OrganizacaoUnidadeId == unidadeId)
              .Select(x => new EnderecoSubcadastroDto { Id = x.Id, EnderecoId = x.EnderecoId, CepId = x.Endereco != null ? x.Endereco.CepId : (long?)null, Complemento = x.Endereco != null ? x.Endereco.Complemento : null, TipoId = x.EnderecoTipoId, Principal = x.EnderecoPrincipal })
              .ToArray();
    }
}
