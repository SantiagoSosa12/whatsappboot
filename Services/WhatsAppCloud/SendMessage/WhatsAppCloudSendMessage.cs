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

            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumber = "120270344405645";
                string accessToken = "EAACeXSkdlAsBACW1DOAzl2OzIJZAXP6bFIXYENBimn8KV0k8AoFNmweYY1HLrEXn07N21jZBWD7xXXAkqAhmP0A7G27eUVmxr8mXfc1sYfaKBxG44bej9n718OOnEcT4C5cKqh0rhllbUD0GmT6Nz4hMO5ZATc4yKhXG1WeZCCOFcQTWZBZCJBy3Y3f7Yh0k1K9KPndPAROwZDZD";
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
