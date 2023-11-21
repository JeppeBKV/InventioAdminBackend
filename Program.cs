using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Settings.CosmosDbUrl = builder.Configuration.GetSection("AzureCosmosDbSettings").GetValue<string>("URL");
Settings.CosmosDbKey = builder.Configuration.GetSection("AzureCosmosDbSettings").GetValue<string>("PrimaryKey");
Settings.CosmosDbName = builder.Configuration.GetSection("AzureCosmosDbSettings").GetValue<string>("DatabaseName");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }
app.MapGet("/", () => "Connected");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class Settings
{
    public static string CosmosDbUrl = "";
    public static string CosmosDbKey = "";
    public static string CosmosDbName = "";
    public static string TokenSecret = "";
}
static public class InventioAdminCosmosDB
{
    static public CosmosClient client = new (Settings.CosmosDbUrl, Settings.CosmosDbKey);
}
