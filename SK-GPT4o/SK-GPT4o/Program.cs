#pragma warning disable SKEXP0001, SKEXP0003, SKEXP0010, SKEXP0011, SKEXP0050, SKEXP0052
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var model = "gpt-4o";

var builder = Kernel.CreateBuilder();

builder.AddOpenAIChatCompletion(
    model,
    config["OPEN_AI_KEY"],
    config["OPEN_AI_ORG_ID"]);

var kernel = builder.Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();

var history = new ChatHistory();

history.AddSystemMessage("You are a friendly and helpful assistant that responds to questions directly");

var message = new ChatMessageContentItemCollection
{
    new TextContent("Can you tell me what's in this image?"),
    new ImageContent(new Uri("https://www.visitmelbourne.com/-/media/atdw/melbourne/things-to-do/art-theatre-and-culture/art-galleries/28a00521a646bfc70a1d6b650516284d_1600x1200.jpeg?ts=20210615280402"))
};

history.AddUserMessage(message);

var result = await chat.GetChatMessageContentAsync(history);
Console.WriteLine($"Let me describe that image for you: {result}");