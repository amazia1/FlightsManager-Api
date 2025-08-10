using System.Text.Json.Serialization;

namespace API.Shared.Dtos
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusEnum
    {
        Boarding,
        Departed,
        Landed,
        Scheduled
    }
}