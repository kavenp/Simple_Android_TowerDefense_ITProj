using UnityEngine;
using System.Collections;

public class Touch_Tile : MonoBehaviour
{
    public GameObject tower;
    public GameObject rangeImage;

    //    void Start ()
    //    {
    //    }
    //
    //    void Update ()
    //    {
    //        onMouseDown ();
    //    }

    //Boys remember to get your goddamn function names right or shit will not work
    void OnMouseDown ()
    {
		Vector3 position = new Vector3 (this.transform.position.x,
			                   tower.transform.position.y, this.transform.position.z);
		Instantiate (tower, position, tower.transform.rotation);
        Instantiate(rangeImage, position, rangeImage.transform.rotation);
    }
        
}
