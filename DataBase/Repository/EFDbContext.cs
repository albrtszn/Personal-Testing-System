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
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefautConnection"));
    //=> optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonalTestingSystemBD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83FD26E8523");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idSubdivi__5441852A");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3213E83FEDB3FDB4");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Answer__idQuesti__4316F928");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F34333F8B");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F6450CB79");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idSubd__5165187F");
        });

        modelBuilder.Entity<EmployeeAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F5BFB567C");

            entity.HasOne(d => d.IdAnswerNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idAns__5DCAEF64");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idRes__5EBF139D");
        });

        modelBuilder.Entity<EmployeeMatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F40F2C6A0");

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
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F21366D75");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeResults)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeR__idEmp__5AEE82B9");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeResults).HasConstraintName("FK__EmployeeR__idRes__59FA5E80");
        });

        modelBuilder.Entity<EmployeeSubsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F8E1F0550");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idRes__628FA481");

            entity.HasOne(d => d.IdSubsequenceNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idSub__619B8048");
        });

        modelBuilder.Entity<FirstPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FirstPar__3213E83F2B6D2E6C");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.FirstParts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FirstPart__idQue__48CFD27E");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F25D9A789");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FCEBCF828");

            entity.HasOne(d => d.IdQuestionTypeNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Question__idQues__3F466844");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Questions).HasConstraintName("FK__Question__idTest__403A8C7D");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83FF84FC582");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3213E83F307125FF");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Result__idTest__571DF1D5");
        });

        modelBuilder.Entity<SecondPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SecondPa__3213E83F5F771C08");

            entity.HasOne(d => d.IdFirstPartNavigation).WithOne(p => p.SecondPart)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecondPar__idFir__4CA06362");
        });

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subdivis__3213E83FA7C92765");
        });

        modelBuilder.Entity<Subsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subseque__3213E83FB2B9928D");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Subsequences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subsequen__idQue__45F365D3");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3213E83FF83CF68C");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Test__idCompeten__3A81B327");
        });

        modelBuilder.Entity<TestPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPurp__3213E83F6A115653");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idEmp__6A30C649");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idTes__6B24EA82");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83F19193715");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__6EF57B66");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83F98922C71");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__72C60C4A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
