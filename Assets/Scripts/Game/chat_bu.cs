//using System;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Chat : MonoBehaviour
//{
//    public bool disabled;
//    private string chatname;
//    private SimChat sc;
//    private float rt = -3f;
//    private Vector2 sp = Vector2.zero;
//    private Rect chatRect = new Rect (
//                                (float) (Screen.width * 0.6),
//                                0.0f,
//                                (float) (Screen.width * 0.4),
//                                (float) (Screen.height * 0.2));
//    private Color c;
//    private List<string> sendingMessages = new List<string> ();
//    private int size;
//    private int defaultSize = 17;
//
//    void Start ()
//    {
//
//        if (disabled)
//        {
//            // No chat in single player
//            return;
//        }
//
//        chatname = Guid.NewGuid ().ToString ("N");
//        sc = new SimChat ("default",
//            gameObject.GetComponent<MonoBehaviour> (), chatname);
//
//        sc.continueCheckMessages ();
//        sc.setReceiveFunction (ReceiveMessage);
//        sc.allMessages.Clear ();
//
//
//        size = (int) (Screen.dpi / 6.0f);
//        if (size < 0.1)
//        {
//            // Screen.dpi returns 0 if dpi cannot be determined
//            size = defaultSize;
//        }
//
//        //sc.sendMessage ("");
//        //sc.message = "";
//    }
//
//    void ReceiveMessage (SimpleMessage [] sm)
//    {
//        rt = Time.time;
//    }
//
//    void OnGUI ()
//    {
//        if (disabled)
//        {
//            // No chat in single player
//            return;
//        }
//
//        //display new message
//
//        //        if (Time.time - rt < 3 &&
//        //            sc.allMessages[sc.allMessages.Count - 1].sender != chatname)
//        //        {
//        //            GUI.skin.label.fontSize = size;
//        //            GUILayout.Label ("Chat: "
//        //                + sc.allMessages[sc.allMessages.Count - 1].message);
//        //        }
//
//        // Show input box
//        GUI.skin.textField.fontSize = size;
//        GUI.skin.button.fontSize = size;
//
//        GUILayout.BeginArea (chatRect);
//        GUILayout.BeginVertical ("box");
//
//        GUILayout.BeginVertical ("box");
//        sp = GUILayout.BeginScrollView (sp);
//
//        //GUILayout.FlexibleSpace ();
//        c = GUI.contentColor;
//        //loop through each of the messages contained in allMessages
//        for (int sm = 0; sm < sc.allMessages.Count; sm++)
//        {
//            GUILayout.BeginHorizontal ();
//            //check if the sender had the same name as me, and change the color
//            if (sc.allMessages[sm].sender == chatname)
//            {
//                GUI.contentColor = Color.red;
//                //GUILayout.FlexibleSpace ();
//                GUILayout.Label (sc.allMessages[sm].message);
//            }
//            else
//            {
//                GUI.contentColor = Color.green;
//                GUILayout.Label (sc.allMessages[sm].message);
//                //GUILayout.FlexibleSpace ();
//            }
//
//            GUILayout.EndHorizontal ();
//        }
//        //display the pending messages
//        GUI.contentColor = Color.red;
//        for (int snm = 0; snm < sendingMessages.Count; snm++)
//        {
//            GUILayout.BeginHorizontal ();
//            GUILayout.FlexibleSpace ();
//            GUILayout.Label (sendingMessages[snm] as String);
//            GUILayout.EndHorizontal ();
//        }
//        GUI.contentColor = c;
//        GUILayout.EndScrollView ();
//        GUILayout.EndVertical ();
//
//        GUILayout.BeginHorizontal ();
//
//        //send a new message
//        sc.message = GUILayout.TextField (sc.message);
//        if (GUILayout.Button ("Send") ||
//            (Event.current.isKey && Event.current.keyCode == KeyCode.Return))
//        {
//            sc.sendMessage ();
//            sendingMessages.Add (sc.message);
//            sc.message = "";
//        }
//        GUILayout.EndHorizontal ();
//
//        GUILayout.EndVertical ();
//        GUILayout.EndArea ();
//    }
//
//}
