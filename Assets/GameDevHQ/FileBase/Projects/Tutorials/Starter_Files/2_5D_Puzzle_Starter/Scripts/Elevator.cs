using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private bool _goingDown = false;

    [SerializeField] private Transform _origin, _target;

    [SerializeField] private float _speed = 1f;

    private Player _player;

    private bool _elevatorMoving = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>() ;
        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if going down == true
        //go up
        // else
        //go down_
        
            if (_goingDown)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _origin.position, Time.deltaTime * _speed);

            }
        

    }

    public void CallElevator()
    {
        //check current pos
        _goingDown = !_goingDown;

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

}
