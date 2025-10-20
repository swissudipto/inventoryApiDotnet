using inventoryApiDotnet.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using inventoryApiDotnet.ServiceregistrationExtension;
using FluentValidation;
using inventoryApiDotnet.AuthenticationExtension;
using inventoryApiDotnet.AuthorizationExtension;

var builder = WebApplication.CreateBuilder(args);

// Add Validators 
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

// Add services to the container.
builder.Services.AddControllers();

// Registers all the application Services & Repositories
builder.Services.RegisterServices();

// Add Authentication
builder.Services.AddAuthenticationExt(builder.Configuration);

// Add Authorization Policies
builder.Services.AddAuthorizationPolicies();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MongoDB id Generator 
BsonSerializer.RegisterIdGenerator(typeof(string), new StringObjectIdGenerator());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors(
  options => options.WithOrigins("*")
                    .AllowAnyMethod()
                    .AllowAnyHeader());

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
