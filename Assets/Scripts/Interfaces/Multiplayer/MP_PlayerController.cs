using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class MP_PlayerController : NetworkBehaviour
{
    // Type of game
    const bool isMultiplayerMode = true;

    // Game boundaries
    const int worldHeightMin = 10;
    const int worldHeightMax = 80;

    const int worldWidthMin = 10;
    const int worldWidthMax = 120;

    // Builder movement variables
    public float turningSpeed;
    public float movingSpeed;

    // Gameplay variables
    private const int upgradeCost = 30;

    private bool gameStarted = false;
    private bool serverLoaded = false;
    int playerGold = 100;

    int previousNumTower1 = 0;
    int previousNumTower2 = 0;
    int previousNumTower3 = 0;

    bool upgradedTower = false;

    // Current buildable tile
    private GameObject currentBuildableTile = null;

    // Direction
    int forward = 1;
    int backward = -1;

    // Buttons
    GameObject buttons;
    ViewController vc;

    // Gold UI
    Text goldDisplay;

    // Dictionary of values
    Dictionary<string, int> towerCostDict = new Dictionary<string, int>();
    Dictionary<string, int> towerRefundDict = new Dictionary<string, int>();

    // Total number of upgrades made
    private int totalTowerLevel = 0;

    GameObject disconnectCanvas;

    /// Instantiate all prefabs and dictionaries
    void Start()
    {
        // Initialise values for Towers
        towerCostDict.Add("Tower", 20);
        towerRefundDict.Add("Tower", 10);
        towerCostDict.Add("Tower2", 40);
        towerRefundDict.Add("Tower2", 20);
        towerCostDict.Add("Tower3", 60);
        towerRefundDict.Add("Tower3", 30);


        // Get buttons
        buttons = GameObject.FindGameObjectWithTag("Buttons");
        vc = buttons.GetComponent<ViewController>();

        // Get gold UI
        goldDisplay = GameObject.FindGameObjectWithTag("GoldDisplay").GetComponent<Text>();
    }

    // Update gameplay on button actions and UI display
    void Update()
    {
        // Check that is local player
        if (!isLocalPlayer)
        {
            return;
        }

        // Movement
        // DebugMove(); /// Used for keyboard testing
        ButtonActions();

        if (goldDisplay != null)
        {
            UpdateGold();
            goldDisplay.text = "Shared Gold: " + playerGold;
        }

        //CheckDC();
    }


    /// Call MP_BuildableTower to build a tower
    [Command]
    public void CmdConstructTower(string tower)
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Ray ray = new Ray(transform.position, down);

        // Raycast beneath builder
        if (Physics.Raycast(ray, out hit, 10))
        {
            // Hit a buildable surface
            if (hit.collider.tag == "BS")
            {
                // Get current tile
                currentBuildableTile = hit.collider.gameObject;

                // Get the script build tower and build tower
                MP_TileBuildTower build_script = currentBuildableTile.GetComponent<MP_TileBuildTower>();
                build_script.BuildTower(this.gameObject.GetInstanceID(), tower, towerCostDict, playerGold);
            }
            else
            {
                currentBuildableTile = null;
            }
        }
    }

    /// Call MP_BuildableTower to sell a tower
    [Command]
    public void CmdSellTower()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Ray ray = new Ray(transform.position, down);

        // Raycast beneath builder
        if (Physics.Raycast(ray, out hit, 10))
        {
            // Hit a buildable surface
            if (hit.collider.tag == "BS")
            {
                // Get current tile
                currentBuildableTile = hit.collider.gameObject;

                // Get the script build tower and sell tower
                MP_TileBuildTower sell_script = currentBuildableTile.GetComponent<MP_TileBuildTower>();
                sell_script.SellTower(this.gameObject.GetInstanceID(), towerRefundDict);
            }
            else
            {
                currentBuildableTile = null;
            }
        }
    }

    /// Call MP_BuildableTower to upgrade a tower
    [Command]
    public void CmdUpgradeTower()
    {
        Vector3 down = transform.TransformDirection(Vector3.down);
        RaycastHit hit;
        Ray ray = new Ray(transform.position, down);

        // Raycast beneath builder
        if (Physics.Raycast(ray, out hit, 10))
        {
            // Hit a buildable surface
            if (hit.collider.tag == "BS")
            {
                // Get current tile
                currentBuildableTile = hit.collider.gameObject;

                // Get the script build tower and upgrade tower
                MP_TileBuildTower build_script = currentBuildableTile.GetComponent<MP_TileBuildTower>();
                build_script.UpgradeTower(this.gameObject.GetInstanceID(), ref upgradedTower);
            }
            else
            {
                currentBuildableTile = null;
            }
        }
    }

    /// Move the player with the keyboard
    void DebugMove()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * turningSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * movingSpeed;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        // Actions
        if (Input.GetKeyDown(KeyCode.B))
        {
            CmdConstructTower("Tower");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdSellTower();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CmdUpgradeTower();
        }
    }

    /// Each update poll ViewController for button presses
    public void ButtonActions()
    {
        // Tower buttons pressed
        if (vc.BuildButtonPressed())
        {
            CmdConstructTower("Tower");
            vc.BuildButtonOff();
        }

        if (vc.BuildButton2Pressed())
        {
            CmdConstructTower("Tower2");
            vc.BuildButton2Off();
        }

        if (vc.BuildButton3Pressed())
        {
            CmdConstructTower("Tower3");
            vc.BuildButton3Off();
        }

        if (vc.SellButtonPressed())
        {
            CmdSellTower();
            vc.SellButtonOff();
        }

        if (vc.UpgradeButtonPressed())
        {
            vc.UpgradeButtonOff();
            CmdUpgradeTower();
        }

        // Movement buttons pressed
        if (vc.RotateButtonPressed())
        {
            ButtonRotate(backward);
        }

        if (vc.UpButtonPressed())
        {
            ButtonTranslate(forward);
        }

        if (vc.DownButtonPressed())
        {
            ButtonTranslate(backward);
        }
    }

    /// Check for game boundaries when moving the player
    public void ButtonTranslate(float verticalInput)
    {
        var z = verticalInput * Time.deltaTime * movingSpeed;
        gameObject.transform.Translate(0, 0, z);

        // Boundary checking
        if (gameObject.transform.position.z < worldHeightMin)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, worldHeightMin);
        }
        if (gameObject.transform.position.z > worldHeightMax)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, worldHeightMax);
        }
        if (gameObject.transform.position.x < worldWidthMin)
        {
            gameObject.transform.position = new Vector3(worldWidthMin, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x > worldWidthMax)
        {
            gameObject.transform.position = new Vector3(worldWidthMax, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    /// Rotate player object
    public void ButtonRotate(float horizontalInput)
    {
        var x = horizontalInput * Time.deltaTime * turningSpeed;
        gameObject.transform.Rotate(0, x, 0);
    }

    /// When a player is disconnected from the server, load the disconnect scene
    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        // Game is hosting and not using unity services
        if(isMultiplayerMode == true && Network.isServer)
        {
            Debug.Log("Local server connection disconnected");
            SceneManager.LoadScene("Oops", LoadSceneMode.Single);
        }
        // Game has lost connection
        else if (info == NetworkDisconnection.LostConnection)
        {
            Debug.Log("Lost connection to the server");
            SceneManager.LoadScene("Disconnect", LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Successfully diconnected from the server");
            SceneManager.LoadScene("Disconnect", LoadSceneMode.Single);
        }
    }

    /// Colour the player a different colour when loaded into the game
    public override void OnStartLocalPlayer()
    {
        Color orange = new Color(178 / 255.0f, 115 / 255.0f, 0, 1);
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", orange);
    }

    /// Add gold to all players
    public void AddGold(int amount)
    {
        this.playerGold += amount;
    }

    /// Get the current amount of gold
    public int GetGold()
    {
        return this.playerGold;
    }

    /// Update gold to all players
    public void UpdateGold()
    {
        UpdateBaseTowerGold();
        DeductUpgradeCost();
    }

    /// When a tower is built, player will calculate the gold remaning
    void UpdateBaseTowerGold()
    {
        // Tower1
        int numBaseTower1 = GameObject.FindGameObjectsWithTag("Tower").Length;
        if (numBaseTower1 != previousNumTower1)
        {
            int cost1 = 0;
            int numChanges1 = numBaseTower1 - previousNumTower1;
            if (numChanges1 > 0)
            {
                // Towers build, deduct gold
                towerCostDict.TryGetValue("Tower", out cost1);
            }
            else
            {
                // Towers sold, increase gold
                towerRefundDict.TryGetValue("Tower", out cost1);
            }

            playerGold -= numChanges1 * cost1;
            previousNumTower1 = numBaseTower1;
        }

        // Tower2
        int numBaseTower2 = GameObject.FindGameObjectsWithTag("Tower2").Length;
        if (numBaseTower2 != previousNumTower2)
        {
            int cost2 = 0;
            int numChanges2 = numBaseTower2 - previousNumTower2;
            if (numChanges2 > 0)
            {
                // Towers build, deduct gold
                towerCostDict.TryGetValue("Tower2", out cost2);
            }
            else
            {
                // Towers sold, increase gold
                towerRefundDict.TryGetValue("Tower2", out cost2);
            }

            playerGold -= numChanges2 * cost2;
            previousNumTower2 = numBaseTower2;
        }

        // Tower3
        int numBaseTower3 = GameObject.FindGameObjectsWithTag("Tower3").Length;
        if (numBaseTower3 != previousNumTower3)
        {
            int cost3 = 0;
            int numChanges3 = numBaseTower3 - previousNumTower3;
            if (numChanges3 > 0)
            {
                // Towers build, deduct gold
                towerCostDict.TryGetValue("Tower3", out cost3);
            }
            else
            {
                // Towers sold, increase gold
                towerRefundDict.TryGetValue("Tower3", out cost3);
            }

            playerGold -= numChanges3 * cost3;
            previousNumTower3 = numBaseTower3;
        }
    }

    //// When a upgrade is built, player will calculate the gold remaning
    private void DeductUpgradeCost()
    {
        int i;
        // Tower1
        GameObject[] baseTower1 = GameObject.FindGameObjectsWithTag("Tower");
        int newTowerLevel = 0;
        if (baseTower1 != null)
        {
            for (i = 0; i < baseTower1.Length; i++)
            {
                ShootEnemies towerShooting =
                    baseTower1[i].GetComponent<ShootEnemies>();
                newTowerLevel += towerShooting.level;
            }
        }

        // Tower2
        GameObject[] baseTower2 = GameObject.FindGameObjectsWithTag("Tower2");
        if (baseTower2 != null)
        {
            for (i = 0; i < baseTower2.Length; i++)
            {
                ShootEnemies towerShooting =
                    baseTower2[i].GetComponent<ShootEnemies>();
                newTowerLevel += towerShooting.level;
            }
        }

        // Tower3
        GameObject[] baseTower3 = GameObject.FindGameObjectsWithTag("Tower3");
        if (baseTower3 != null)
        {
            for (i = 0; i < baseTower3.Length; i++)
            {
                ShootEnemies towerShooting =
                    baseTower3[i].GetComponent<ShootEnemies>();
                newTowerLevel += towerShooting.level;
            }
        }

        if (newTowerLevel > totalTowerLevel)
        {
            int newLevels = newTowerLevel - totalTowerLevel;
            playerGold -= upgradeCost * newLevels;
            totalTowerLevel = newTowerLevel;
        }
        else if (newTowerLevel != totalTowerLevel)
        {
            // No extra refund on selling upgraded towers
            totalTowerLevel = newTowerLevel;
        }
    }

    [Command]
    public void CmdLoadDisconnectedScene()
    {
        SceneManager.LoadScene("Disconnect", LoadSceneMode.Single);
    }

    [Command]
    public void CmdLoadOopsScene()
    {
        SceneManager.LoadScene("Oops", LoadSceneMode.Single);
    }

    public void SetDisconnectCanvas()
    {
        CmdSetDisconnectCanvas();
    }

    [Command]
    public void CmdSetDisconnectCanvas()
    {
        GameObject dc = Resources.Load("DisconnectCanvas") as GameObject;
        disconnectCanvas = (GameObject) Instantiate(dc, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(disconnectCanvas);

        RpcSetDisconnectCanvas(disconnectCanvas);
    }

    [ClientRpc]
    void RpcSetDisconnectCanvas(GameObject DisconnectCanvas)
    {
        DisconnectCanvas = (GameObject) Instantiate(Resources.Load("DisconnectCanvas") as GameObject, Vector3.zero, Quaternion.identity);
    }

    public void CheckDC()
    {
        // NetworkClient nc = new NetworkClient();
        // Debug.Log("nc = " + nc.isConnected + " gs = " + gameStarted + " nc = " + Network.connections.Length);

        // if(GameObject.FindGameObjectsWithTag("Player").Length > 1)
        // {
        //     gameStarted = true;
        // }

        // if(GameObject.FindGameObjectsWithTag("Buttons").Length > 0)
        // {
        //     serverLoaded = true;
        // }

        // if (GameObject.FindGameObjectsWithTag("Player").Length <= 1 && gameStarted)
        // {
        //     //Network.CloseConnection(Network.connections[0], true);
        //     CmdLoadDisconnectedScene();
        // }

        // Debug.Log("serverLoaded = " + serverLoaded + "gameStarted = " + gameStarted);

        // if(GameObject.FindGameObjectWithTag("Buttons") == null && serverLoaded && gameStarted)
        // {
        //     LoadDisconnectedScene();
        // }

        // if(GameObject.FindGameObjectsWithTag("QUIT").Length > 0)
        // {
        //     LoadDisconnectedScene();
        // }



        // if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
        // {
        //     //Network.CloseConnection(Network.connections[0], true);
        //     LoadDisconnectedScene();
        // }


        // if(nc.isConnected == false && gameStarted == true && Network.connections.Length < 2)
        // {
        //     //Debug.Log("nc = " + nc.isConnected + " gs = " + gameStarted + " nc = " + Network.connections.Length);

        //     LoadOopsScene();
        // }



        // MP_GameCoordinator mgc = GameObject.FindGameObjectWithTag("GameController").GetComponent<MP_GameCoordinator>();
        // if(mgc.isGameStarted() == true)
        // {
        //     if(mgc.GetQuitStatus() == true && Network.isClient)
        //     {
        //         LoadDisconnectedScene();
        //     }
        // }
        // MP_GameCoordinator mgc = GameObject.FindGameObjectWithTag("GameController").GetComponent<MP_GameCoordinator>();
        // if (mgc.isGameStarted() == true)
        // {
        //     if (Network.connections.Length == 1)
        //     {
        //         Network.CloseConnection(Network.connections[0], true);
        //         LoadDisconnectedScene();
        //     }
        // }
    }


}