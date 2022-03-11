using System.Text.Json;

namespace Asteh.Domain.Policies
{
	public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy SnakeCase { get; } = new SnakeCaseNamingPolicy();

		public override string ConvertName(string name) => ToSnakeCase(name);

        private static string ToSnakeCase(string input)
        {
            var result = input.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : $"{x}");
            return string.Concat(result).ToLower();
        }
    }
}
