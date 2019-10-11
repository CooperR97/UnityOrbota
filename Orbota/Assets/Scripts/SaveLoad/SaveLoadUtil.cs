using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;
using System;

/**
 ***1. It is important to go into the LevelManager, under Checkpoints set the CheckpointAttributionAxis and CheckpointAttributionDirection. 
 *      
 *      An example setup would be, if your level starts at the top and ends at the bottom, set the Axis 
 *      to 'y' and the direction to 'descending' so that it will pick the highest checkpoint to start you at. This is VERY important in order
 *      to avoid the checkpoints messing with the level.
 *      
 ***2. Add checkpoints to your level. Note that every checkpoint in a level MUST HAVE A UNIQUE NAME, because they are identified by name. 
 *      
 *      The checkpoint prefab is located in Prefabs/SavingLoading/ORBCheckpoint.prefab or make your own.
 *      
 ***3. Add this script to a gameObject titled SaveLoadUtil IN THE LEVEL. 
 * 
 *      There is a prefab you can use, in Prefabs/SavingLoading/SaveLoadUtil.prefab
 *      
 *      
 * **/


public class SaveLoadUtil : MonoBehaviour
{
    public int saveIncrement = 200;
    public int checkpointRange = 3;

    private GameObject saveText;

    private string dataPath;
    private int saveTimer;

    //Fields to store player object and components
    private GameObject player;
    private Character playerCharacter;
    private Transform playerTransform;
    private Health playerHealth;
    private InventoryHandler playerInventory;

    //Fields to store data loaded in from file
    string loadedSceneName;
    string loadedCheckPointName;
    Character loadedCharacter;
    int loadedHealth;
    private int[] loadedEquippedItems = { 1, 2, 3 };
    private int[] loadedInventoryItems = { 321, 23, 36 };

    private float xDistanceToCheckpoint;
    private float yDistanceToCheckpoint;

    private string splitCode = "-%%%%-";

    private CheckPoint[] levelCheckPoints;

    private Dictionary<string, string> Keys = new Dictionary<string, string>()
    {
        {"scene", "LVL-->"},
        {"checkpoint", "PSTN-->"},
        {"character", "CHAR-->"},
        {"health", "HTH-->"},
        {"equippedItems", "EQITM-->" },
        {"inventoryItems", "INVITM-->" }
    };

    // Start is called before the first frame update
    void Start()
    {
        HandleSaveText();
        if(Directory.Exists(Path.Combine(Application.persistentDataPath, "SAVE_FILES")))
        {
            dataPath = Path.Combine(Application.persistentDataPath, "SAVE_FILES");
        }
        else
        {
            string newSavePath = Path.Combine(Application.persistentDataPath, "SAVE_FILES");
            Directory.CreateDirectory(newSavePath);
            dataPath = newSavePath;
        }

        //If in the LoadGame scene, load the saved game
        if (SceneManager.GetActiveScene().name == "LoadGame")
        {
            Load();
        }
    }

    /**
     * Check whether or not we need to save
     * */
    public void FixedUpdate()
    {
        if(saveText == null)
        {
            HandleSaveText();
        }
        if(levelCheckPoints != null && levelCheckPoints.Length != 0)
        {
            if (playerTransform != null && playerHealth != null && playerCharacter != null && playerInventory != null)
            {
                Array.ForEach(levelCheckPoints, checkPoint =>
                {
                    if(checkPoint != null)
                    {
                        xDistanceToCheckpoint = System.Math.Abs(playerTransform.position.x - checkPoint.transform.position.x);
                        yDistanceToCheckpoint = System.Math.Abs(playerTransform.position.y - checkPoint.transform.position.y);

                        if ((xDistanceToCheckpoint < checkpointRange && yDistanceToCheckpoint < checkpointRange) && saveTimer >= saveIncrement)
                        {
                            ShowSaveText();
                            Save(checkPoint);
                            saveTimer = 0;
                        }
                    }
                });
                saveTimer++;
            }
            else
            {
                FetchPlayerData();
            }
        }
    }

    /**
     * TODO: The inventory item Arrays need to be retrieved from the Player Inventory component.
     * */

    public void Save(CheckPoint checkPoint)
    {
        /**
        * Find player instance and save...
        *      1. The scene (level) that the player is currently on
        *      2. The transform.position of the player (where in the level they are)
        *      3. The corgi character component ()
        *      4. The current health of the player
        *      5. Current Equipped Items --> Array of Integers representing equipped Items
        *      5. Current Inventory --> Array of Integers to represent the items currently in inventory
        * **/
        string positionJSON = checkPoint.name;
        string sceneJSON = SceneManager.GetActiveScene().name;
        string characterJSON = JsonUtility.ToJson(playerCharacter);
        string healthJSON = playerHealth.CurrentHealth.ToString();
        string equippedItemsJSON = "";
        string inventoryItemsJSON = "";

        //************************************
        //Replace loadedEquippedItems and loadedInvetoryItems with Inventory.equippedItems and Inventory.InventoryItems or whatever they are
        Array.ForEach(loadedEquippedItems, num => equippedItemsJSON += (num + ", "));
        Array.ForEach(loadedInventoryItems, num => inventoryItemsJSON += (num + ", "));

        using (StreamWriter streamWriter = File.CreateText(Path.Combine(dataPath, "SAVE_DATA.txt")))
        {
            streamWriter.Write(splitCode + Keys["scene"]);
            streamWriter.Write(sceneJSON);

            streamWriter.Write(splitCode + Keys["checkpoint"]);
            streamWriter.Write(positionJSON);

            streamWriter.Write(splitCode + Keys["character"]);
            streamWriter.Write(characterJSON);

            streamWriter.Write(splitCode + Keys["health"]);
            streamWriter.Write(healthJSON);

            streamWriter.Write(splitCode + Keys["equippedItems"]);
            streamWriter.Write(equippedItemsJSON);

            streamWriter.Write(splitCode + Keys["inventoryItems"]);
            streamWriter.Write(inventoryItemsJSON);
        }
    }

