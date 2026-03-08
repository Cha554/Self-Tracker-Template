using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using Newtonsoft.Json;
using Photon.Pun;
/*
  **Change Where Ever It Says 'YOUR NAME' To Discord Display Name**

You can do this so that others know who the self tracker is tracking
If your in game name is diffrent from your Discord name.
 
 
           **Creds to Cha554 for making this Template**
 
 */
[BepInPlugin("com.mist.selftracker", "YOUR NAME's Self Tracker", "1.0.0")]
public class SelfTracker : BaseUnityPlugin
{
    

    private async Task Awake()
    {
        await Task.Delay(5000);
        SendWeb(
            "YOUR NAME has loaded into the game.",
            "**Name:** " + PhotonNetwork.LocalPlayer.NickName
        );
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && i < 1)
        {
            i++;
            room = PhotonNetwork.CurrentRoom.Name;
            SendWeb(
                "YOUR NAME has joined a room.",
                "**Name:** " + PhotonNetwork.LocalPlayer.NickName + "\n" +
                "**Room:** " + PhotonNetwork.CurrentRoom.Name + "\n" +
                "**Players:** " + PhotonNetwork.CurrentRoom.PlayerCount + "/10"
            );
        }
        if (!PhotonNetwork.InRoom && i >= 1)
        {
            i = 0;
            SendWeb(
                "YOUR NAME has left a room.",
                "**Name:** " + PhotonNetwork.LocalPlayer.NickName + "\n" +
                "**Room:** " + room
            );
        }
    }

    private void OnApplicationQuit()
    {
        SendWeb(
            "YOUR NAME has closed the game.",
            "**Name:** " + PhotonNetwork.LocalPlayer.NickName
        );
    }

    public static async void SendWeb(string Title, string Desc)
    {
        await SendEmbedToDiscordWebhook("----------YOUR WEB HOOK----------", Title, Desc, "#000000"/*your color*/);
    }

    #region Utilities [DONT CHANGE]
    private static int i;
    private static string room;
    private static int ConvertHexColorToDecimal(string color)
    {
        if (color.StartsWith("#"))
        {
            color = color[1..];
        }
        return int.Parse(color, NumberStyles.HexNumber);
    }

    public static async Task SendEmbedToDiscordWebhook(string webhookUrl, string title, string description, string colorHex)
    {
        var embed = new
        {
            title,
            description,
            color = ConvertHexColorToDecimal(colorHex)
        };
        var payload = new
        {
            embeds = new[] { embed }
        };

        string jsonPayload = JsonConvert.SerializeObject((object)payload);
        StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

        using HttpClient httpClient = new();
        HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
        if (!response.IsSuccessStatusCode)
        {
            await response.Content.ReadAsStringAsync();
        }
    }
    #endregion
}