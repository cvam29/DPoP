using Aspire.Hosting;
using Aspire.Hosting.Azure;

var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

builder.AddRedis("cache");

builder.AddAzureFunctionsProject("backend-functions", "../DpopPortfolio.Functions/DpopPortfolio.Functions.csproj")
    .WithHostStorage(storage)
    .WithEnvironment("DpopPortfolio__Issuer", "http://localhost:7071")
    .WithEnvironment("DpopPortfolio__Audience", "dpop-portfolio-api")
    .WithEnvironment("DpopPortfolio__FrontendOrigin", "http://localhost:3000")
    .WithEnvironment("DpopPortfolio__CacheProvider", "Redis")
    .WithEnvironment("DpopPortfolio__StorageProvider", "Azurite");

builder.Build().Run();
