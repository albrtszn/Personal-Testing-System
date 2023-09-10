using CRUD.implementations;
using CRUD.interfaces;
using DataBase;
using DataBase.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Personal_Testing_System;
using Personal_Testing_System.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped< EFDbContext>();

builder.Services.AddScoped<IAnswerRepo, AnswerRepo>();
builder.Services.AddScoped<IEmployeeAnswerRepo, EmployeeAnswerRepo>();
builder.Services.AddScoped<IEmployeeMatchingRepo, EmployeeMatchingRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<IEmployeeSubsequenceRepo, EmployeeSubsequenceRepo>();
builder.Services.AddScoped<IFirstPartRepo, FirstPartRepo>();
builder.Services.AddScoped<IQuestionRepo, QuestionRepo>();
builder.Services.AddScoped<IQuestionTypeRepo, QuestionTypeRepo>();
builder.Services.AddScoped<ISecondPartRepo, SecondPartRepo>();
builder.Services.AddScoped<ISubdivisionRepo, SubdivisionRepo>();
builder.Services.AddScoped<ISubsequenceRepo, SubsequenceRepo>();
builder.Services.AddScoped<ITestPurposeRepo, TestPurposeRepo>();
builder.Services.AddScoped<ITestRepo, TestRepo>();
builder.Services.AddScoped<IResultRepo, ResultRepo>();
builder.Services.AddScoped<ICompetenceRepo, CompetenceRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IResultRepo, ResultRepo>();
builder.Services.AddScoped<IEmployeeResultRepo, EmployeeResultRepo>();
builder.Services.AddScoped<ILogRepo, LogRepo>();
builder.Services.AddScoped<ITokenEmployeeRepo, TokenEmployeeRepo>();
builder.Services.AddScoped<ITokenAdminRepo, TokenAdminRepo>();

builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<EmployeeAnswerService>();
builder.Services.AddScoped<EmployeeMatchingService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeSubsequenceService>();
builder.Services.AddScoped<FirstPartService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<QuestionTypeService>();
builder.Services.AddScoped<SecondPartService>();
builder.Services.AddScoped<SubdivisionService>();
builder.Services.AddScoped<SubsequenceService>();
builder.Services.AddScoped<TestPurposeService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<CompetenceService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<ResultService>();
builder.Services.AddScoped<EmployeeResultService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<TokenEmployeeService>();
builder.Services.AddScoped<TokenAdminService>();
builder.Services.AddScoped<MasterService>();

//builder.Services.Configure<TokenTimeToLiveInHours>(builder.Configuration.GetSection("TokenTimeToLiveInHours"));
//builder.Services.AddScoped<TokenTimeToLiveInHours>();

/*builder.Services.AddControllers().AddNewtonsoftJson(jsonOptions =>
{
    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter());
});*/

/*builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });*/

/*builder.Services.AddDbContext<EFDbContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"))
    );*/

builder.Services.AddControllers()
   .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Personal-Testing-System", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//init data in database
/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EFDbContext>();
    InitDB.InitData(context);
}*/

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{*/
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseStaticFiles();
app.UseDefaultFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