    /**
     * Read in data from a file, if necessary load new Level
     * */
    public void Load()
    {
        if(dataPath == "")
        {
            dataPath = Path.Combine(Application.persistentDataPath, "SAVE_FILES");
        }

        using (StreamReader streamReader = File.OpenText(Path.Combine(dataPath, "SAVE_DATA.txt")))
        {
            string rawJSON = streamReader.ReadToEnd();
            string[] splitJSON = rawJSON.Split(new string[] { splitCode }, StringSplitOptions.RemoveEmptyEntries);

            loadedSceneName = Array.Find(splitJSON, ele => ele.Contains(Keys["scene"])).Substring(Keys["scene"].Length);
        }

        //If not in the correct saved scene
        if(loadedSceneName != SceneManager.GetActiveScene().name)
        {
            SceneManager.LoadScene(loadedSceneName);
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnProperSceneLoad;
        }
    }

    /**
     * Start the Initialization process once the proper scene is loaded
     * */
    public void OnProperSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Start();
        StartCoroutine(Initialize(.01f));
        SceneManager.sceneLoaded -= OnProperSceneLoad;
    }

    /**
     * Called only once the proper scene has been loaded
     * Reads in the character data as well as checkpoint location from the file
     * 
     * TODO: 
     * At the end of this method, set the player's inventory to the necessary state (loadedEquippedItems and loadedInventoryItems)
     * */
    private IEnumerator Initialize(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        levelCheckPoints = GameObject.FindObjectsOfType<CheckPoint>();
        FetchPlayerData();
        LoadCharacter();
        levelCheckPoints = GameObject.FindObjectsOfType<CheckPoint>();

        CheckPoint checkPoint = Array.Find(levelCheckPoints, cp => cp.name == loadedCheckPointName);

        playerCharacter.RespawnAt(checkPoint.transform, Character.FacingDirections.Left);
        playerHealth.Damage(playerHealth.MaximumHealth - loadedHealth, new GameObject(), 0, 0);
        //Set player inventory
    }

    /**
     * Read in and parse character data
     * - Current checkpoint
     * - Current health
     * - Equipped items
     * - Inventory items
     * */
    public void LoadCharacter()
    {
        if (dataPath == "")
        {
            dataPath = Path.Combine(Application.persistentDataPath, "SAVE_FILES");
        }

        using (StreamReader streamReader = File.OpenText(Path.Combine(dataPath, "SAVE_DATA.txt")))
        {
            string rawJSON = streamReader.ReadToEnd();
            string[] splitJSON = rawJSON.Split(new string[] { splitCode }, StringSplitOptions.RemoveEmptyEntries);
            
            loadedCheckPointName = Array.Find(splitJSON, ele => ele.Contains(Keys["checkpoint"])).Substring(Keys["checkpoint"].Length);
            
            loadedHealth = Int32.Parse(Array.Find(splitJSON, ele => ele.Contains(Keys["health"])).Substring(Keys["health"].Length));

            string loadedEquippedItemsJSON = Array.Find(splitJSON, ele => ele.Contains(Keys["equippedItems"])).Substring(Keys["equippedItems"].Length).Trim();
            string loadedInventoryItemsJSON = Array.Find(splitJSON, ele => ele.Contains(Keys["inventoryItems"])).Substring(Keys["inventoryItems"].Length).Trim();

            int i = 0;
            Array.ForEach(loadedEquippedItemsJSON.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries), itemNum => {
                loadedEquippedItems[i] = Int32.Parse(itemNum);
                i++;
            });

            i = 0;
            Array.ForEach(loadedInventoryItemsJSON.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries), itemNum => {
                loadedInventoryItems[i] = Int32.Parse(itemNum);
                i++;
            });
        }

    }
    
    /**
     * Retrieve the player object and ALL necessary components
     * */
    private void FetchPlayerData()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTransform = player.GetComponent<Transform>();
            playerHealth = player.GetComponent<Health>();
            playerCharacter = player.GetComponent<Character>();
            playerInventory = player.GetComponent<InventoryHandler>();
        }
    }

    private void ShowSaveText()
    {
        if(saveText != null && !saveText.activeSelf)
        {
            saveText.SetActive(true);
            StartCoroutine(RemoveSaveText(2.5f));
        }
    }

    private IEnumerator RemoveSaveText(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        saveText.SetActive(false);
    }

    private void HandleSaveText()
    {
        saveText = GameObject.Find("SavingText");
        levelCheckPoints = GameObject.FindObjectsOfType<CheckPoint>();

        if (saveText != null)
        {
            saveText.SetActive(false);
        }
    }
}
