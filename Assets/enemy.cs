using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {

    public float damage = 10f;
    public float movementSpeed = 3.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //// ENEMIES NEED MORE EFFECTIVE TRACKING / ATTACK MOVEMENT
        // Enemy Travel
        gameObject.transform.position = Vector3.Slerp(transform.position, 
                                        GameObject.FindGameObjectWithTag("Player").transform.position, 
                                        movementSpeed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<track>().timeDamage(damage);
            Destroy(gameObject);
        }
    }
}
