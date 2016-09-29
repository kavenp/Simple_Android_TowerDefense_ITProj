using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playReplay : MonoBehaviour {

    // Filename for the temporary file to hold the replay
	const string fileName = "tempReplay.dat";
	//const string fileName = "Example Replays\MPReplay.dat";
	
	public GameObject tower; // For spawning towers
	public GameObject tower2;
	public GameObject tower3;
	
	public Dictionary<int,GameObject> towerType;
	
	public GameObject player; // For spawning players
	public GameObject enemy;
	
	public Text goldDisplay;
	
	float startTime; // Start time of playback (used for synchronization)

	
	Dictionary<int,GameObject> nodes;
	
	GameObject[] currentPlayers;
	GameObject[] currentTowers;
	GameObject[] enemies;

	byte[] replay; // The replay file
	int replayIndex; // The current point in the replay file
	
	int count;
	
	public int count2;
	
	
	// Use this for initialization
	void Start () {
	
	    count2 = 0;
	
	    replay = System.IO.File.ReadAllBytes(fileName); // Read in the replay file
	    replayIndex = 0;
		
		if(replay[replay.Length-1]!=14){
		    Debug.Log("Replay File not fully recorded!");
			Destroy(gameObject);
		}
		
		// Skip the header
	    while(replay[replayIndex]!=2){
		    replayIndex+=1;
	    }
	    replayIndex+=1;
		
		goldDisplay = GameObject.FindGameObjectWithTag("GoldDisplay").GetComponent<Text>();
		
		// Initialize the start time
		startTime = Time.time;
		
		count = 0;
		
		enemies = new GameObject[]{};
		
		nodes = new Dictionary<int,GameObject>();
		
		
		// Instantiate the nodes to their respective values
		foreach(GameObject i in GameObject.FindGameObjectsWithTag("Node")){
		    nodes.Add(i.GetComponent<NodeScript>().nodeNumber,i);
		}
		
		towerType = new Dictionary<int,GameObject>();
		
		towerType.Add(1,tower);
		towerType.Add(2,tower2);
		towerType.Add(3,tower3);
	}
	
	// Update is called once per frame
	void Update () {
	
	
	    // If end of replay reached
	    while(replayIndex < replay.Length){
	        if(replay[replayIndex] == 14){
			    SceneManager.LoadScene("ReplayOver", LoadSceneMode.Single);
				Destroy(gameObject);
				return;
			}
	        // Begin update OpCode (used as error checking)
	        if(replay[replayIndex]==11){
	            replayIndex+=1; // Move to next OpCode
			
			    float timeStamp = (float)BitConverter.ToDouble(replay,replayIndex);
			
			
			    // If updates have become de-synched, skip this update
			    if((Time.time-startTime) < timeStamp){
				    count = 0;
				    replayIndex-=1;
				    return;
			    }else{
				    count+=1;
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
			
			    Vector3 playerPosition = new Vector3(xPos,2,zPos);
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
		    
			foreach (GameObject i in currentPlayers){
			    Destroy(i);
			}
			
		    while(replay[replayIndex]==5){
			    replayIndex+=1; // Move to first parameter
				
				
			    // Read in xPos and zPos of tower
		        float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
				
				// Find the tower to despawn
				currentTowers = GameObject.FindGameObjectsWithTag("Tower");
				if(destroyTowers(currentTowers, xPos, zPos)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
				if(destroyTowers(currentTowers, xPos, zPos)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
				if(destroyTowers(currentTowers, xPos, zPos)){
				    continue;
				}
				
		    }
		    int type;
		    // While receiving Spawn Tower OpCodes
		    while(replay[replayIndex]==7){
		        replayIndex+=1; // Move to first parameter
				
			
			    // Read in xPos and zPos of tower
		        float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        
				type = BitConverter.ToInt32(replay,replayIndex);
		        replayIndex+=4;
				
				GameObject finTower = towerType[type];
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower");
				if(towerAtPos(currentTowers, xPos, zPos)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
				if(towerAtPos(currentTowers, xPos, zPos)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
				if(towerAtPos(currentTowers, xPos, zPos)){
				    continue;
				}
				
		    	// Spawn the tower
		        Vector3 towerPosition = new Vector3(xPos,2,zPos);
		        Instantiate(finTower,towerPosition,finTower.transform.rotation);
		    }
			
			while(replay[replayIndex]==16){
			    replayIndex+=1;
			    float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
				
				int currDamage;
				currDamage = BitConverter.ToInt32(replay,replayIndex);
		        replayIndex+=4;
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower");
				if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
				if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
				    continue;
				}
				
				currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
				if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
				    continue;
				}
				
				
			}
			
			// Read the gold value on this update
			if(replay[replayIndex]==9){
			    replayIndex+=1;
			    int gold = BitConverter.ToInt32(replay,replayIndex);
				replayIndex+=4;
				
			    goldDisplay.text = "Gold: " + gold;
			}
			
			replayIndex+=5; // Skip lives value
			
			int count3 = 0;
			
			//if(count <= 1){
			    enemies = GameObject.FindGameObjectsWithTag("Enemy");
			//}
			
			bool isEnemy = false;
			/**if(!(count <= 1)){
			    replayIndex += 25;
			}*/
			
			// Update the enemies positions (only done every few seconds)
			while(replay[replayIndex]==13 && (count <= 1)){
			    
				// Enemies have been updated this update
			    isEnemy = true;
			    replayIndex += 1;
				
				// Get enemy position
				float xPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
		        float zPos = (float)BitConverter.ToDouble(replay,replayIndex);
		        replayIndex+=8;
				
				// Get enemy health, and which node they are up to
				int health = BitConverter.ToInt32(replay,replayIndex);
		        replayIndex+=4;
				
				int node = BitConverter.ToInt32(replay,replayIndex);
		        replayIndex+=4;
				
				Vector3 enemyPosition = new Vector3(xPos,2,zPos);
				
				// If there are enemies already instantiated, and haven't been moved
				// to an update spot, updated those enemies
				if(enemies.Length > 0){
				    GameObject enemy = findClosest(xPos, zPos, enemies);
				
				    enemy.transform.position = enemyPosition;
				
				    enemy.GetComponent<EnemyHealth>().health = health;
					
					enemy.GetComponent<EnemyPathing>().setNode(nodes[node]);
					
					// This enemy has been updated, so remove it from the list of enemies to be updated
					enemies = removeObject(enemy,enemies);
				
				}else{
				
				    // Otherwise, instantiated a new enemy
				    GameObject e = (GameObject)Instantiate(enemy,enemyPosition,enemy.transform.rotation);
					e.GetComponent<EnemyHealth>().health = health;
					e.GetComponent<EnemyPathing>().setNode(nodes[node]);
				}
				
			}
			
			// If there are leftover enemies on screen after an enemy update, delete them
			if(isEnemy){
			    foreach(GameObject i in enemies){
					Destroy(i);
				}
			    enemies = new GameObject[]{};
			}
		
		    // Skip to the end of Update (if not already there)
		    while(replay[replayIndex]!=12){
		        replayIndex+=1;
		    }
		    replayIndex+=1;
			if(replayIndex == replay.Length){
			     foreach(GameObject i in enemies){
					Destroy(i);
				}
			}
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
	
	GameObject[] removeObject(GameObject target,GameObject[] list){

		int index = 0;
		bool found = false;
	    // Move the object to the end of the array
		foreach(GameObject i in list){
		    if(i == target){
				list[index] = list[list.Length-1];
				list[list.Length-1] = i;
				found = true;
				break;
			}
			index+=1;
		}
		
		// Delete the end of the array (containing the object to delete)
		if(found){
			Array.Resize(ref list,list.Length - 1);
		}
		
		return list;	
	}
	
	// Find the closest object to an x,z position
	private GameObject findClosest(float x, float z, GameObject[] objects){
	    Vector3 pos = new Vector3(x,2,z);
		GameObject closest = null;
		float minDistance = 99999999; // Assumed no enemy objects occur outside the map
		foreach(GameObject i in objects){
		    if(Vector3.Distance(pos,i.transform.position) < minDistance){
			    closest = i;
				minDistance = Vector3.Distance(pos,i.transform.position);
			}
		}
		
		return closest;
	}
	
	private bool destroyTowers(GameObject[] currentTowers,float xPos,float zPos){
				
		foreach(GameObject i in currentTowers){
			if((i.transform.position.x == xPos)
			&& (i.transform.position.z == zPos)){
				Destroy(i);
				return true;
			}
		}
		
		return false;
	}
	
	private bool upgradeTowers(GameObject[] currentTowers,float xPos, float zPos, int damage){
	    foreach(GameObject i in currentTowers){
			if((i.transform.position.x == xPos)
			&& (i.transform.position.z == zPos)){
				replayShootEnemies t = (replayShootEnemies)i.GetComponent("replayShootEnemies");
				t.setAdditionalDamage(damage);
				return true;
			}
		}
		return false;
	}
	
	private bool towerAtPos(GameObject[] currentTowers, float xPos, float zPos){
	    foreach(GameObject i in currentTowers){
		    if((i.transform.position.x == xPos)
			&& (i.transform.position.z == zPos)){
			    return true;
			}
		}
		return false;
	}
	
}
