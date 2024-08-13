using Serilog;

Log.Logger = new LoggerConfiguration()  
            .WriteTo.Console()
            .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Serilog in Services
builder.Services.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/simple_log", () =>
{
    Log.Information("Into simple log");
    return Results.Ok("simple log");
})
.WithName("SimpleLog")
.WithOpenApi();

app.MapGet("/divide/{a}/{b}", (int a, int b) =>
{
    try{
        var result = a/b;
        Log.Information($"{a}/{b} equals {result}", a, b, result);
        return Results.Ok(result);
    }catch(Exception ex){
        Log.Fatal(ex, "Bad bro...");
    }

    return Results.Ok("Sum");
})
.WithName("Sum")
.WithOpenApi();

app.Run();