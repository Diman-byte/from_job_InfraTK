using System.Text.Json;
using Common;
using Common.DependencyInjection;
using Common.Services.KeyCloak;
using DataBase.Entities.ProjectDataBase.DbContext;
using Grpc.Core;
using Grpc.InfoApi;
using InfoService.Services;

namespace InfoService.GrpcApi.Services;

public class InfoApiService : InfoApi.InfoApiBase
{
    private readonly KeyCloakApi _keyCloakApi;
    private readonly ConfigurationReaderService _configuration;
    private readonly DIExtensions.InstanceFactory<ProjectDbContext> _projectDbContextFactory;

    public InfoApiService(KeyCloakApi keyCloakApi, ConfigurationReaderService configuration, DIExtensions.InstanceFactory<ProjectDbContext> projectDbContextFactory)
    {
        _keyCloakApi = keyCloakApi;
        _configuration = configuration;
        _projectDbContextFactory = projectDbContextFactory;
    }

    public override Task<DataBaseInfo> GetConfigDbInfo(ArgRequest request, ServerCallContext context)
    {
        var config = _configuration.ConfigurationDbConfig;
        return Task.FromResult(new DataBaseInfo()
        {
            DataBase = config.DataBase,
            DbConfig = JsonSerializer.Deserialize<DbConfig>(JsonSerializer.Serialize(config))
        });
    }

    public override Task<DataBaseServerInfo> GetProjectDbServerInfo(ArgRequest request, ServerCallContext context)
    {
        var config = _configuration.ProjectDbConfig;
        return Task.FromResult(new DataBaseServerInfo()
        {
            Prefix = config.DataBase,
            DbConfig = JsonSerializer.Deserialize<DbConfig>(JsonSerializer.Serialize(config))
        });
    }

    public override Task<DataBaseServerInfo> GetMessageLogDbServerInfo(ArgRequest request, ServerCallContext context)
    {
        var config = _configuration.MessageLogDbConfig;
        return Task.FromResult(new DataBaseServerInfo()
        {
            Prefix = config.DataBase,
            DbConfig = JsonSerializer.Deserialize<DbConfig>(JsonSerializer.Serialize(config))
        });
    }

    public override Task<DataBaseServerInfo> GetHistoryDbServerInfo(ArgRequest request, ServerCallContext context)
    {
        var config = _configuration.HistoryDbConfig;
        return Task.FromResult(new DataBaseServerInfo()
        {
            Prefix = config.DataBase,
            DbConfig = JsonSerializer.Deserialize<DbConfig>(JsonSerializer.Serialize(config)),
        });
    }

    public override Task<KeyCloakInfo> GetKeyCloakInfo(EmptyArgRequest request, ServerCallContext context)
    {
        return Task.FromResult(new KeyCloakInfo()
        {
            BaseUri = _keyCloakApi.KeyCloakServerInfo.BaseUri,
            ClientId = _keyCloakApi.KeyCloakServerInfo.ClientId,
            Realm = _keyCloakApi.KeyCloakServerInfo.Realm,
            Scope = _keyCloakApi.KeyCloakServerInfo.Scope
        });
    }

    public override Task<HistoryDbInfo> GetCassandraDbInfo(ArgRequest request, ServerCallContext context)
    {
        var config = _configuration.HistoryDbConfig;
        return Task.FromResult(new HistoryDbInfo()
        {
            Database = config.DataBase,
            Host = config.Host,
            Port = config.Port,
            UserName = config.UserName,
            Password = config.Password,
            CommandTimeout = config.CommandTimeout,
        });
    }

    public override Task<ServicesInfo> GetServicesInfo(ArgRequest request, ServerCallContext context)
    {
        ServicesInfo result = new ServicesInfo();

        try
        {
            using var projectDbContext = _projectDbContextFactory.Create();
            var dataCollectors = projectDbContext.DataCollectorServices.ToList();
            var winDataCollectors = projectDbContext.WinDataCollectorServices.ToList();
            var dataProcessingServices = projectDbContext.DataProcessingServices.ToList();
            var dataServerServices = projectDbContext.DataServerServices.ToList();

            List<ServiceInfo> services = dataCollectors.Select(dataCollector => new ServiceInfo()
                {
                    Id = dataCollector.Id.ToString(),
                    Name = dataCollector.Name,
                    Desc = dataCollector.Description,
                    Host = dataCollector.Host,
                    Port = dataCollector.Port.ToString(),
                    ServiceType = ServiceTypeEnum.DataCollectorService.ToString()
                })
                .ToList();
            
            services.AddRange(winDataCollectors.Select(winDataCollector => new ServiceInfo()
            {
                Id = winDataCollector.Id.ToString(),
                Name = winDataCollector.Name,
                Desc = winDataCollector.Description,
                Host = winDataCollector.Host,
                Port = winDataCollector.Port.ToString(),
                ServiceType = ServiceTypeEnum.WinDataCollectorService.ToString()
            }));

            services.AddRange(dataProcessingServices.Select(dataProcessingService => new ServiceInfo()
            {
                Id = dataProcessingService.Id.ToString(),
                Name = dataProcessingService.Name,
                Desc = dataProcessingService.Description,
                Host = dataProcessingService.Host,
                Port = dataProcessingService.Port.ToString(),
                ServiceType = ServiceTypeEnum.DataProcessingService.ToString()
            }));
            
            services.AddRange(dataServerServices.Select(dataServerService=> new ServiceInfo()
            {
                Id = dataServerService.Id.ToString(),
                Name = dataServerService.Name,
                Desc = dataServerService.Description,
                Host = dataServerService.Host,
                Port = dataServerService.Port.ToString(),
                ServiceType = ServiceTypeEnum.DataServerService.ToString()
            }));

            result.ServiceInfo.Add(services);

            result.Message = "Ok";
        }
        catch (Exception ex)
        {
            result.Message = $"Error: {ex}";
        }
        
        return Task.FromResult(result);
    }
}