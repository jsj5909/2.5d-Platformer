using System.Collections;
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
    private float _yVelocity;
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

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

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
        float horizontalInput = Input.GetAxis("Horizontal");
        
        if (_controller.isGrounded == true)
        {
            _canWallJump = false;
            _direction = new Vector3(horizontalInput, 0, 0);
            _velocity = _direction * _speed;



            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = _jumpHeight;
                _canDoubleJump = true;
            }
        }
        else
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_canDoubleJump == true && _canWallJump != true)
                {
                    _yVelocity += _jumpHeight;
                    _canDoubleJump = false;
                }
                if(_canWallJump == true)
                {
                    _velocity = _hitInfo.normal * _speed;
                    _yVelocity = _jumpHeight;
                }
            }

            _yVelocity -= _gravity;
        }

        _velocity.y = _yVelocity;

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
}
