using Serilog;
using Serilog.Sinks.AwsCloudWatch;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Opcional: escreve logs no console para depuração
    .WriteTo.AmazonCloudWatch(
        awsOptions =>
        {
            awsOptions.LogGroupName = "SeuGrupoDeLogs";
            awsOptions.LogStreamName = "SeuFluxoDeLogs";
            awsOptions.Region = "us-east-1"; // Substitua pela sua região
            // ... outras configurações, como credenciais
        })
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Logging.AddAWSProvider();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
