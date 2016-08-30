using UnityEngine;
using System;
using System.Collections;

// Tests that projectiles moving towards a target
// that has just been destroyed is destroyed in the
// next update.
public class TestProjectileSelfDestruction : MonoBehaviour {

    // Target of projectile.
    public GameObject enemy;

    // True if the target has been destroyed.
    private bool shouldTerminate = false;

	void Update () {
	    if (shouldTerminate)
        {
            // Projectile is still alive
            String exMsg = "Projectile not immediately " +
                "destroyed upon target destruction";

            throw new Exception(exMsg);
        }

        if (enemy == null)
        {
            // The projectile should terminate before its next update
            shouldTerminate = true;
        }

    }
}
