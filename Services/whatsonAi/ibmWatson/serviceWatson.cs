using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.Assistant.v2;
using IBM.Watson.Assistant.v2.Model;
using Newtonsoft.Json;

namespace WhatsappNet.Services.whatsonAi.ibmWatson
{

    public sealed class ServiceWatson
    {

        private static readonly ServiceWatson _instance = new();
        private static string? _key;
        private static string? _url;
        private static string? _enviromentId;

        private AssistantService _assistant;

        private ServiceWatson()
        {
            this.chargeEnviromentVariables();
            this.createAssistant();
        }

        private void chargeEnviromentVariables()
        {
            DotNetEnv.Env.Load();
            _key = System.Environment.GetEnvironmentVariable("KEY_WATSON2");
            _url = System.Environment.GetEnvironmentVariable("SERVICE_INSTANCE_URL2");
            _enviromentId = System.Environment.GetEnvironmentVariable("LIVE_ENVIROMENT_ID2");
        }

        public static ServiceWatson GetInstance()
        {
            return _instance;
        }





        public void createAssistant()
        {

            IamAuthenticator authenticator = new IamAuthenticator(apikey: _key);
            this._assistant = new AssistantService("2023-07-27", authenticator);
            this._assistant.SetServiceUrl(_url);
        }

        public OwnOutput chatWithWatson(string messageToSend)
        {

            var result = this._assistant.MessageStateless(
                assistantId: _enviromentId,
                input: new MessageInputStateless()
                {
                    Text = messageToSend
                }
                );
            var settings = new JsonSerializerSettings
            {
                Error = (obj, args) =>
                {
                    var context = args.ErrorContext;
                    context.Handled = true;
                }
            };

            OutputWatson json = JsonConvert.DeserializeObject<OutputWatson>(result.Response, settings);
            return json.output;
        }
    }

}