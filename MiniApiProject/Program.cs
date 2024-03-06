using Microsoft.EntityFrameworkCore;
using MiniApiProject.Data;
using MiniApiProject.Models;

namespace MiniApiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionstring = builder.Configuration.GetConnectionString("MiniApiDb");
            builder.Services.AddDbContext<MiniApiContext>(options=>options.UseSqlServer(connectionstring));
            var app = builder.Build();


            //get all the people in the database
            app.MapGet("/api/getall", (MiniApiContext dbContext) =>
            {

                var people = dbContext.Persons


                .Include(p => p.Phones)
                .Include(p => p.Interest_Persons)
                .ThenInclude(p => p.Interest)
                .Include(p => p.Link_Persons)
                .ThenInclude(p => p.Link)
                .ToList();

                var result = people.Select(p => new
                {
                    p.FirstName,
                    p.LastName,
                    Phones = p.Phones.Select(p => new { p.PhoneNumber }).ToList(),
                    Interests = p.Interest_Persons.Select(pi => new { pi.Interest.InterestId, pi.Interest.Titel, pi.Interest.Description }).ToList(),
                    Links = p.Link_Persons.Select(lp => new { lp.Link.LinkId, lp.Link.Url }).ToList(),

                }).ToList();





                return Results.Json(result);

            });

            //get a single person
            app.MapGet("/api/person/{personId}", (int personId, MiniApiContext dbContext) =>
            {
                var person = dbContext.Persons
                 .Include(p => p.Phones)
                 .Include(p => p.Interest_Persons)
                 .ThenInclude(p => p.Interest)
                
                 .Include(p => p.Link_Persons)
                 .ThenInclude(p => p.Link)
               
                 .FirstOrDefault(p => p.PersonId == personId);

                if (person == null)
                {
                    return Results.NotFound($"Person with ID:{personId} NotFound");
                }

                var result = new
                {
                    person.PersonId,
                    person.FirstName,
                    person.LastName,
                    Phones = person.Phones.Select(p => new { p.PhoneNumber }).ToList(),
                    Interests = person.Interest_Persons.Select(pi => new { pi.Interest.InterestId, pi.Interest.Titel, pi.Interest.Description }).ToList(),
                    Links = person.Link_Persons.Select(lp => new { lp.Link.LinkId, lp.Link.Url }).ToList(),
                };
                return Results.Json(result);
            });

            //get interests related to a specific person
            app.MapGet("/api/getinterest/{personId}", (int personId, MiniApiContext dbcontext) =>

            {

                var persons = dbcontext.Persons
                .Include(p => p.Interest_Persons)
                .ThenInclude(p => p.Interest)
                .Where(p => p.PersonId == personId)
                .SingleOrDefault();

                if (persons == null)
                {
                    return Results.NotFound($"Person with ID:{personId} NotFound");
                }

                var result = new
                {

                    persons.FirstName,
                    persons.LastName,
                    Interests = persons.Interest_Persons.Select(pi => new { pi.Interest.InterestId, pi.Interest.Titel, pi.Interest.Description }).ToList(),

                };

                return Results.Json(result);


            });
            //get links related to  a specific person
            app.MapGet("/api/getlink/{personId}", (int personId, MiniApiContext dbcontext) =>

            {

                var persons = dbcontext.Persons
                .Include(p => p.Link_Persons)
                .ThenInclude(p => p.Link)
                .Where(p => p.PersonId == personId)
                .SingleOrDefault();

                if (persons == null)
                {
                    return Results.NotFound($"Person with ID:{personId} NotFound");
                }

                var result = new
                {

                    persons.FirstName,
                    persons.LastName,
                    Links = persons.Link_Persons.Select(pi => new { pi.Link.LinkId, pi.Link.Url }).ToList(),

                };

                return Results.Json(result);


            });
            // get the link for a specific interst 

            app.MapGet("/api/getlink_intrest/{intrestid}", (int intrestid, MiniApiContext dbcontext) =>

            {

                var persons = dbcontext.Interests
                
                .Include(p => p.Link_Interests)
                .ThenInclude(p => p.Link)
                .Where(p => p.InterestId == intrestid)
                .SingleOrDefault();

                if (persons == null)
                {
                    return Results.NotFound($"Intrest with ID:{intrestid} NotFound");
                }

                var result = new
                {

                  
                    Interest = persons.Interest_Persons.Select(pi => new { pi.Interest.Description, pi.Interest.Titel}).ToList(),
                    Link = persons.Link_Interests.Select(pi => new {pi.Link.Url}).ToList(), 

                };

                return Results.Json(result);


            });

            //create a new person
            app.MapPost("/api/createperson", (MiniApiContext dbContext, Person person) =>
            {
                var newPerson = new Person
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Phones = person.Phones?.Select(p => new Phone { PhoneNumber = p.PhoneNumber }).ToList(),
                    Interest_Persons = person.Interest_Persons?.Select(ip => new Interest_Person
                    {
                        Interest = new Interest
                        {
                            Titel = ip.Interest?.Titel,
                            Description = ip.Interest?.Description
                        }
                    }).ToList(),
                    Link_Persons = person.Link_Persons?.Select(lp => new Link_Person
                    {
                        Link = new Link
                        {
                            Url = lp?.Link?.Url
                        }
                    }).ToList(),
                };

                dbContext.Add(newPerson);
                dbContext.SaveChanges();

                //  associate Link and interest
                foreach (var interestPerson in newPerson.Interest_Persons)
                {
                    foreach (var linkPerson in newPerson.Link_Persons)
                    {
                        dbContext.Link_Interests.Add(new Link_Interest
                        {
                            InterestId = interestPerson.Interest.InterestId,
                            LinkId = linkPerson.Link.LinkId
                        });
                    }
                }

                dbContext.SaveChanges();

                return Results.Json($"New person with {newPerson.PersonId} created.");
            });


            //add  link/links to a specific peson and a specific interest 

            app.MapPost("/api/addlinks/{personId}/{interestId}", (MiniApiContext dbContext, List<Link> links, int personId, int interestId) =>
            {
                var existingPerson = dbContext.Persons
                    .Include(p => p.Interest_Persons)
                        .ThenInclude(ip => ip.Interest)
                    .FirstOrDefault(p => p.PersonId == personId);

                if (existingPerson == null)
                {
                    return Results.NotFound($"Person with ID {personId} not found.");
                }

                var existingInterest = existingPerson.Interest_Persons.FirstOrDefault(ip => ip.Interest.InterestId == interestId);

                if (existingInterest == null)
                {
                    return Results.NotFound($"Interest with ID {interestId} not found for person with ID {personId}.");
                }
               
           
                foreach (var link in links)
                {
                    dbContext.Link_Interests.Add(new Link_Interest
                    {
                        InterestId = interestId,
                        Link = new Link { Url = link.Url }
                    });
                }

                dbContext.SaveChanges();

                return Results.Json($"New links added to interest {interestId} for person with ID {personId}.");
            });


            //add  interest/interests to a specific person

            app.MapPost("/api/addinterest/{personId}", (MiniApiContext dbContext, List<Interest> interests, int personId) =>
            {
                var existingPerson = dbContext.Persons
                    .Include(p => p.Interest_Persons)
                        .ThenInclude(ip => ip.Interest)
                    .FirstOrDefault(p => p.PersonId == personId);

                if (existingPerson == null)
                {
                    return Results.NotFound($"Person with ID {personId} not found.");
                }

                foreach (var interest in interests)
                {
                    var newInterestPerson = new Interest_Person
                    {
                        PersonId = existingPerson.PersonId,
                        Interest = new Interest
                        {
                            Titel = interest.Titel,
                            Description = interest.Description,
                        }
                    };

                    existingPerson.Interest_Persons.Add(newInterestPerson);
                }

                dbContext.SaveChanges();

                return Results.Json($"New interests added to person with ID {personId}.");
            });

            //delete all information of a specific person

            app.MapDelete("/api/removeperson/{personId}", (MiniApiContext dbContext, int personId) =>
            {
                var existingPerson = dbContext.Persons
                    .Include(p => p.Phones)
                    .Include(p => p.Interest_Persons)
                        .ThenInclude(ip => ip.Interest)
                    .Include(p => p.Link_Persons)
                        .ThenInclude(lp => lp.Link)
                    .FirstOrDefault(p => p.PersonId == personId);

                if (existingPerson == null)
                {
                    return Results.NotFound($"Person with ID {personId} not found.");
                }

               
                dbContext.Phones.RemoveRange(existingPerson.Phones);

               
                dbContext.Interest_Persons.RemoveRange(existingPerson.Interest_Persons);
                    

              
                dbContext.Link_Persons.RemoveRange(existingPerson.Link_Persons);

              
                dbContext.Persons.Remove(existingPerson);

                dbContext.SaveChanges();

                return Results.Json($"Person with ID {personId} and related data deleted.");
            });



            app.Run();
        }
    }
}
