using ClassWork_WEBAPI.API.Settings;
using ClassWork_WEBAPI.BLL.Services;
using ClassWork_WEBAPI.DAL;
using ClassWork_WEBAPI.DAL.Initializer;
using ClassWork_WEBAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    string? connectionString = builder.Configuration.GetConnectionString("LocalDB");
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<GenreRepository>();
builder.Services.AddScoped<BookRepository>();


builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<ImageService>();

string corsPolicy = "allowAllCFG";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(corsPolicy, cfg =>
    {
        cfg.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string root = app.Environment.ContentRootPath;
string storagePath = Path.Combine(root, StaticFilesSettings.StorageDir);
string booksPath = Path.Combine(storagePath, StaticFilesSettings.BooksDir);
string authorsPath = Path.Combine(storagePath, StaticFilesSettings.AuthorsDir);

if (!Directory.Exists(booksPath)) { Directory.CreateDirectory(booksPath); }
if (!Directory.Exists(authorsPath)) { Directory.CreateDirectory(authorsPath); }

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(booksPath),
    RequestPath = StaticFilesSettings.BookUrl
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(authorsPath),
    RequestPath = StaticFilesSettings.AuthorUrl
});


app.UseCors(corsPolicy);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Seed().Wait();

app.Run();
