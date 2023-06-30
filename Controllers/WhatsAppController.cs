using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WhatsappNet.Models.WhatsAppModel;
using WhatsappNet.Services.WhatsAppCloud.SendMessage;
using WhatsappNet.Services.whatsonAi.ibmWatson;
using WhatsappNet.Util;

namespace WhatsappNet.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : Controller
    {

        private readonly InterfaceWhatsAppCloudSendMessage _whatsAppCloudMessage;
        private readonly InterfaceUtil _InterfaceUtil;
        private string? _phoneNumberToSend;

        public WhatsAppController(InterfaceWhatsAppCloudSendMessage whatsAppCloudMessage, InterfaceUtil util)
        {
            _whatsAppCloudMessage = whatsAppCloudMessage;
            _InterfaceUtil = util;
            this.getPhoneNumber();
        }

        private void getPhoneNumber() {
            DotNetEnv.Env.Load();
            this._phoneNumberToSend = Environment.GetEnvironmentVariable("PHONE_NUMBER");
        }

        [HttpGet("test")]
        public async Task<IActionResult> Sample()
        {
            var data = new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = this._phoneNumberToSend,
                type = "text",
                text = new
                {
                    preview_url = false,
                    body = "Mensaje de prueba enviado por Santiago Sosa Reyes, _Desde la cloud Api de_ *Facebook* y C# 22"
                }
            };
          
            var result = await _whatsAppCloudMessage.Execute(data);
            return Ok("ok sample");
        }


        [HttpGet]
        public IActionResult VerifyToken()
        {
            DotNetEnv.Env.Load();
            string? accesToken = Environment.GetEnvironmentVariable("OWN_TOKEN");
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();
            if (challenge != null && token != null && token == accesToken)
            {
                return Ok(challenge);
            }else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RecivedMessage([FromBody] WhatsAppCloudModel body)
        {
            try {
                var Message = body.Entry[0]?.Changes?[0]?.Value?.Messages?[0];
                if (Message != null)
                {
                    var userNumber = Message.From;
                    var userText = GetUserText(Message).ToUpper();
                    Console.WriteLine("User message: " + userText);
                    List<object> listObjectMessage = new List<object>();
                    
                    this.usingWatson(listObjectMessage , userText);
                    this.sendAllMessages(listObjectMessage);
                }
                return Ok("EVENT_RECEIVED");
            }catch(Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }

        private async void sendAllMessages(List<object> messageList) {
            foreach (var item in messageList)
            {
                await _whatsAppCloudMessage.Execute(item);
            }
        }

        private void usingWatson(List<object> messageList, string userMessage) {
            OwnOutput? responseWatson = ServiceWatson.GetInstance().chatWithWatson(userMessage);

            OwnGeneric generalMessage = responseWatson.generic[0];

            var responseToUser = _InterfaceUtil.TextMessage(generalMessage.text , this._phoneNumberToSend);
            messageList.Add(responseToUser);

            List<OwnOptions> optionsMessages = responseWatson.generic[1].options;
            
            if (optionsMessages.Count > 0)
            {
                List<String> messages = new List<string>();
                foreach (OwnOptions option in optionsMessages)
                {
                    messages.Add(option.label);//Extract each label message from list optiones
                }
                sendPartialMessages(messages , messageList);       
            }
        }

        private void sendPartialMessages(List<String> messages, List<object> messageList) {
            List<String> partialMessages = new List<String>();
            String messageToSelectButton = "Tenemos estas opciones:";
            for (int i = 0; i < messages.Count; i++)
            {
                partialMessages.Add(messages[i]);
                if((i + 1) % 4 == 0 ) {
                    messageToSelectButton = "O estas otras:";
                }
                if((i + 1) % 2 == 0) {
                    var responseUser2 = _InterfaceUtil.buttonMessage(partialMessages , messageToSelectButton ,  this._phoneNumberToSend);
                    Console.WriteLine(" partia Messages tiene los siguientes elementos: " + partialMessages.Count() +  " Complete response: " + JsonConvert.SerializeObject(responseUser2));
                    messageList.Add(responseUser2);
                    partialMessages.Clear();
                }   
            }
        } 

        private string? GetUserText(Message message)
        {
            string? typeMessage = message.Type;
            if(typeMessage?.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }else if (typeMessage?.ToUpper() == "INTERACTIVE")
            {
                string interactiveType = message.Interactive.Type;
                if (interactiveType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                } else if (interactiveType.ToUpper() == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                }else
                {
                    return string.Empty;
                }
            }else
            {
                return string.Empty;
            }
        }
    }
}
