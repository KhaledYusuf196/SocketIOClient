using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] InputField playerName;
    // Start is called before the first frame update
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
}
