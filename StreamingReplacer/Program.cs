using Newtonsoft.Json;
using StreamingReplacer;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

WriteWithColor("Input File Path:", ConsoleColor.Blue);

var inputFilePath = Console.ReadLine()?.Trim('\"');
var inputFileDirectory = Path.GetDirectoryName(inputFilePath);
Console.WriteLine();

if (inputFilePath is null || inputFileDirectory is null)
{
    WriteWithColor($"File path '{inputFilePath}' is invalid.", ConsoleColor.Red);
    EnterToExit();
    return;
}
WriteWithColor("Please input a ',' separated list of words that require the message content to be deleted.", ConsoleColor.Blue);

var wordsToReplace = Console.ReadLine()?.Split(',').Select(x => x.Trim()).ToArray();
Console.WriteLine();
    
if (wordsToReplace is null)
{
    WriteWithColor("No words to replace.", ConsoleColor.Red);
    EnterToExit();
    return;
}

var cleansedFileName = $"cleansed_{Path.GetFileName(inputFilePath)}";

if (File.Exists(cleansedFileName))
{
    File.Delete(cleansedFileName);
    File.Create(cleansedFileName);
}

var serializer = new JsonSerializer();
using var inputStream = File.OpenRead(inputFilePath);
using var textReader = new StreamReader(inputStream);
using var jsonReader = new JsonTextReader(textReader);
using var jsonArray = new JsonArrayStreamer<TaskActivity>(jsonReader);

using var outputStream = File.OpenWrite(Path.Combine(inputFileDirectory, cleansedFileName));
using var textWriter = new StreamWriter(outputStream);
using var jsonWriter = new JsonTextWriter(textWriter){Formatting = Formatting.Indented};

Console.ForegroundColor = ConsoleColor.Magenta;
jsonWriter.WriteStartArray();
var linesProcessed = 0;
var messagesReplaced = 0;
foreach (var line in jsonArray)
{
    linesProcessed++;
    foreach (var word in wordsToReplace)
    {
        if (line.Message.Contains(word))
        {
            messagesReplaced++;
            line.Message = string.Empty;
        }
    }
    serializer.Serialize(jsonWriter, line);
    Console.Write("\rObjects Processed: {0}, Messages Replaced: {1}", linesProcessed, messagesReplaced);
}

jsonWriter.WriteEndArray();

Console.WriteLine("\r\n");
WriteWithColor("Completed", ConsoleColor.Green);
EnterToExit();

void EnterToExit()
{
    Console.ResetColor();
    Console.WriteLine($"Press 'Enter' to exit.");
    Console.ReadLine();
}

void WriteWithColor(string message, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ResetColor();
}
