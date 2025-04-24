using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsApp.Models.DB;

namespace NewsApp.Database;

public partial class NewsDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public NewsDbContext()
    {
    }

    public NewsDbContext(DbContextOptions<NewsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; } = null!;

    public virtual DbSet<Author> Authors { get; set; } = null!;

    // To use local SQL Server DB
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=NewsDB;TrustServerCertificate=True;Trusted_Connection=true");

    // To use in-memory DB
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase("NewsDB");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Articles");

            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne<Author>().WithMany()
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_Articles_Authors")
                .IsRequired();
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Authors");

            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne<User>().WithOne(p => p.Author)
                .HasForeignKey<Author>(d => d.UserId)
                .HasConstraintName("FK_Authors_Users")
                .IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
