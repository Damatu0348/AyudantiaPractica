using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.models;
using Bogus;

namespace api.src.Data
{
    public class DataSeeder
    {
        private static Random random = new Random();
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();
                if(!context.Roles.Any())
                {
                    context.Roles.AddRange
                    (
                        new Role {Nombre = "Admin"},
                        new Role {Nombre = "User"}
                    );
                    context.SaveChanges();
                }
                var existingRuts = new HashSet<string>();
                if(!context.Users.Any())
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(u => u.Rut, f => GenerateUniqueRandomRut(existingRuts))
                        .RuleFor(u => u.Name, f => f.Person.FullName)
                        .RuleFor(u => u.Email, f => f.Person.Email)
                        .RuleFor(u => u.RoleId, f => f.Random.Number(1, 2));
                    var users = userFaker.Generate(10);
                    context.Users.AddRange(users);
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Price, f => f.Random.Number(1000, 10000));
                    var products = productFaker.Generate(10);
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
                context.SaveChanges();                
            }
        }

        private static string GenerateUniqueRandomRut(HashSet<string> existingRuts)
        {
            string rut;
            do
            {
                rut = GenerateRandomRut();
            } while (existingRuts.Contains(rut));

            existingRuts.Add(rut);
            return rut;
        }

        private static string GenerateRandomRut()
        {
            int number = random.Next(1000000, 25000000); // Número de RUT aleatorio entre 1.000.000 y 25.000.000
            char dv = CalculateDv(number); // Calcula el dígito verificador (DV)
            return $"{number}-{dv}";
        }

        // Método para calcular el dígito verificador (DV)
        private static char CalculateDv(int rut)
        {
            int sum = 0;
            int multiplier = 2;

            while (rut > 0)
            {
                int digit = rut % 10;
                sum += digit * multiplier;
                multiplier = (multiplier == 7) ? 2 : multiplier + 1;
                rut /= 10;
            }

            int remainder = 11 - (sum % 11);
            if (remainder == 11)
            {
                return '0';
            }
            else if (remainder == 10)
            {
                return 'K';
            }
            else
            {
                return remainder.ToString()[0];
            }
        }
        }
}