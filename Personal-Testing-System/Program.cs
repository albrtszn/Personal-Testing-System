using CRUD.implementations;
using CRUD.interfaces;
using DataBase;
using DataBase.Repository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Personal_Testing_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IAnswerRepo, AnswerRepo>();
builder.Services.AddTransient<IEmployeeAnswerRepo, EmployeeAnswerRepo>();
builder.Services.AddTransient<IEmployeeMatchingRepo, EmployeeMatchingRepo>();
builder.Services.AddTransient<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddTransient<IEmployeeSubsequenceRepo, EmployeeSubsequenceRepo>();
builder.Services.AddTransient<IFirstPartRepo, FirstPartRepo>();
builder.Services.AddTransient<IQuestionRepo, QuestionRepo>();
builder.Services.AddTransient<IQuestionsInTestRepo, QuestionsInTestRepo>();
builder.Services.AddTransient<IQuestionTypeRepo, QuestionTypeRepo>();
builder.Services.AddTransient<ISecondPartRepo, SecondPartRepo>();
builder.Services.AddTransient<ISubdivisionRepo, SubdivisionRepo>();
builder.Services.AddTransient<ISubsequenceRepo, SubsequenceRepo>();
builder.Services.AddTransient<ITestPurposeRepo, TestPurposeRepo>();
builder.Services.AddTransient<ITestRepo, TestRepo>();
builder.Services.AddTransient<IResultRepo, ResultRepo>();
builder.Services.AddTransient<ICompetenceRepo, CompetenceRepo>();
builder.Services.AddTransient<IAdminRepo, AdminRepo>();

builder.Services.AddScoped<EFDbContext>();
builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<EmployeeAnswerService>();
builder.Services.AddScoped<EmployeeMatchingService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeSubsequenceService>();
builder.Services.AddScoped<FirstPartService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<QuestionsInTestService>();
builder.Services.AddScoped<QuestionTypeService>();
builder.Services.AddScoped<SecondPartService>();
builder.Services.AddScoped<SubdivisionService>();
builder.Services.AddScoped<SubsequenceService>();
builder.Services.AddScoped<TestPurposeService>();
builder.Services.AddScoped<TestResultService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<CompetenceService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<MasterService>();

/*builder.Services.AddControllers().AddNewtonsoftJson(jsonOptions =>
{
    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
});*/

/*builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });*/

builder.Services.AddControllers()
   .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//init data in database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EFDbContext>();
    InitDB.InitData(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
