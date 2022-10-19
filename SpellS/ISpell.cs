using System.Collections;
using UnityEngine;

public interface ISpell
{
    public delegate void SpellResetEvent();
    public event SpellResetEvent OnSpellReset;

    public void ThrowSpell(Vector2 position);

    public void ResetSpell(float delay);
}
