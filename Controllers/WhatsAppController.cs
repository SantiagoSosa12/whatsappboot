using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WhatsappNet.Models.WhatsAppModel;
using WhatsappNet.Services.WhatsAppCloud.SendMessage;
using WhatsappNet.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsappNet.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsAppController : Controller
    {

        private readonly InterfaceWhatsAppCloudSendMessage _whatsAppCloudMessage;
        private readonly InterfaceUtil _InterfaceUtil;

        private string _phoneNumberToSend;

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


        [HttpGet("getToken")]
        public IActionResult VerifyToken()
        {
            string accesToken = "DFGHJABAVC&VTSC";
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
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];
                if (Message != null)
                {
                    var userNumber = Message.From;
                    var userText = GetUserText(Message);

                    object objectMessage;

                    switch(userText.ToUpper())
                    {
                        case "TEXT":
                            objectMessage = _InterfaceUtil.TextMessage("Example massage" , this._phoneNumberToSend);
                            break;
                        case "IMAGE":
                            objectMessage = _InterfaceUtil.imageMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png", this._phoneNumberToSend);
                            break;
                        case "VIDEO":
                            objectMessage = _InterfaceUtil.videoMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/video_whatsapp.mp4", this._phoneNumberToSend);
                            break;
                        case "AUDIO":
                            objectMessage = _InterfaceUtil.audioMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/audio_whatsapp.mp3", this._phoneNumberToSend);
                            break;
                        case "DOCUMENT":
                            objectMessage = _InterfaceUtil.documentMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/document_whatsapp.pdf", this._phoneNumberToSend);
                            break;
                        case "LOCATION":
                            objectMessage = _InterfaceUtil.locationMessage(this._phoneNumberToSend);
                            break;
                        case "BUTTON":
                            objectMessage = _InterfaceUtil.buttonMessage(this._phoneNumberToSend);
                            break;
                        default:
                            objectMessage = _InterfaceUtil.TextMessage("Sorry, I am not undestarnd", this._phoneNumberToSend);
                            break;
                    }

                    await _whatsAppCloudMessage.Execute(objectMessage);

                }
                return Ok("EVENT_RECEIVED");
            }catch(Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }

        private string GetUserText(Message message)
        {
            string typeMessage = message.Type;
            if(typeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }else if (typeMessage.ToUpper() == "INTERACTIVE")
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
