using inventoryApiDotnet.Context;
using inventoryApiDotnet.Interface;
using inventoryApiDotnet.Model;
using inventoryApiDotnet.Services;
using inventoryApiDotnet.Repository;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.IdGenerators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();               

builder.Services.AddScoped<IIntentoryService, InventoryService>();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<MongoDBService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockservice, Stockservice>();

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
BsonSerializer.RegisterIdGenerator(typeof(string), new StringObjectIdGenerator());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//BsonSerializer.RegisterIdGenerator(typeof(ObjectId), new StringObjectIdGenerator());
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;

app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
      );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
