using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class SpecialSpell_ArmadilloRight : SpecialSpell_ArmadilloLeft
{
    protected override void Awake()
    {
        base.Awake();
        x_Axis_SprayDirection = 1f;
    }
}
