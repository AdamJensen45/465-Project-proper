var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Movie>("api-movie");

builder.Build().Run();
