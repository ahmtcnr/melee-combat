using UnityEngine;

public class PlayerMovement
{
    [System.Serializable]
    public struct MovementSettings
    {
        public float moveSpeed;
        public float rotateSpeed;
    }

    private MovementSettings _movementSettings;
    private CharacterController _characterController;
    private MovementStates _movementStates, _previousState = MovementStates.Idle;
    private Transform _transform;
    private Joystick _joystick;
    private Animator _animator;

    public PlayerMovement(Transform transform, MovementSettings movementSettings)
    {
        this._transform = transform;
        this._characterController = transform.GetComponent<CharacterController>();
        this._joystick = Object.FindObjectOfType<Joystick>();
        this._animator = transform.GetComponent<Animator>();
        this._movementSettings = movementSettings;
    }

    public void Move()
    {
        _characterController.Move(_joystick.DirectionV3.normalized * (_movementSettings.moveSpeed * Time.deltaTime));


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


    public void LookAt()
    {
        if (_joystick.DirectionV3.magnitude > 0)
        {
            LookAt(_joystick.DirectionV3.normalized);
        }
    }

    public void LookAt(Vector3 target)
    {
        _transform.forward = Vector3.Lerp(_transform.forward, target, _movementSettings.rotateSpeed * Time.deltaTime);
    }

    public void HardLookAt(Vector3 target)
    {
        _transform.forward = target - _transform.position;
    }


    private readonly int IsRunning = Animator.StringToHash("is_running");
    private readonly int Idle = Animator.StringToHash("idle");


    private enum MovementStates
    {
        Running,
        Idle,
    }
}