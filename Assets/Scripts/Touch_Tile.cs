using UnityEngine;
using System.Collections;

public class Touch_Tile : MonoBehaviour
{
    public GameObject tower;

    private Object createdTower = null;

    //Boys remember to get your goddamn function
    //names right or shit will not work
    void OnMouseDown ()
    {
        if (createdTower != null)
        {
            return;
        }

        Vector3 towerPosition = new Vector3 (
                               this.transform.position.x,
			                   tower.transform.position.y,
                               this.transform.position.z);

        createdTower = Instantiate(tower, towerPosition,
            tower.transform.rotation);
    }

}
