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
    public const string EVENT_SETMOVE = "setMove";
    public const string EVENT_REGISTERED = "registered";
    public const string EVENT_NEWPLAYER = "newPlayer";
    public const string EVENT_REMOVEPLAYER = "removePlayer";
    public const string EVENT_DISABLECONTROLS = "disableControls";
    public const string EVENT_DRAW = "Draw";
    public const string EVENT_WIN = "Win";
    public const string EVENT_LOSE = "Lose";
    static ClientManager instance;
    private string playerName;
    private string id;
    Dictionary<string, PlayerData> players;
    public static ClientManager Instance => instance;

    // Start is called before the first frame update
    void Start()
    {
        players = new Dictionary<string, PlayerData>();
        sIO = GetComponent<SocketIOComponent>();
        sIO.On(EVENT_CONNECT, OnConnected);
        sIO.On(EVENT_DISCONNECT, OnDisconnect);
        sIO.On(EVENT_REGISTERED, OnRegistered);
        sIO.On(EVENT_NEWPLAYER, OnNewPlayer);
        sIO.On(EVENT_REMOVEPLAYER, OnRemovePlayer);
        sIO.On(EVENT_DISABLECONTROLS, OnDisableControls);
        sIO.On(EVENT_DRAW, OnDraw);
        sIO.On(EVENT_WIN, OnWin);
        sIO.On(EVENT_LOSE, OnLose);
    }

    

    private void OnDisableControls(SocketIOEvent obj)
    {
        UIManager.Instance.DisableControls();
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

    public void SetMove(int move)
    {
        Dictionary<string, string> jsonData = new Dictionary<string, string>() { { "PlayerId", id}, { "Move", move.ToString()} };
        sIO.Emit(EVENT_SETMOVE, new JSONObject(jsonData));
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
        id = obj.data.GetField("PlayerId").str;
        List<string> playerids = obj.data.GetField("Players").keys;
        foreach (var id in playerids)
        {
            string Name = obj.data.GetField("Players").GetField(id).GetField("playerName").str;
            players[id] = new PlayerData() { PlayerId = id, playerName = Name };
        }

        Debug.Log("Registered " + players.Count);

    }

    private void OnNewPlayer(SocketIOEvent obj)
    {
        string ID = obj.data.GetField("PlayerData").GetField("id").str;
        string Name = obj.data.GetField("PlayerData").GetField("playerName").str;
        players.Add(ID, new PlayerData() { PlayerId = ID, playerName = Name});
        Debug.Log("New Player " + players.Count);
    }

    private void OnRemovePlayer(SocketIOEvent obj)
    {
        if(players.ContainsKey(obj.data.GetField("PlayerId").str))
            players.Remove(obj.data.GetField("PlayerId").str);
        Debug.Log("Remove Player " + players.Count);
    }

    private void OnLose(SocketIOEvent obj)
    {
        string myMove = obj.data.GetField("myMove").str;
        string otherMove = obj.data.GetField("otherMove").str;
        UIManager.Instance.showLose(myMove, otherMove);
        UIManager.Instance.EnableControls();
    }

    private void OnWin(SocketIOEvent obj)
    {
        string myMove = obj.data.GetField("myMove").str;
        string otherMove = obj.data.GetField("otherMove").str;
        UIManager.Instance.showWin(myMove, otherMove);
        UIManager.Instance.EnableControls();
    }

    private void OnDraw(SocketIOEvent obj)
    {
        string myMove = obj.data.GetField("myMove").str;
        string otherMove = obj.data.GetField("otherMove").str;
        UIManager.Instance.showDraw(myMove, otherMove);
        UIManager.Instance.EnableControls();
    }

    #endregion
}
