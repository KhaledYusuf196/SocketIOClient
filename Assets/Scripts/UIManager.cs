using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputField playerName;
    [SerializeField] Text stateText;
    [SerializeField] Button rock;
    [SerializeField] Button paper;
    [SerializeField] Button scissors;

    static UIManager uiManager;

    public static UIManager Instance => uiManager;

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Connect()
    {
        if (!string.IsNullOrEmpty(playerName.text))
        {
            ClientManager.Instance.Connect(playerName.text);
        }
    }

    public void Disconnect()
    {
        ClientManager.Instance.Disconnect();
    }

    public void ChangeName()
    {
        ClientManager.Instance.ChangeName(playerName.text);
    }

    public void setRock()
    {
        stateText.text = "Rock";
        ClientManager.Instance.SetMove(1);
    }
    public void setPaper()
    {
        stateText.text = "Paper";
        ClientManager.Instance.SetMove(2);
    }
    public void setScissors()
    {
        stateText.text = "Scissors";
        ClientManager.Instance.SetMove(3);
    }

    public void DisableControls()
    {
        rock.enabled = false;
        scissors.enabled = false;
        paper.enabled = false;
    }
    
    public void EnableControls()
    {
        rock.enabled = true;
        scissors.enabled = true;
        paper.enabled = true;
    }

    internal void showLose(string myMove, string otherMove)
    {
        switch (myMove)
        {
            case "1":
                myMove = "Rock";
                break;
            case "2":
                myMove = "Paper";
                break;
            case "3":
                myMove = "Scissors";
                break;
        }
        switch (otherMove)
        {
            case "1":
                otherMove = "Rock";
                break;
            case "2":
                otherMove = "Paper";
                break;
            case "3":
                otherMove = "Scissors";
                break;
        }
        stateText.text = $"Loser\nYou: {myMove}\nOther: {otherMove}";
    }

    internal void showWin(string myMove, string otherMove)
    {
        switch (myMove)
        {
            case "1":
                myMove = "Rock";
                break;
            case "2":
                myMove = "Paper";
                break;
            case "3":
                myMove = "Scissors";
                break;
        }
        switch (otherMove)
        {
            case "1":
                otherMove = "Rock";
                break;
            case "2":
                otherMove = "Paper";
                break;
            case "3":
                otherMove = "Scissors";
                break;
        }
        stateText.text = $"Winner\nYou: {myMove}\nOther: {otherMove}";
    }

    internal void showDraw(string myMove, string otherMove)
    {
        switch (myMove)
        {
            case "1":
                myMove = "Rock";
                break;
            case "2":
                myMove = "Paper";
                break;
            case "3":
                myMove = "Scissors";
                break;
        }
        switch (otherMove)
        {
            case "1":
                otherMove = "Rock";
                break;
            case "2":
                otherMove = "Paper";
                break;
            case "3":
                otherMove = "Scissors";
                break;
        }
        stateText.text = $"Draw\nYou: {myMove}\nOther: {otherMove}";
    }
}
