using UnityEngine;
using UnityEngine.UI;

// Shows visually the range of a tower and the tower's level
// when the tower is being pressed.
public class TouchTower : MonoBehaviour
{
    // The object providing a visual representation
    // of the range of a tower.
    private GameObject rangeCanvas;

    // Where the tower's level is displayed.
    private Text towerLevelDisplay;

    // For accessing the tower's level.
    private ShootEnemies shootEnemies;

    void Start ()
    {
		rangeCanvas =
            transform.parent.transform.Find("Range Canvas").gameObject;

        rangeCanvas.SetActive (false);

        shootEnemies = transform.parent.GetComponent<ShootEnemies>();

        GameObject towerLevelDisplayObject =
            GameObject.FindGameObjectWithTag("TowerLevelDisplay");

        towerLevelDisplay = towerLevelDisplayObject.GetComponent<Text>();
    }

    void OnMouseDown ()
    {
        Debug.Log ("Tower touched");
        rangeCanvas.SetActive (true);

        towerLevelDisplay.text = "Level: " + (shootEnemies.level + 1);
    }

    void OnMouseUp ()
    {
        rangeCanvas.SetActive (false);
        towerLevelDisplay.text = "";
    }

}
