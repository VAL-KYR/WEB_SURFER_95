using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class track : MonoBehaviour {

    public float startTime;
    public float enemyTime;
    public float boostTime;
    public float goodItemTime;
    public float badItemTime;
    public float slowTime;


    // Debug Management
    [System.Serializable]
    public class mDebug
    {
        public bool gizmos = false;
    }
    public mDebug debug = new mDebug();

    // Track Management
    [System.Serializable]
    public class mTrack
    {
        public GameObject finishLine;
        public Text timer;
        public Slider progressBar;
        public Image speedIndicator;
        public List<Sprite> speedSprites = new List<Sprite>(3);
        public bool started = false;
        public bool goal = false;
        public bool finish = false;
        public float length = 50f;
        public float playerSpeed = 0.5f;
        public float completionTime = 1000f;
        public float timeLeft;
        public string remainingTime;

        public bool speedBoost = false;
        public float speedBoostAmount = 1.10f;
        public float speedBoostTime = 20f;

        public bool speedReduce = false;
        public float speedReduceAmount = 0.70f;
        public float speedReduceTime = 20f;

        public float addTimeAmount = 10f;
    }
    public mTrack webTrack = new mTrack();

    // Item Laning Management
    [System.Serializable]
    public class mItems
    {
        public int numLanes = 3;
        public List<GameObject> lanes = new List<GameObject>();
        public List<GameObject> goodTypes = new List<GameObject>();
        public List<GameObject> badTypes = new List<GameObject>();
        public float goodItemRate = 5.0f;
        public float badItemRate = 2.0f;

    }
    public mItems item = new mItems();

    // Enemy Management
    [System.Serializable]
    public class mEnemy
    {
        public List<GameObject> types = new List<GameObject>();
        public GameObject spawnZone;
        public float spawnDistance = 20f;
        public float spawnRadius = 5f;
        public float spawnRate = 1.0f;

        public List<GameObject> list = new List<GameObject>();
    }
    public mEnemy enemy = new mEnemy();

    // Use this for initialization
    void Start () {
        // Create a goal line for the track
        webTrack.finishLine.transform.position = new Vector3(webTrack.finishLine.transform.position.z, webTrack.finishLine.transform.position.y, webTrack.length);
        webTrack.timeLeft = webTrack.completionTime;
        webTrack.remainingTime = clockTime(webTrack.timeLeft);

        // Create a spawnzone for enemies
        enemy.spawnZone = new GameObject();
        enemy.spawnZone.name = "EnemySpawnZone";
        enemy.spawnZone.transform.SetParent(gameObject.transform);
        enemy.spawnZone.transform.position = new Vector3(0, 0, enemy.spawnDistance);
        enemy.spawnZone.AddComponent<SphereCollider>();
        enemy.spawnZone.GetComponent<SphereCollider>().radius = enemy.spawnRadius;
        enemy.spawnZone.GetComponent<SphereCollider>().isTrigger = true;

        // Create the lanes for items
        createLanes();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("LPalmTrigger") < 0.5f && Input.GetAxis("RPalmTrigger") < 0.5f) && !webTrack.started && !webTrack.finish)
        {
            webTrack.started = true;
        }

        // Track game running
        if (webTrack.started)
        {
           

            // Player Reaches end
            if (gameObject.transform.position.z >= webTrack.length)
            {
                webTrack.goal = true;
            }
            
            // ADD TO COMPLETION TIME WITH POWERUPS
            
            // End Sceneario
            if (webTrack.timeLeft <= 0f || webTrack.goal)
            {
                webTrack.finish = true;

                // conditions for win/loss
                if (webTrack.goal)
                {
                    Debug.Log("Player wins with: " + webTrack.remainingTime);
                }
                else
                {
                    Debug.Log("Player lost");
                }

            }

            // RUNNING PROCESSES DURING GAME
            else
            {
                // Move Player
                // Speed up
                if (webTrack.speedBoost)
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, 
                                                                gameObject.transform.position.y,
                                                                gameObject.transform.position.z + webTrack.playerSpeed * webTrack.speedBoostAmount);
                // Slow down
                else if (webTrack.speedReduce)
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                                gameObject.transform.position.y,
                                                                gameObject.transform.position.z + webTrack.playerSpeed * webTrack.speedReduceAmount);
                // Normal
                else
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                                gameObject.transform.position.y,
                                                                gameObject.transform.position.z + webTrack.playerSpeed);

                // Player boosting
                if (webTrack.speedBoost)
                {
                    boostTime += Time.deltaTime;
                    webTrack.speedIndicator.sprite = webTrack.speedSprites[2];

                    // end boost
                    if (boostTime >= webTrack.speedBoostTime)
                    {
                        webTrack.speedBoost = false;
                        boostTime = 0f;
                    }
                }

                // Player slowing
                else if (webTrack.speedReduce)
                {
                    slowTime += Time.deltaTime;
                    webTrack.speedIndicator.sprite = webTrack.speedSprites[0];

                    // end slow
                    if (slowTime >= webTrack.speedReduceTime)
                    {
                        webTrack.speedReduce = false;
                        slowTime = 0f;
                    }
                }

                // Player normal Speed indicator
                else
                {
                    webTrack.speedIndicator.sprite = webTrack.speedSprites[1];
                }

                // Spawn Items
                if (goodItemTime >= item.goodItemRate)
                {
                    spawnGoodItem();
                    goodItemTime = 0f;
                }
                if (badItemTime >= item.badItemRate)
                {
                    spawnBadItem();
                    badItemTime = 0f;
                }

                // Spawn enemies
                if (enemyTime >= enemy.spawnRate)
                {
                    spawnEnemy();
                    enemyTime = 0f;
                }

                // Running time code
                startTime += Time.deltaTime;
                enemyTime += Time.deltaTime;
                goodItemTime += Time.deltaTime;
                badItemTime += Time.deltaTime;


                // Send to timer
                webTrack.timer.text = webTrack.remainingTime + " remaining";

                // Calculate time left
                webTrack.timeLeft = webTrack.completionTime - startTime;
                webTrack.remainingTime = clockTime(webTrack.timeLeft);

                // Show Progress
                webTrack.progressBar.value = gameObject.transform.position.z / webTrack.length;
            }
        }
    }

    string clockTime(float timeInSeconds)
    {
        int seconds = (int)(timeInSeconds % 60);
        int minutes = (int)(timeInSeconds / 60);
        int hours = (int)(timeInSeconds / 3600);

        string time = hours + "h:" + minutes + "m:" + seconds + "s time";

        return time;
    }

    void createLanes()
    {
        //item.lanes = new List<GameObject>(item.numLanes);
        for (int i = 0; i < item.numLanes; i++)
        {
            item.lanes[i] = new GameObject();
        }
        /*
        for (int i = 0; i < item.lanes.Count; i++)
        {
            item.lanes[i].transform.position = new Vector3(-gameObject.GetComponent<player>().movement.boundRange, 0, 0);
        }
        */

        // Move Into Position
        item.lanes[0].transform.position = new Vector3(-gameObject.GetComponent<player>().movement.boundRange + 1f, 
                                                        0, enemy.spawnDistance);
        item.lanes[1].transform.position = new Vector3(0, 
                                                        0, enemy.spawnDistance);
        item.lanes[2].transform.position = new Vector3(gameObject.GetComponent<player>().movement.boundRange - 1f, 
                                                        0, enemy.spawnDistance);

        foreach (GameObject g in item.lanes)
        {
            g.transform.SetParent(gameObject.transform);
            g.name = "ItemSpawnZone";
        }
            

    }

    void spawnGoodItem()
    {
        int laneChoice = (int)Random.RandomRange(0, item.numLanes-1);
        GameObject newGoodItem;
        int newItemTypeIndex = (int)Random.Range(0, enemy.types.Count);
        newGoodItem = Instantiate(item.goodTypes[newItemTypeIndex],
                                item.lanes[laneChoice].transform.position,
                                Quaternion.identity);
    }

    void spawnBadItem()
    {
        int laneChoice = (int)Random.RandomRange(0, item.numLanes - 1);
        GameObject newBadItem;
        int newItemTypeIndex = (int)Random.Range(0, item.badTypes.Count);
        newBadItem = Instantiate(item.badTypes[newItemTypeIndex],
                                item.lanes[laneChoice].transform.position,
                                Quaternion.identity);
    }

    public void itemEffects(string effect)
    {
        if (effect == "speedBoost")
        {
            webTrack.speedBoost = true;
        }
        else if (effect == "addTime")
        {
            webTrack.completionTime +=  webTrack.addTimeAmount;
        }
        else if (effect == "speedReduce")
        {
            webTrack.speedReduce = true;
        }
        
    }

    void spawnEnemy()
    {
        // Instantiate an enemy inside the spawnZone
        GameObject newEnemy;
        int newEnemyTypeIndex = (int)Random.Range(0, enemy.types.Count);
        newEnemy = Instantiate(enemy.types[newEnemyTypeIndex], 
                                enemy.spawnZone.transform.position + Random.insideUnitSphere * enemy.spawnRadius, 
                                Quaternion.identity);

        // Add enemy to list
        enemy.list.Add(newEnemy);
    }

    public void timeDamage(float damage)
    {
        webTrack.completionTime -= damage;
    }


}
