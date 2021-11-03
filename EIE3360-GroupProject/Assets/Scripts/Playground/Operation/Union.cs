using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Union : Operation {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            BoolOperation(gameObjects[0], gameObjects[1], 1);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            BoolOperation(gameObjects[0], gameObjects[1], 1);
    }
}
