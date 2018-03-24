using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class player : MonoBehaviour {



    // Debug Management
    [System.Serializable]
    public class mDebug
    {
        public bool gizmos = false;
    }
    public mDebug debug = new mDebug();

    // Movement Management
    [System.Serializable]
    public class Movement
    {
        public float speed = 0.5f;
    }
    public Movement movement = new Movement();




    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // limit movement HERE /////////////////////////////////////////////////
        
        if (Input.GetAxis("Horizontal") != 0)
        {
            Debug.Log(Input.GetAxis("Horizontal"));
            transform.position = new Vector3(movement.speed * Input.GetAxis("Horizontal") + transform.position.x, transform.position.y, transform.position.z);
        }
		
	}
}
