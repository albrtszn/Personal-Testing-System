using System;
using System.Collections.Generic;
using DataBase.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repository;

public partial class EFDbContext : DbContext
{
    private IConfiguration configuration;
    public EFDbContext(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public EFDbContext(DbContextOptions<EFDbContext> options,
                       IConfiguration _configuration)
        : base(options)
    {
        configuration = _configuration;
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
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefautConnection"),
            builder => builder.EnableRetryOnFailure(5,
                TimeSpan.FromSeconds(5), null)
            );
    //=> optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonalTestingSystemBD;User ID=test;Password=password;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F5182F85E");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idSubdivi__5535A963");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3213E83F4753CDFB");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Answer__idQuesti__440B1D61");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F0398D501");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F87DF1516");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idSubd__52593CB8");
        });

        modelBuilder.Entity<EmployeeAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F64111DEE");

            entity.HasOne(d => d.IdAnswerNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idAns__5EBF139D");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idRes__5FB337D6");
        });

        modelBuilder.Entity<EmployeeMatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F03F21297");

            entity.HasOne(d => d.IdFirstPartNavigation).WithMany(p => p.EmployeeMatchings).HasConstraintName("FK__EmployeeM__idFir__66603565");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idRes__68487DD7");

            entity.HasOne(d => d.IdSecondPartNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idSec__6754599E");
        });

        modelBuilder.Entity<EmployeeResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F6532813D");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeResults)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeR__idEmp__5BE2A6F2");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeResults).HasConstraintName("FK__EmployeeR__idRes__5AEE82B9");
        });

        modelBuilder.Entity<EmployeeSubsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F951980B2");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idRes__6383C8BA");

            entity.HasOne(d => d.IdSubsequenceNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idSub__628FA481");
        });

        modelBuilder.Entity<FirstPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FirstPar__3213E83F885A02F5");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.FirstParts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FirstPart__idQue__49C3F6B7");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F228A2B12");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FF663D847");

            entity.HasOne(d => d.IdQuestionTypeNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Question__idQues__403A8C7D");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Questions).HasConstraintName("FK__Question__idTest__412EB0B6");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F2CAF308B");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3213E83F821A354F");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Result__idTest__5812160E");
        });

        modelBuilder.Entity<SecondPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SecondPa__3213E83F4E78BC2D");

            entity.HasOne(d => d.IdFirstPartNavigation).WithOne(p => p.SecondPart)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecondPar__idFir__4D94879B");
        });

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subdivis__3213E83F05A92F1D");
        });

        modelBuilder.Entity<Subsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subseque__3213E83FA0FB9C90");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Subsequences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subsequen__idQue__46E78A0C");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3213E83F7FC00EE1");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Test__idCompeten__3B75D760");
        });

        modelBuilder.Entity<TestPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPurp__3213E83F18346BC9");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idEmp__6B24EA82");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idTes__6C190EBB");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83F478906E8");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__6FE99F9F");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83FD3706546");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__73BA3083");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
