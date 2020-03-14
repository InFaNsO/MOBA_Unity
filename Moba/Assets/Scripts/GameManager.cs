using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    [System.Serializable]
    struct TeamColor
    {
        public Color slot1;
        public Color slot2;
        public Color slot3;
        public Color slot4;
        public Color slot5;
    }

    [SerializeField] int numPlayers = 4;
    [SerializeField] TeamColor TeamGloomy;
    [SerializeField] TeamColor TeamSunny;

    [System.Serializable]
    struct PlayerInfo
    {
        public string PlayerName;
        public string HeroSelected;
        public NetworkConnection clientConnection;
    }
    [SyncVar] List<PlayerInfo> sunnyPlayers;
    [SyncVar] List<PlayerInfo> gloomyPlayers;

    [SyncVar] private bool gameStarted = false;

    public static GameManager sInstance = null;


    void Start()
    {
        if (sInstance = null)
            sInstance = this;
    }

    [Command]
    public bool Cmd_AddNewPlayer(string name, string heroSelected, int connectionID)
    {
        if (sunnyPlayers.Count >= numPlayers && gloomyPlayers.Count >= numPlayers)
            return false;


        NetworkConnection con = null;
        foreach(var connection in NetworkServer.connections)
        {
            if(connection.connectionId == connectionID)
            {
                con = connection;
                break;
            }
        }


        PlayerInfo info = new PlayerInfo();
        info.PlayerName = name;
        info.HeroSelected = heroSelected;
        info.clientConnection = con;

        if (sunnyPlayers.Count <= gloomyPlayers.Count)
            sunnyPlayers.Add(info);
        else
            gloomyPlayers.Add(info);

        return true;
    }

    private void OnGui()
    {
        GUI.BeginGroup(new Rect(Screen.width - 200, 0, 200, 400));
        if(NetworkServer.active && sunnyPlayers.Count > 0)
        {
            if(GUILayout.Button("Start Match!!!"))
            {

            }
        }

        GUILayout.Label("-=Team Sunny=-");
        foreach (var info in sunnyPlayers)
        {
            GUILayout.Label(info.PlayerName + " : " + info.HeroSelected);
        }
        GUILayout.Label("-=Team Gloomy=-");
        foreach (var info in gloomyPlayers)
        {
            GUILayout.Label(info.PlayerName + " : " + info.HeroSelected);
        }

        GUI.EndGroup();
    }


    void StartMatch()
    {
        gameStarted = true;
        for (int i = 0; i < sunnyPlayers.Count; ++i)
        {
            var pInfo = sunnyPlayers[i];
            var spawn = GameObject.Find("Sunny" + (i + 1).ToString());
            var fab = HeroCatelog.sInstance.GetHeroByName(pInfo.HeroSelected);

            var h = Instantiate(fab, spawn.transform.position, spawn.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(h, pInfo.clientConnection);
        }

        for (int i = 0; i < gloomyPlayers.Count; ++i)
        {
            var pInfo = gloomyPlayers[i];
            var spawn = GameObject.Find("Gloomy" + (i + 1).ToString());
            var fab = HeroCatelog.sInstance.GetHeroByName(pInfo.HeroSelected);

            var h = Instantiate(fab, spawn.transform.position, spawn.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(h, pInfo.clientConnection);
        }
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }
}
