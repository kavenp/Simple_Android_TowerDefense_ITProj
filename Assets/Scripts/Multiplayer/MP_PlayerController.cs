using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MP_PlayerController : NetworkBehaviour
{
	public float turningSpeed;
	public float movingSpeed;

	void Update ()
	{
		if (!isLocalPlayer)
		{
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * turningSpeed;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * movingSpeed;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);

	}

}
