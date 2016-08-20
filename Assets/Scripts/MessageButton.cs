using UnityEngine;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour
{

    private TouchScreenKeyboard keyboard;
    public Text msgDisplay;

    void OnMouseDown()
    {
        Debug.Log("Use emulator to test chat.");

        keyboard = TouchScreenKeyboard.Open("",
            TouchScreenKeyboardType.Default);
    }

    void Update()
    {
        if (keyboard != null && keyboard.done)
        {
            msgDisplay.text = keyboard.text;
        }
    }

}
