using System.Collections;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private bool _animationDebugMode;
    
    [ShowIf("_animationDebugMode")]
    
    [SerializeField] private PlayerMovement.MovementSettings _movementSettings;
    [SerializeField] private PlayerCombat.CombatSettings _combatSettings;

    private PlayerMovement _playerMovement;
    private PlayerCombat _playerCombat;

    private bool _canHit = true;

    private void Awake()
    {
        _playerMovement = new PlayerMovement(transform, _movementSettings);
        _playerCombat = new PlayerCombat(transform, _combatSettings);
    }

    private void Update()
    {
        _playerMovement.Move();

        _playerMovement.LookAt();
    }


    private void FixedUpdate()
    {
        if (!_canHit) return;

        var collisions = _playerCombat.CheckCollision();

        if (collisions.Length > 0)
        {
            if (collisions[0].TryGetComponent(out IDamageable damageable))
            {
                damageable.ReceiveDamage(ReceiveDamageAction.punch);
            }

            _playerMovement.HardLookAt(collisions[0].transform.position);
            _playerCombat.Punch();
            StartDelayCounter();
        }
    }

    private Coroutine DelayCounter;

    private void StartDelayCounter()
    {
        if (DelayCounter != null)
        {
            StopCoroutine(DelayCounter);
        }

        DelayCounter = StartCoroutine(HitDelay());

        IEnumerator HitDelay()
        {
            _canHit = false;
            yield return new WaitForSeconds(_combatSettings.punchCoolDown);
            _canHit = true;
        }
    }


    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, _combatSettings.punchDistance);
    }
}