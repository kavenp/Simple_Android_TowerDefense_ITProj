// Author: Matthew Eldridge (695350)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

// Holds a record of the tower pos (needed for despawn event as despawning
// an object gets rid of any reference to its previous location
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
	
	List<int> fullRecordLocations = new List<int>();
	int index;
	// Filename for the temporary file to hold the replay
	// List of the towers currently spawned
	List<TowerPos> currentTowers = new List<TowerPos>();
	private string fileName;
	// The starting time of the recording (used for synchronization)
	float startTime;
	
	float timeInterval;
	
	float massSaveInterval;
	
	Dictionary<GameObject, int> towerDam;
	
	// Use this for initialization
	void Start () {
	
	    #if UNITY_ANDROID
	    fileName = Application.persistentDataPath + "/tempReplay.dat";
	    #else
	    fileName = "tempReplay.dat";
	    #endif
	    
		index = 0;
	
	    startTime = Time.time;
		
		timeInterval = startTime;
		
		massSaveInterval = startTime;
	
	    // Writing the header for the replay file, should be the size of
		// the float type, and size of the int type on the current system
		// Currently not used, but may be required later
	    var stream = new FileStream(fileName, FileMode.Create);
	    
		writeByte(stream,1);
		writeByte(stream,sizeof(float));
		writeByte(stream,sizeof(int));
		writeByte(stream,2);
	    stream.Close();
		
		towerDam = new Dictionary<GameObject,int>();
		
		
		DontDestroyOnLoad(transform.gameObject);
	    
	}
	
	// Update is called once per frame
	void Update () {
	
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		var stream = new FileStream(fileName, FileMode.Append);
		byte [] byteStream;
		// Recording doesn't start until game has started
		if(players.Length < 1)
		{
		    // If the game has ended, record that it has ended and close recording
		    if(Application.loadedLevelName == "GameOver"){
				foreach(int i in fullRecordLocations){
				    stream.Write(new byte[]{23},0,1);
				    byteStream = BitConverter.GetBytes(i);
		            stream.Write(byteStream,0,byteStream.Length);
				}
				Debug.Log(fullRecordLocations.Count);
			    stream.Write(new byte[]{14},0,1); //End Recording Operation Code
				stream.Close();
				Destroy(gameObject);
			}
			startTime = Time.time;
			timeInterval = startTime;
			stream.Close();
		    return;
		}
		
		writeByte(stream,11);
		
		// Record the timestamp for the current update
		
		writeFloat(stream,Time.time-startTime);
		
		// Record each player
		foreach(GameObject i in players){
		    recordPlayer(stream,i);
		}
		
		// Record tower despawn events
		GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
		towers = towers.Concat(GameObject.FindGameObjectsWithTag("Tower2")).ToArray();
		towers = towers.Concat(GameObject.FindGameObjectsWithTag("Tower3")).ToArray();
		despawnTowers(stream, 3, towers);
		
		// Record tower spawn events
		towers = GameObject.FindGameObjectsWithTag("Tower");
		spawnTowers(stream, 1, towers);
		towers = GameObject.FindGameObjectsWithTag("Tower2");
		spawnTowers(stream, 2, towers);
		towers = GameObject.FindGameObjectsWithTag("Tower3");
		spawnTowers(stream, 3, towers);
		
		// Record tower update events
		towers = GameObject.FindGameObjectsWithTag("Tower");
		towers = towers.Concat(GameObject.FindGameObjectsWithTag("Tower2")).ToArray();
		towers = towers.Concat(GameObject.FindGameObjectsWithTag("Tower3")).ToArray();
		updateLevel(stream, towers);
		
		// Record players gold
		int gold = players[0].GetComponent<MP_PlayerController>().GetGold();
		recordGold(stream,gold);
		
		// Record players lives
		GameObject playerBase = GameObject.Find("PlayerBase");
		int lives = playerBase.GetComponent<LivesScript>().numLives;
		recordLives(stream,lives);
		
		
		// If it's been more than a second since last updating the enemies, update again
		if((Time.time - timeInterval) > 0){
		    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			// Record each enemy
			foreach(GameObject i in enemies){
				writeByte(stream,13);
			    recordEnemy(stream,i);
				
			}
			// Update the time interval
		    timeInterval = Time.time;
		}
		
		// End update record
		writeByte(stream,12);
		
		// Record the full state of the game if a time interval has been reached
		if((Time.time - massSaveInterval) > 15){
		     recordFullGameState(stream);
			 massSaveInterval = Time.time;
		}
		
		stream.Close();
	}
	
	// Records an (x,z) position of an object to the bytefile
	private void recordPosition(FileStream stream, GameObject i){
		writeFloat( stream,i.transform.position.x);
		writeFloat( stream,i.transform.position.z);
	}
	
	// Record the position of an object using it's x and z co-ordinates
	private void recordPosition(FileStream stream, float x, float z){
		writeFloat( stream,x);
		writeFloat( stream,z);
	}
	
	// Record a tower spawn event
	private void spawnTowers(FileStream stream, int type, GameObject[] towers){
	    foreach(GameObject i in towers){
		    
			bool isInList = false;
			
			// If the tower has not been recorded as existing, record the spawn event
			foreach(TowerPos k in currentTowers){
			    if(k.tower==i){
				    isInList = true;
				}
			}
		    if(!isInList){
			
		        writeByte(stream,7);
			    recordPosition(stream,i);
				writeInt( stream, type);
				
				ShootEnemies t = (ShootEnemies)i.GetComponent("ShootEnemies");
		        int currDamage = t.getAdditionalDamage();
				
				towerDam.Add(i,currDamage);
			    currentTowers.Add(new TowerPos(i,i.transform.position.x,i.transform.position.z));
		    }
		}
	}
	
	// Record a tower despawn event
	private void despawnTowers(FileStream stream, int type, GameObject[] towers){
	    List<TowerPos> removeTowers = new List<TowerPos>();
		
		// For each tower that has been recorded as existing
		foreach(TowerPos i in currentTowers){
			// If the tower doesn't actually exist, record a despawn event
		    int isIn = Array.IndexOf(towers,i.tower);
		    if(isIn <= -1){
		        writeByte(stream,5);
			    recordPosition(stream,i.xPos,i.zPos);
			    removeTowers.Add(i);
		    }
		}
		// Remove the despawned towers
		foreach(TowerPos i in removeTowers){
		    towerDam.Remove(i.tower);
		    currentTowers.Remove(i);
		}
	}
	
	// Record the damage of the towers
	private void updateLevel(FileStream stream, GameObject[] towers){
	    foreach(GameObject i in towers){
		    ShootEnemies t = (ShootEnemies)i.GetComponent("ShootEnemies");
		    int currDamage = t.getAdditionalDamage();
			// If the damage amount per shot has changed, record it
			if(currDamage != towerDam[i]){
				writeByte(stream,16);
				recordPosition(stream,i.transform.position.x,i.transform.position.z);
				writeInt( stream, currDamage);
				towerDam.Remove(i);
				towerDam.Add(i,currDamage);
				
			}
		}
	}
	
	// Record the full state of the game
	private void recordFullGameState(FileStream stream){
	
	    // Add a new state record entry
	    fullRecordLocations.Add(index);
	    writeByte(stream,20);
		
		// Timestamp of the record
		writeFloat( stream,Time.time-startTime);
		
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		// Record each player
		foreach(GameObject i in players){
		    recordPlayer(stream,i);
		}
		
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		// Record each enemy
		foreach(GameObject i in enemies){
			writeByte(stream,29);
			recordEnemy(stream,i);
		}
		
		currentTowers = new List<TowerPos>();
		
		// Record each tower
		GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
		fullRecordTowers(stream,1,towers);
		
		towers = GameObject.FindGameObjectsWithTag("Tower2");
		fullRecordTowers(stream,2,towers);
		
		towers = GameObject.FindGameObjectsWithTag("Tower3");
		fullRecordTowers(stream,3,towers);
		
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");
			
		// Record each bullet
		foreach(GameObject i in bullets){
			
			BulletBehaviour bullet = (BulletBehaviour)i.GetComponent("BulletBehaviour");
			GameObject target = bullet.target;
			
			if(target == null){
			    continue;
			}
			
			writeByte(stream,21);
			// Record the bullets position
			recordPosition(stream,i);
			// Record the bullets height
			writeFloat( stream,i.transform.position.y);
			
			float speed = bullet.speed;
			int damage = bullet.getDamage();
			
			// Record the targets position
			recordPosition(stream,target);
			
			// Record the damage and speed
			writeInt( stream, damage);
			writeFloat( stream,speed);
		}
		
		// Record the lives
		GameObject playerBase = GameObject.Find("PlayerBase");
		int lives = playerBase.GetComponent<LivesScript>().numLives;
		recordLives(stream,lives);
		
		// End full state recording
		writeByte(stream,22);
	}
	
	// Fully record the towers
	public void fullRecordTowers(FileStream stream, int type,GameObject [] towers){
	    int currDamage;
		float lastShotTime;
		// For each tower
		foreach(GameObject i in towers){
		    writeByte(stream,27);
			// Record it's position
			recordPosition(stream,i);
			// Record it's type
		    writeInt( stream, type);
			
			ShootEnemies t = (ShootEnemies)i.GetComponent("ShootEnemies");
		    currDamage = t.getAdditionalDamage();
			lastShotTime = t.getLastShotTime();
			// Record it's damage value
			writeInt( stream, currDamage);
			// Record it's cooldown
			writeFloat( stream,lastShotTime);
			// Record all the enemies within range of the tower
			foreach(GameObject j in t.inRange){
			    writeByte(stream,25);
				recordPosition(stream,j);
			}
			
			currentTowers.Add(new TowerPos(i,i.transform.position.x,i.transform.position.z));
			
		}
	}
	
	// Records a player
	private void recordPlayer(FileStream stream,GameObject player){
	    int ind;
		writeByte(stream,3);
		
		MP_PlayerController local = (MP_PlayerController)player.GetComponent("MP_PlayerController");
		
		if(local.isLocal()){
			ind = 1;
		}else{
			ind = 0;
		}
		    
		// Record ID of the player
		writeInt( stream, ind);
		
		// Record the players position
		recordPosition(stream,player);
		
		// Record the players rotation
		writeFloat( stream,player.transform.eulerAngles.y);
	}
	
	// Records lives
	private void recordLives(FileStream stream,int lives){
		writeByte(stream,10);
		writeInt( stream, lives);
	}
	// Records gold
	private void recordGold(FileStream stream, int gold){
	    writeByte(stream,9);
		writeInt( stream, gold);
	}
	
	// Records an enemy
	private void recordEnemy(FileStream stream,GameObject enemy){
		int health;
		int maxHealth;
		int nodeNumber;
		// Update it's position
		recordPosition(stream,enemy);
		
		// Health
		health = enemy.GetComponent<EnemyHealth>().health;
		writeInt( stream, health);
		
		// Max Health
		maxHealth = enemy.GetComponent<EnemyHealth>().maxHealth;
		writeInt( stream, maxHealth);
		
		// And node it's following
		nodeNumber = enemy.GetComponent<EnemyPathing>().currNode.GetComponent<NodeScript>().nodeNumber;
		writeInt( stream, nodeNumber);
	}
	
	private void writeByte(FileStream stream, byte val){
	    stream.Write(new byte[]{val},0,1);
		index+=1;
	}
	
	private void writeInt(FileStream stream, int val){
	    byte[] byteStream = BitConverter.GetBytes(val);
	    stream.Write(byteStream,0,byteStream.Length);
		index+=4;
	}
	
	private void writeFloat(FileStream stream, float val){
		byte[] byteStream = BitConverter.GetBytes((double)val);
	    stream.Write(byteStream,0,byteStream.Length);
		index+=8;
	}
	
}
