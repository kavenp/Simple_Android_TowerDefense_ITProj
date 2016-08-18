using UnityEngine;
using System.Collections;

public class TouchTower : MonoBehaviour
{

    private GameObject rangeCanvas;

    void Start()
    {
        rangeCanvas = gameObject.transform.Find("Range Canvas").gameObject;
        rangeCanvas.SetActive(false);
    }

    void OnMouseDown()
    {
        rangeCanvas.SetActive(true);
    }

    void OnMouseUp()
    {
        rangeCanvas.SetActive(false);
    }

}
