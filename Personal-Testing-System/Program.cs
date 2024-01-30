using CRUD.implementations;
using CRUD.interfaces;
using DataBase;
using DataBase.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Personal_Testing_System;
using Personal_Testing_System.Controllers;
using Personal_Testing_System.Hubs;
using Personal_Testing_System.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<EFDbContext>();

builder.Services.AddScoped<IAnswerRepo, AnswerRepo>();
builder.Services.AddScoped<IProfileRepo, ProfileRepo>();
builder.Services.AddScoped<IGroupPositionRepo, GroupPositionRepo>();
builder.Services.AddScoped<ICompetenciesForGroupRepo, CompetenciesForGroupRepo>();
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
builder.Services.AddScoped<IEmployeeResultSubcompetenceRepo, EmployeeResultSubcompetenceRepo>();
builder.Services.AddScoped<IQuestionSubcompetenceRepo, QuestionSubcompetenceRepo>();
builder.Services.AddScoped<ISubcompetenceRepo, SubcompetenceRepo>();
builder.Services.AddScoped<IMessageRepo, MessageRepo>();
builder.Services.AddScoped<IGlobalConfigureRepo, GlobalConfigureRepo>();
builder.Services.AddScoped<ITestScoreRepo, TestScoreRepo>();
builder.Services.AddScoped<ISubcompetenceScoreRepo, SubcompetenceScoreRepo>();
builder.Services.AddScoped<ICompetenceCoeffRepo, CompetenceCoeffRepo>();

builder.Services.AddScoped<AnswerService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<GroupPositionService>();
builder.Services.AddScoped<CompetenciesForGroupService>();
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
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<TestPurposeService>();
builder.Services.AddScoped<CompetenceService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<ResultService>();
builder.Services.AddScoped<EmployeeResultService>();
builder.Services.AddScoped<LogService>();
builder.Services.AddScoped<TokenEmployeeService>();
builder.Services.AddScoped<TokenAdminService>();
builder.Services.AddScoped<EmployeeResultSubcompetenceService>();
builder.Services.AddScoped<QuestionSubcompetenceService>();
builder.Services.AddScoped<SubcompetenceService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<GlobalConfigureService>();
builder.Services.AddScoped<TestScoreService>();
builder.Services.AddScoped<SubcompetenceScoreService>();
builder.Services.AddScoped<CompetenceCoeffService>();

builder.Services.AddScoped<MasterService>();

//builder.Services.AddScoped<IHubContext<NotificationHub, INotificationClient>>();
//builder.Services.AddScoped<NotificationHub>();
//builder.Services.AddSignalRCore();
builder.Services.AddSignalR();/*options =>
{
    options.EnableDetailedErrors = true;
});*/

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

builder.Services.AddDbContext<EFDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"))
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

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
    c.EnableAnnotations();
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.GroupName}_{apiDesc.ActionDescriptor.RouteValues["controller"]}");
    //c.OrderActionsBy(null);
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath}");
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.ActionDescriptor}");
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}");
    //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.GroupName}");
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
app.UseCors(builder => builder.AllowAnyOrigin());

app.UseHttpsRedirection();

app.MapHub<NotificationHub>("notification-hub");

app.UseAuthorization();

app.MapControllers();

app.Run();
