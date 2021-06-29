using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField] private GameObject _callButton;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                MeshRenderer mr = _callButton.GetComponent<MeshRenderer>();

                mr.material.SetColor("_Color", Color.green);
            }

        }
    }

}
