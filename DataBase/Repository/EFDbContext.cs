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

    public virtual DbSet<CompetenciesForGroup> CompetenciesForGroups { get; set; }

    public virtual DbSet<ElployeeResultSubcompetence> ElployeeResultSubcompetences { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAnswer> EmployeeAnswers { get; set; }

    public virtual DbSet<EmployeeMatching> EmployeeMatchings { get; set; }

    public virtual DbSet<EmployeeResult> EmployeeResults { get; set; }

    public virtual DbSet<EmployeeSubsequence> EmployeeSubsequences { get; set; }

    public virtual DbSet<FirstPart> FirstParts { get; set; }

    public virtual DbSet<GlobalConfigure> GlobalConfigures { get; set; }

    public virtual DbSet<GroupPosition> GroupPositions { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionSubcompetence> QuestionSubcompetences { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<SecondPart> SecondParts { get; set; }

    public virtual DbSet<Subcompetence> Subcompetences { get; set; }

    public virtual DbSet<SubcompetenceScore> SubcompetenceScores { get; set; }

    public virtual DbSet<Subdivision> Subdivisions { get; set; }

    public virtual DbSet<Subsequence> Subsequences { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestPurpose> TestPurposes { get; set; }

    public virtual DbSet<TestScore> TestScores { get; set; }

    public virtual DbSet<TokenAdmin> TokenAdmins { get; set; }

    public virtual DbSet<TokenEmployee> TokenEmployees { get; set; }

    public virtual DbSet<СompetenceСoeff> СompetenceСoeffs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefautConnection"),
            builder => builder.EnableRetryOnFailure(5,
                TimeSpan.FromSeconds(5), null)
            );
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("fitpsuon_fitpsuon")
            .UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admin__3213E83F6201C4DF");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Admin__idSubdivi__2610A626");
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3213E83FB82710A7");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Answers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Answer__idQuesti__14E61A24");
        });

        modelBuilder.Entity<Competence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F7021A3A8");
        });

        modelBuilder.Entity<CompetenciesForGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Competen__3213E83F9D003187");

            entity.HasOne(d => d.IdGroupPositionsNavigation).WithMany(p => p.CompetenciesForGroups)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Competenc__idGro__6501FCD8");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.CompetenciesForGroups)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Competenc__idTes__640DD89F");
        });

        modelBuilder.Entity<ElployeeResultSubcompetence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Elployee__3213E83F7306EB97");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.ElployeeResultSubcompetences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ElployeeR__idRes__7CD98669");

            entity.HasOne(d => d.IdSubcompetenceNavigation).WithMany(p => p.ElployeeResultSubcompetences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ElployeeR__idSub__7DCDAAA2");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F898AAB26");

            entity.HasOne(d => d.IdSubdivisionNavigation).WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employee__idSubd__2334397B");
        });

        modelBuilder.Entity<EmployeeAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F196BECC9");

            entity.HasOne(d => d.IdAnswerNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idAns__2F9A1060");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeAnswers)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeA__idRes__308E3499");
        });

        modelBuilder.Entity<EmployeeMatching>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F9284DFCB");

            entity.HasOne(d => d.IdFirstPartNavigation).WithMany(p => p.EmployeeMatchings).HasConstraintName("FK__EmployeeM__idFir__373B3228");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idRes__39237A9A");

            entity.HasOne(d => d.IdSecondPartNavigation).WithMany(p => p.EmployeeMatchings)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeM__idSec__382F5661");
        });

        modelBuilder.Entity<EmployeeResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FC5E5B15B");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.EmployeeResults)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeR__idEmp__53D770D6");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeResults).HasConstraintName("FK__EmployeeR__idRes__2BC97F7C");
        });

        modelBuilder.Entity<EmployeeSubsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F6BCDE496");

            entity.HasOne(d => d.IdResultNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idRes__345EC57D");

            entity.HasOne(d => d.IdSubsequenceNavigation).WithMany(p => p.EmployeeSubsequences)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeS__idSub__336AA144");
        });

        modelBuilder.Entity<FirstPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FirstPar__3213E83F4F4BC206");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.FirstParts)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__FirstPart__idQue__1A9EF37A");
        });

        modelBuilder.Entity<GlobalConfigure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GlobalCo__3213E83F796DFB2B");
        });

        modelBuilder.Entity<GroupPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupPos__3213E83FA42EB5C7");

            entity.HasOne(d => d.IdProfileNavigation).WithMany(p => p.GroupPositions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__GroupPosi__idPro__61316BF4");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3213E83F84029BA4");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Message__3213E83FA617EF6B");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Messages)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Message__idEmplo__0E04126B");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Profile__3213E83F54074B93");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F5E20152F");

            entity.HasOne(d => d.IdQuestionTypeNavigation).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Question__idQues__11158940");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Questions).HasConstraintName("FK__Question__idTest__1209AD79");
        });

        modelBuilder.Entity<QuestionSubcompetence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83F1A455150");

            entity.HasOne(d => d.IdQuestionNavigation).WithOne(p => p.QuestionSubcompetence)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EmployeeM__idQue__12C8C788");

            entity.HasOne(d => d.IdSubcompetenceNavigation).WithMany(p => p.QuestionSubcompetences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__EmployeeM__idSub__13BCEBC1");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3213E83F68F35AEE");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Result__3213E83F80B7C161");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.Results)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Result__idTest__52E34C9D");
        });

        modelBuilder.Entity<SecondPart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SecondPa__3213E83FB06FDA90");

            entity.HasOne(d => d.IdFirstPartNavigation).WithOne(p => p.SecondPart)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecondPar__idFir__1E6F845E");
        });

        modelBuilder.Entity<Subcompetence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subcompe__3213E83FEE573FB4");
        });

        modelBuilder.Entity<SubcompetenceScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subcompe__3213E83F2BD7D43B");

            entity.HasOne(d => d.IdSubcompetenceNavigation).WithMany(p => p.SubcompetenceScores)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subcompet__idSub__26CFC035");
        });

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subdivis__3213E83F4561AB6A");

            entity.HasOne(d => d.IdGroupPositionsNavigation).WithMany(p => p.Subdivisions)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Subdivisi__idGro__65F62111");
        });

        modelBuilder.Entity<Subsequence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subseque__3213E83F6F5146E1");

            entity.HasOne(d => d.IdQuestionNavigation).WithMany(p => p.Subsequences)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Subsequen__idQue__17C286CF");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3213E83FA04FFFD0");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Test__idCompeten__0C50D423");
        });

        modelBuilder.Entity<TestPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestPurp__3213E83FBF4C816B");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idEmp__3BFFE745");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestPurposes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TestPurpo__idTes__3CF40B7E");
        });

        modelBuilder.Entity<TestScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestScor__3213E83F0A72DEC6");

            entity.Property(e => e.NumberPoints).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdTestNavigation).WithMany(p => p.TestScores)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TestScore__idTes__22FF2F51");
        });

        modelBuilder.Entity<TokenAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenAdm__3213E83F583A25D1");

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.TokenAdmins)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenAdmi__idAdm__40C49C62");
        });

        modelBuilder.Entity<TokenEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenEmp__3213E83FC9ED6F4B");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.TokenEmployees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__TokenEmpl__idEmp__44952D46");
        });

        modelBuilder.Entity<СompetenceСoeff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Сompeten__3213E83F4BB645E7");

            entity.HasOne(d => d.IdCompetenceNavigation).WithMany(p => p.СompetenceСoeffs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Сompetenc__idCom__3429BB53");

            entity.HasOne(d => d.IdGroupNavigation).WithMany(p => p.СompetenceСoeffs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Сompetenc__idGro__351DDF8C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
