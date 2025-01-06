using Common.MsgLog;
using Common.Services.KeyCloak;
using DataBase.Entities;
using DataBase.Entities.ConfigurationDataBase.DbContext;
using DataBase.Entities.MessageLogDataBase.DbContext;
using DataBase.Entities.ProjectDataBase.DbContext;
using InfoService.GrpcApi.Services;
using InfoService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Activation = DataBase.Models.Configuration.License.Activation;
using HistoryDB;
using Common.CommonModels;
using Common.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddSingleton<ConfigurationReaderService>();
builder.Services.AddSingleton<KeyCloakApi>();

builder.Services.AddDbContext<ConfigurationDbContext>(
    (sp, optBuilder) =>
    {
        var readerService = sp.GetRequiredService<ConfigurationReaderService>();
        var dbConfig = readerService.ConfigurationDbConfig with { ApplicationName = readerService.AppSettings.Name };
        PgsqlDbHelper.PgsqlOptionsBuilder(optBuilder, dbConfig.ToString()!);
    }, ServiceLifetime.Transient
);

builder.Services.AddDbContext<ProjectDbContext>(
    (sp, optBuilder) =>
    {
        var dbConfig = sp.GetRequiredService<ConfigurationReaderService>().ProjectDbConfig;
        PgsqlDbHelper.PgsqlOptionsBuilder(optBuilder, dbConfig.ToString()!);
    }, ServiceLifetime.Transient
);

builder.Services.AddDbContext<MessageLogDbContext>(
    (sp, optBuilder) =>
    {
        var dbConfig = sp.GetRequiredService<ConfigurationReaderService>().MessageLogDbConfig;
        PgsqlDbHelper.PgsqlOptionsBuilder(optBuilder, dbConfig.ToString()!);
    }, ServiceLifetime.Transient
);

builder.Services.AddInstanceFactory<ProjectDbContext, DIExtensions.InstanceFactory<ProjectDbContext>>(
    s =>
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
        var connectionString = s.GetRequiredService<ConfigurationReaderService>().ProjectDbConfig!.ToString()!;
        PgsqlDbHelper.PgsqlOptionsBuilder(optionsBuilder, connectionString);
        return new(optionsBuilder.Options);
    });

builder.Services.AddGrpcReflection();

builder.Host.UseSerilog(
    (context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
);
builder.Host.UseDefaultServiceProvider(options => options.ValidateOnBuild = true);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

await using (var configurationDbContext = app.Services.GetRequiredService<ConfigurationDbContext>())
{
    await configurationDbContext.Database.EnsureCreatedAsync(CancellationToken.None);

    await GenerateProductKey(configurationDbContext);
}

await using (var messageLogDbContext = app.Services.GetRequiredService<MessageLogDbContext>())
{
    await messageLogDbContext.Database.EnsureCreatedAsync(CancellationToken.None);
}

await using (var projectDbContext = app.Services.GetRequiredService<ProjectDbContext>())
{
    await projectDbContext.Database.EnsureCreatedAsync(CancellationToken.None);
    // var migrateTask = projectDbContext.Database.MigrateAsync();
    // await migrateTask;
    //
    // if (migrateTask.Exception != null)
    // {
    //     throw migrateTask.Exception;
    // }
}

// var historyDbConfig = app.Services.GetRequiredService<ConfigurationReaderService>().HistoryDbConfig;
// var historyDataBaseInfo = new HistoryDataBaseInfo()
// {
//     DataBase = historyDbConfig.DataBase,
//     Host = historyDbConfig.Host,
//     Port = historyDbConfig.Port,
//     User = historyDbConfig.UserName,
//     Password = historyDbConfig.Password,
//     CommandTimeout = historyDbConfig.CommandTimeout
// };
//
// if (!InitializeCassandra())
// {
//     Console.WriteLine("Не удалось инициализировать БД Apache Cassandra");
// }

app.MapGrpcService<InfoApiService>();
app.MapGet("/", () => "Info Service. Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
return;

// bool InitializeCassandra()
// {
//     try
//     {
//         var cassandraDb = new CassandraHistory();
//
//         if (!cassandraDb.TryConnect(historyDataBaseInfo, out MsgLogClass msgLogTryConnect))
//         {
//             Console.WriteLine(msgLogTryConnect.LogDetails);
//             return false;
//         }
//
//         if (!cassandraDb.TryInitializeHistDB(historyDataBaseInfo.DataBase, out MsgLogClass msgLogTryInitializeHistDB))
//         {
//             Console.WriteLine(msgLogTryInitializeHistDB.LogDetails);
//             return false;
//         }
//
//         cassandraDb.TryDisсonnect();
//     }
//     catch (Exception e)
//     {
//         Console.WriteLine(e);
//         return false;
//     }
//
//     return true;
// }

async Task GenerateProductKey(ConfigurationDbContext configurationDbContext)
{
    var dbSystemIdentifiers = configurationDbContext.Database.SqlQueryRaw<ulong>($"SELECT system_identifier FROM pg_control_system()").ToArray();

    Common.Activation.GenerateProductKey(dbSystemIdentifiers[0].ToString(), out var productkey, out var errors);

    var licenses = configurationDbContext.Licenses.ToList();
    if (licenses.Count == 0)
    {
        licenses.Add(new Activation()
        {
            Id = 1,
            ProductKey = productkey
        });

        configurationDbContext.Licenses.AddRange(licenses);
        await configurationDbContext.SaveChangesAsync();
    }
}