using BugStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSwaggerDoc();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddDependencyInjection();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(0);
});

builder.WebHost
    .UseSetting(WebHostDefaults.ServerUrlsKey, "http://*:" + Environment.GetEnvironmentVariable("PORT"));

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

app.MapEndpoints();

app.Run();
