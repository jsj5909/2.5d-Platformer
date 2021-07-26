using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{ 
    private Player _player; 

    [SerializeField] private Vector3 _climbPosition;
    [SerializeField] private Vector3 _finishClimbPosition;
    [SerializeField] private Vector3 _startClimbDownPosition;

    [SerializeField] private bool _topOfLadder;
    
    [SerializeField] private Animator _anim;

    [SerializeField] private bool _leftLadder = true;

    

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
            if (_topOfLadder == false)
            {
               
                    Debug.Log("On Ladder");
                    _player.CanClimbLadder(_climbPosition,_finishClimbPosition, _startClimbDownPosition, _leftLadder);
                
            }
            else  //top of ladder actions
            {
                if (_anim.GetBool("LadderClimb") == true)
                {
                    _player.LadderClimbUpComplete();
                }
                else
                {
                    _player.CanClimbLadder(_climbPosition, _finishClimbPosition, _startClimbDownPosition, _leftLadder);
                    
                }




            }

            //    Debug.Log("On Ladder");
           // _player.CanClimbLadder(_climbPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        

        if(other.tag == "Player")
        {
            Debug.Log("Off Ladder");
            _player.CanClimbLadder(_climbPosition,_finishClimbPosition,_startClimbDownPosition,_leftLadder);
            _player.SetAtLadderTop(false);
        }
        
    }
}


//at top
//  if climbing up
// trigger ladder exit
// else
// allow climb down and normal animation sequence

//at bottom
//if climbing down
//stop animation at bottom and allow moveoff
//else 
//allow climb up and normal animation sequence