namespace WhatsappNet.Util
{
    public interface InterfaceUtil
    {
        object buttonMessage(string number);
        object locationMessage(string number, string latitude = "44.05898096392521", string longitud = "-92.5037224307199", string name = "IBM EEUU", string address = "2800 37th St NW, Rochester, MN 55901, Estados Unidos");
        object documentMessage(string url, string number);
        object videoMessage(string url, string number);
        object audioMessage(string url, string number);
        object imageMessage(string url, string number);

        object TextMessage(string message, string number);

    }
}
