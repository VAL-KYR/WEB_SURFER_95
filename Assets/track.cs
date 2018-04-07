using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class track : MonoBehaviour {

    public float startTime;
    public float enemyTime;

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
        public bool started = false;
        public bool goal = false;
        public bool finish = false;
        public float length = 50f;
        public float playerSpeed = 0.5f;
        public float completionTime = 1000f;
        public float timeLeft;
        public string remainingTime;
    }
    public mTrack webTrack = new mTrack();

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
        webTrack.finishLine.transform.position = new Vector3(0,0,webTrack.length);
        webTrack.timeLeft = webTrack.completionTime;
        webTrack.remainingTime = clockTime(webTrack.timeLeft);

        // Create a spawnzone for enemies
        enemy.spawnZone = new GameObject();
        enemy.spawnZone.name = "SpawnZone";
        enemy.spawnZone.transform.SetParent(gameObject.transform);
        enemy.spawnZone.transform.position = new Vector3(0, 0, enemy.spawnDistance);
        enemy.spawnZone.AddComponent<SphereCollider>();
        enemy.spawnZone.GetComponent<SphereCollider>().radius = enemy.spawnRadius;
        enemy.spawnZone.GetComponent<SphereCollider>().isTrigger = true;
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
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 
                                                            gameObject.transform.position.y,
                                                            gameObject.transform.position.z + webTrack.playerSpeed);

                // Spawn enemies
                if (enemyTime >= enemy.spawnRate)
                {
                    spawnEnemy();
                    enemyTime = 0f;
                }

                // Running time code
                startTime += Time.deltaTime;
                enemyTime += Time.deltaTime;

                // Calculate time left
                webTrack.timeLeft = webTrack.completionTime - startTime;
                webTrack.remainingTime = clockTime(webTrack.timeLeft);
            }
        }
    }

    string clockTime(float timeInSeconds)
    {
        int seconds = (int)(timeInSeconds % 60);
        int minutes = (int)(timeInSeconds / 60);
        int hours = (int)(timeInSeconds / 3600);

        string time = hours + "h:" + minutes + "m:" + seconds + "s time left";

        return time;
    }

    void spawnEnemy()
    {
        // Instantiate an enemy inside the spawnZone
        GameObject newEnemy;
        int newEnemyTypeIndex = (int)Random.Range(0, enemy.types.Count);
        newEnemy = Instantiate(enemy.types[newEnemyTypeIndex], 
                                enemy.spawnZone.transform.position + Random.insideUnitSphere * enemy.spawnRadius, 
                                Quaternion.identity);
        newEnemy.tag = "enemy";


        // Set Instance script info


        // Add enemy to list
        enemy.list.Add(newEnemy);
    }

    public void timeDamage(float damage)
    {
        webTrack.completionTime -= damage;
    }
}
