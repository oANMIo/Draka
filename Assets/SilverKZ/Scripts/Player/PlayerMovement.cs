using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 4f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _moveInput;
    private PlayerAttack _playerAttack;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        //if (_playerAttack.IsAttack) return;

        MoveInput();
        Flip();
        SetAnimation();
    }

    private void FixedUpdate()
    {
        //if (_playerAttack.IsAttack) return;

        _rb.MovePosition(_rb.position + _moveInput.normalized * _moveSpeed * Time.fixedDeltaTime);
    }

    private void SetAnimation()
    {
        _animator.SetFloat("Speed", _moveInput.sqrMagnitude);
    }

    private void MoveInput()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void Flip()
    {
        if (_moveInput.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(_moveInput.x), 1, 1);
        }
    }
}
