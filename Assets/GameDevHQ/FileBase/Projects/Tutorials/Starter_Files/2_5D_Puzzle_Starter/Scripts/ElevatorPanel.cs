using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPanel : MonoBehaviour
{
    [SerializeField] private GameObject _callButton;

    [SerializeField] private int _coinsRequired = 8;

    private Elevator _elevator;

    private Player _player;

    private bool _elevatorCalled = false;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _elevator = GameObject.Find("ElevatorPlatform").GetComponent<Elevator>();

        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }

        if (_elevator == null)
        {
            Debug.LogError("Elevator is Null");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            MeshRenderer mr = _callButton.GetComponent<MeshRenderer>();
            if (Input.GetKeyDown(KeyCode.E)  && (_player.Coins() >= _coinsRequired))
            {

               if(_elevatorCalled == false)
                {
                    mr.material.color = Color.green;
                    _elevator.CallElevator();
                    _elevatorCalled = true;
                }
               else
                {
                    _elevator.CallElevator();
                }
                
                /*
                if (_elevatorCalled == true)
               {
                    mr.material.SetColor("_BaseColor", Color.green);
                }
                else
                {
                    mr.material.SetColor("_BaseColor", Color.green);
                    _elevatorCalled = true;

                    
                }

                _elevator.CallElevator();
                */
            }

        }
    }

}
