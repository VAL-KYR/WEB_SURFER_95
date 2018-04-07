using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour {

    private float fireTime = 0;

    // Debug Management
    [System.Serializable]
    public class mDebug
    {
        public bool gizmos = false;
    }
    public mDebug debug = new mDebug();
    
    // Bullet Management
    [System.Serializable]
    public class mBullet
    {
        public GameObject model;
        public float damage = 10f;
        public int pierceNum = 0;
        public float travelTime = 2.0f;
    }
    public mBullet bullet = new mBullet();

    // Gun Management
    [System.Serializable]
    public class mGun
    {
        public GameObject chamber;
        public string oculusFireButton;
        public float rateOfFire = 1.0f;
        public float travelTime = 2.0f;
    }
    public mGun gun = new mGun();

    

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {


        fireTime += Time.deltaTime;

        if (fireTime >= gun.rateOfFire && Input.GetAxis(gun.oculusFireButton) > 0.5f)
        {
            Fire();
            fireTime = 0;
        }

		
	}

    void Fire()
    {
        GameObject firedBullet;
        firedBullet = Instantiate(bullet.model, gun.chamber.transform.position, gun.chamber.transform.rotation);
        firedBullet.GetComponent<bullet>().ballistics.gunOrigin = gameObject;
        firedBullet.GetComponent<bullet>().ballistics.damage = bullet.damage;
        firedBullet.GetComponent<bullet>().ballistics.pierceNum = bullet.pierceNum;
        firedBullet.GetComponent<bullet>().ballistics.travelTime = bullet.travelTime;
    }

    private void OnDrawGizmos()
    {
        if (debug.gizmos)
        {
            // Camera look direction
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward * 100f);
        }
    }
}
