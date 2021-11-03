using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : Operation {

    public int operation = 0;
    private float timer = 1f;

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
            if (gameObjects.Length == 2)
                Scaling(operation, gameObjects[0], gameObjects[1]);
            else if (gameObjects.Length == 1)
                Scaling(operation, gameObjects[0]);
            else
                Debug.Log("Nothing to Scale");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            timer += Time.deltaTime;
            //Debug.Log(timer);
            if (gameObjects.Length == 2)
                Scaling(operation, gameObjects[0], gameObjects[1]);
            else if (gameObjects.Length == 1)
                Scaling(operation, gameObjects[0]);
            else
                Debug.Log("Nothing to Scale");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        timer = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
    }
}
