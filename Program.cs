using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;



var builder = WebApplication.CreateBuilder(args);

// Configure the app to listen on all IP addresses
builder.WebHost.UseUrls("http://0.0.0.0:5011");

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.WithOrigins("http://192.168.0.193:3000") // Replace with your frontend URL
			   .AllowAnyMethod()
			   .AllowAnyHeader();
			   //.AllowCredentials(); // Allow credentials (cookies, authorization headers)
	});
});


// Configure Kestrel options for large file uploads
builder.WebHost.ConfigureKestrel(options =>
{
	// Set the max request body size to 500MB
	options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500 MB
	options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10); // Increase timeout to 10 minutes
});

builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500 MB limit
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors("AllowAll"); // Ensure this line is before app.UseAuthorization();
//app.UseAuthentication(); // Authentication must be used before authorization
//app.UseAuthorization();  // Authorization middleware

app.MapControllers();

app.Run();
