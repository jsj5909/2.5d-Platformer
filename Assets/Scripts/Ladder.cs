using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private Player _player; 

    [SerializeField] private Vector3 _climbPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Debug.LogError("Player is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag.ToString());
       
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("On Ladder");
            _player.CanClimbLadder(_climbPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        

        if(other.tag == "Player")
        {
            Debug.Log("Off Ladder");
            _player.CanClimbLadder(Vector3.zero);
        }
        
    }
}
