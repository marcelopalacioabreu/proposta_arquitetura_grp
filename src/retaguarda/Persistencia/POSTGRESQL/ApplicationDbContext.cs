using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Dominio.Entidades.Base;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http;

namespace Retaguarda.Persistencia.POSTGRESQL
{
    public class ApplicationDbContext : Retaguarda.Persistencia.ApplicationDbContext, Retaguarda.Persistencia.IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options, httpContextAccessor)
        {
        }

        public DbSet<Organizacao> Organizacoes { get; set; } = null!;
        public DbSet<OrganizacaoSetor> OrganizacaoSetores { get; set; } = null!;
        public DbSet<OrganizacaoUnidade> OrganizacaoUnidades { get; set; } = null!;
        public DbSet<OrganizacaoUnidadeSetor> OrganizacaoUnidadeSetores { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Pessoa> Pessoas { get; set; } = null!;
        public DbSet<Perfil> Perfis { get; set; } = null!;
        public DbSet<PerfilUsuario> PerfilUsuarios { get; set; } = null!;
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; } = null!;
        public DbSet<SetorUsuario> SetorUsuarios { get; set; } = null!;
        public DbSet<Funcao> Funcoes { get; set; } = null!;
        // Address related
        public DbSet<Pais> Paises { get; set; } = null!;
        public DbSet<Uf> Ufs { get; set; } = null!;
        public DbSet<Municipio> Municipios { get; set; } = null!;
        public DbSet<Bairro> Bairros { get; set; } = null!;
        public DbSet<Logradouro> Logradouros { get; set; } = null!;
        public DbSet<Imovel> Imoveis { get; set; } = null!;
        public DbSet<Cep> Ceps { get; set; } = null!;
        public DbSet<Endereco> Enderecos { get; set; } = null!;
        // New catalog and relation entities
        public DbSet<NivelGoverno> NiveisGoverno { get; set; } = null!;
        public DbSet<TipoUnidade> TiposUnidade { get; set; } = null!;
        public DbSet<NaturezaJuridica> NaturezasJuridicas { get; set; } = null!;
        public DbSet<Situacao> Situacoes { get; set; } = null!;
        public DbSet<TipoEndereco> TiposEndereco { get; set; } = null!;
        public DbSet<TipoContato> TiposContato { get; set; } = null!;
        public DbSet<Contato> Contatos { get; set; } = null!;
        public DbSet<DocumentoTipo> DocumentoTipos { get; set; } = null!;
        public DbSet<Documento> Documentos { get; set; } = null!;
        public DbSet<TipoImovel> TiposImovel { get; set; } = null!;
        public DbSet<SituacaoImovel> SituacoesImovel { get; set; } = null!;

        public DbSet<OrquestracaoFluxoProcesso> OrquestracaoFluxoProcessos { get; set; } = null!;

        // relation tables
        public DbSet<OrganizacaoEndereco> OrganizacaoEnderecos { get; set; } = null!;
        public DbSet<OrganizacaoUnidadeEndereco> OrganizacaoUnidadeEnderecos { get; set; } = null!;
        public DbSet<OrganizacaoSetorEndereco> OrganizacaoSetorEnderecos { get; set; } = null!;
        public DbSet<PessoaEndereco> PessoaEnderecos { get; set; } = null!;
        public DbSet<UsuarioEndereco> UsuarioEnderecos { get; set; } = null!;

        public DbSet<ContatoRelacionamento> ContatoRelacionamentos { get; set; } = null!;
        public DbSet<DocumentoRelacionamento> DocumentoRelacionamentos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organizacao>(b =>
            {
                b.ToTable("Organizacoes");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Codigo).HasMaxLength(50);
                b.Property(x => x.Sigla).HasMaxLength(30);
                b.Property(x => x.RazaoSocial).HasMaxLength(300);
                b.Property(x => x.NomeFantasia).HasMaxLength(300);
                b.Property(x => x.Cnpj).HasMaxLength(14);
                b.Property(x => x.InscricaoEstadual).HasMaxLength(50);
                b.Property(x => x.InscricaoMunicipal).HasMaxLength(50);
                b.Property(x => x.HierarquiaCodigo).HasMaxLength(600);
                b.Property(x => x.Nivel);
            });

            modelBuilder.Entity<OrganizacaoSetor>(b =>
            {
                b.ToTable("OrganizacaoSetores");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Hierarquia).HasMaxLength(1000);
                b.HasOne(x => x.SetorPai).WithMany().HasForeignKey(x => x.SetorPaiId).OnDelete(DeleteBehavior.Restrict);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Setores).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Usuario>(b =>
            {
                b.ToTable("Usuarios");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Usuarios).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Pessoa).WithMany().HasForeignKey(x => x.PessoaId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Perfil>(b =>
            {
                b.ToTable("Perfis");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Perfis).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PerfilPermissao>(b =>
            {
                b.ToTable("PerfilPermissoes");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Perfil).WithMany(p => p.Permissoes).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Organizacao).WithMany().HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => new { x.PerfilId, x.Nome }).IsUnique();
            });

            modelBuilder.Entity<PerfilUsuario>(b =>
            {
                b.ToTable("PerfilUsuarios");
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Perfil).WithMany().HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SetorUsuario>(b =>
            {
                b.ToTable("SetorUsuarios");
                b.HasKey(x => x.Id);
                b.Property(x => x.HabilitarPermissoesNegativas).IsRequired();
                b.Property(x => x.Padrao).IsRequired().HasDefaultValue(false);
                b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Setor).WithMany().HasForeignKey(x => x.SetorId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Pessoa>(b =>
            {
                b.ToTable("Pessoas");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(300);
                b.Property(x => x.TipoPessoaChave).IsRequired().HasMaxLength(8);
                b.Property(x => x.Documento).HasMaxLength(100);
                b.Property(x => x.Email).HasMaxLength(200);
                b.Property(x => x.Telefone).HasMaxLength(50);
            });

            modelBuilder.Entity<OrganizacaoUnidade>(b =>
            {
                b.ToTable("OrganizacaoUnidades");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Codigo).HasMaxLength(50);
                b.Property(x => x.Sigla).HasMaxLength(30);
                b.Property(x => x.Cnpj).HasMaxLength(14);
                b.Property(x => x.HierarquiaCodigo).HasMaxLength(600);
                b.Property(x => x.HierarquiaNome).HasMaxLength(1000);
                b.Property(x => x.Nivel);
                b.HasOne(x => x.Organizacao).WithMany().HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrganizacaoUnidadeSetor>(b =>
            {
                b.ToTable("OrganizacaoUnidadeSetores");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.OrganizacaoUnidade).WithMany().HasForeignKey(x => x.OrganizacaoUnidadeId).OnDelete(DeleteBehavior.Cascade);
            });

            // Address model
            modelBuilder.Entity<Pais>(b =>
            {
                b.ToTable("Paises");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Uf>(b =>
            {
                b.ToTable("Ufs");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Sigla).IsRequired().HasMaxLength(8);
                b.HasOne(x => x.Pais).WithMany().HasForeignKey(x => x.PaisId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Municipio>(b =>
            {
                b.ToTable("Municipios");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Uf).WithMany().HasForeignKey(x => x.UfId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Bairro>(b =>
            {
                b.ToTable("Bairros");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Logradouro>(b =>
            {
                b.ToTable("Logradouros");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(300);
                b.HasOne(x => x.Bairro).WithMany().HasForeignKey(x => x.BairroId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Imovel>(b =>
            {
                b.ToTable("Imoveis");
                b.HasKey(x => x.Id);
                b.Property(x => x.Cadastro).IsRequired().HasMaxLength(200);
                b.Property(x => x.Numero).HasMaxLength(50);
                b.Property(x => x.Complemento).HasMaxLength(500);
                b.Property(x => x.InscricaoImobiliaria).HasMaxLength(200);
                b.Property(x => x.Latitude);
                b.Property(x => x.Longitude);
                b.HasOne(x => x.Logradouro).WithMany().HasForeignKey(x => x.LogradouroId).OnDelete(DeleteBehavior.SetNull);
                b.HasOne(x => x.Cep).WithMany().HasForeignKey(x => x.CepId).OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Cep>(b =>
            {
                b.ToTable("Ceps");
                b.HasKey(x => x.Id);
                b.Property(x => x.Codigo).IsRequired().HasMaxLength(20);
                // Imovel linkage is optional
                b.HasOne(x => x.Imovel).WithMany().HasForeignKey(x => x.ImovelId).OnDelete(DeleteBehavior.SetNull);
            });

            // catalogs and relation tables
            modelBuilder.Entity<NivelGoverno>(b => { b.ToTable("NiveisGoverno"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<TipoUnidade>(b => { b.ToTable("TipoUnidade"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<NaturezaJuridica>(b => { b.ToTable("NaturezasJuridicas"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<Situacao>(b => { b.ToTable("Situacoes"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<TipoEndereco>(b => { b.ToTable("TipoEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<TipoContato>(b => { b.ToTable("TipoContatos"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<Contato>(b => { b.ToTable("Contatos"); b.HasKey(x=>x.Id); b.Property(x=>x.Nome).HasMaxLength(200); b.Property(x=>x.ContatoValor).HasMaxLength(500); });
            modelBuilder.Entity<DocumentoTipo>(b => { b.ToTable("DocumentoTipos"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<Documento>(b => { b.ToTable("Documentos"); b.HasKey(x=>x.Id); b.Property(x=>x.Numero).HasMaxLength(200); b.Property(x=>x.Digito).HasMaxLength(20); b.Property(x=>x.OrgaoEmissor).HasMaxLength(100); b.Property(x=>x.UfEmissor).HasMaxLength(8); b.Property(x=>x.Observacao).HasMaxLength(1000); });
            modelBuilder.Entity<TipoImovel>(b => { b.ToTable("TipoImovel"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<SituacaoImovel>(b => { b.ToTable("SituacaoImovel"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });

            modelBuilder.Entity<OrganizacaoEndereco>(b=>{ b.ToTable("OrganizacaoEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.EnderecoPrincipal).IsRequired(); });
            modelBuilder.Entity<OrganizacaoUnidadeEndereco>(b=>{ b.ToTable("OrganizacaoUnidadeEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.EnderecoPrincipal).IsRequired(); });
            modelBuilder.Entity<OrganizacaoSetorEndereco>(b=>{ b.ToTable("OrganizacaoSetorEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.EnderecoPrincipal).IsRequired(); });
            modelBuilder.Entity<PessoaEndereco>(b=>{ b.ToTable("PessoaEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.EnderecoPrincipal).IsRequired(); });
            modelBuilder.Entity<UsuarioEndereco>(b=>{ b.ToTable("UsuarioEnderecos"); b.HasKey(x=>x.Id); b.Property(x=>x.EnderecoPrincipal).IsRequired(); });

            modelBuilder.Entity<ContatoRelacionamento>(b=>{ b.ToTable("ContatoRelacionamentos"); b.HasKey(x=>x.Id); });
            modelBuilder.Entity<DocumentoRelacionamento>(b=>{ b.ToTable("DocumentoRelacionamentos"); b.HasKey(x=>x.Id); });

            modelBuilder.Entity<Endereco>(b =>
            {
                b.ToTable("Enderecos");
                b.HasKey(x => x.Id);
                b.Property(x => x.Complemento).HasMaxLength(500);
                b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Cep).WithMany().HasForeignKey(x => x.CepId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Funcao>(b =>
            {
                b.ToTable("Funcoes");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Funcoes).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrquestracaoFluxoProcesso>(b =>
            {
                b.ToTable("OrquestracaoFluxoProcessos");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Descricao).HasMaxLength(2000);
                b.Property(x => x.WorkflowDefinitionId).HasMaxLength(200);
                b.Property(x => x.WorkflowVersion);
            });
        }

        public override int SaveChanges()
        {
            ApplyPadraoCampos();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyPadraoCampos();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyPadraoCampos()
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<MultilocatarioEntidade>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DataInsercao = utcNow;
                    entry.Entity.DataAlteracao = null;
                    entry.Entity.Ativo = true;

                    // apply atuacao context when available and when sensible
                    // apply escopo values from HttpContext.Items when available (no compile-time project refs)
                    var ctx = _httpContextAccessor?.HttpContext;
                    long? org = null, unidade = null, setor = null;
                    if (ctx != null)
                    {
                        if (ctx.Items.ContainsKey("escopo.organizacaoId")) org = Convert.ToInt64(ctx.Items["escopo.organizacaoId"]);
                        if (ctx.Items.ContainsKey("escopo.organizacaoUnidadeId")) unidade = Convert.ToInt64(ctx.Items["escopo.organizacaoUnidadeId"]);
                        if (ctx.Items.ContainsKey("escopo.setorId")) setor = Convert.ToInt64(ctx.Items["escopo.setorId"]);
                    }
                    var t = entry.Entity.GetType().Name;
                    // avoid setting OrganizacaoId on Organizacao entity itself
                    if (org.HasValue && t != nameof(Retaguarda.Dominio.Entidades.Organizacao))
                    {
                        if (!entry.Entity.OrganizacaoId.HasValue) entry.Entity.OrganizacaoId = org;
                    }
                    if (unidade.HasValue)
                    {
                        if (!entry.Entity.OrganizacaoUnidadeId.HasValue) entry.Entity.OrganizacaoUnidadeId = unidade;
                    }
                    if (setor.HasValue)
                    {
                        if (!entry.Entity.SetorId.HasValue) entry.Entity.SetorId = setor;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Always set DataAlteracao
                    entry.Entity.DataAlteracao = utcNow;

                    // Prevent ordinary updates from changing Ativo. Allow Ativo change only when it's the sole modified property.
                    var modifiedNonAtivo = entry.Properties.Any(p => p.Metadata.Name != nameof(MultilocatarioEntidade.Ativo) && p.IsModified);
                    if (modifiedNonAtivo)
                    {
                        // restore original value
                        var original = entry.OriginalValues.GetValue<bool>(nameof(MultilocatarioEntidade.Ativo));
                        entry.Entity.Ativo = original;
                    }
                }
            }
        }
    }
}
