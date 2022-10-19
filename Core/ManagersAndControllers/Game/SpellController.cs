using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SpellController : MonoBehaviour
{
    public delegate void projectileSFX();
    public static event projectileSFX projectileSFXEvent;
    public delegate void wallSFX();
    public static event wallSFX wallSFXEvent;
    public delegate void skyDropSFX();
    public static event skyDropSFX skyDropSFXEvent;
    public delegate void specialSFX(int index);
    public static event specialSFX specialSFXEvent;

    private Transform playerSpellSource;

    private PlayerSpells_SO currentPlayerSpells;
    private SpellMonoBehaviour instantiatedProjectileSpell;
    private SpellMonoBehaviour instantiatedSkydropSpell;
    private SpellMonoBehaviour instantiatedWallSpell;
    private SpellMonoBehaviour instantiatedSpecialSpell;
    private void Awake()
    {
        currentPlayerSpells = ResourceLoader.LoadPlayerSpells_SO();
        playerSpellSource = PlayerManager.Instance.SpellSource;
        InstantiateBasicSpells();
    }
    private void OnEnable()
    {
        projectileSFXEvent += GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().GetProjectileAudio;
        wallSFXEvent += GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().GetWallAudio;
        skyDropSFXEvent += GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().GetSkyDropAudio;
        specialSFXEvent += GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().GetSpecialAudio;
    }
    public void OnPlayerLoaded() => ToggleSpells(true);

    public void OnPlayerDeath() => ToggleSpells(false);

    public void OnGestureRecognized(string recognizedGesture)
    {
        if (instantiatedSpecialSpell != null)
            Destroy(instantiatedSpecialSpell.gameObject);

        var recognizedSpell = currentPlayerSpells.availableSpecialSpells
            .SingleOrDefault(x => x.GetComponent<SpellMonoBehaviour>().SpellStats_SO.spellName == recognizedGesture);

        if (recognizedSpell == null || recognizedGesture == Settings.GestureErrorMessage)
            return;

        GetSpecialAudio(recognizedSpell);

        instantiatedSpecialSpell = Instantiate(recognizedSpell,
            GameManager.Instance.SpellSpawn.transform.position,
            recognizedSpell.transform.rotation,
            PlayerManager.Instance.transform);

        instantiatedSpecialSpell.gameObject.SetActive(true);

        instantiatedSpecialSpell.ThrowSpecialSpell(recognizedGesture);
    }
    private void GetSpecialAudio(SpellMonoBehaviour recognizedGesture)
    {
        if (recognizedGesture == currentPlayerSpells.availableSpecialSpells[0]) specialSFXEvent(0); // arm left
        if (recognizedGesture == currentPlayerSpells.availableSpecialSpells[1]) specialSFXEvent(1); // arm right
        if (recognizedGesture == currentPlayerSpells.availableSpecialSpells[2]) specialSFXEvent(2); // Vaporize
    }
    public void ToggleSpells(bool spellsActive)
    {
        instantiatedProjectileSpell.gameObject.SetActive(spellsActive);
        instantiatedSkydropSpell.gameObject.SetActive(spellsActive);
        instantiatedWallSpell.gameObject.SetActive(spellsActive);
    }

    public void OnSwipeUp(Vector2 position)
    {
        if (PlayerManager.Instance.SpecialSpellModeActive || instantiatedWallSpell.CooldownActive)
            return;
        wallSFXEvent();
        instantiatedWallSpell.ThrowSpell(position, playerSpellSource.position);
    }

    public void OnSwipeDown(Vector2 position)
    {
        if (PlayerManager.Instance.SpecialSpellModeActive || instantiatedSkydropSpell.CooldownActive)
            return;
        skyDropSFXEvent();
        instantiatedSkydropSpell.ThrowSpell(position, playerSpellSource.position);
    }

    public void OnProjectileSwipe(Vector2 direction)
    {
        if (PlayerManager.Instance.SpecialSpellModeActive || instantiatedProjectileSpell.CooldownActive)
            return;
        projectileSFXEvent();
        instantiatedProjectileSpell.ThrowSpell(direction, playerSpellSource.position);
    }

    private void InstantiateBasicSpells()
    {
        instantiatedProjectileSpell = Instantiate(currentPlayerSpells.projectileSpell,
            GameManager.Instance.SpellSpawn.transform.position,
            currentPlayerSpells.projectileSpell.transform.rotation,
            PlayerManager.Instance.transform);

        instantiatedSkydropSpell = Instantiate(currentPlayerSpells.skydropSpell,
            GameManager.Instance.SpellSpawn.transform.position,
            currentPlayerSpells.skydropSpell.transform.rotation,
            PlayerManager.Instance.transform);

        instantiatedWallSpell = Instantiate(currentPlayerSpells.wallSpell,
            GameManager.Instance.SpellSpawn.transform.position,
            currentPlayerSpells.wallSpell.transform.rotation,
            PlayerManager.Instance.transform);
    }
}
