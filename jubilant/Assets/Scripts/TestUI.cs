using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUI : MonoBehaviour
{
    public TMP_InputField input;

    // Start is called before the first frame update
    void Start()
    {
        Client.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData()
    {
        GameManager.ip = input.text;
        Package welcome = new Welcome();
        welcome.Send();
    }
}
