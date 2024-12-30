﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ms;
using Swashbuckle.AspNetCore.SwaggerGen;

// config/secret for different env 

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Look for static files in webroot
    //WebRootPath = "webroot"
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {

// only when deploying to kubernetes
#if (DEBUG == false)
    options.DocumentFilter<PathPrefixInsertDocumentFilter>("/ms");
#endif

});


//// Add a custom scoped service.
//builder.Services.AddScoped<ITodoRepository, TodoRepository>();

builder.Services.AddHealthChecks();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
//app.UseStaticFiles();


// CORS
// Authentication
// Authorization

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health");

app.Run();