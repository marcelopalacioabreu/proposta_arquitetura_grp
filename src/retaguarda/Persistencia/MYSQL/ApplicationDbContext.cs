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
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Perfil> Perfis { get; set; } = null!;
        public DbSet<PerfilPermissao> PerfilPermissoes { get; set; } = null!;
        public DbSet<Funcao> Funcoes { get; set; } = null!;

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
