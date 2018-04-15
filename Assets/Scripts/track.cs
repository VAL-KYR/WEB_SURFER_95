using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class track : MonoBehaviour {

    public float startTime;
    public float enemyTime;
    public float boostTime;
    public float goodItemTime;
    public float badItemTime;
    public float slowTime;

    public List<GameObject> environment = new List<GameObject>();
    public GameObject sunset;

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
        public GameObject playerBody;
        public GameObject winObject;
        public GameObject loseObject;
        public List<Sprite> speedSprites = new List<Sprite>(3);
        public bool started = false;
        public bool goal = false;
        public bool finish = false;
        public float length = 50f;
        public float playerSpeed = 0.05f;
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
        public GameObject goodSound;
        public GameObject badSound;
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
        enemy.spawnZone.transform.position = new Vector3(0, 5f, enemy.spawnDistance);
        enemy.spawnZone.AddComponent<SphereCollider>();
        enemy.spawnZone.GetComponent<SphereCollider>().radius = enemy.spawnRadius;
        enemy.spawnZone.GetComponent<SphereCollider>().isTrigger = true;

        // Create the lanes for items
        createLanes();
    }

    // Update is called once per frame
    void Update()
    {
        // Game start
        if (!webTrack.started && !webTrack.finish)
        {
            // Send to timer
            webTrack.timer.text = "Hold Both PalmTriggers to begin surfing the web!";

            if ((Input.GetAxis("LPalmTrigger") > 0.5f && Input.GetAxis("RPalmTrigger") > 0.5f))
            {
                webTrack.started = true;
            }
        }

        // Game restart
        else if ((Input.GetAxis("LPalmTrigger") > 0.5f && Input.GetAxis("RPalmTrigger") > 0.5f) && webTrack.started && webTrack.finish)
        {
            SceneManager.LoadScene("environment");
        }

        // Track game running
        if (webTrack.started)
        {
           

            // Player Reaches end
            if (gameObject.transform.position.z >= webTrack.length)
            {
                webTrack.goal = true;
            }
            
            // End Sceneario
            if (webTrack.timeLeft <= 0f || webTrack.goal)
            {
                webTrack.finish = true;

                // conditions for win/loss
                if (webTrack.goal)
                {
                    Debug.Log("You won with: " + webTrack.remainingTime);

                    // Send to timer
                    webTrack.timer.text = "You won with: " + webTrack.remainingTime + " left!";

                    sunset.AddComponent<Rigidbody>();
                    sunset.GetComponent<Rigidbody>().useGravity = true;
                    sunset.GetComponent<Rigidbody>().AddForce(new Vector3(Random.RandomRange(5, -5f),
                                                                    Random.RandomRange(5, -5f),
                                                                    Random.RandomRange(5, -5f)));

                    webTrack.winObject.SetActive(true);
                    webTrack.playerBody.SetActive(false);
                }
                else
                {
                    Debug.Log("Player lost");

                    // Send to timer
                    webTrack.timer.text = "Mom used phone, it's super effective! You Lose!";

                    webTrack.loseObject.SetActive(true);
                    webTrack.playerBody.SetActive(false);

                    gameEnd();
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

                // Move Lane item placers
                foreach (GameObject g in item.lanes)
                {
                    g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, enemy.spawnDistance + gameObject.transform.position.z);
                }

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

        // Move Into Position
        item.lanes[0].transform.position = new Vector3(-gameObject.GetComponent<player>().movement.boundRange + 1f, 
                                                        0, enemy.spawnDistance);
        item.lanes[1].transform.position = new Vector3(0, 
                                                        0, enemy.spawnDistance);
        item.lanes[2].transform.position = new Vector3(gameObject.GetComponent<player>().movement.boundRange - 1f, 
                                                        0, enemy.spawnDistance);

        foreach (GameObject g in item.lanes)
        {
            g.name = "ItemSpawnZone";
        }
            

    }

    void spawnGoodItem()
    {
        int laneChoice = (int)Random.RandomRange(0, item.numLanes);
        GameObject newGoodItem;
        int newGoodItemTypeIndex = (int)Random.Range(0, item.goodTypes.Count);
        newGoodItem = Instantiate(item.goodTypes[newGoodItemTypeIndex],
                                item.lanes[laneChoice].transform.position,
                                Quaternion.identity);
    }

    void spawnBadItem()
    {
        int laneChoice = (int)Random.RandomRange(0, item.numLanes);
        GameObject newBadItem;
        int newBadItemTypeIndex = (int)Random.Range(0, item.badTypes.Count);
        newBadItem = Instantiate(item.badTypes[newBadItemTypeIndex],
                                item.lanes[laneChoice].transform.position,
                                Quaternion.identity);
    }

    public void itemEffects(string effect)
    {
        if (effect == "speedBoost")
        {
            item.goodSound.GetComponent<AudioSource>().Play();
            webTrack.speedBoost = true;
        }
        else if (effect == "addTime")
        {
            item.goodSound.GetComponent<AudioSource>().Play();
            webTrack.completionTime +=  webTrack.addTimeAmount;
        }
        else if (effect == "speedReduce")
        {
            item.badSound.GetComponent<AudioSource>().Play();
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
        gameObject.GetComponent<AudioSource>().pitch = Random.RandomRange(0.8f, 1.2f);
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void gameEnd()
    {
        foreach (GameObject g in environment)
        {
            g.AddComponent<Rigidbody>();
            g.GetComponent<Rigidbody>().useGravity = true;
            g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.RandomRange(5,-5f), 
                                                            Random.RandomRange(5, -5f), 
                                                            Random.RandomRange(5, -5f)));
        }
    }


}
