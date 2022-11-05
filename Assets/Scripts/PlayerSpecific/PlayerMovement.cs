using System;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovement : MonoBehaviour

{
    [SerializeField] private Joystick _joystick;

    [SerializeField] private float _moveSpeed = 6;
    [SerializeField] private float _rotateSpeed;


    private CharacterController _characterController;

    private Animator _animator;

    private MovementStates _movementStates, _previousState = MovementStates.Idle;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //_playerAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _characterController.Move(_joystick.DirectionV3.normalized * (_moveSpeed * Time.deltaTime));
        transform.forward = Vector3.Lerp(transform.forward, _joystick.DirectionV3.normalized, _rotateSpeed * Time.deltaTime);

        if (_characterController.velocity.magnitude > 0.01)
        {
            _movementStates = MovementStates.Running;
        }
        else
        {
            _movementStates = MovementStates.Idle;
        }

        if (_previousState != _movementStates)
        {
            switch (_movementStates)
            {
                case MovementStates.Idle:
                    _animator.SetTrigger(RunToStop);
                    _animator.SetBool(IsRunning, false);
                    break;
                case MovementStates.Running:
                    _animator.SetBool(IsRunning, true);
                    break;
            }
        }

        _previousState = _movementStates;
    }

    private void FixedUpdate()
    {
        //_rb.MovePosition(transform.position + _joystick.DirectionV3.normalized * (_moveSpeed * Time.deltaTime));
    }


    private enum MovementStates
    {
        Running,
        Idle,
    }


    private static readonly int IsRunning = Animator.StringToHash("is_running");
    private static readonly int RunToStop = Animator.StringToHash("run_to_stop");
}