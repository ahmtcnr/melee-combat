using System;
using UnityEditor;
using UnityEngine;

public class DummyController : MonoBehaviour, IDamageable
{
    private Transform _playerTransform;

    [SerializeField] private float _lookAtDistance;
    [SerializeField] private float _rotateSpeed;

    private Animator _animator;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        if (Vector3.Distance(_playerTransform.position, transform.position) < _lookAtDistance)
        {
            var dirToPlayer = (_playerTransform.position - transform.position).normalized;
            dirToPlayer.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, dirToPlayer, _rotateSpeed * Time.deltaTime);
        }
    }
    
    public void ReceiveDamage(ReceiveDamageAction receiveDamageAction)
    {
        _animator.SetTrigger(ReceivePunch);
    }

    private readonly int ReceivePunch = Animator.StringToHash("receive_punch");
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _lookAtDistance);
    }

   
}