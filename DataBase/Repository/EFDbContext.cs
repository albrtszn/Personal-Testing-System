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

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<SecondPart> SecondParts { get; set; }

    public virtual DbSet<Subdivision> Subdivisions { get; set; }

    public virtual DbSet<Subsequence> Subsequences { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestPurpose> TestPurposes { get; set; }

    public virtual DbSet<TokenAdmin> TokenAdmins { get; set; }

    public virtual DbSet<TokenEmployee> TokenEmployees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonalTestingSystemBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F9296F71D");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idSubdivi__5441852A");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3213E83F366E7CD9");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Answer__idQuesti__4316F928");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F2E0DEC54");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F037E4481");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idSubd__5165187F");
        });

        modelBuilder.Entity<EmployeeAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FFA47128A");

            entity.HasOne(d => d.IdAnswerNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idAns__5DCAEF64");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idRes__5EBF139D");
        });

        modelBuilder.Entity<EmployeeMatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FC693D823");

            entity.HasOne(d => d.IdFirstPartNavigation).WithMany(p => p.EmployeeMatchings).HasConstraintName("FK__EmployeeM__idFir__656C112C");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idRes__6754599E");

            entity.HasOne(d => d.IdSecondPartNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idSec__66603565");
        });

        modelBuilder.Entity<EmployeeResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FB40E14A4");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeResults)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EmployeeR__idEmp__5AEE82B9");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeResults).HasConstraintName("FK__EmployeeR__idRes__59FA5E80");
        });

        modelBuilder.Entity<EmployeeSubsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FBF1EF555");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idRes__628FA481");

            entity.HasOne(d => d.IdSubsequenceNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idSub__619B8048");
        });

        modelBuilder.Entity<FirstPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FirstPar__3213E83F7FEA0945");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.FirstParts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FirstPart__idQue__48CFD27E");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F32DC2642");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F41CAE1B3");

            entity.HasOne(d => d.IdQuestionTypeNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Question__idQues__3F466844");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Questions).HasConstraintName("FK__Question__idTest__403A8C7D");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F8E7D36F5");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3213E83FF75C02A9");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Result__idTest__571DF1D5");
        });

        modelBuilder.Entity<SecondPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SecondPa__3213E83FE40451C2");

            entity.HasOne(d => d.IdFirstPartNavigation).WithOne(p => p.SecondPart)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecondPar__idFir__4CA06362");
        });

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subdivis__3213E83FA6B61C9C");
        });

        modelBuilder.Entity<Subsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subseque__3213E83FCCCBF4BD");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Subsequences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subsequen__idQue__45F365D3");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3213E83F9034389A");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Test__idCompeten__3A81B327");
        });

        modelBuilder.Entity<TestPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPurp__3213E83FEEC35354");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idEmp__6A30C649");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idTes__6B24EA82");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83FEEC8DDB4");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__6EF57B66");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83FF69C08D5");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__72C60C4A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
