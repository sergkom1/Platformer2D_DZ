using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Mover : MonoBehaviour
{
    private const float SPEED_CONF = 50;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    private const string BORDER_TAG = "Border"; // �� �����

    [SerializeField] private float _speedX = 1; //�������� �� X
    [SerializeField] private float _speedY = 1; //�������� �� Y
    [SerializeField] public float acceleration = 500; // ��������� �������
    [SerializeField] public float maxSpeed = 2000; // ������������ �������� �������
    [SerializeField] public float rotationSpeed = 10; // �������� ��������

    public Rigidbody2D _rigidbody;
    private float _directionX;
    private float _directionY;
    private bool _isBoost;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _directionX = Input.GetAxis(HORIZONTAL_AXIS);
        _directionY = Input.GetAxis(VERTICAL_AXIS);

        if (Input.GetKeyDown(KeyCode.Space)) //�������� ������� ���� 
        {
            _isBoost = true;
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_speedX * _directionX * SPEED_CONF * Time.fixedDeltaTime, _speedY * _directionY * SPEED_CONF * Time.fixedDeltaTime);
        Vector2 direction = new Vector2(_directionX, _directionY);
        // ������� ������� � ������� ��������
        if (direction != Vector2.zero)
        {
            // ��������� ���� �������� � ��������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ������������ ������ ������
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90 ����� "������" �������� �����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isBoost)
        {

            // �������� ������� ����������� �������� �������
            Vector2 directionBoost = _rigidbody.velocity.normalized;

            // ��������� ��������� � ����������� ��������
            _rigidbody.AddForce(directionBoost * acceleration);

            // ������������ �������� �������, ����� �� �� �������� ������������ ��������
            if (_rigidbody.velocity.magnitude > maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
            }

            _isBoost = false;
        }
    }

}
