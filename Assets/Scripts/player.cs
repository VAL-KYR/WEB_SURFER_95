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
        public bool rawInput = false;
        public bool modInput = false;
        public bool gizmos = false;
    }
    public mDebug debug = new mDebug();

    // Player Management
    [System.Serializable]
    public class mPlayer
    {
        public float maxHealth = 100f;
        public float currentHealth;
    }
    public mPlayer playerStats = new mPlayer();

    // Movement Management
    [System.Serializable]
    public class Movement
    {
        public bool smoothSpeed = false;
        public bool lurchSpeed = false;
        public float smoothAccel = 1.8f;
        public float speed = 0.5f;
        public float boundRange = 5f;
    }
    public Movement movement = new Movement();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        // Side to Side Movement 
        if (Input.GetAxis("TouchLThumbstick") != 0)
        {

            if (debug.rawInput)
                Debug.Log("Raw input value: " + Input.GetAxis("TouchLThumbstick"));


            if ((Input.GetAxis("TouchLThumbstick") > 0 && transform.position.x < movement.boundRange))
            {
                transform.position = new Vector3(movement.speed * inputRecurve(Mathf.Abs(Input.GetAxis("TouchLThumbstick"))) + transform.position.x,
                                                transform.position.y,
                                                transform.position.z); 
            }

            if ((Input.GetAxis("TouchLThumbstick") < 0 && transform.position.x > -movement.boundRange))
            {
                transform.position = new Vector3(-movement.speed * inputRecurve(Mathf.Abs(Input.GetAxis("TouchLThumbstick"))) + transform.position.x,
                                                transform.position.y,
                                                transform.position.z);
            }
            
        }
        else if (Input.GetAxis("TouchRThumbstick") != 0)
        {
            if (debug.rawInput)
                Debug.Log("Raw input value: " + Input.GetAxis("TouchRThumbstick"));


            if ((Input.GetAxis("TouchRThumbstick") > 0 && transform.position.x < movement.boundRange))
            {
                transform.position = new Vector3(movement.speed * inputRecurve(Mathf.Abs(Input.GetAxis("TouchRThumbstick"))) + transform.position.x,
                                                transform.position.y,
                                                transform.position.z);
            }

            if ((Input.GetAxis("TouchRThumbstick") < 0 && transform.position.x > -movement.boundRange))
            {
                transform.position = new Vector3(-movement.speed * inputRecurve(Mathf.Abs(Input.GetAxis("TouchRThumbstick"))) + transform.position.x,
                                                transform.position.y,
                                                transform.position.z);
            }
        }


		
	}

    private void OnDrawGizmos()
    {
        if (debug.gizmos)
        {
            // Camera look direction
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(GameObject.Find("CenterEyeAnchor").transform.position, GameObject.Find("CenterEyeAnchor").transform.forward * 100f);
        }
    }

    float inputRecurve(float inputSignal)
    {
        // convert raw input with curve
        float returnSignal;
        if (movement.smoothSpeed)
            returnSignal =  Mathf.Pow(inputSignal, movement.smoothAccel/1.0f);
        else if (movement.lurchSpeed)
            returnSignal = Mathf.Pow(inputSignal, 1.0f / movement.smoothAccel);
        else
            returnSignal = inputSignal;

        if (debug.modInput)
            Debug.Log("Modified input value: " + returnSignal);

        return returnSignal;
    }
}
