using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{

    CharacterController _controller;

    [SerializeField] private Vector3 _handPosition;

    [SerializeField] private Vector3 _standPosition;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LedgeGrabChecker"))
        {
            Player player = other.transform.parent.GetComponent<Player>();

            if (player != null)
            {
                player.GrabLedge(_handPosition, this);
            }
        }
    }

    public Vector3 GetStandPos()
    {
        return _standPosition;
    }

}
