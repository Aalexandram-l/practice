using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web; 

public class JsonService
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true, 
        IgnoreNullValues = true, 
        Converters = { new DateTimeConverter() }, 
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
    };

    public static string SerializeStudent(Student student)
    {
        return JsonSerializer.Serialize(student, _options);
    }

    public static Student DeserializeStudent(string json)
    {
        return JsonSerializer.Deserialize<Student>(json, _options);
    }

    public static void SaveJsonToFile(string json, string filePath)
    {
        File.WriteAllText(filePath, json);
    }

    public static string LoadJsonFromFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string _dateFormat = "yyyy-MM-dd";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), _dateFormat, null);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_dateFormat));
    }
}