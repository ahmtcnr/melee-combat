using System;
using UnityEditor;
using UnityEngine;


public class PlayerCombat
{
    [System.Serializable]
    public struct CombatSettings
    {
        public float punchDistance;
        public float punchCoolDown;
        public LayerMask contactLayer;
    }

    private CombatSettings _combatSettings;
    private Transform _transform;
    private Animator _animator;

    private bool _punchHandSwitch;

    public PlayerCombat(Transform transform, CombatSettings combatSettings)
    {
        this._combatSettings = combatSettings;
        this._transform = transform;
        this._animator = transform.GetComponent<Animator>();
    }


    public Collider[] CheckCollision()
    {
        return Physics.OverlapSphere(_transform.position, _combatSettings.punchDistance, _combatSettings.contactLayer);
    }

    private void RightHandPunch()
    {
        _animator.SetTrigger(PunchRight);
    }

    private void LeftHandPunch()
    {
        _animator.SetTrigger(PunchLeft);
    }

    public void Punch()
    {
        if (_punchHandSwitch)
        {
            RightHandPunch();
        }
        else
        {
            LeftHandPunch();
        }

        _punchHandSwitch = !_punchHandSwitch;
    }

    private readonly int PunchRight = Animator.StringToHash("punch_right");
    private readonly int PunchLeft = Animator.StringToHash("punch_left");
}