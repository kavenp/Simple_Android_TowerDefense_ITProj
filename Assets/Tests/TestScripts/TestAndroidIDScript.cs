using UnityEngine;
using UnityEngine.UI;

public class TestAndroidIDScript : MonoBehaviour
{
    public Text display;

    void Start()
    {
        display.text = SystemInfo.deviceUniqueIdentifier;
    }
}
