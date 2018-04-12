using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("board"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<track>().itemEffects(gameObject.tag);
            Destroy(gameObject);
        }
    }
}
