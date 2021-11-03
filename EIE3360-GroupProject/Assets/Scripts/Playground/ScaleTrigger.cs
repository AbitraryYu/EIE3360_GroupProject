using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTrigger : Operation {

    public bool ScaleUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            sign = ScaleToggle(ScaleUp);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            sign = ScaleToggle(ScaleUp);
    }
}
