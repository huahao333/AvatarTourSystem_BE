using System.Text.Json;

namespace AvatarTourSystem_BE.JsonPolicies
{
    public class KebabCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            // Chuyển từ PascalCase/camelCase sang kebab-case
            return string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
        }
    }
}
