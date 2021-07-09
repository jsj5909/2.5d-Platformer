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

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
      /*  
        if (_hangingFromLedge == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("ClimbUp");
            }
        }
      */
        
    }

    private void CalculateMovement()
    {
       
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Debug.Log("Ground: " + _controller.isGrounded);

        if (_controller.isGrounded == true)
        {
            _anim.SetBool("Jumping", false);

            _canWallJump = false;

            _direction = new Vector3(horizontalInput, -_gravity * Time.deltaTime, 0);

            _velocity = _direction * _speed;
           // _velocity.y = -_gravity;
          //  _yVelocity = -_gravity;
            
           

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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
                 _velocity.y = _jumpHeight;
                Debug.Log("Jump From Ground!");
                _anim.SetBool("Jumping", true);
            }

           // Debug.Log("hInput: " + horizontalInput.ToString());
           // _direction = new Vector3(horizontalInput, _yVelocity * Time.deltaTime, 0);

        }
        else
        {

            
           // _velocity = _direction * _speed;

            //_yVelocity -= _gravity * Time.deltaTime;
            _velocity.y -= _gravity * Time.deltaTime;
           

            if (Input.GetKeyDown(KeyCode.Space))
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
                  
                    _velocity.y = _jumpHeight;

                    _facing = new Quaternion();

                    _facing = Quaternion.LookRotation(_hitInfo.normal);

                    transform.rotation = _facing;

                }

                

             }
   
         }




        // _direction = new Vector3(0, -_yVelocity * Time.deltaTime, horizontalInput);


        // if (Input.GetKeyDown(KeyCode.Space))
        //  {
        // Debug.Log("Jump!!!!!!!!!");
        //     _jumping = true;

        //    _anim.SetBool("Jumping", _jumping);
        //    _direction.y += _jumpHeight;

        //  }



        // }
        //else
        //    {
        //       _direction.y -= _gravity* Time.deltaTime;
        //}



        // Vector3 velocity = _direction * _speed;
       // _velocity = _direction * _speed;

        //_velocity.y = _yVelocity;
      //  Debug.Log("Velocity: " + _velocity.ToString());
        _controller.Move(_velocity * Time.deltaTime);
    }

    public void AddCoins()
    {
        _coins++;

        _uiManager.UpdateCoinDisplay(_coins);
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
        
        if (_controller.isGrounded == false && hit.transform.tag == "Wall")
        {
            Debug.DrawRay(hit.point, hit.normal, Color.blue);
            _hitInfo = hit;
            _canWallJump = true;
        }

        if((hit.collider.tag == "MovableBox"))
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null || body.isKinematic)
                return;

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0,0);

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
    }

    public void ClimbUpComplete()
    {
        transform.position = _activeLedge.GetStandPos();
        _anim.SetBool("GrabLedge", false);
        _controller.enabled = true;
    }

}
