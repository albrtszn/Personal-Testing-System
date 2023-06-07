using CRUD.implementations;
using CRUD.interfaces;
using DataBase.Repository;

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
builder.Services.AddTransient<ITestResultRepo, TestResultRepo>();
builder.Services.AddTransient<ITestTypeRepo, TestTypeRepo>();

builder.Services.AddScoped<EFDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
