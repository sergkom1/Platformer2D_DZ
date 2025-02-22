using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Mover : MonoBehaviour
{
    private const float SPEED_CONF = 150;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    private const string BORDER_TAG = "Border";

    [SerializeField] private float _speedX = 1; //движение по X
    [SerializeField] private float _speedY = 1; //движение по Y
    [SerializeField] private float _boostForce = 1; //Длинна буста

    public Rigidbody2D _rigidbody;
    private float _direction;
    private bool _isBoost;
    private bool _isGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _direction = Input.GetAxis(HORIZONTAL_AXIS);

        if (_isGround && Input.GetKeyDown(KeyCode.W))
        {
            _isBoost = true;
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_speedX * _direction * SPEED_CONF * Time.fixedDeltaTime, _rigidbody.velocity.y);
        if (_isBoost)
        {
            _rigidbody.AddForce(new Vector2(0, _boostForce));
            _isBoost = false;
            _isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(BORDER_TAG))
            _isGround = true;
    }
}
