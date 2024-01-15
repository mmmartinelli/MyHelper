using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;

namespace src;

public static class JsonHelper
{
    public static string Serialize(object? o)
    {
        JsonSerializerOptions jso = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(o ?? new JsonArray(), jso);
    }

    public static string Serialize(object? o, JsonNamingPolicy? namingPolicy)
    {
        JsonSerializerOptions jso = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(o ?? new JsonArray(), jso);
    }

    public static T? Deserialize<T>(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return default;

        JsonSerializerOptions jso = new()
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(s, jso);
    }

    public static T? DeserializeSilent<T>(string s)
    {
        try
        {
            JsonSerializerOptions jso = new()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(s, jso);
        }
        catch
        {
            return default;
        }
    }

    public static string GetNode(string? s, string? nodeName)
    {
        if (string.IsNullOrWhiteSpace(s) || string.IsNullOrWhiteSpace(nodeName))
            return new JsonArray().ToJsonString();

        JsonSerializerOptions jso = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true
        };

        JsonNode? jsonNode = JsonNode.Parse(s);

        return jsonNode?[nodeName]?.ToJsonString(jso) ?? new JsonArray().ToJsonString();
    }
}