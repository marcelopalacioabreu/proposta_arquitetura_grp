using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Persistencia.MYSQL;

namespace Retaguarda.Persistencia.Inicializadores
{
    public static class SeedData
    {
        public static void EnsureSeed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            var db = sp.GetService<Retaguarda.Persistencia.IApplicationDbContext>();
            if (db == null) return;

            try
            {
                // Ensure a default organization exists
                var org = db.Organizacoes.FirstOrDefault(o => o.Nome == "Organização padrão");
                if (org == null)
                {
                    org = new Organizacao
                    {
                        Nome = "Organização padrão",
                        DataInsercao = DateTime.UtcNow,
                        Ativo = true
                    };
                    db.Organizacoes.Add(org);
                    db.SaveChanges();
                }

                // Ensure a default unidade exists for the organization
                var unidade = db.OrganizacaoUnidades.FirstOrDefault(u => u.Nome == "Unidade padrão" && u.OrganizacaoId == org.Id);
                if (unidade == null)
                {
                    unidade = new OrganizacaoUnidade
                    {
                        Nome = "Unidade padrão",
                        OrganizacaoId = org.Id,
                        DataInsercao = DateTime.UtcNow,
                        Ativo = true
                    };
                    db.OrganizacaoUnidades.Add(unidade);
                    db.SaveChanges();
                }

                // Ensure a default setor exists for the organization and unit
                var setor = db.OrganizacaoSetores.FirstOrDefault(s => s.Nome == "Setor padrão" && s.OrganizacaoId == org.Id && s.OrganizacaoUnidadeId == unidade.Id);
                if (setor == null)
                {
                    setor = new OrganizacaoSetor
                    {
                        Nome = "Setor padrão",
                        OrganizacaoId = org.Id,
                        OrganizacaoUnidadeId = unidade.Id,
                        DataInsercao = DateTime.UtcNow,
                        Ativo = true
                    };
                    db.OrganizacaoSetores.Add(setor);
                    db.SaveChanges();
                }

                var exists = db.Usuarios.FirstOrDefault(u => u.Email == "admin@local");
                if (exists == null)
                {
                    var user = new Usuario
                    {
                        Nome = "Administrador",
                        Email = "admin@local",
                        DataInsercao = DateTime.UtcNow,
                        SenhaHash = HashPassword("admin"),
                        OrganizacaoId = org.Id,
                        SetorId = setor.Id,
                        UltimoAcessoOrganizacaoId = org.Id,
                        UltimoAcessoOrganizacaoUnidadeId = unidade.Id,
                        UltimoAcessoSetorId = setor.Id,
                        Ativo = true
                    };

                    db.Usuarios.Add(user);
                    db.SaveChanges();

                    // Ensure an administrator profile exists and associate the admin user to it
                    var adminPerfil = db.Perfis.FirstOrDefault(p => p.Nome == "Administrador");
                    if (adminPerfil == null)
                    {
                        adminPerfil = new Perfil { Nome = "Administrador", AdministradorDoSistema = true, OrganizacaoId = org.Id, DataInsercao = DateTime.UtcNow, Ativo = true };
                        db.Perfis.Add(adminPerfil);
                        db.SaveChanges();
                    }

                    var existsAssoc = db.PerfilUsuarios.FirstOrDefault(pu => pu.UsuarioId == user.Id && pu.PerfilId == adminPerfil.Id);
                    if (existsAssoc == null)
                    {
                        db.PerfilUsuarios.Add(new PerfilUsuario { UsuarioId = user.Id, PerfilId = adminPerfil.Id, OrganizacaoId = org.Id, DataInsercao = DateTime.UtcNow, Ativo = true });
                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                // swallow errors during seed to avoid blocking startup in dev
            }
        }

        // Local PBKDF2-SHA256 hasher matching Retaguarda.Servicos.Util.PasswordHasher
        private static string HashPassword(string password, int iterations = 100_000)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);

            return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }
    }
}
