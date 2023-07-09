using System;
using System.Collections.Generic;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repository;

public partial class EFDbContext : DbContext
{
    public EFDbContext()
    {
    }

    public EFDbContext(DbContextOptions<EFDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Competence> Competences { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAnswer> EmployeeAnswers { get; set; }

    public virtual DbSet<EmployeeMatching> EmployeeMatchings { get; set; }

    public virtual DbSet<EmployeeResult> EmployeeResults { get; set; }

    public virtual DbSet<EmployeeSubsequence> EmployeeSubsequences { get; set; }

    public virtual DbSet<FirstPart> FirstParts { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<SecondPart> SecondParts { get; set; }

    public virtual DbSet<Subdivision> Subdivisions { get; set; }

    public virtual DbSet<Subsequence> Subsequences { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestPurpose> TestPurposes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonalTestingSystemBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83FD610BA03");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idSubdivi__52593CB8");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3213E83F8DB242CA");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Answer__idQuesti__412EB0B6");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F41BB67E6");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F17609E04");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idSubd__4F7CD00D");
        });

        modelBuilder.Entity<EmployeeAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F51BEC2CD");

            entity.HasOne(d => d.IdAnswerNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idAns__5BE2A6F2");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idRes__5CD6CB2B");
        });

        modelBuilder.Entity<EmployeeMatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F1CC4F651");

            entity.HasOne(d => d.IdFirstPartNavigation).WithMany(p => p.EmployeeMatchings).HasConstraintName("FK__EmployeeM__idFir__6383C8BA");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idRes__656C112C");

            entity.HasOne(d => d.IdSecondPartNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idSec__6477ECF3");
        });

        modelBuilder.Entity<EmployeeResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F8E97D8B4");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeResults)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EmployeeR__idEmp__59063A47");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeResults).HasConstraintName("FK__EmployeeR__idRes__5812160E");
        });

        modelBuilder.Entity<EmployeeSubsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F9A47323E");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idRes__60A75C0F");

            entity.HasOne(d => d.IdSubsequenceNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idSub__5FB337D6");
        });

        modelBuilder.Entity<FirstPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FirstPar__3213E83FA1119958");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.FirstParts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FirstPart__idQue__46E78A0C");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FA844EF91");

            entity.HasOne(d => d.IdQuestionTypeNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Question__idQues__3D5E1FD2");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Questions).HasConstraintName("FK__Question__idTest__3E52440B");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FB8C44A02");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3213E83F19F5CB2F");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Result__idTest__5535A963");
        });

        modelBuilder.Entity<SecondPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SecondPa__3213E83FA93991D9");

            entity.HasOne(d => d.IdFirstPartNavigation).WithOne(p => p.SecondPart)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecondPar__idFir__4AB81AF0");
        });

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subdivis__3213E83F70A031C8");
        });

        modelBuilder.Entity<Subsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subseque__3213E83FF1803AE4");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Subsequences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subsequen__idQue__440B1D61");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3213E83F5282D328");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Test__idCompeten__38996AB5");
        });

        modelBuilder.Entity<TestPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPurp__3213E83FBD6E001C");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idEmp__68487DD7");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idTes__693CA210");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
