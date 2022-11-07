using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(p => p.AddPolicy(MyAllowSpecificOrigins, builder =>
{
    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
}));

var credentials = new BasicAWSCredentials(Environment.GetEnvironmentVariable("AWS-ACCESS-KEY-ID"), Environment.GetEnvironmentVariable("AWS-SECRET-KEY-ID"));
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(Environment.GetEnvironmentVariable("AWS-REGION"))
};
var client = new AmazonDynamoDBClient(credentials, config);
builder.Services.AddSingleton<IAmazonDynamoDB>(client);
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
