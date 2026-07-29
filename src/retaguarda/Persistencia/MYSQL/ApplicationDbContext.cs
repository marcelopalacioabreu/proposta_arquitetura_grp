using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Persistencia.MYSQL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Organizacao> Organizacoes { get; set; } = null!;
        public DbSet<OrganizacaoSetor> OrganizacaoSetores { get; set; } = null!;
        public DbSet<OrganizacaoUnidade> OrganizacaoUnidades { get; set; } = null!;
        public DbSet<OrganizacaoUnidadeSetor> OrganizacaoUnidadeSetores { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organizacao>(b =>
            {
                b.ToTable("Organizacoes");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<OrganizacaoSetor>(b =>
            {
                b.ToTable("OrganizacaoSetores");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Setores).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Usuario>(b =>
            {
                b.ToTable("Usuarios");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
                b.HasOne(x => x.Organizacao).WithMany(o => o.Usuarios).HasForeignKey(x => x.OrganizacaoId).OnDelete(DeleteBehavior.Cascade);
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
                b.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Setor).WithMany().HasForeignKey(x => x.SetorId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrganizacaoUnidade>(b =>
            {
                b.ToTable("OrganizacaoUnidades");
                b.HasKey(x => x.Id);
                b.Property(x => x.Nome).IsRequired().HasMaxLength(200);
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
                b.HasOne(x => x.Logradouro).WithMany().HasForeignKey(x => x.LogradouroId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Cep>(b =>
            {
                b.ToTable("Ceps");
                b.HasKey(x => x.Id);
                b.Property(x => x.Codigo).IsRequired().HasMaxLength(20);
                b.HasOne(x => x.Imovel).WithMany().HasForeignKey(x => x.ImovelId).OnDelete(DeleteBehavior.Cascade);
            });

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
        }
    }
}
