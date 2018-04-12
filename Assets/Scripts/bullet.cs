using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    // Bullet Management
    [System.Serializable]
    public class mBallistics {
        public GameObject gunOrigin;

        public float airTime = 0f;
        public float damage;
        public int pierceNum;
        public float travelTime;
    }
    public mBallistics ballistics = new mBallistics();

    // Use this for initialization
    void Start () {

        // fire force
        gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 2000f);
    }
	
	// Update is called once per frame
	void Update () {

        ballistics.airTime += Time.deltaTime;

        if (ballistics.airTime > ballistics.travelTime)
        {
            Destroy(gameObject);
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        
    }
}
