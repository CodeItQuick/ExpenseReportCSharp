using Microsoft.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Tests.Adapter.WebApplication
{
    public static class AntiForgeryTokenExtractor
    {
        public static string AntiForgeryFieldName { get; } = "AntiForgeryTokenField";
        public static string AntiForgeryCookieName { get; } = "AntiForgeryTokenCookie";

        private static string ExtractCookieValue(HttpResponseMessage response)
        {
            string antiForgeryCookie = response.Headers
                .GetValues("Set-Cookie")
                .FirstOrDefault(x => x.Contains(AntiForgeryCookieName)) ?? string.Empty;

            if (antiForgeryCookie is null)
                throw new ArgumentException($"Cookie '{AntiForgeryCookieName}' not found in HTTP response", nameof(response));

            string antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value.ToString();

            return antiForgeryCookieValue;
        }

        private static string ExtractAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");
            if (requestVerificationTokenMatch.Success)
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found", nameof(htmlBody));
        }

        public static async Task<(string field, string cookie)> ExtractAntiForgeryValues(HttpResponseMessage response)
        {
            var cookie = ExtractCookieValue(response);
            var token = ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync());

            return (token, cookie);
        }
    }
}