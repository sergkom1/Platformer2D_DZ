using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Mover : MonoBehaviour
{
    private const float SPEED_CONF = 50;
    private const string HORIZONTAL_AXIS = "Horizontal";
    private const string VERTICAL_AXIS = "Vertical";
    private const string BORDER_TAG = "Border"; // не нужен

    [SerializeField] private float _speedX = 1; //скорость по X
    [SerializeField] private float _speedY = 1; //скорость по Y
    [SerializeField] public float acceleration = 500; // Ускорение объекта
    [SerializeField] public float maxSpeed = 2000; // Максимальная скорость объекта
    [SerializeField] public float rotationSpeed = 10; // Скорость поворота

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

        if (Input.GetKeyDown(KeyCode.Space)) //нажимаем разовый буст 
        {
            _isBoost = true;
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_speedX * _directionX * SPEED_CONF * Time.fixedDeltaTime, _speedY * _directionY * SPEED_CONF * Time.fixedDeltaTime);
        Vector2 direction = new Vector2(_directionX, _directionY);
        // Поворот объекта в сторону движения
        if (direction != Vector2.zero)
        {
            // Вычисляем угол поворота в радианах
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Поворачиваем объект плавно
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90 чтобы "голова" смотрела вперёд
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isBoost)
        {

            // Получаем текущее направление движения объекта
            Vector2 directionBoost = _rigidbody.velocity.normalized;

            // Применяем ускорение в направлении движения
            _rigidbody.AddForce(directionBoost * acceleration);

            // Ограничиваем скорость объекта, чтобы он не превышал максимальную скорость
            if (_rigidbody.velocity.magnitude > maxSpeed)
            {
                _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
            }

            _isBoost = false;
        }
    }

}
