using System.Net.Http.Json;
using System.Text.Json;

var apiKey = Environment.GetEnvironmentVariable("OPENWEATHER_API_KEY");
if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.Error.WriteLine("Falta la variable de entorno OPENWEATHER_API_KEY.");
    return;
}

var city = args.Length > 0 ? args[0] : "Mexico City";
using var http = new HttpClient();

var url =
    $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(city)}&appid={apiKey}&units=metric&lang=es";

try
{
    var json = await http.GetFromJsonAsync<JsonElement>(url);
    var temp = json.GetProperty("main").GetProperty("temp").GetDecimal();
    var desc = json.GetProperty("weather")[0].GetProperty("description").GetString();

    var report = $"Clima en {city}: {temp:F1} °C, {desc}";
    Console.WriteLine(report);
    await File.WriteAllTextAsync("output.txt", report);
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine($"Error al consultar la API: {ex.Message}");
}
