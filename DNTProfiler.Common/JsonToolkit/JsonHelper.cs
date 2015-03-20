using System.IO;
using System.Linq;
using System.Text;
using DNTProfiler.Common.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DNTProfiler.Common.JsonToolkit
{
    public static class JsonHelper
    {
        private const string IndentString = "    ";


        public static T DeserializeFromFile<T>(string filePath, JsonSerializerSettings settings = null)
        {
            if (!File.Exists(filePath))
                return default(T);

            using (var fileStream = File.OpenRead(filePath))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        var serializer = settings == null ? JsonSerializer.Create() : JsonSerializer.Create(settings);
                        return serializer.Deserialize<T>(reader);
                    }
                }
            }
        }

        public static T DeserializeObject<T>(string jsonData, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(jsonData, settings);
        }

        public static void SerializeToFile(string filePath, object data, JsonSerializerSettings settings = null)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var streamReader = new StreamWriter(fileStream))
                {
                    using (var reader = new JsonTextWriter(streamReader))
                    {
                        var serializer = settings == null ? JsonSerializer.Create() : JsonSerializer.Create(settings);
                        serializer.Serialize(reader, data);
                    }
                }
            }
        }

        public static string ToFormattedJson(this object data, TypeNameHandling typeNameHandling = TypeNameHandling.None)
        {
            if (data == null)
                return string.Empty;

            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                Converters = { new StringEnumConverter() },
                ContractResolver = new OrderedContractResolver(),
                TypeNameHandling = typeNameHandling
            });
        }

        public static string ToFormattedJson(this string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
                return string.Empty;

            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < jsonContent.Length; i++)
            {
                var ch = jsonContent[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(IndentString));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(IndentString));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        var escaped = false;
                        var index = i;
                        while (index > 0 && jsonContent[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(IndentString));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString().Trim();
        }
    }
}