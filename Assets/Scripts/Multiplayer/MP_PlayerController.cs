using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class MP_PlayerController : NetworkBehaviour
{
    // Game boundaries
    const int worldHeightMin = 10;
    const int worldHeightMax = 80;

    const int worldWidthMin = 10;
    const int worldWidthMax = 120;

    // Builder movement variables
    public float turningSpeed;
    public float movingSpeed;

    private const int upgradeCost = 30;

    int playerGold = 500;

    int previousNumTower1 = 0;
    int previousNumTower2 = 0;
    int previousNumTower3 = 0;

    bool upgradedTower = false;

    // Current buildable tile
    private GameObject currentBuildableTile = null;

    // Towers
    public GameObject tower;

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

    void Update()
    {
        // Check that is local player
        if (!isLocalPlayer)
        {
            return;
        }

        // Movement
        //DebugMove();
        ButtonActions();

        if (goldDisplay != null)
        {
            UpdateGold();
            goldDisplay.text = "Shared Gold: " + playerGold;
        }
    }


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

    void DebugMove()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * turningSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * movingSpeed;
        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        // Hit build button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdConstructTower("Tower");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdSellTower();
        }
    }

    public void ButtonActions()
    {
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
            CmdUpgradeTower();
            vc.UpgradeButtonOff();
        }

        // Perform state analysis
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

    public void ButtonRotate(float horizontalInput)
    {
        var x = horizontalInput * Time.deltaTime * turningSpeed;
        gameObject.transform.Rotate(0, x, 0);
    }

    public override void OnStartLocalPlayer()
    {
        Color orange = new Color(178 / 255.0f, 115 / 255.0f, 0, 1);
        GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", orange);
    }

    public void AddGold(int amount)
    {
        this.playerGold += amount;
    }

    public int GetGold()
    {
        return this.playerGold;
    }

    public void UpdateGold()
    {
        UpdateBaseTowerGold();
    }

    void UpdateBaseTowerGold()
    {
        // Tower1
        int numBaseTower1 = GameObject.FindGameObjectsWithTag("Tower").Length;
        int baseTowerCost1; towerCostDict.TryGetValue("Tower", out baseTowerCost1);
        int baseRefundCost1; towerRefundDict.TryGetValue("Tower", out baseRefundCost1);
        if (numBaseTower1 != previousNumTower1)
        {
            int numChanges1 = numBaseTower1 - previousNumTower1;
            playerGold -= numChanges1 * baseTowerCost1;
            previousNumTower1 = numBaseTower1;
        }

        // Tower2
        int numBaseTower2 = GameObject.FindGameObjectsWithTag("Tower2").Length;
        int baseTowerCost2; towerCostDict.TryGetValue("Tower2", out baseTowerCost2);
        int baseRefundCost2; towerRefundDict.TryGetValue("Tower2", out baseRefundCost2);
        if (numBaseTower2 != previousNumTower2)
        {
            int numChanges2 = numBaseTower2 - previousNumTower2;
            playerGold -= numChanges2 * baseTowerCost2;
            previousNumTower2 = numBaseTower2;
        }

        // Tower3
        int numBaseTower3 = GameObject.FindGameObjectsWithTag("Tower3").Length;
        int baseTowerCost3; towerCostDict.TryGetValue("Tower3", out baseTowerCost3);
        int baseRefundCost3; towerRefundDict.TryGetValue("Tower3", out baseRefundCost3);
        if (numBaseTower3 != previousNumTower3)
        {
            int numChanges3 = numBaseTower3 - previousNumTower3;
            playerGold -= numChanges3 * baseTowerCost3;
            previousNumTower3 = numBaseTower3;
        }

        // Update on upgrade
        if(upgradedTower == true)
        {
            playerGold -= upgradeCost;
            upgradedTower = false;
        }
    }
}
