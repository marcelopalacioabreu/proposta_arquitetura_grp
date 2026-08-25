using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using System.Threading;
using System.Threading.Tasks;

namespace Retaguarda.Persistencia
{
    public interface IApplicationDbContext
    {
        DbSet<Organizacao> Organizacoes { get; set; }
        DbSet<OrganizacaoSetor> OrganizacaoSetores { get; set; }
        DbSet<OrganizacaoUnidade> OrganizacaoUnidades { get; set; }
        DbSet<OrganizacaoUnidadeSetor> OrganizacaoUnidadeSetores { get; set; }
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Pessoa> Pessoas { get; set; }
        DbSet<PessoaFisica> PessoasFisicas { get; set; }
        DbSet<PessoaJuridica> PessoasJuridicas { get; set; }
        DbSet<Perfil> Perfis { get; set; }
        DbSet<PerfilUsuario> PerfilUsuarios { get; set; }
        DbSet<PerfilPermissao> PerfilPermissoes { get; set; }
        DbSet<Tipo> Tipos { get; set; }

        DbSet<Pais> Paises { get; set; }
        DbSet<Uf> Ufs { get; set; }
        DbSet<Municipio> Municipios { get; set; }
        DbSet<Bairro> Bairros { get; set; }
        DbSet<Logradouro> Logradouros { get; set; }
        DbSet<Imovel> Imoveis { get; set; }
        DbSet<Cep> Ceps { get; set; }
        DbSet<Endereco> Enderecos { get; set; }

        DbSet<NivelGoverno> NiveisGoverno { get; set; }
        DbSet<NaturezaJuridica> NaturezasJuridicas { get; set; }
        DbSet<Situacao> Situacoes { get; set; }
        DbSet<Contato> Contatos { get; set; }
        DbSet<Documento> Documentos { get; set; }

        DbSet<OrquestracaoFluxoProcesso> OrquestracaoFluxoProcessos { get; set; }

        DbSet<OrganizacaoEndereco> OrganizacaoEnderecos { get; set; }
        DbSet<OrganizacaoUnidadeEndereco> OrganizacaoUnidadeEnderecos { get; set; }
        DbSet<OrganizacaoSetorEndereco> OrganizacaoSetorEnderecos { get; set; }
        DbSet<PessoaEndereco> PessoaEnderecos { get; set; }
        DbSet<UsuarioEndereco> UsuarioEnderecos { get; set; }

        DbSet<ContatoRelacionamento> ContatoRelacionamentos { get; set; }
        DbSet<DocumentoRelacionamento> DocumentoRelacionamentos { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
