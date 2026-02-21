using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using UnityEngine;

namespace DVG.Sheets
{
    public class SheetLoader
    {
        private const string TsvFormat = "https://docs.google.com/spreadsheets/d/{0}/export?format=tsv&gid={1}";
        private const string CsvFormat = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
        private readonly HttpClient _client = new();

        public string TableId { get; set; }
        public Sheet[] Sheets { get; set; }

        public Task<Dictionary<string, JsonArray>> LoadAsCsv() =>
            Load(CsvUrl, SheetParser.CsvToJsonObject);

        public Task<Dictionary<string, JsonArray>> LoadAsTsv() =>
            Load(TsvUrl, SheetParser.TsvToJsonObject);

        private string CsvUrl(int id) => string.Format(CsvFormat, TableId, id);
        private string TsvUrl(int id) => string.Format(TsvFormat, TableId, id);

        private async Task<Dictionary<string, JsonArray>> Load(Func<int, string> urlFormat, Func<string, int, JsonArray> parser)
        {
            var loads = Array.ConvertAll(Sheets, s => Load(s, urlFormat));
            var result = await Task.WhenAll(loads);
            try
            {
                return result.ToDictionary(r => r.request.Name,
                    r => parser(r.response, r.request.HeaderRows));
            }
            catch (Exception e)
            {
                Trace.TraceError($"Failed to parse sheet: {e.Message}");
            }
            return null;
        }

        private async Task<(Sheet request, string response)> Load(Sheet sheet, Func<int, string> urlFormatter)
        {
            var requestUrl = urlFormatter(sheet.Id);
            using HttpResponseMessage response = await _client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return (sheet, responseBody);
        }
    }
}