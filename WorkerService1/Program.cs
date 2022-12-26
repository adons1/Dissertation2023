using WorkerService1;

try
{
    IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "WORKER_123";
    })
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

    host.Run();
}
catch(Exception ex)
{
    File.WriteAllText("logs.txt", ex.Message);
}

