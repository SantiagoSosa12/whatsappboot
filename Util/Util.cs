namespace WhatsappNet.Util
{
    public class Util: InterfaceUtil
    {
        public object TextMessage(string message, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "text",
                text = new
                {
                    preview_url = false,
                    body = message
                }
            };
        }

        public object imageMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "image",
                image = new
                {
                    link =  url
                }
            };
        }

        public object audioMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "audio",
                audio = new
                {
                    link = url
                }
            };
        }

        public object videoMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "video",
                video = new
                {
                    link = url
                }
            };
        }

        public object documentMessage(string url, string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "document",
                document = new
                {
                    link = url
                }
            };
        }
        public object locationMessage(string number, string latitude = "44.05898096392521", string longitud = "-92.5037224307199", string name="IBM EEUU", string address= "2800 37th St NW, Rochester, MN 55901, Estados Unidos")
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "location",
                location = new
                {
                    latitude = latitude,
                    longitude = longitud,
                    name = name,
                    address = address
                }
            };
        }

        public object buttonMessage(string number)
        {
            return new
            {
                messaging_product = "whatsapp",
                recipient_type = "individual",
                to = number,
                type = "interactive",
                interactive = new
                {
                    type = "button",
                    body = new
                    {
                        text = "Select an option"
                    },
                    action = new
                    {
                        buttons = new List<object>
                        {
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "01",
                                    title = "Buy"
                                }
                            },
                            new
                            {
                                type = "reply",
                                reply = new
                                {
                                    id = "02",
                                    title = "Sell"
                                }
                            }
                        }
                    }
                }
            };
        }

    }
}
