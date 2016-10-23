// Author: Matthew Eldridge (695350)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playReplay : MonoBehaviour {

    // Filename for the temporary file to hold the replay
    public string fileName = "empty";
	//const string fileName = "Example Replays\MPReplay.dat";
	
	public GameObject tower; // For spawning towers
	public GameObject tower2; // For spawning level 2 towers
	public GameObject tower3; // For spawning level 3 towers

	public Dictionary<int,GameObject> towerType; // Contains tower type identifiers
	
	public GameObject player; // For spawning players
	public GameObject enemy; // For spawning enemies
	public GameObject bullet; // For spawning bullets
	
	public Text goldDisplay; // For displaying gold
	public Text livesDisplay; // For displaying lives
	
	private bool isEnemy; // Checks if an enemy has been altered in current update
	
	Color orange = new Color(178 / 255.0f, 115 / 255.0f, 0, 1); // For colouring the player
	
	float startTime; // Start time of playback (used for synchronization
	
	int fullStateCounter; // Counts up to the byte where full state recordings occur
	
	List<int> fullStateIndex = new List<int>(); // List of indexes where full state recording occurs
	
	GameObject buttons; //
    replayViewController vc; //

	
	Dictionary<int,GameObject> nodes; // Dictionary of nodes given by identity integer
	
	GameObject[] currentPlayers;
	GameObject[] currentTowers;
	GameObject[] enemies;
	GameObject[] bullets;
	GameObject[] targets;

	byte[] replay; // The replay file
	int replayIndex; // The current point in the replay file

	
	private bool runOpCode(){
	    switch(readByte()){
		    case 3:  //Updates a players position and rotation
			    updatePlayer();
				break;
		    case 5:  //Despawns a built tower
			    despawnTower();
				break;
			case 7:  //Spawns a new tower
			    spawnTower();
				break;
			case 16: //Updates a tower
			    updateTower();
				break;
			case 9:  //Sets the gold value
			    int gold = BitConverter.ToInt32(replay,replayIndex);
				replayIndex+=4;
				goldDisplay = GameObject.FindGameObjectWithTag("GoldDisplay").GetComponent<Text>();
			    goldDisplay.text = "Gold: " + gold.ToString();
				break;
			case 10: //Sets the lives value
				int lives = BitConverter.ToInt32(replay,replayIndex);
				replayIndex+=4;
				GameObject playerBase = GameObject.Find("PlayerBase");
				enemyReachBase ls = (enemyReachBase)playerBase.GetComponent("enemyReachBase");
				ls.numLives = lives;
				livesDisplay.text = "Lives: " + lives.ToString();
				break;
		    case 13: //Updates an enemy (done in partial state)
			    updateEnemy(false);
				break;
			case 29: //Updates the enemy (in full state)
			    updateEnemy(true);
				break;
			case 12: //Ends update
			    return false;
			case 27: //Spawns a tower (in full state)
			    fullSpawnTower();
				break;
			case 21: //Spawns a bullet (only occurs in full state)
			    spawnBullet();
				break;
			case 22: //Ends the full update
				return false;
			default:
				break;
		}
		return true;
	}
	
	// Use this for initialization
	void Start () {
	    GameObject test = GameObject.Find("EventCoordinator");
		
		
	    // Get the correct file path depending on device running the game
		if(test == null){
			#if UNITY_ANDROID
			fileName = Application.persistentDataPath + "/tempReplay.dat";
			#else
			fileName = "tempReplay.dat";
			#endif
		}else{
		    #if UNITY_ANDROID
			fileName = Application.persistentDataPath + "/saveReplay.dat";
			#else
			fileName = "saveReplay.dat";
			#endif
		}
	
	    buttons = GameObject.FindGameObjectWithTag("Buttons");
        vc = buttons.GetComponent<replayViewController>();
		
	    replay = System.IO.File.ReadAllBytes(fileName); // Read in the replay file
	    replayIndex = 0;
		
		// Is replay file of correct format and ended properly
		if(replay[replay.Length-1]!=14){
		    Debug.Log("Replay File not fully recorded!");
			Destroy(gameObject);
		}
		
		// Find all locations of full save states
		int otherIndex = replay.Length - 6;
		while(replay[otherIndex] == 23){
		    otherIndex+=1;
			fullStateIndex.Add(BitConverter.ToInt32(replay,otherIndex));
			otherIndex-=6;
		}
		
		// Put them in correct order
		fullStateIndex.Reverse();
		
		fullStateCounter=0;
		
		// Skip the header
	    while(readByte()!=2){
	    }
		
		// Initialize the start time
		
		startTime = Time.time;
		
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
	    
		// Run the buttons
	    ButtonActions();
	
	
	    // If end of replay reached
	    while(replayIndex < replay.Length){
		   
		   // Skip all full state updates
		    while(replay[replayIndex] == 20){
			    replayIndex+=9;
			    while(replay[replayIndex] != 22){
				    if(replay[replayIndex] == 3){
					    replayIndex+=29;
					}else if(replay[replayIndex] == 7){
					    replayIndex+=33;
						while(replay[replayIndex] == 25){
						    replayIndex+=17;
						}
					}else if(replay[replayIndex] == 13){
					    replayIndex+=29;
					}else if(replay[replayIndex] == 21){
					    replayIndex+=53;
					}else if(replay[replayIndex] == 10){
					    replayIndex += 5;
					}else{
					    Debug.Log("Missing");
					    while(replay[replayIndex] != 22){
					        replayIndex+=1;
						}
					}
				}
				replayIndex+=1;
				fullStateCounter+=1;
			}
			
			// Skip all fullstate location opcodes
			while(replay[replayIndex] == 23){
			    replayIndex+=5;
			}
			
			// Replay file has reached the end
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
			    if((Time.time - startTime) < timeStamp){
				    replayIndex-=1;
				    return;
			    }else{
			        replayIndex+=8;
			    }
		    }else{
			    Debug.Log("What?");
			}

		    currentPlayers = GameObject.FindGameObjectsWithTag("Player");
			isEnemy = false;
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			// Keep running opcodes until you reach the end of update
			while(runOpCode()){}
		    
			// Delete any players that have left
			foreach (GameObject i in currentPlayers){
			    Destroy(i);
			}

			// Delete any enemies that are left
			if(isEnemy){
			    foreach(GameObject i in enemies){
					Destroy(i);
				}
			    enemies = new GameObject[]{};
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
		// If the towers position is the same as the given position, destroy it
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
	    // If the towers position is the same as the given position, update its damage
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
	    // Find if a tower is at a location
	    foreach(GameObject i in currentTowers){
		    if((i.transform.position.x == xPos)
			&& (i.transform.position.z == zPos)){
			    return true;
			}
		}
		return false;
	}
	
	// Run a full state update (used for skipping through replay)
	public void runState(int index){
	
	    // Only run if this is an actual full state update
	    if(replay[index] == 20){
		    replayIndex = index;
		}else{
		    return;
		}
		replayIndex+=1;
		
		float timeStamp = (float)BitConverter.ToDouble(replay,replayIndex);
		replayIndex+=8;
		
		// Update startTime to synchronize with new position in replay time
		startTime = Time.time - timeStamp;
		
		// Destroy all old objects
		GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
		foreach(GameObject i in towers){
		    Destroy(i);
		}
		towers = GameObject.FindGameObjectsWithTag("Tower2");
		foreach(GameObject i in towers){
		    Destroy(i);
		}
		towers = GameObject.FindGameObjectsWithTag("Tower3");
		foreach(GameObject i in towers){
		    Destroy(i);
		}
		
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject i in enemies){
		    Destroy(i);
		}
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("Projectile");
		foreach(GameObject i in bullets){
		    Destroy(i);
		}
		
		// 5500 - 150 = 5350
		
		currentPlayers = GameObject.FindGameObjectsWithTag("Player");
		
		// Keep running opcodes until end update opcode runs
		while(runOpCode()){}
		
		// Destroy all non-existing players
		foreach (GameObject i in currentPlayers){
		    Destroy(i);
		}
		
	}
	
	public void ButtonActions()
    {
	    // Move to next recorded state
        if (vc.nextButtonPressed())
        {
		    if(fullStateCounter >= fullStateIndex.Count){
			    Debug.Log("Last state");
			}else{
			    runState(fullStateIndex[fullStateCounter]);
				fullStateCounter+=1;
			}
            vc.nextButtonOff();
        }
		
		// Move to previous recorded state
		if (vc.prevButtonPressed())
        {
		    if(fullStateCounter-2 < 0){
			    Debug.Log("First state");
			}else{
			    runState(fullStateIndex[fullStateCounter-2]);
				fullStateCounter-=1;
			}
            vc.prevButtonOff();
        }
		
    }
	
	
	
	private void updatePlayer(){
		int id = readInt();
			
		//Read the xPos, zPos and yRotation of player
		float xPos = readFloat();
			
		float zPos = readFloat();
			
		float yRot = readFloat();
			
		Vector3 playerPosition = new Vector3(xPos,4,zPos);
		Quaternion playerRotation = Quaternion.Euler(0,yRot,0);
			
		// If no more players to move around (this is a new player),
		// spawn them, otherwise move a different player to this spot
		if(currentPlayers.Length == 0){
			GameObject play = (GameObject)Instantiate(player,playerPosition,playerRotation);
			play.GetComponent<MP_PlayerController>().enabled = false;
		    if(id == 1){
                //play.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", orange);
		    }
		}else{
		    GameObject movePlayer = currentPlayers[currentPlayers.Length-1];
			movePlayer.transform.position = playerPosition;
			movePlayer.transform.rotation = playerRotation;
		    if(id == 1){
                //movePlayer.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", orange);
		    }
		// Remove any moved players so they don't get moved again
		currentPlayers = removeObject(currentPlayers.Length-1,currentPlayers);
		}
	}
	
	private void despawnTower(){
		
		
		// Read in xPos and zPos of tower
		float xPos = readFloat();
		float zPos = readFloat();
		
		// Find the tower to despawn and despawn it
		currentTowers = GameObject.FindGameObjectsWithTag("Tower");
		if(destroyTowers(currentTowers, xPos, zPos)){
		    return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
		if(destroyTowers(currentTowers, xPos, zPos)){
		    return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
		if(destroyTowers(currentTowers, xPos, zPos)){
		    return;
		}
	}
	
	private void spawnTower(){
		
		
		// Read in xPos and zPos of tower
		float xPos = readFloat();
		float zPos = readFloat();
		
		int type = readInt();
		
		GameObject finTower = towerType[type];
		
		// If there's already a tower at the location just return
		currentTowers = GameObject.FindGameObjectsWithTag("Tower");
		if(towerAtPos(currentTowers, xPos, zPos)){
		    return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
		if(towerAtPos(currentTowers, xPos, zPos)){
		    return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
		if(towerAtPos(currentTowers, xPos, zPos)){
		    return;
		}
		
		// Otherwise spawn the tower
		Vector3 towerPosition = new Vector3(xPos,4,zPos);
		Instantiate(finTower,towerPosition,finTower.transform.rotation);
	}
	
	private void fullSpawnTower(){
		
		// Read in xPos and zPos of tower
		float xPos = readFloat();
		float zPos = readFloat();
		
		int type = readInt();

		GameObject finTower = towerType[type];
		
		int damage = readInt(); // Towers damage
		float coolDown = readFloat(); // Towers cooldown
		
		// Spawn the tower
		Vector3 towerPosition = new Vector3(xPos,4,zPos);
		GameObject t = (GameObject)Instantiate(finTower,towerPosition,finTower.transform.rotation);
		
		// Update damage and cooldown
		replayShootEnemies shoot = (replayShootEnemies)t.GetComponent("replayShootEnemies");
		shoot.setLastShotTime(coolDown);
		shoot.setAdditionalDamage(damage);
		
		// Find all enemies within range of tower and add them to inRange
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		
		while(readByte() == 25){
			float exPos = readFloat();
		    float ezPos = readFloat();
			shoot.inRange.Add(findClosest(exPos,ezPos,enemies));
		}
		replayIndex-=1; // readByte moves forward, even if the byte isn't the one we want
	}
	
	private void updateTower(){
	
	    // Read the xPos and zPos of tower
		float xPos = readFloat();
		float zPos = readFloat();
		
		// Get the new damage
		int currDamage;
		currDamage = readInt();
		
		// Find and update the tower
		currentTowers = GameObject.FindGameObjectsWithTag("Tower");
		if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
			return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower2");
		if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
			return;
		}
		
		currentTowers = GameObject.FindGameObjectsWithTag("Tower3");
		if(upgradeTowers(currentTowers, xPos, zPos, currDamage)){
			return;
		}
	}
	
	private void updateEnemy(bool fullUpdate){
	    isEnemy = true;
				
		// Get enemy position
		float xPos = readFloat();
		float zPos = readFloat();
		
		// Get enemy health, max health, and which node they are up to
		int health = readInt();
		
		int maxHealth = readInt();
		
		int node = readInt();
		
		Vector3 enemyPosition = new Vector3(xPos,2,zPos);
		
		// If there are enemies already instantiated, and haven't been moved
		// to an update spot, updated those enemies
		if(!fullUpdate && enemies.Length > 0){
			GameObject enemy = findClosest(xPos, zPos, enemies);
		
			enemy.transform.position = enemyPosition;
		
			enemy.GetComponent<EnemyHealth>().health = health;
			enemy.GetComponent<EnemyHealth>().maxHealth = maxHealth;
		
			enemy.GetComponent<EnemyPathing>().setNode(nodes[node]);
		
			// This enemy has been updated, so remove it from the list of enemies to be updated
			enemies = removeObject(enemy,enemies);
		
		}else{
		
			// Otherwise, instantiated a new enemy
			GameObject e = (GameObject)Instantiate(enemy,enemyPosition,enemy.transform.rotation);
			e.GetComponent<EnemyHealth>().health = health;
			e.GetComponent<EnemyHealth>().maxHealth = maxHealth;
			e.GetComponent<EnemyPathing>().setNode(nodes[node]);
		}
	}
	
	private void spawnBullet(){
		// Get list of potential targets
	    targets = GameObject.FindGameObjectsWithTag("Enemy");
		
		// Get position of the bullet
		float xPos = readFloat();
		float zPos = readFloat();
		float yPos = readFloat();
		
		// Get the position of the bullets target
		float txPos = readFloat();
		float tzPos = readFloat();
		
		// Set the bullets damage
		int damage = readInt();
		// Set the bullets speed
		float speed = readFloat();
		
		Vector3 bulletPosition = new Vector3(xPos,yPos,zPos);
		Vector3 targetPosition = new Vector3(txPos,2,tzPos);
		
		// Create the bullet
		GameObject y = (GameObject)Instantiate(bullet,bulletPosition,bullet.transform.rotation);
		
		// Set the bullets attributes
		replayBulletBehaviour bB = (replayBulletBehaviour)y.GetComponent("replayBulletBehaviour");
		bB.target = findClosest(txPos,tzPos,targets);
		bB.startpos = bulletPosition;
		bB.targetpos = targetPosition;
		bB.setDamage(damage);
		bB.setSpeed(speed);
	}
	
	// Read a byte value from the replay file
	private byte readByte(){
		byte ret = replay[replayIndex];
		replayIndex += 1;
		return ret;
	}
	
	// Read an integer value from the replay file
	private int readInt(){
		int ret = BitConverter.ToInt32(replay,replayIndex);
		replayIndex += 4;
		return ret;
	}
	
	// Read a float value from the replay file
	private float readFloat(){
		float ret = (float)BitConverter.ToDouble(replay,replayIndex);
		replayIndex += 8;
		return ret;
	}
	
}
