﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _ladderSpeed = 1.0f;
    [SerializeField]
    private float _gravity = 1.0f;
    [SerializeField]
    private float _jumpHeight = 15.0f;
    private float _yVelocity = 0;
    private bool _canDoubleJump = false;
    [SerializeField]
    private int _coins;
    private UIManager _uiManager;
    [SerializeField]
    private int _lives = 3;
    [SerializeField] float _pushSpeed = 2;

    private bool _canWallJump = false;

    private Vector3 _direction, _velocity;

    ControllerColliderHit _hitInfo;

    private bool _hangingFromLedge = false;

    private Ledge _activeLedge;

    private Animator _anim;

    private bool _wallJumping = false;

    private Quaternion _facing = new Quaternion();

    private bool _jumping = false;

    private Vector3 _currentLadderClimbPosition;
    private Vector3 _currentLadderTopPosition;
    private Vector3 _currentLadderClimbDownStart;
    private bool _currentLadderLeftClimb = false;

    private bool _ladderClimb = false;
    private bool _canClimbLadder = false;

    private bool _rolling = false;
    
    
    private bool _finishingLadderClimb = false;
    private bool _atLadderTop = false;

    private float zPos = 0.0f;

    [SerializeField] private GameObject _endLevelCutScene;

    // Start is called before the first frame update
    void Start()
    {
        // Application.targetFrameRate = 60;

        _anim = GetComponentInChildren<Animator>();

        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        _uiManager.UpdateLivesDisplay(_lives);
    }

    /// <summary>
    /// need to move input to update and everything else to fixed update
    /// </summary>

    // Update is called once per frame
    void Update()
    {
        //testing purposes
        if(Input.GetKeyDown(KeyCode.P))
        {
            AddCoins();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && !_jumping)
        {
            _rolling = true;
           

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumping = true;
        }

        if (_hangingFromLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            _controller.enabled = false;

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _controller.enabled = true;
        }

        if (_canClimbLadder == true )
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ClimbLadder();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                BeginClimbDownLadder();
            }
        }
        if (_ladderClimb == true)
        {
            _anim.speed = 0;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                if (!_atLadderTop)
                {
                    _anim.SetBool("LadderClimb", true);
                    _anim.SetBool("LadderClimbDown", false);
                    _anim.speed = 1;
                    _controller.Move(Vector3.up * _ladderSpeed * Time.deltaTime);
                    _anim.SetFloat("Direction", 1);
                }
                else
                {
                    _controller.enabled = false;
                }
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _anim.SetBool("LadderClimb", false);
                _anim.SetBool("LadderClimbDown", true);
                _anim.SetFloat("Direction", -1f);
                _anim.speed = 1;
                _controller.Move(Vector3.down * _ladderSpeed * Time.deltaTime);

                if (_controller.isGrounded)
                {
                    _ladderClimb = false;
                    _anim.SetBool("LadderClimb", false);
                    _anim.SetBool("LadderClimbDown", false);
                }
            }
        }
        
        if(_finishingLadderClimb == true)
        {
            _controller.enabled = false;
        }
        else if(_finishingLadderClimb == false && _hangingFromLedge == false)
        {
            _controller.enabled = true;
        }

        

    }

    private void FixedUpdate()
    {
        CalculateMovement();



    }

    private void CalculateMovement()
    {


        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Debug.Log("Ground: " + _controller.isGrounded);

        if (_controller.isGrounded == true)
        {
            _anim.SetBool("Jumping", false);

            //_yVelocity = 0;

            _canWallJump = false;
            _canDoubleJump = false;

            _direction = new Vector3(horizontalInput, 0, 0);

            _velocity = _direction * _speed;
            _velocity.y += -_gravity;


            if (horizontalInput != 0)
            {
                Vector3 facing = transform.localEulerAngles;

                facing.y = _direction.x < 0 ? 270 : 90;

                transform.localEulerAngles = facing;

                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            }
            else
            {
                _anim.SetFloat("Speed", Mathf.Abs(horizontalInput));
            }

            if (_jumping)
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
                _velocity.y = _jumpHeight;
                Debug.Log("Jump From Ground!");
                _anim.SetBool("Jumping", true);
                _jumping = false;
            }

            if(_rolling)
            {
                _anim.SetTrigger("Roll");
                _rolling = false;

                _controller.enabled = false;

            }

            // Debug.Log("hInput: " + horizontalInput.ToString());
            // _direction = new Vector3(horizontalInput, _yVelocity * Time.deltaTime, 0);
            //_velocity = _direction * _speed;

        }
        else
        {
            if (_ladderClimb == true ) return;
            _velocity.y -= _gravity * Time.deltaTime;

            _anim.SetFloat("Speed", 0);

            if (_jumping)
            {
                if (_canDoubleJump == true && _canWallJump != true)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                    _velocity = _direction * _speed;
                    _velocity.y = _yVelocity;


                }
                if (_canWallJump == true)
                {
                    _velocity = _hitInfo.normal * _speed;

                    _velocity.y += _jumpHeight;

                    _velocity.z = 0.0f;


                    Vector3 facing = transform.localEulerAngles;

                    facing.y = _hitInfo.normal.x < 0 ? 270 : 90;

                    transform.localEulerAngles = facing;

                }
                _jumping = false;

            }

        }
        if (_controller.enabled)
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
    }

    public void AddCoins()
    {
        _coins++;

        _uiManager.UpdateCoinDisplay(_coins);

        if(_coins == 15)
        {
            _endLevelCutScene.SetActive(true);
        }
    }

    public void Damage()
    {
        _lives--;

        _uiManager.UpdateLivesDisplay(_lives);

        if (_lives < 1)
        {
            SceneManager.LoadScene(0);
        }
    }

    public int Coins()
    {
        return _coins;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.DrawRay(hit.point, hit.normal, Color.blue, 1f);
        if (_controller.isGrounded == false && hit.transform.tag == "Wall")
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue, 2f);
            _hitInfo = hit;
            _canWallJump = true;
        }

        if ((hit.collider.tag == "MovableBox"))
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, 0);

            body.velocity = pushDir * _pushSpeed;

        }

    }

    public void GrabLedge(Vector3 handPosition, Ledge currentLedge)
    {
        _controller.enabled = false;

        _anim.SetBool("GrabLedge", true);

        _anim.SetFloat("Speed", 0.0f);
        _anim.SetBool("Jumping", false);

        transform.position = handPosition;

        _hangingFromLedge = true;

        _activeLedge = currentLedge;

        //_controller.enabled = true;
    }

    public void CanClimbLadder(Vector3 ladderPosition, Vector3 finishClimbPosition, Vector3 climbDownStart, bool direction)
    {
        _canClimbLadder = !_canClimbLadder;

        _currentLadderClimbPosition = ladderPosition;
        _currentLadderTopPosition = finishClimbPosition;
        _currentLadderClimbDownStart = climbDownStart;
        _currentLadderLeftClimb = direction;


        Debug.Log("ladderTop: " + _currentLadderTopPosition.ToString());
    }

    public void ClimbLadder()
    {
        _controller.enabled = false;

        transform.position = _currentLadderClimbPosition;
        _anim.SetBool("LadderClimb", true);
        _ladderClimb = true;
        _canClimbLadder = false;
        Debug.Log("CLimbing Ladder");


        if (_currentLadderLeftClimb == true)
        {
            if(transform.rotation.eulerAngles.y != -90)
                transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else
        {
            if (transform.rotation.eulerAngles.y != 90)
                transform.rotation = Quaternion.LookRotation(Vector3.right);
        }


        _controller.enabled = true;
        //transform  = -2.34,0,0
    }

    private void BeginClimbDownLadder()
    {
        _controller.enabled = false;

        transform.position = _currentLadderClimbDownStart;
        _anim.SetBool("LadderClimb", true);
        _ladderClimb = true;
        _canClimbLadder = false;


        if (_currentLadderLeftClimb == true)
        {
            if (transform.rotation.eulerAngles.y != -90)
                transform.rotation = Quaternion.LookRotation(Vector3.left);
        }
        else
        {
            if (transform.rotation.eulerAngles.y != 90)
                transform.rotation = Quaternion.LookRotation(Vector3.right);
        }


        _controller.enabled = true;

    }

    public void LadderClimbUpComplete()
    {
       
        
        if(_anim.GetBool("LadderClimb") == true)
        {
            _anim.SetBool("LadderClimb", false);
            _anim.SetTrigger("FinishClimbToTop");
            _anim.speed = 1;
            _ladderClimb = false;
            _finishingLadderClimb = true;

           
        }
        else
        {
            Debug.Log("Ladder climb false at top of ladder");
            //can climb ladder = true
            _atLadderTop = true;
            _canClimbLadder = true;
            //BeginClimbDownLadder();
        }
    }

    public void SnapToLadderTop()
    {
        _controller.enabled = false;
        Debug.Log("Ladder top: " + _currentLadderTopPosition.ToString());
        transform.position = _currentLadderTopPosition;
        _controller.enabled = true;

        _finishingLadderClimb = false;
        Debug.Log("Player Snapped");
    }

    public void ClimbUpComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _anim.SetBool("GrabLedge", false);
        _controller.enabled = true;
        _hangingFromLedge = false;
    }

    public bool GetLadderClimb()
    {
        return _ladderClimb;
    }

    public void SetLadderClimb(bool value)
    {
        _ladderClimb = value;
    }

    public bool GetAtLadderTop()
    {
        return _atLadderTop;
    }

    public void SetAtLadderTop(bool value)
    {
        _atLadderTop = value;
    }

    public void FinishRoll()
    {

        //_controller.enabled = true;

        //_rolling = false;
         _controller.enabled = false;

       // Vector3 facing = transform.localEulerAngles;

        Quaternion direction = Quaternion.LookRotation(Vector3.right);

       
        
        if(transform.rotation == direction)
        {
            transform.position = new Vector3(transform.position.x + 5,
              transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 5,
                transform.position.y, transform.position.z);
        }
       
        
        _controller.enabled = true;


    }
}
