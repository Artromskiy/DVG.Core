using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DVG.Sheets
{
    public static class SheetParser
    {
        private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };
        public static string TsvToJson(string tsv, int headerRows) =>
            TsvToJsonObject(tsv, headerRows).ToJsonString(_options);
        public static string CsvToJson(string csv, int headerRows) =>
            CsvToJsonObject(csv, headerRows).ToJsonString(_options);

        public static JsonArray TsvToJsonObject(string csv, int headerRows) =>
            ParseTable(ParseDsv(csv, '\t'), headerRows);

        public static JsonArray CsvToJsonObject(string csv, int headerRows) =>
            ParseTable(ParseDsv(csv, ','), headerRows);

        private static JsonArray ParseTable(string[,] table, int headerRows)
        {
            int rowCount = table.GetLength(0);
            int colCount = table.GetLength(1);

            var result = new JsonArray();

            for (int r = headerRows; r < rowCount; r++)
            {
                var obj = new JsonObject();
                bool hasData = false;

                for (int c = 0; c < colCount; c++)
                {
                    var value = table[r, c];
                    if (value == null)
                        continue;

                    var path = BuildPath(table, c, headerRows);
                    Insert(obj, path, value);
                    hasData = true;
                }

                if (hasData)
                    result.Add(obj);
            }

            return result;
        }


        private static string[,] ParseDsv(string csv, char divisor)
        {
            var lines = csv
                .Split('\n')
                .Select(l => l.TrimEnd())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            int rows = lines.Count;
            int cols = lines.Max(l => l.Split(divisor).Length);

            var table = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                var cells = lines[r].Split(divisor);
                for (int c = 0; c < cells.Length; c++)
                    table[r, c] = string.IsNullOrWhiteSpace(cells[c]) ? null : cells[c];
            }

            return table;
        }

        private static List<string> BuildPath(string[,] table, int col, int headerRows)
        {
            var path = new List<string>();

            int startRow = headerRows - 1;
            int startCol = col;

            int c = startCol;
            int r;

            while (c >= 0)
            {
                r = startRow;

                while (r >= 0 && table[r, c] == null)
                    r--;

                if (r < 0)
                {
                    c--;
                }
                else
                {
                    path.AddRange(table[r, c].Split('/').Reverse());

                    startRow = r - 1;

                    if (r == 0)
                        break;
                }
            }

            path.Reverse();
            return path;
        }

        private static void Insert(JsonObject root, List<string> path, string rawValue)
        {
            JsonObject current = root;

            for (int i = 0; i < path.Count; i++)
            {
                var token = path[i];

                bool isArray = token.Contains("[]");
                bool isObject = token.Contains("{}");

                var key = token.Replace("[]", "").Replace("{}", "");
                bool isLast = i == path.Count - 1;

                if (isArray)
                {
                    if (!current.TryGetPropertyValue(key, out var arrNode))
                    {
                        arrNode = new JsonArray();
                        current[key] = arrNode;
                    }

                    var array = arrNode.AsArray();

                    if (isLast)
                    {
                        if (isObject)
                        {
                            var obj = new JsonObject
                            {
                                [rawValue] = new JsonObject()
                            };
                            array.Add(obj);
                        }
                        else
                        {
                            array.Add(ParseValue(rawValue));
                        }
                        return;
                    }

                    var element = new JsonObject();
                    array.Add(element);
                    current = element;
                    continue;
                }

                if (isObject && isLast)
                {
                    if (!current.TryGetPropertyValue(key, out var map))
                    {
                        map = new JsonObject();
                        current[key] = map;
                    }

                    map.AsObject()[rawValue] = new JsonObject();
                    return;
                }

                if (isLast)
                {
                    current[key] = ParseValue(rawValue);
                    return;
                }

                if (!current.TryGetPropertyValue(key, out var next))
                {
                    next = new JsonObject();
                    current[key] = next;
                }

                current = next.AsObject();
            }
        }



        private static JsonNode ParseValue(string value)
        {
            if (int.TryParse(value, out var i))
                return JsonValue.Create(i);

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var f))
                return JsonValue.Create(f);

            if (bool.TryParse(value, out var b))
                return JsonValue.Create(b);

            return JsonValue.Create(value);
        }
    }
}