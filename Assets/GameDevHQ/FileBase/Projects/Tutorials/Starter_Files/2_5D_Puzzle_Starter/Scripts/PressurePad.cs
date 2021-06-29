using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{

    [SerializeField] MeshRenderer _mr;
    //detect moving box
    //when close to center disable box rigid body or set to kinematic

    //change color of pressure pad to blue

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "MovableBox")
        {
            if(other.transform.position.x < 0.3f)
            {
                other.attachedRigidbody.isKinematic = true;

                _mr.material.color = Color.blue;

               
            }

        }
    }

}
