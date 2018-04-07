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
    }
    public mBullet bullet = new mBullet();

    // Gun Management
    [System.Serializable]
    public class mGun
    {
        public GameObject chamber;
        public string oculusFireButton;
        public float rateOfFire = 1.0f;
        public float damage = 10f;
        public int pierceNum = 0;
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
        //firedBullet = Instantiate(bullet.model, gun.chamber.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        firedBullet = Instantiate(bullet.model, gun.chamber.transform.position, transform.rotation);
    }
}
