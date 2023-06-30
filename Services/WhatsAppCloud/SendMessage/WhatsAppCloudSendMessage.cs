using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WhatsappNet.Services.WhatsAppCloud.SendMessage

{
    public class WhatsAppCloudSendMessage: InterfaceWhatsAppCloudSendMessage
    {
        public async Task<bool> Execute(object model)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            DotNetEnv.Env.Load();
            string? tokenFacebook = Environment.GetEnvironmentVariable("TOKEN_FACEBOOK");
            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumber = "120270344405645";
                string? accessToken = tokenFacebook;
                string uri = $"{endpoint}/v16.0/{phoneNumber}/messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if(response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
