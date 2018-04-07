﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    // Bullet Management
    [System.Serializable]
    public class mBallistics {
        public GameObject gunOrigin;
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
        
	}

    private void OnTriggerEnter(Collider other)
    {


        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
