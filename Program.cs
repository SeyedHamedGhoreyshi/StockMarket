using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Interfaces;
using StockMarket.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}
) ;


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IstockRepository , StockRepository>() ;
builder.Services.AddScoped<IcommentRepository , CommentRepository>() ; 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();
    Console.WriteLine(endpoint?.DisplayName ?? "No endpoint found");
    await next();
});






app.Run();

