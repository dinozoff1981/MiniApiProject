using Microsoft.EntityFrameworkCore;
using MiniApiProject.Models;

namespace MiniApiProject.Data
{
    public class MiniApiContext : DbContext
    {

        public MiniApiContext(DbContextOptions<MiniApiContext> options) : base(options)

        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interest_Person>()
                .HasKey(ip => new
                {
                    ip.InterestId,
                    ip.PersonId
                });
            modelBuilder.Entity<Link_Person>()
              .HasKey(lp => new
              {
                  lp.LinkId,
                  lp.PersonId
              });
            modelBuilder.Entity<Link_Interest>()
           .HasKey(li => new
           {
               li.LinkId,
               li.InterestId
           });

            modelBuilder.Entity<Interest_Person>()
                .HasOne(p => p.Person)
                .WithMany(i => i.Interest_Persons)
                .HasForeignKey(p => p.PersonId);

            modelBuilder.Entity<Interest_Person>()
               .HasOne(i => i.Interest)
               .WithMany(i => i.Interest_Persons)
               .HasForeignKey(p => p.InterestId);

            modelBuilder.Entity<Link_Person>()
               .HasOne(p => p.Person)
               .WithMany(l => l.Link_Persons)
               .HasForeignKey(p => p.PersonId);

            modelBuilder.Entity<Link_Person>()
             .HasOne(l => l.Link)
             .WithMany(l => l.Link_Persons)
             .HasForeignKey(p => p.LinkId);

            modelBuilder.Entity<Link_Interest>()
                .HasOne(li => li.interest)
                .WithMany(li => li.Link_Interests)
                .HasForeignKey(p => p.InterestId);

            modelBuilder.Entity<Link_Interest>()
                .HasOne(li => li.Link)
                .WithMany(li => li.Link_Interests)
                .HasForeignKey(p => p.LinkId);




            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Interest> Interests { get; set; }
        public DbSet<Person> Persons { get; set; }

        public DbSet<Phone> Phones { get; set; }   
        
      public  DbSet<Link> Links { get; set; }
        public DbSet<Interest_Person> Interest_Persons { get; set; }
        public DbSet<Link_Interest> Link_Interests { get; set; }

        public DbSet<Link_Person> Link_Persons { get; set; }


    }
}