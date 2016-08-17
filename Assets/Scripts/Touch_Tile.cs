using UnityEngine;
using System.Collections;

public class Touch_Tile : MonoBehaviour
{
    public GameObject tower;


    //    void Start ()
    //    {
    //    }
    //
    //    void Update ()
    //    {
    //        onMouseDown ();
    //    }

    void OnMouseDown ()
    {
		Vector3 position = new Vector3 (this.transform.position.x,
			                   tower.transform.position.y, this.transform.position.z);
		Instantiate (tower, position, tower.transform.rotation); 
    }
        
}
