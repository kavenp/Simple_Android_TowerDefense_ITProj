using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class playReplay : MonoBehaviour {

    // Filename for the temporary file to hold the replay
	const string fileName = "tempReplay.dat";
	//const string fileName = "Example Replays\MPReplay.dat";
	
	public GameObject tower; // For spawning towers
	public GameObject player; // For spawning players
	
	float startTime; // Start time of playback (used for synchronization)

	GameObject[] currentPlayers;
	
	GameObject[] players;
	byte[] replay; // The replay file
	int replayIndex; // The current point in the replay file
	
	
	// Use this for initialization
	void Start () {
	
	    players = new GameObject[10];
	    replay = System.IO.File.ReadAllBytes(fileName); // Read in the replay file
	    replayIndex = 0;
		
		// Skip the header
	    while(replay[replayIndex]!=2){
		    replayIndex+=1;
	    }
	    replayIndex+=1;
		
		// Initialize the start time
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
	    // If end of replay reached
	    while(replayIndex < replay.Length){
	
	        // Begin update OpCode (used as error checking)
	        if(replay[replayIndex]==11){
	            replayIndex+=1; // Move to next OpCode
			
			    float timeStamp = (float)BitConverter.ToDouble(replay,replayIndex);
			
			
			    // If updates have become de-synched, skip this update
			    if((Time.time-startTime) < timeStamp){
				    replayIndex-=1;
				    Debug.Log("Backstep!");
				    return;
			    }else{
			        replayIndex+=8;
			    }
		    }
		    currentPlayers = GameObject.FindGameObjectsWithTag("Player");
		
		    // While receiving update player OpCodes
		    while(replay[replayIndex]==3){
			    replayIndex+=1; // Move to first parameter
			
			    replayIndex+=4; // Skip the ID number, possibly implemented later
			
			    //Read the xPos, zPos and yRotation of player
			    float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
			    replayIndex+=8;
			
			    float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
			
			    float yRot = (float)BitConverter.ToDouble(replay,replayIndex);
			    replayIndex+=8;
			
			    Vector3 playerPosition = new Vector3(xPos,10,zPos);
			    Quaternion playerRotation = Quaternion.Euler(0,yRot,0);
			
			    // If no more players to move around (this is a new player),
			    // spawn them, otherwise move a different player to this spot
			    if(currentPlayers.Length == 0){
				    GameObject play = (GameObject)Instantiate(player,playerPosition,playerRotation);
				    play.GetComponent<MP_PlayerController>().enabled = false;
			    }else{
			        GameObject movePlayer = currentPlayers[currentPlayers.Length-1];
				    movePlayer.transform.position = playerPosition;
				    movePlayer.transform.rotation = playerRotation;
				
				    // Remove any moved players so they don't get moved again
				    currentPlayers = removeObject(currentPlayers.Length-1,currentPlayers);
			    }
			
		    }
		
		    // Skip despawn towers for now, will be implemented later
		    while(replay[replayIndex]==5){
		        replayIndex+=17;
		    }
		
		    GameObject[] towers;
		    towers = GameObject.FindGameObjectsWithTag("Tower");
		
		    // While receiving Spawn Tower OpCodes
		    while(replay[replayIndex]==7){
		        replayIndex+=1; // Move to first parameter
			
			    // Read in xPos and zPos of tower
		        float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		    
		    	// Spawn the tower
		        Vector3 towerPosition = new Vector3(xPos,10,zPos);
		        Instantiate(tower,towerPosition,tower.transform.rotation);
		    }
		
		    // Skip to the end of Update, will implement other things later
		    while(replay[replayIndex]!=12){
		        replayIndex+=1;
		    }
		
		    replayIndex+=1;
	        }
		}
	
	// Removes an object from an array
	GameObject[] removeObject(int loc,GameObject[] list){
	
	    // Move the object to the end of the array
		for(int i = loc; i < list.Length-1; i++){
		    list[loc] = list[loc+1];
		}
		
		// Delete the end of the array (containing the object to delete)
		if(loc < list.Length){
			Array.Resize(ref list,list.Length - 1);
		}
		
		return list;	
	}
}
