using UnityEngine;
using System.Collections;

public class EnemyPathing : MonoBehaviour {

	public GameObject currNode;
	public float enemySpeed;
	private Vector3 moveLoc;

	// Use this for initialization
	void Start () {
		currNode = GameObject.Find("Node1");
	
	}

	// Update is called once per frame
	void Update () {
		moveLoc = currNode.transform.position;
		transform.position = Vector3.MoveTowards(transform.position, moveLoc, Time.deltaTime * enemySpeed);
		float dist;
		dist = Vector3.Distance (transform.position, moveLoc);
		if (dist < 1) {
			currNode = currNode.GetComponent<NodeScript> ().nextNode;
		}
		
	}
}
