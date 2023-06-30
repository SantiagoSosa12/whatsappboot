using WhatsappNet.Services.WhatsAppCloud.SendMessage;
using WhatsappNet.Services.whatsonAi.ibmWatson;
using WhatsappNet.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<InterfaceWhatsAppCloudSendMessage , WhatsAppCloudSendMessage>();
builder.Services.AddSingleton<InterfaceUtil, Util>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
