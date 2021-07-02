using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 2f;
    [SerializeField] private GameObject _pivot;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _frontFork, _rearFork;

    private bool _ballLaunched;
    private bool _canDetach;
    private bool _canDrag = true;
    private bool _isDragging;
    private Camera _main;
    // private LineRenderer _line;
    private Rigidbody2D _rigidbody2D;
    private SpringJoint2D _springJoint2D;
    private Vector3 _launchPosition;

    private void Start()
    {
        _canDrag = true;
        // _line.enabled = false;
        // _line = GetComponent<LineRenderer>();
        _main = Camera.main;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _springJoint2D = GetComponent<SpringJoint2D>();
        transform.position = _pivot.transform.position;
        // _rigidbody2D.velocity = Vector3.zero;
        // _rigidbody2D.angularVelocity = 0;
        // _rigidbody2D.rotation = 0;
        _rigidbody2D.isKinematic = true;
    }

    private void Update()
    {
        ShowLine();
        if (_rigidbody2D == null) return;
        DetectTouch();

        if (_canDetach)
        {
            DetachBall();
        }
    }

    private void ShowLine()
    {
        if (_ballLaunched)
        {
            _line.SetPosition(1, _rearFork.position);
        }
        else
        {
            _line.SetPosition(1, transform.position);
        }
        _line.SetPosition(0, _frontFork.position);
        _line.SetPosition(2, _rearFork.position);

    }

    private void DetectTouch()
    {
        // if (!Touchscreen.current.primaryTouch.press.isPressed)
        if(Touch.activeTouches.Count == 0)
        {
            if (_isDragging)
            {
                LaunchBall();
            }
            _isDragging = false;
            return;
        }

        if (_canDrag)
        {
            _isDragging = true;
            _rigidbody2D.isKinematic = true;

            Vector2 touchPosition = new Vector2();
            foreach(var touch in Touch.activeTouches)
            {
                touchPosition += touch.screenPosition;
            }
            touchPosition /= Touch.activeTouches.Count;
            
            // Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector3 worldPosition = _main.ScreenToWorldPoint(touchPosition);
            worldPosition.z = transform.position.z;

            transform.position = worldPosition;
        }
    }

    private void LaunchBall()
    {
        _launchPosition = transform.position;
        Debug.Log($"LaunchPosition: {_launchPosition.x}, Pivot: {_pivot.transform.position.x - (_launchPosition.x / 2)} ");
        _rigidbody2D.isKinematic = false;
        _canDetach = true;
        _canDrag = false;
        // _ballLaunched = true;
    }

    private void DetachBall()
    {
        float distance = _launchPosition.x - _pivot.transform.position.x;
        if (transform.position.x > _pivot.transform.position.x + Math.Abs(distance / 4))
        {
            _ballLaunched = true;
            _springJoint2D.enabled = false;
            _canDetach = false;
            StartCoroutine(ResetBallRoutine());
        }
        else
        {
            StartCoroutine(ResetBallRoutine());
        }
    }

    IEnumerator ResetBallRoutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        _ballLaunched = false;
        _canDrag = true;
        transform.position = _pivot.transform.position;
        _springJoint2D.enabled = true;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.rotation = 0;

    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
}
