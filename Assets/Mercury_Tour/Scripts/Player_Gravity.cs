using UnityEngine;

public class Player_Gravity : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 _velocity;
    private const float _gravity = -9.81f;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
