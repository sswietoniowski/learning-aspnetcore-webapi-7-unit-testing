namespace Hr.Api.Dtos;

public class StatisticsDto
{
    public string LocalIpAddress { get; set; } = string.Empty;
    public int LocalPort { get; set; }
    public string RemoteIpAddress { get; set; } = string.Empty;
    public int RemotePort { get; set; }
}