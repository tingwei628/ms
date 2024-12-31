using System;
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
using ms.Repositories;
using ms.Services;
using Npgsql;

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

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("PgCluster");
    return new NpgsqlConnection(connectionString);
});


// Add a custom scoped service.
// todo microservices
// repo
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// service
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddLogging();

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
// sso/oidc/oauth2
// traefik forwardauth authorization to .net 6 which implemented oidc
//核心模組建議
//用戶模組：用戶登入、註冊是電子商店的基礎。
//商品模組：商品上架、查詢是商店的核心功能。
//訂單模組：訂單生成、支付處理是營收的關鍵。
//支付模組：整合金流服務，確保交易順暢。

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health");

app.Run();