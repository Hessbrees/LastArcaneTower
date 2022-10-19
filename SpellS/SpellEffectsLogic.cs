using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SpellS
{
    public class SpellEffectsLogic
    {
        private SpellStats_SO spellSO;
        private List<ISpellEffect> spellEffects;
        private SpellUpgrades_SO spellUpgrades_SO;
        
        public SpellEffectsLogic(SpellStats_SO spellSO, SpellUpgrades_SO spellUpgrades_SO)
        {
            this.spellSO = spellSO;
            this.spellEffects = new List<ISpellEffect>();
            this.spellUpgrades_SO = spellUpgrades_SO;

            SetSpellEffects();
        }

        public void TriggerSpellEffects(Collision2D collision, SpellTypes spellType, Vector2 position)
        {
            var enemyControl = collision.transform.GetComponent<EnemyHPMPControl>();

            if (CheckIfCollisionIsEnemy(collision))
            {
                foreach (var spellEffect in this.spellEffects)
                {
                    if (spellEffect.SpellEffect == SpellEffects.DOT && enemyControl.IsDOT_Active)
                    {
                        continue;
                    }
                    spellEffect.TriggerSpellEffect(collision, position, spellType);
                }
            }
        }

        public void TriggerSpellEffects(Collider2D collision, SpellTypes spellType, Vector2 position)
        {
            var enemyControl = collision.transform.GetComponent<EnemyHPMPControl>();

            if (CheckIfCollisionIsEnemy(collision))
            {
                foreach (var spellEffect in this.spellEffects)
                {
                    if (spellEffect.SpellEffect == SpellEffects.DOT && enemyControl.IsDOT_Active)
                    {
                        continue;
                    }

                    spellEffect.TriggerSpellEffect(collision, position, spellType);
                }
            }
        }

        public void SetSpellEffects()
        {
            this.spellEffects.Clear();

            bool AOE = false;
            int radius = 0;

            foreach (var spellUpgrade in this.spellUpgrades_SO.SpellUpgrades)
            {
                int spellEffectValue = spellUpgrade.upgradeValuePerLevel * spellUpgrade.currentUpgradeLevel;
                switch (spellUpgrade.spellUpgradeType)
                {
                    case SpellEffects.Damage:                
                        spellEffects.Add(new SpellEffect_Damage(spellSO.dmg + spellEffectValue));
                        break;
                    case SpellEffects.Knockback:
                        spellEffects.Add(new SpellEffect_Knockback(spellEffectValue));
                        break;
                    case SpellEffects.AOE:
                        AOE = true;
                        radius = spellEffectValue;
                        break;
                    // Hard-coded for now, didnt implement more specific upgrades for DOT
                    case SpellEffects.DOT:
                        spellEffects.Add(new SpellEffect_DOT(spellEffectValue, 1, 5));
                        break;
                    default:
                        break;
                }
            }

            if (AOE)
            {
                spellEffects.Add(new SpellEffect_AOE(radius, this.spellEffects.ToArray()));
            }
        }

        private bool CheckIfCollisionIsEnemy(Collision2D collision)
        {
            return collision.gameObject.layer == LayerMask.NameToLayer(Settings.EnemyLayerName);
        }

        private bool CheckIfCollisionIsEnemy(Collider2D collision)
        {
            return collision.gameObject.layer == LayerMask.NameToLayer(Settings.EnemyLayerName);
        }
    }
}
