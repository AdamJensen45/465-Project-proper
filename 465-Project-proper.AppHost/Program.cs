var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Movie>("api-movie");

builder.AddProject<Projects.API_Users>("api-users");

builder.Build().Run();
