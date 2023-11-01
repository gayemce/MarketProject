using MarketServer.WebApi.Context;
using MarketServer.WebApi.Options;
using MarketServer.WebApi.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Automapper service
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//AppDbContex i�in service
builder.Services.AddScoped<AppDbContext>();

builder.Services.AddCors(cfr =>
{
    cfr.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//Authentication: Kullan�c� kontorol�
//Authorization: Yetki kontr�l�

builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "Issuer",
        ValidAudience = "Audience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My secret key My secret key My secret key My secret key"))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//EmailSettings uygulaman�n ba��nda de�erler ile beraber set edilir.
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

//CreateServiceTool methodu dahil edildi
builder.Services.CreateServiceTool();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

//app.UseAuthorization(); Token olu�turuldu�u i�in gerek kalmad�

app.MapControllers();

app.Run();
