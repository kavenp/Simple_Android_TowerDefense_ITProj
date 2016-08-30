using UnityEngine;
using System.Collections;

// Allow scripts that rely on a game controller
// to continue to work when the game controller
// may be different for a scene, as in single
// player and multiplayer mode.
// Also allows for mock game controllers for testing. 
public interface IGameController
{
    IEnumerator SpawnWaves();
    bool isGamePaused();
    void setPauseFlag(bool flag);
}
