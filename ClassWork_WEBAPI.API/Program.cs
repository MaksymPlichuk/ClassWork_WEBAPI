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

string mapperKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODA0NzIzMjAwIiwiaWF0IjoiMTc3MzI0MTgwOSIsImFjY291bnRfaWQiOiIwMTljZGQ3MDE5YWU3ZWM2OTcxOTUzNGJkN2ZjNmZjNiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2tlcTVlZmJqamgwdGMxemY2a2hrcnpzIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.U-he0BPGOZM7xlhFrhvO7cV7-WLTOxMlFoMa-4qItarcBS4TXfAAQ325P2WQeIz9uVZSngOp1xboowsAmwu2bcEi07KuIcWDTSS-7IV9xcN-1TevFA1Q05Xa4_kjLV3zYr9YfzfJMxzngzpHNcemueJemHaEOsrm-nS7A1oY19EU47pI5SX-Y22so4l6mCnb39t_etDZZ8zNHBJYl9oV-zm5FdFF4nY1Fw0Xqz_QCgyIFa9wZuaVL84O47z-lB7ZOvs89ogU5-Am92DDBqshPHFq_2aDjTnS-qqKyjY6Lq6dvYUfRvy_Y7gTuZm56uVmK58DTNnX7Pbh1lRSS_g0vA";
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = mapperKey;
}, AppDomain.CurrentDomain.GetAssemblies());


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
