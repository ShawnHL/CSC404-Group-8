using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed;
    public float gravity;
    
    private float _horizontalInput;
    private float _rotationInput;
    
    // Jumping
    private bool _jumpInput;
    private bool _hasPressedJump;
    private bool _releasedJump;
    
    // The player metadata
    private Rigidbody _rigidbody;
    private Transform _transform;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _transform = gameObject.GetComponent<Transform>();

        _hasPressedJump = false;
        _releasedJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");
        _jumpInput = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        ChangePlayerDirection();
        if (_jumpInput)
        {
            _hasPressedJump = true;
        }

        if (_hasPressedJump && !_jumpInput)
        {
            _releasedJump = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _releasedJump = false;
            _hasPressedJump = false;
        }
    }

    void ChangePlayerDirection()
    {
        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y -= gravity * Time.deltaTime;
        _rigidbody.velocity = newVelocity + transform.forward * _horizontalInput * movementSpeed;
        
        Vector3 userRotation = transform.rotation.eulerAngles;
        userRotation += new Vector3(0, _rotationInput, 0);
        _transform.rotation = Quaternion.Euler(userRotation);

        if (_jumpInput && !_releasedJump)
        {
            _rigidbody.AddForce(Vector3.up, ForceMode.VelocityChange);
        }

    }
}
