using System.Text;
using Common.Services.Configuration;
using Common.Services.KeyCloak;

namespace InfoService.Services;

public class ConfigurationReaderService
{
    public AppSettings AppSettings { get; set; }
    public DbConnectionConfig ConfigurationDbConfig { get; set; }
    public DbConnectionConfig ProjectDbConfig { get; set; }
    public DbConnectionConfig MessageLogDbConfig { get; set; }
    public DbConnectionConfig HistoryDbConfig { get; set; }

    public ConfigurationReaderService(IConfiguration configuration, KeyCloakApi keyCloakApi)
    {
        AppSettings = configuration.GetSection(nameof(AppSettings))
            .Get<AppSettings>() ?? throw new InvalidOperationException();
        
        ConfigurationDbConfig = configuration.GetSection(nameof(ConfigurationDbConfig))
            .Get<DbConnectionConfig>() ?? throw new InvalidOperationException();
        
        ProjectDbConfig = configuration.GetSection(nameof(ProjectDbConfig))
            .Get<DbConnectionConfig>() ?? throw new InvalidOperationException();
        
        MessageLogDbConfig = configuration.GetSection(nameof(MessageLogDbConfig))
            .Get<DbConnectionConfig>() ?? throw new InvalidOperationException();
        
        HistoryDbConfig = configuration.GetSection(nameof(HistoryDbConfig))
            .Get<DbConnectionConfig>() ?? throw new InvalidOperationException();
        
        keyCloakApi.KeyCloakServerInfo = configuration.GetSection(nameof(KeyCloakServerInfo))
            .Get<KeyCloakServerInfo>() ?? throw new InvalidOperationException();
    }
}

public record AppSettings(string Name, string Version)
{
    public string Name { get; } = Name;
    public string Version { get; } = Version;
}

public record DbConnectionConfig
{
    public string DataBase { get; init; } = "";
    public string Host { get; init; } = "";
    public bool Pooling { get; init; }
    public ushort Port { get; init; }
    public string UserName { get; init; } = "";
    public string Password { get; init; } = "";
    public string? Prefix { get; init; }
    public string ApplicationName { get; init; } = "";
    public int CommandTimeout { get; init; }

    public override string? ToString()
    {
        var properties = GetType().GetProperties().Where(p => p.Name != nameof(Prefix)).ToArray();
        var propertyNames = properties.Select(p => p.Name).ToArray();
        var values = properties
            .Select(p => p.GetValue(this)?.ToString()).ToArray();

        var str = new StringBuilder();
        for (int i = 0; i < propertyNames.Length; i++)
        {
            str.Append($"{propertyNames[i]}={values[i]};");
        }

        return str.ToString();
    }
}