using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class TowerPos
{
    public GameObject tower;
    public float xPos;
	public float zPos;
	
	public TowerPos(GameObject t, float x, float z){
	    this.tower = t;
		this.xPos = x;
		this.zPos = z;
	}
	
}

public class recordReplay : MonoBehaviour {
	
	// Filename for the temporary file to hold the replay
	const string fileName = "tempReplay.dat";
	
	// List of the towers currently spawned
	List<TowerPos> currentTowers = new List<TowerPos>();
	
	// The starting time of the recording (used for synchronization)
	float startTime;
	
	// Use this for initialization
	void Start () {
	
	    startTime = Time.time;
	
	    // Writing the header for the replay file, should be the size of
		// the float type, and size of the int type on the current system
		// Currently not used, but may be required later
	    var stream = new FileStream(fileName, FileMode.Create);
	    
	    stream.Write(new byte[]{1}, 0, 1); // Begin header OpCode
	    
	    stream.Write(new byte[]{sizeof(float)}, 0, 1);
	    
	    stream.Write(new byte[]{sizeof(int)}, 0, 1);
	    
	    stream.Write(new byte[]{2},0,1); // End header OpCode
	    
	    stream.Close();
	    
	}
	
	// Update is called once per frame
	void Update () {
	
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		// Recording doesn't start until game has started
		if(players.Length < 1)
		{
			startTime = Time.time;
		    return;
		}
		
		var stream = new FileStream(fileName, FileMode.Append);
		
		stream.Write(new byte[]{11},0,1); //Update Start OpCode
		
		byte [] byteStream;
		
		// Record the timestamp for the current update
		byteStream = BitConverter.GetBytes((double)(Time.time-startTime));
		stream.Write(byteStream,0,byteStream.Length);
		
		// Records which player is recorded
		int index=0;
		
		// For each player
		foreach(GameObject i in players){
		    index+=1;
		    stream.Write(new byte[]{3},0,1); // Player update OpCode
		    
			// Record ID of the player
		    byteStream = BitConverter.GetBytes(index);
		    stream.Write(byteStream,0,byteStream.Length);
		    
			// Record the players position
		    recordPosition(byteStream,stream,i);
		    
			// Record the players rotation
		    byteStream = BitConverter.GetBytes((double)i.transform.eulerAngles.y);
		    stream.Write(byteStream,0,byteStream.Length);
		}
		
		GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
		
		List<TowerPos> removeTowers = new List<TowerPos>();
		
		// For each tower that has been recorded as existing
		foreach(TowerPos i in currentTowers){
		    
			// If the tower doesn't actually exist, record a despawn event
		    int isIn = Array.IndexOf(towers,i.tower);
		    if(isIn <= -1){
		        stream.Write(new byte[]{5},0,1); // Tower despawn OpCode
				
			    recordPosition(byteStream,stream,i.xPos,i.zPos);
			
			    removeTowers.Add(i);
			
		    }
		}
		
		foreach(TowerPos i in removeTowers){
		    currentTowers.Remove(i);
		}
		
		// For each tower that actually exists
		foreach(GameObject i in towers){
		    
			bool isInList = false;
			// If the tower has not been recorded as existing, record the spawn event
			foreach(TowerPos k in currentTowers){
			    if(k.tower==i){
				    isInList = true;
				}
			}
		    if(!isInList){
			
		        stream.Write(new byte[]{7},0,1); // Tower spawn OpCode
			
			    recordPosition(byteStream,stream,i);
			
			    currentTowers.Add(new TowerPos(i,i.transform.position.x,i.transform.position.z));
		    }
		}
		
		int gold = players[0].GetComponent<MP_PlayerController>().GetGold();
		
		stream.Write(new byte[]{9},0,1); // Set gold value OpCode
		
		byteStream = BitConverter.GetBytes(gold);
	    stream.Write(byteStream,0,byteStream.Length);
		
		GameObject playerBase = GameObject.Find("PlayerBase");
		int lives = playerBase.GetComponent<LivesScript>().numLives;
		
		stream.Write(new byte[]{10},0,1); // Set lives value OpCode
		
		byteStream = BitConverter.GetBytes(lives);
	        stream.Write(byteStream,0,byteStream.Length);
		
		stream.Write(new byte[]{12},0,1); // End Update OpCode
		
		stream.Close();
		
	
	}
	
	// Records an (x,z) position of an object to the bytefile
	private void recordPosition(byte [] byteStream, FileStream stream, GameObject i){
		byteStream = BitConverter.GetBytes((double)i.transform.position.x);
		stream.Write(byteStream,0,byteStream.Length);
		    
		byteStream = BitConverter.GetBytes((double)i.transform.position.z);
		stream.Write(byteStream,0,byteStream.Length);
	}
	
	private void recordPosition(byte [] byteStream, FileStream stream, float x, float z){
		byteStream = BitConverter.GetBytes((double)x);
		stream.Write(byteStream,0,byteStream.Length);
		    
		byteStream = BitConverter.GetBytes((double)z);
		stream.Write(byteStream,0,byteStream.Length);
	}
	
}
