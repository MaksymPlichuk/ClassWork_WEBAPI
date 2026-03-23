using ClassWork_WEBAPI.DAL.Entities;
using ClassWork_WEBAPI.DAL.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWork_WEBAPI.DAL.Initializer
{
    public static class Seeder
    {
        public static async Task Seed(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUserEntity>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRoleEntity>>();

            await context.Database.MigrateAsync();

            if (!roleManager.Roles.Any())
            {
                var adminRole = new AppRoleEntity
                {
                    Name = "admin"
                };
                var userRole = new AppRoleEntity
                {
                    Name = "user"
                };
                await roleManager.CreateAsync(adminRole);
                await roleManager.CreateAsync(userRole);
            }

            var genres = new List<GenreEntity>();

            if (!context.Genres.Any())
            {
                genres.AddRange(
                   new GenreEntity { Name = "Фентезі" },
                   new GenreEntity { Name = "Фантастика" },
                   new GenreEntity { Name = "Детективи" },
                   new GenreEntity { Name = "Романтична проза" },
                   new GenreEntity { Name = "Трилери та жахи" },
                   new GenreEntity { Name = "Класична література" },
                   new GenreEntity { Name = "Комікси та манґи" }
               );

                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }
            if (!context.Authors.Any())
            {
                if (genres.Count == 0)
                {
                    genres = await context.Genres.ToListAsync();
                }

                var authors = new List<AuthorEntity>()
                {
                    new AuthorEntity
                    {
                        Name = "Стівен Кінг",
                        BirthDate = new DateTime(1947, 9, 21).ToUniversalTime() ,
                        Books = new List<BookEntity>
                        {
                            new BookEntity { Title = "Сяйво", Description = "Готель, що зводить з розуму.", Rating = 4.8f, Pages = 447, PublishYear = 1977, Genres = new List<GenreEntity> { genres[4] } },
                            new BookEntity { Title = "Воно", Description = "Зло, що живе у каналізації.", Rating = 4.9f, Pages = 1138, PublishYear = 1986, Genres = new List<GenreEntity> { genres[4] } },
                            new BookEntity { Title = "Мізері", Description = "Про фанатку та її полоненого письменника.", Rating = 4.7f, Pages = 310, PublishYear = 1987, Genres = new List<GenreEntity> { genres[4] } },
                            new BookEntity { Title = "Керрі", Description = "Дівчина з телекінезом.", Rating = 4.5f, Pages = 199, PublishYear = 1974, Genres = new List<GenreEntity> { genres[4] } },
                            new BookEntity { Title = "11/22/63", Description = "Подорож у часі для порятунку Кеннеді.", Rating = 4.9f, Pages = 849, PublishYear = 2011, Genres = new List<GenreEntity> { genres[1], genres[4] } }
                        }
                    },
                    new AuthorEntity
                    {
                        Name = "Аґата Крісті",
                        BirthDate = new DateTime(1890, 9, 15).ToUniversalTime(),
                        Books = new List<BookEntity>
                        {
                            new BookEntity { Title = "Вбивство у «Східному експресі»", Description = "Еркюль Пуаро розслідує злочин у поїзді.", Rating = 4.9f, Pages = 256, PublishYear = 1934, Genres = new List<GenreEntity> { genres[2] } },
                            new BookEntity { Title = "І не лишилось жодного", Description = "Десять людей на безлюдному острові.", Rating = 5.0f, Pages = 272, PublishYear = 1939, Genres = new List<GenreEntity> { genres[2], genres[4] } },
                            new BookEntity { Title = "Смерть на Нілі", Description = "Трагедія під час круїзу.", Rating = 4.7f, Pages = 352, PublishYear = 1937, Genres = new List<GenreEntity> { genres[2] } },
                            new BookEntity { Title = "Вбивство Роджера Акройда", Description = "Детектив з неочікуваним фіналом.", Rating = 4.8f, Pages = 288, PublishYear = 1926, Genres = new List<GenreEntity> { genres[2] } },
                            new BookEntity { Title = "Тіло в бібліотеці", Description = "Розслідування міс Марпл.", Rating = 4.5f, Pages = 224, PublishYear = 1942, Genres = new List<GenreEntity> { genres[2] } }
                        }
                    },
                    new AuthorEntity
                    {
                        Name = "Джон Р. Р. Толкін",
                        BirthDate = new DateTime(1892, 1, 3).ToUniversalTime(),
                        Books = new List<BookEntity>
                        {
                            new BookEntity { Title = "Гобіт", Description = "Подорож Більбо до Одинокої гори.", Rating = 4.8f, Pages = 310, PublishYear = 1937, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Братство Персня", Description = "Початок великої подорожі.", Rating = 4.9f, Pages = 423, PublishYear = 1954, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Дві вежі", Description = "Облога Гельмової западини.", Rating = 4.9f, Pages = 352, PublishYear = 1954, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Повернення короля", Description = "Вирішальна битва за Середзем'я.", Rating = 5.0f, Pages = 416, PublishYear = 1955, Genres = new List<GenreEntity> { genres[0] } },
                            new BookEntity { Title = "Сильмариліон", Description = "Історія створення Арди.", Rating = 4.6f, Pages = 365, PublishYear = 1977, Genres = new List<GenreEntity> { genres[0], genres[5] } }
                        }
                    },
                    new AuthorEntity
                    {
                        Name = "Джордж Орвелл",
                        BirthDate = new DateTime(1903, 6, 25).ToUniversalTime(),
                        Books = new List<BookEntity>
                        {
                            new BookEntity { Title = "1984", Description = "Антиутопія про Великого Брата.", Rating = 4.9f, Pages = 328, PublishYear = 1949, Genres = new List<GenreEntity> { genres[1], genres[5] } },
                            new BookEntity { Title = "Колгосп тварин", Description = "Сатира на диктатуру.", Rating = 4.8f, Pages = 112, PublishYear = 1945, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "У злиднях Парижа і Лондона", Description = "Автобіографічні нариси.", Rating = 4.3f, Pages = 230, PublishYear = 1933, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Дні в Бірмі", Description = "Роман про колоніальне життя.", Rating = 4.1f, Pages = 300, PublishYear = 1934, Genres = new List<GenreEntity> { genres[5] } },
                            new BookEntity { Title = "Данина Каталонії", Description = "Репортаж з громадянської війни.", Rating = 4.6f, Pages = 280, PublishYear = 1938, Genres = new List<GenreEntity> { genres[5] } }
                        }
                    }
                };

                await context.Authors.AddRangeAsync(authors);
                await context.SaveChangesAsync();
            }
        }
    }
}
