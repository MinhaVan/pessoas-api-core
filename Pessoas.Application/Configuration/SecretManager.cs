using System.Collections.Generic;

namespace Pessoas.Core.Application.Configuration;

public class SecretManager
{
    public IpRateLimiting IpRateLimiting { get; set; }
    public AuthenticatedRateLimit AuthenticatedRateLimit { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public TokenConfigurations TokenConfigurations { get; set; }
    public Asaas Asaas { get; set; }
    public URL URL { get; set; }
    public Infra Infra { get; set; }
    public string AllowedHosts { get; set; }
}
public class Infra
{
    public string Redis { get; set; }
    public RabbitMqSettings RabbitMQ { get; set; }
}

public class RabbitMqSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class IpRateLimiting
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public List<GeneralRule> GeneralRules { get; set; }
}

public class GeneralRule
{
    public string Endpoint { get; set; }
    public string Period { get; set; }
    public int Limit { get; set; }
}

public class AuthenticatedRateLimit
{
    public string Period { get; set; }
    public int Limit { get; set; }
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; }
    public string RedisConnection { get; set; }
    public string RabbitConnection { get; set; }
}

public class TokenConfigurations
{
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public int DaysToExpiry { get; set; }
}

public class Asaas
{
    public string Url { get; set; }
    public string AcessToken { get; set; }
    public string TokenWebHookAsaas { get; set; }
}

public class URL
{
    public string RouterAPI { get; set; }
    public string GatewayAPI { get; set; }
}