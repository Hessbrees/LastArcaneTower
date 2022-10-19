using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader 
{
    public static Player_SO LoadPlayer_SO() => Resources.Load<Player_SO>("Player/Player_SO");

    public static PlayerSprites_SO LoadPlayerSprites_SO() => Resources.Load<PlayerSprites_SO>("Player/PlayerSprites_SO");

    public static Levels_SO LoadLevels_SO() => Resources.Load<Levels_SO>("Levels_SO");

    public static ScenesSettings_SO LoadScenesSettings_SO() => Resources.Load<ScenesSettings_SO>("ScenesSettings_SO");

    public static SpecialSpellsList_SO LoadSpecialSpellsList_SO() => Resources.Load<SpecialSpellsList_SO>("SpecialSpellsList_SO");

    public static PlayerSpells_SO LoadPlayerSpells_SO() => Resources.Load<PlayerSpells_SO>("Player/PlayerSpells_SO");
}
