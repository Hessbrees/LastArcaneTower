using Assets.Scripts.SpellS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCooldownLogic
{
    private float cooldownTime;
    private float nextSpellThrowTime;

    public bool CooldownActive { get; private set; } = false;
    public float CurrentCooldown => nextSpellThrowTime - Time.time;
    public SpellCooldownLogic(float cooldownTime) => this.cooldownTime = cooldownTime;

    public void ProcessCooldown()
    {
        if (CooldownActive)
            return;

        nextSpellThrowTime = Time.time + cooldownTime;
        ToggleCooldown(true);
    }

    public void CheckCooldownTime()
    {
        if (Time.time > nextSpellThrowTime)
        {
           ToggleCooldown(false);
        }
    }

    private void ToggleCooldown(bool active) =>  CooldownActive = active;
}
