using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PlayerData
{
    public string PlayerId;
    public string playerName;
}

public class ClientManager : MonoBehaviour
{
    SocketIOComponent sIO;
    public const string EVENT_CONNECT = "connect";
    public const string EVENT_DISCONNECT = "close";
    public const string EVENT_SETNAME = "setName";
    public const string EVENT_REGISTERED = "registered";
    public const string EVENT_NEWPLAYER = "newPlayer";
    public const string EVENT_REMOVEPLAYER = "removePlayer";
    static ClientManager instance;
    private string playerName;
    private string id;
    Dictionary<string, PlayerData> players;

    // Start is called before the first frame update
    public static ClientManager Instance => instance;
    void Start()
    {
        players = new Dictionary<string, PlayerData>();
        sIO = GetComponent<SocketIOComponent>();
        sIO.On(EVENT_CONNECT, OnConnected);
        sIO.On(EVENT_DISCONNECT, OnDisconnect);
        sIO.On(EVENT_REGISTERED, OnRegistered);
        sIO.On(EVENT_NEWPLAYER, OnNewPlayer);
        sIO.On(EVENT_REMOVEPLAYER, OnRemovePlayer);
    }

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Connect(string playerName)
    {
        this.playerName = playerName;
        sIO.Connect();
        
    }

    public void Disconnect()
    {
        sIO.Close();
    }

    public void ChangeName(string playerName)
    {
        this.playerName = playerName;
        Dictionary<string, string> jsonData = new Dictionary<string, string> { { "playerName", playerName } };
        sIO.Emit(EVENT_SETNAME, new JSONObject(jsonData));
    }
    
    #region SocketIO Events
    private void OnDisconnect(SocketIOEvent obj)
    {
        Debug.Log("Disconnected from server");
        
    }

    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("Connected to server");
        ChangeName(playerName);
    }

    private void OnRegistered(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        id = obj.data.GetField("PlayerId").str;
        List<string> playerids = obj.data.GetField("Players").keys;
        foreach (var id in playerids)
        {
            string Name = obj.data.GetField("Players").GetField(id).GetField("playerName").str;
            players[id] = new PlayerData() { PlayerId = id, playerName = Name };
        }


    }

    private void OnNewPlayer(SocketIOEvent obj)
    {
        string ID = obj.data.GetField("PlayerData").GetField("id").str;
        string Name = obj.data.GetField("PlayerData").GetField("playerName").str;
        players.Add(ID, new PlayerData() { PlayerId = ID, playerName = Name});
    }

    private void OnRemovePlayer(SocketIOEvent obj)
    {
        players.Remove(obj.data.GetField("PlayerId").str);
    }

    #endregion
}
