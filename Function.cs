using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Text.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GameCharacterLambda;

public class Function
{
    private static readonly Random Random = new();

    public APIGatewayHttpApiV2ProxyResponse FunctionHandler(
        APIGatewayHttpApiV2ProxyRequest request,
        ILambdaContext context)
    {
        string name = "Unknown";
        string characterClass = "Warrior";

        if (request.QueryStringParameters != null)
        {
            if (request.QueryStringParameters.TryGetValue("name", out var nameValue)
                && !string.IsNullOrWhiteSpace(nameValue))
            {
                name = nameValue;
            }

            if (request.QueryStringParameters.TryGetValue("class", out var classValue)
                && !string.IsNullOrWhiteSpace(classValue))
            {
                characterClass = classValue;
            }
        }

        string[] weapons =
        {
            "Steel Sword",
            "Magic Staff",
            "Battle Axe",
            "Golden Bow",
            "Shadow Dagger"
        };

        string[] elements =
        {
            "Fire",
            "Water",
            "Earth",
            "Wind",
            "Lightning"
        };

        int level = Random.Next(1, 11);
        int power = Random.Next(50, 101);
        string weapon = weapons[Random.Next(weapons.Length)];
        string element = elements[Random.Next(elements.Length)];

        var result = new
        {
            name,
            characterClass,
            level,
            power,
            weapon,
            element,
            message = $"{name} became a powerful {characterClass}!"
        };

        return new APIGatewayHttpApiV2ProxyResponse
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                ["Content-Type"] = "application/json"
            },
            Body = JsonSerializer.Serialize(result)
        };
    }
}