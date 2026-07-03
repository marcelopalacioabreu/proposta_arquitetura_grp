using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Persistencia.Inicializadores
{
    public static class SeedData
    {
        public static void EnsureSeed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            var db = sp.GetService<ApplicationDbContext>();
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

                // Ensure a default setor exists for the organization
                var setor = db.OrganizacaoSetores.FirstOrDefault(s => s.Nome == "Setor padrão" && s.OrganizacaoId == org.Id);
                if (setor == null)
                {
                    setor = new OrganizacaoSetor
                    {
                        Nome = "Setor padrão",
                        OrganizacaoId = org.Id,
                        DataInsercao = DateTime.UtcNow,
                        Ativo = true
                    };
                    db.OrganizacaoSetores.Add(setor);
                    db.SaveChanges();
                }

                var exists = db.Usuarios.FirstOrDefault(u => u.Username == "admin");
                if (exists == null)
                {
                    var user = new Usuario
                    {
                        Nome = "Administrador",
                        Username = "admin",
                        Email = "admin@local",
                        DataInsercao = DateTime.UtcNow,
                        SenhaHash = HashPassword("admin"),
                        OrganizacaoId = org.Id,
                        OrganizacaoSetorId = setor.Id
                        ,Ativo = true
                    };

                    db.Usuarios.Add(user);
                    db.SaveChanges();
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
