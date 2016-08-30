using UnityEngine;
using System.Collections;
using System;

namespace Mocks
{
    // A mock that allows checks for whether the game is paused.
    // Does not spawn waves of enemies.
    // For testing purposes only.
    public class PauseOnlyGameController : MonoBehaviour, IGameController
    {
        // Whether the game is currently in a paused state.
        private bool isPaused;

        // Initialises the game in an unpaused state.
        void Start()
        {
            isPaused = false;
        }

        // Checks whether the game is paused.
        public bool isGamePaused()
        {
            return this.isPaused == true;
        }

        // Sets the flag indicating whether or
        // not the game is paused.
        public void setPauseFlag(bool flag)
        {
            this.isPaused = flag;
        }

        // Not implemented.
        public IEnumerator SpawnWaves()
        {
            throw new NotImplementedException();
        }
    }

}
