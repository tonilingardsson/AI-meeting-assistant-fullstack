using System.Net.Http.Json;
using System.Text.Json;

namespace AI_Meeting_Assistant.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public GeminiService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;

            _apiKey = configuration["Gemini:ApiKey"]
                ?? throw new InvalidOperationException(
                    "Gemini:ApiKey is not configured.");

            _model = configuration["Gemini:Model"]
                ?? throw new InvalidOperationException(
                    "Gemini:Model is not configured.");

            if (string.IsNullOrWhiteSpace(_apiKey)
                || string.IsNullOrWhiteSpace(_model))
            {
                throw new InvalidOperationException(
                    "Gemini API key and model must not be empty.");
            }
        }

        public async Task<string> GenerateTextAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            string url =
                "https://generativelanguage.googleapis.com/v1beta/models/"
                + Uri.EscapeDataString(_model)
                + ":generateContent";

            using var request = new HttpRequestMessage(
                HttpMethod.Post, url);

            request.Headers.Add("x-goog-api-key", _apiKey);
            request.Content = JsonContent.Create(body);

            using var response = await _httpClient.SendAsync(
                request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"Gemini request failed with HTTP {(int)response.StatusCode}.",
                    null,
                    response.StatusCode);
            }

            string json = await response.Content.ReadAsStringAsync(
                cancellationToken);

            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty(
                    "candidates", out var candidates)
                || candidates.GetArrayLength() == 0
                || !candidates[0].TryGetProperty(
                    "content", out var content)
                || !content.TryGetProperty("parts", out var parts))
            {
                throw new InvalidOperationException(
                    "Gemini returned no text response.");
            }

            var textParts = new List<string>();

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("thought", out var thought)
                    && thought.ValueKind == JsonValueKind.True)
                {
                    continue;
                }

                if (part.TryGetProperty("text", out var text))
                {
                    string? value = text.GetString();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        textParts.Add(value);
                    }
                }
            }

            string result = string.Join("\n", textParts);

            if (string.IsNullOrWhiteSpace(result))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty text response.");
            }

            return result;
        }
    }
}