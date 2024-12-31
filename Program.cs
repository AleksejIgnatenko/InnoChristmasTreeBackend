using InnoChristmasTree;
using InnoChristmasTree.GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db
builder.Services.AddDbContext<InnoChristmasTreeDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Настройка GraphQL
builder.Services.AddGraphQLServer()
    .AddQueryType<GraphQLQueries>()
    .AddMutationType<GraphQLMutation>()
    .AddSubscriptionType<GraphQLSubscription>()
    .AddInMemorySubscriptions()
    .AddProjections();

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

app.UseWebSockets();

// Endpoint GraphQL API
app.MapGraphQL("/graphql");

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("https://aleksejignatenko.github.io");
    x.WithMethods().AllowAnyMethod();
});

//app.UseCors(x =>
//{
//    x.WithHeaders().AllowAnyHeader();
//    x.WithOrigins("http://localhost:3000");
//    x.WithMethods().AllowAnyMethod();
//});

app.Run();
