using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellLogic : MonoBehaviour, ISpell
{
    public event ISpell.SpellResetEvent OnSpellReset;

    protected virtual void InvokeOnSpellReset()
    {
        if (OnSpellReset != null) OnSpellReset();
    }

    public abstract void ResetSpell(float delay);

    public abstract void ThrowSpell(Vector2 position);
}
