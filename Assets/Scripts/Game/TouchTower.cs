using UnityEngine;

// Shows visually the range of a tower when
// the tower is being pressed.
public class TouchTower : MonoBehaviour
{
    // The object providing a visual representation
    // of the range of a tower.
    private GameObject rangeCanvas;

    void Start ()
    {
		rangeCanvas = gameObject.transform.parent.transform.Find("Range Canvas").gameObject; 
		//rangeCanvas = gameObject.transform.Find ("Range Canvas").gameObject;
        rangeCanvas.SetActive (false);
    }

    void OnMouseDown ()
    {
        Debug.Log ("Tower touched");
        rangeCanvas.SetActive (true);
    }

    void OnMouseUp ()
    {
        rangeCanvas.SetActive (false);
    }

}
