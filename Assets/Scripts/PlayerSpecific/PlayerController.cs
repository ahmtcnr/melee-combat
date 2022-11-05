using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour

{
    [SerializeField] private Joystick _joystick;

    [SerializeField] private float _moveSpeed = 6;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private float _punchDistance;
    [SerializeField] private LayerMask _contactLayer;

    private CharacterController _characterController;
    private Animator _animator;

    private MovementStates _movementStates, _previousState = MovementStates.Idle;

    private bool _hasNearEnemy;

    private Vector3 _lookAtTarget;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        //_playerAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();

        if (_hasNearEnemy)
        {
            LookAt(_lookAtTarget);
        }
        else
        {
            LookAt(_joystick.DirectionV3.normalized);
        }
    }


    private void FixedUpdate()
    {
        CheckEnemy();
    }

    private void CheckEnemy()
    {
        var targets = Physics.OverlapSphere(transform.position, _punchDistance, _contactLayer);
        if (targets.Length > 0)
        {
            _lookAtTarget = (targets[0].transform.position - transform.position).normalized;
            _hasNearEnemy = true;
            Debug.Log(targets[0].name);
        }
        else
        {
            _hasNearEnemy = false;
        }
    }


    private void LookAt(Vector3 target)
    {
        transform.forward = Vector3.Lerp(transform.forward, target, _rotateSpeed * Time.deltaTime);
    }

    private void Move()
    {
        _characterController.Move(_joystick.DirectionV3.normalized * (_moveSpeed * Time.deltaTime));


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
                    _animator.SetBool(IsRunning, false);
                    break;
                case MovementStates.Running:
                    _animator.SetBool(IsRunning, true);
                    break;
            }
        }

        _previousState = _movementStates;
    }


    private enum MovementStates
    {
        Running,
        Idle,
    }

    private static readonly int IsRunning = Animator.StringToHash("is_running");
    private static readonly int Idle = Animator.StringToHash("idle");

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, _punchDistance);
    }
}