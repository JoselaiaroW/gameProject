using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{

    private GameObject player;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Capsule");
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>())
        {
            Debug.Log("HAS MUERTO");
            player.GetComponent<Transform>().position = new Vector3(110f,110f,110f);
        } 

    }
}
