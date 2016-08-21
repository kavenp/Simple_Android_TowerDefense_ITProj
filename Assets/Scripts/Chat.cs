using System;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    public bool disabled;

    private string chatname;
    private SimChat sc;
    private float rt = -3f;
    private Rect chatRect = new Rect(
        (float)(Screen.width * 0.6),
        0.0f,
        (float)(Screen.width * 0.4),
        (float)(Screen.height * 0.2));
    private Color c;
    private List<string> sendingMessages = new List<string>();
    private int size;
    private int defaultSize = 17;

    void Start ()
    {
        if (disabled)
        {
            // No chat in single player
            return;
        }

        chatname = Guid.NewGuid().ToString("N");
        sc = new SimChat("default",
            gameObject.GetComponent<MonoBehaviour>(), chatname);
        sc.continueCheckMessages();
        sc.setReceiveFunction(ReceiveMessage);

        size = (int)(Screen.dpi / 6.0f);
        if (size < 0.1)
        {
            // Screen.dpi returns 0 if dpi cannot be determined
            size = defaultSize;
        }
    }

    void ReceiveMessage(SimpleMessage[] sm)
    {
        rt = Time.time;
    }

    void OnGUI()
    {
        if (disabled)
        {
            // No chat in single player
            return;
        }

        //display new message
        if (Time.time - rt < 3 &&
            sc.allMessages[sc.allMessages.Count - 1].sender != chatname)
        {
            GUI.skin.label.fontSize = size;
            GUILayout.Label("Chat: "
                + sc.allMessages[sc.allMessages.Count - 1].message);
        }

        // Show input box
        GUI.skin.textField.fontSize = size;
        GUI.skin.button.fontSize = size;

        GUILayout.BeginArea(chatRect);
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        //send a new message
        sc.message = GUILayout.TextField(sc.message);
        if (GUILayout.Button("Send") ||
            (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
        {
            sc.sendMessage();
            sendingMessages.Add(sc.message);
            sc.message = "";
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

}
