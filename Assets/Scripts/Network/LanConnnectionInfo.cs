public struct LanConnnectionInfo
{
    public const int PORT = 7750; // Port is fixed and defined in NetworkManager settings

    public string ipAddress;
    public string gameName; // 
    public string hostName;

    // Parses the info from the broadcasted data
    public LanConnnectionInfo(string fromAddress, string data)
    {
        ipAddress = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1); ;

        string[] tokens = data.Split(';');
        gameName = tokens[0];
        hostName = tokens[1];
    }
}