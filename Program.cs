using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace C__JSON_Reader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            string filePath = "data.json";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Файл {filePath} не найден!");
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу, чтобы завершить.");
                Console.ReadKey();
            }
            else { 

                try
                {
                    string json = File.ReadAllText(filePath);
                    JsonDocument document = JsonDocument.Parse(json);

                    Console.WriteLine("Содержимое JSON-файла:");
                    Console.WriteLine("======================");
                    PrintJsonElement(document.RootElement, 0);
                    Console.WriteLine("======================");
                    Console.WriteLine();
                    Console.WriteLine("Нажмите любую клавишу, чтобы завершить.");
                    Console.ReadKey();

                }
                catch (JsonException)
                {
                    Console.WriteLine("Ошибка чтения JSON. Проверьте формат файла.");
                    Console.WriteLine();
                }
            }
            
        }

        static void PrintJsonElement(JsonElement element, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 2);

            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    Console.WriteLine($"{indent}{{");
                    foreach (var property in element.EnumerateObject())
                    {
                        Console.Write($"{indent}  \"{property.Name}\": ");
                        PrintJsonElement(property.Value, indentLevel + 1);
                    }
                    Console.WriteLine($"{indent}}}");
                    break;

                case JsonValueKind.Array:
                    Console.WriteLine($"{indent}[");
                    foreach (var item in element.EnumerateArray())
                    {
                        PrintJsonElement(item, indentLevel + 1);
                    }
                    Console.WriteLine($"{indent}]");
                    break;

                case JsonValueKind.String:
                    Console.WriteLine($"\"{element.GetString()}\"");
                    break;

                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int intValue))
                        Console.WriteLine($"{intValue}");
                    else
                        Console.WriteLine($"{element.GetDouble()}");
                    break;

                case JsonValueKind.True:
                    Console.WriteLine("true");
                    break;

                case JsonValueKind.False:
                    Console.WriteLine("false");
                    break;

                case JsonValueKind.Null:
                    Console.WriteLine("null");
                    break;

                default:
                    Console.WriteLine($"{indent}Unknown type: {element.ValueKind}");
                    break;
            }
        }
    }
}
