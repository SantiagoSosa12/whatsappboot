namespace WhatsappNet.Services.WhatsAppCloud.SendMessage
{
    public interface InterfaceWhatsAppCloudSendMessage
    {
        Task<bool> Execute(object model);
    }
}
