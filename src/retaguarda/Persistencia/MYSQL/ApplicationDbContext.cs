using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Dominio.Entidades.Base;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Http;

namespace Retaguarda.Persistencia.MYSQL
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
        public DbSet<PessoaFisica> PessoasFisicas { get; set; } = null!;
        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; } = null!;
        public DbSet<Perfil> Perfis { get; set; } = null!;
        public DbSet<PerfilUsuario> PerfilUsuarios { get; set; } = null!;
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; } = null!;
        public DbSet<Tipo> Tipos { get; set; } = null!;
        // Address related
        public DbSet<EnderecoPais> EnderecoPaises { get; set; } = null!;
        public DbSet<EnderecoUF> EnderecoUFs { get; set; } = null!;
        public DbSet<EnderecoMunicipio> EnderecoMunicipios { get; set; } = null!;
        public DbSet<EnderecoBairro> EnderecoBairros { get; set; } = null!;
        public DbSet<EnderecoLogradouro> EnderecoLogradouros { get; set; } = null!;
        public DbSet<Imovel> Imoveis { get; set; } = null!;
        public DbSet<EnderecoCEP> EnderecoCEPs { get; set; } = null!;
        public DbSet<Endereco> Enderecos { get; set; } = null!;
        // New catalog and relation entities
        public DbSet<NivelGoverno> NiveisGoverno { get; set; } = null!;
        public DbSet<NaturezaJuridica> NaturezasJuridicas { get; set; } = null!;
        public DbSet<Situacao> Situacoes { get; set; } = null!;
        public DbSet<Contato> Contatos { get; set; } = null!;
        public DbSet<Documento> Documentos { get; set; } = null!;

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
            });

            modelBuilder.Entity<OrganizacaoSetor>(b =>
            {
                b.ToTable("OrganizacaoSetores");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.CodigoHierarquico).HasMaxLength(1000);
            });

            modelBuilder.Entity<Usuario>(b =>
            {
                b.ToTable("Usuarios");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Email).HasMaxLength(200);
                b.Property(x => x.SenhaHash).HasMaxLength(500);
            });

            modelBuilder.Entity<Perfil>(b =>
            {
                b.ToTable("Perfis");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.AdministradorDoSistema).IsRequired().HasDefaultValue(false);
            });

            modelBuilder.Entity<PerfilPermissao>(b =>
            {
                b.ToTable("PerfilPermissoes");
                b.HasKey(x => x.Id);
                b.Property(x => x.Chave).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Perfil).WithMany(p => p.Permissoes).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
                b.HasIndex(x => new { x.PerfilId, x.Chave }).IsUnique();
            });

            modelBuilder.Entity<PerfilUsuario>(b =>
            {
                b.ToTable("PerfilUsuarios");
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Perfil).WithMany().HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Pessoa>(b =>
            {
                b.ToTable("Pessoas");
                b.HasKey(x => x.Id);
                b.Property(x => x.TipoPessoa).IsRequired();
                b.HasDiscriminator<int>("Discriminator")
                    .HasValue<Pessoa>(0)
                    .HasValue<PessoaFisica>(1)
                    .HasValue<PessoaJuridica>(2);
            });

            modelBuilder.Entity<PessoaFisica>(b =>
            {
                b.ToTable("Pessoas");
                b.Property(x => x.Nome).IsRequired().HasMaxLength(300);
                b.Property(x => x.NomeSocial).HasMaxLength(300);
                b.Property(x => x.Cpf).HasMaxLength(14);
                b.Property(x => x.NomeMae).HasMaxLength(300);
                b.Property(x => x.NomePai).HasMaxLength(300);
            });

            modelBuilder.Entity<PessoaJuridica>(b =>
            {
                b.ToTable("Pessoas");
                b.Property(x => x.RazaoSocial).IsRequired().HasMaxLength(300);
                b.Property(x => x.NomeFantasia).HasMaxLength(300);
                b.Property(x => x.Cnpj).HasMaxLength(14);
                b.Property(x => x.Anotacoes).HasMaxLength(2000);
                b.Property(x => x.InscricaoEstadual).HasMaxLength(50);
                b.Property(x => x.InscricaoMunicipal).HasMaxLength(50);
            });

            modelBuilder.Entity<OrganizacaoUnidade>(b =>
            {
                b.ToTable("OrganizacaoUnidades");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Codigo).HasMaxLength(50);
                b.Property(x => x.Sigla).HasMaxLength(30);
                b.Property(x => x.Nivel);
            });

            modelBuilder.Entity<OrganizacaoUnidadeSetor>(b =>
            {
                b.ToTable("OrganizacaoUnidadeSetores");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.OrganizacaoUnidade).WithMany().HasForeignKey(x => x.OrganizacaoUnidadeId).OnDelete(DeleteBehavior.Cascade);
            });

            // Address model
            modelBuilder.Entity<EnderecoPais>(b =>
            {
                b.ToTable("Paises");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<EnderecoUF>(b =>
            {
                b.ToTable("Ufs");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Sigla).IsRequired().HasMaxLength(8);
                b.HasOne(x => x.Pais).WithMany().HasForeignKey(x => x.PaisId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EnderecoMunicipio>(b =>
            {
                b.ToTable("Municipios");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Uf).WithMany().HasForeignKey(x => x.UfId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EnderecoBairro>(b =>
            {
                b.ToTable("Bairros");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EnderecoLogradouro>(b =>
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
                b.Property(x => x.InscricaoImobiliaria).HasMaxLength(200);
                b.Property(x => x.Latitude);
                b.Property(x => x.Longitude);
            });

            modelBuilder.Entity<EnderecoCEP>(b =>
            {
                b.ToTable("EnderecoCEPs");
                b.HasKey(x => x.Id);
                b.Property(x => x.Codigo).IsRequired().HasMaxLength(20);
            });

            // catalogs and relation tables
            modelBuilder.Entity<NivelGoverno>(b => { b.ToTable("NiveisGoverno"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<NaturezaJuridica>(b => { b.ToTable("NaturezasJuridicas"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).HasMaxLength(50); b.Property(x=>x.Nome).HasMaxLength(200); });
            modelBuilder.Entity<Situacao>(b => { b.ToTable("Situacoes"); b.HasKey(x=>x.Id); b.Property(x=>x.Codigo).IsRequired().HasMaxLength(50); b.Property(x=>x.Nome).IsRequired().HasMaxLength(200); b.Property(x=>x.Contexto).IsRequired().HasMaxLength(50); b.Property(x=>x.Descricao).HasMaxLength(2000); b.HasIndex(x => new { x.OrganizacaoId, x.Contexto, x.Ativo }).HasName("idx_Situacoes_Contexto_Ativo"); b.HasIndex(x => new { x.Codigo, x.Contexto, x.OrganizacaoId }).IsUnique().HasName("idx_Situacoes_Codigo_Contexto_Unico"); });
            modelBuilder.Entity<Tipo>(b => 
            { 
                b.ToTable("Tipos"); 
                b.HasKey(x=>x.Id); 
                b.Property(x=>x.Codigo).IsRequired().HasMaxLength(50); 
                b.Property(x=>x.Nome).IsRequired().HasMaxLength(200); 
                b.Property(x=>x.Contexto).IsRequired().HasMaxLength(50);
                b.Property(x=>x.Descricao).HasMaxLength(2000); 
                // Índices para performance
                b.HasIndex(x => new { x.OrganizacaoId, x.Contexto, x.Ativo }).HasName("idx_Tipos_Contexto_Ativo");
                b.HasIndex(x => new { x.Codigo, x.Contexto, x.OrganizacaoId }).IsUnique().HasName("idx_Tipos_Codigo_Contexto_Unico");
            });
            modelBuilder.Entity<Contato>(b => { b.ToTable("Contatos"); b.HasKey(x=>x.Id); b.Property(x=>x.Nome).HasMaxLength(200); b.Property(x=>x.ContatoValor).HasMaxLength(500); });
            modelBuilder.Entity<Documento>(b => { b.ToTable("Documentos"); b.HasKey(x=>x.Id); b.Property(x=>x.Numero).HasMaxLength(200); b.Property(x=>x.Digito).HasMaxLength(20); b.Property(x=>x.OrgaoEmissor).HasMaxLength(100); b.Property(x=>x.UfEmissor).HasMaxLength(8); b.Property(x=>x.Observacao).HasMaxLength(1000); });

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
                b.HasOne(x => x.Cep).WithMany().HasForeignKey(x => x.CepId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrquestracaoFluxoProcesso>(b =>
            {
                b.ToTable("OrquestracaoFluxoProcessos");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.Property(x => x.Descricao).HasMaxLength(2000);
                b.Property(x => x.WorkflowDefinitionId).HasMaxLength(200);
                b.Property(x => x.WorkflowVersion);
                b.Property(x => x.WorkflowJson).HasColumnType("longtext");
                b.Property(x => x.WorkflowNome).HasMaxLength(500);
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
