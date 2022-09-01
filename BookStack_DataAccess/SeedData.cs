using BookStack_DataAccess.Authorization;
using BookStack_DataAccess.Data;
using BookStack_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;




// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -udl -outDir Pages\Contacts --referenceScriptLibraries

namespace BookStack_DataAccess
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@BookStack.com");
                await EnsureRole(serviceProvider, adminID, Constants.AdministratorRole);

                // allowed user can create and edit contacts that they create
                var Client1ID = await EnsureUser(serviceProvider, testUserPw, "Client1@BookStack.com");
                await EnsureRole(serviceProvider, Client1ID, Constants.ClientRole);

                var Client2ID = await EnsureUser(serviceProvider, testUserPw, "Client2@BookStack.com");
                await EnsureRole(serviceProvider, Client2ID, Constants.ClientRole);

                SeedDB(context, adminID);
                SeedDB(context, Client1ID);
                SeedDB(context, Client2ID);
                SeedCategories(context);
                SeedBooks(context);

            }
        }



        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string adminID)
        {

            context.SaveChanges();
        }

        public static void SeedCategories(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }
            context.Categories.AddRange
            (
              new Category
              {
                  Name = "Clasic",
                  DisplayOrder = 1
              },
              new Category
              {

                  Name = "Computer Science",
                  DisplayOrder = 2

              },
                new Category
                {

                    Name = "Comic",
                    DisplayOrder = 3

                },
                new Category
                {

                    Name = "All Categories",
                    DisplayOrder = 4

                });
            context.SaveChanges();
        }



        public static void SeedBooks(ApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }
            var classicCategory = context.Categories.Where(a => a.Name == "Clasic").Single();
            var comicCategory = context.Categories.Where(a => a.Name == "Comic").Single();
            var computerCategory = context.Categories.Where(a => a.Name == "Computer Science").Single();
            if (classicCategory != null & comicCategory != null & computerCategory != null)
            {
                var classicId = classicCategory.Id;
                var comicId = comicCategory.Id;
                var computerId = computerCategory.Id;

                context.Products.AddRange
                    (new Product
                    {
                        Name = "Animal Farm",
                        Description = "Animal Farm (ART STORIA Literary Classics Annotated Edition) is an allegorical novella by George Orwell, first published in England on 17 August 1945. The book tells the story of a group of farm animals who rebel against their human farmer, hoping to create a society where the animals can be equal, free, and happy.",
                        CategoryId = classicId,
                        Price = 27,
                        ImageUrl = "",

                    },
                     new Product
                     {
                         Name = "Crime and Punishment",
                         Description = "On an exceptionally hot evening early in July a young man came out of the garret in which he lodged in S. Place and walked slowly, as though in hesitation, towards K. bridge.He had successfully avoided meeting his landlady on the staircase.His garret was under the roof of a high, five - storied house and was more like a cupboard than a room.The landlady who provided him with garret,dinners,and attendance,lived on the floor below,and every time he went out he was obliged to pass her kitchen,the door of which invariably stood open.And each time he passed,the young man had a sick,frightened feeling,which made him scowl and feel ashamed.He was hopelessly in debt to his landlady,and was afraid of meeting her.",
                         CategoryId = classicId,
                         Price = 32,
                         ImageUrl = "",

                     },
                        new Product
                        {
                            Name = "Anna Karenina",
                            Description = "A beautiful society wife from St. Petersburg, determined to live life on her own terms, sacrifices everything to follow her conviction that love is stronger than duty. A socially inept but warmhearted landowner pursues his own visions instead of conforming to conventional views. The adulteress and the philosopher head the vibrant cast of characters in Anna Karenina, Tolstoy's tumultuous tale of passion and self-discovery.This novel marks a turning point in the author's career, the juncture at which he turned from fiction toward faith. Set against a backdrop of the historic social changes that swept Russia during the late nineteenth century, it reflects Tolstoy's own personal and psychological transformation.Two worlds collide in the course of this epochal story: that of the old - time aristocrats,who struggle to uphold their traditions of serfdom and authoritarian government,and that of the Westernizing liberals,who promote technology, rationalism,and democracy.This cultural clash unfolds in a compelling, emotional drama of seduction,betrayal,and redemption.",
                            CategoryId = classicId,
                            Price = 37,
                            ImageUrl = "",

                        },
                          new Product
                          {
                              Name = "Nineteen Eighty-Four",
                              Description = "Animal Farm (ART STORIA Literary Classics Annotated Edition) is an allegorical novella by George Orwell, first published in England on 17 August 1945. The book tells the story of a group of farm animals who rebel against their human farmer, hoping to create a society where the animals can be equal, free, and happy.",
                              CategoryId = classicId,
                              Price = 27,
                              ImageUrl = "",

                          },
                          new Product
                          {
                              Name = "Fahrenheit 451",
                              Description = "Guy Montag is a fireman. His job is to destroy the most illegal of commodities, the printed book, along with the houses in which they are hidden. Montag never questions the destruction and ruin his actions produce, returning each day to his bland life and wife, Mildred, who spends all day with her television “family.” But when he meets an eccentric young neighbor, Clarisse, who introduces him to a past where people didn’t live in fear and to a present where one sees the world through the ideas in books instead of the mindless chatter of television, Montag begins to question everything he has ever known category 2: comic",
                              CategoryId = classicId,
                              Price = 37,
                              ImageUrl = "",

                          },
                            new Product
                            {
                                Name = " HUCKLEBERRY FINN",
                                Description = "Animal Farm (ART STORIA Literary Classics Annotated Edition) is an allegorical novella by George Orwell, first published in England on 17 August 1945. The book tells the story of a group of farm animals who rebel against their human farmer, hoping to create a society where the animals can be equal, free, and happy.",
                                CategoryId = classicId,
                                Price = 26,
                                ImageUrl = "",

                            },
                               new Product
                               {
                                   Name = "4:Frankenstein",
                                   Description = "Frankenstein; or, The Modern Prometheus, is a novel written by English author Mary Shelley about the young student of science Victor Frankenstein, who creates a grotesque but sentient creature in an unorthodox scientific experiment. Shelley started writing the story when she was eighteen, and the novel was published when she was twenty. The first edition was published anonymously in London in 1818. Shelley's name appears on the second edition, published in France in 1823.Shelley had travelled through Europe in 1814, journeying along the river Rhine in Germany with a stop in Gernsheim which is just 17 km (10 mi) away from Frankenstein Castle, where two centuries before an alchemist was engaged in experiments. Later, she travelled in the region of Geneva (Switzerland)—where much of the story takes place—and the topics of galvanism and other similar occult ideas were themes of conversation among her companions, particularly her lover and future husband, Percy Shelley. Mary, Percy, Lord Byron, and John Polidori decided to have a competition to see who could write the best horror story. After thinking for days, Shelley dreamt about a scientist who created life and was horrified by what he had made; her dream later evolved into the story within the novel.",
                                   CategoryId = classicId,
                                   Price = 41,
                                   ImageUrl = "",

                               }, new Product
                               {
                                   Name = "Pride and Prejudice Jane Austen",
                                   Description = "SISTERS ELIZABETH & JANE BENNET come from a respectable family, but their parents and sisters do not always behave with propriety. When their romantic hopes are dashed, Elizabeth discovers that proud Mr. Darcy has played a role in their disappointments. But things go from bad to worse, when their youngest sister puts the family’s reputation at risk. Can they ever recover from the association? Will love continue to elude them?",
                                   CategoryId = classicId,
                                   Price = 35,
                                   ImageUrl = "",

                               },
                                 new Product
                                 {
                                     Name = "call of the wild",
                                     Description = "There is an ecstasy that marks the summit of life, and beyond which life cannot rise. And such is the paradox of living, this ecstasy comes when one is most alive, and it comes as a complete forgetfulness that one is alive.",
                                     CategoryId = classicId,
                                     Price = 29,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "war and peace",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = classicId,
                                     Price = 25,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Algorithms",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 31,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "A Programmer's Guide to Computer Science",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 42,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Python: - The Bible",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 36,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Help Your Kids with Computer Science",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 22,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Building Data Science Solutions with Anaconda",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 43,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "The Pragmatic Programmer",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 32,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Pandas in 7 Days",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 57,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Agile Machine Learning with DataRobot",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 46,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Coding for Beginners",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 34,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Classic Computer Science Problems in Python",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = computerId,
                                     Price = 27,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "SpongeBob",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = comicId,
                                     Price = 31,
                                     ImageUrl = "",

                                 },
                                 new Product
                                 {
                                     Name = "Venom vs. Carnage",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = comicId,
                                     Price = 41,
                                     ImageUrl = "",

                                 }
                                 ,
                                 new Product
                                 {
                                     Name = "Sabrina: 60 Magical Stories",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = comicId,
                                     Price = 22,
                                     ImageUrl = "",

                                 }
                                 ,
                                 new Product
                                 {
                                     Name = "Cat Kid Comic Club",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = comicId,
                                     Price = 31,
                                     ImageUrl = "",

                                 }
                                 ,
                                 new Product
                                 {
                                     Name = "Batman: Three Jokers",
                                     Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                     CategoryId = comicId,
                                     Price = 29,
                                     ImageUrl = "",

                                 },
                                     new Product
                                     {
                                         Name = "The Last Ronin",
                                         Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                         CategoryId = comicId,
                                         Price = 38,
                                         ImageUrl = "",

                                     }
                                     ,
                                     new Product
                                     {
                                         Name = "Nova Dragon",
                                         Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                         CategoryId = comicId,
                                         Price = 47,
                                         ImageUrl = "",
                                     }
                                         ,
                                         new Product
                                         {
                                             Name = "The Vault of Horror",
                                             Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                             CategoryId = comicId,
                                             Price = 27,
                                             ImageUrl = "",

                                         },
                                             new Product
                                             {
                                                 Name = "Iron Man",
                                                 Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                                 CategoryId = comicId,
                                                 Price = 51,
                                                 ImageUrl = "",

                                             },
                                             new Product
                                             {
                                                 Name = "Archie: 80 Years of Christmas",
                                                 Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                                 CategoryId = comicId,
                                                 Price = 27,
                                                 ImageUrl = "",


                                             }
                                 );

                context.SaveChanges();

            }
        }
    }
}
