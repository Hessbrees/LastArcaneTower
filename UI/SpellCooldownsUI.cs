using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownsUI : MonoBehaviour
{
    private Image projectileSpellImage;
    private Image wallSpellImage;
    private Image skyDropSpellImage;
    private Image specialSpellImage;

    private Image projectileSpellParentImage;
    private Image wallSpellParentImage;
    private Image skyDropSpellParentImage;
    private Image specialSpellParentImage;

    private GameObject projectileSpellObj;
    private GameObject wallSpellObj;
    private GameObject skyDropSpellObj;
    private GameObject specialSpellObj;

    private GameObject projectileSpellParentObj;
    private GameObject wallSpellParentObj;
    private GameObject skyDropSpellParentObj;
    private GameObject specialSpellParentObj;
    private void Start()
    {
        // Repetetive code, this can be optimized

        projectileSpellObj = transform.Find("ProjectileSpellCD").Find("CDBar").gameObject;
        wallSpellObj = transform.Find("WallSpellCD").Find("CDBar").gameObject;
        skyDropSpellObj = transform.Find("BigSpellCD").Find("CDBar").gameObject;
        specialSpellObj = transform.Find("SpecialSpellCD").Find("CDBar").gameObject;

        projectileSpellParentObj = transform.Find("ProjectileSpellCD").gameObject;
        wallSpellParentObj = transform.Find("WallSpellCD").gameObject;
        skyDropSpellParentObj = transform.Find("BigSpellCD").gameObject;
        specialSpellParentObj = transform.Find("SpecialSpellCD").gameObject;

        projectileSpellImage = projectileSpellObj.GetComponent<Image>();
        wallSpellImage = wallSpellObj.GetComponent<Image>();
        skyDropSpellImage = skyDropSpellObj.GetComponent<Image>();
        specialSpellImage = specialSpellObj.GetComponent<Image>();
    }

    public void GetProjectileSpellsCD(float projectileSpellCD, float maxprojectileSpellCD)
    {
        if (projectileSpellCD > 0)
        {
            if (!projectileSpellParentObj.activeInHierarchy) projectileSpellParentObj.SetActive(true);
            projectileSpellImage.fillAmount = calculateCD(projectileSpellCD, maxprojectileSpellCD);
        }
        else projectileSpellParentObj.SetActive(false);
    }

    public void GetpWallSpellsCD(float wallSpellCD, float maxWallSpellCD)
    {
        if (wallSpellCD > 0)
        {
            if (!wallSpellParentObj.activeInHierarchy) wallSpellParentObj.SetActive(true);
            wallSpellImage.fillAmount = calculateCD(wallSpellCD, maxWallSpellCD);
        }
        else wallSpellParentObj.SetActive(false);
    }

    public void GetSkyDropSpellsCD(float skyDropSpellCD, float maxSkyDropSpellCD)
    {
        if (skyDropSpellCD > 0)
        {
            if (!skyDropSpellParentObj.activeInHierarchy) skyDropSpellParentObj.SetActive(true);
            skyDropSpellImage.fillAmount = calculateCD(skyDropSpellCD, maxSkyDropSpellCD);
        }
        else skyDropSpellParentObj.SetActive(false);
    }

    public void GetSpecialSpellsCD(float specialSpellCD, float maxSpecialSpellCD)
    {
        if (specialSpellCD > 0)
        {
            if (!specialSpellParentObj.activeInHierarchy) specialSpellParentObj.SetActive(true);
            specialSpellImage.fillAmount = calculateCD(specialSpellCD, maxSpecialSpellCD);
        }
        else specialSpellParentObj.SetActive(false);
    }

    private float calculateCD(float spellCD, float maxSpellCD) => spellCD / maxSpellCD;

}
