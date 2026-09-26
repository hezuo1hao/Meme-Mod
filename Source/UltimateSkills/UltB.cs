using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.GunName;
using LBoL.Core.StatusEffects;

namespace lvalonmeme.SampleCharacterUlt
{
    public sealed class SampleCharacterUltBDef : lvalonmemeulttemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 35;
            config.Value1 = 2;

            // Add the relative status effects in the description box.   
            config.RelativeEffects = [nameof(Weak)];
            return config;
        }
    }

    [EntityLogic(typeof(SampleCharacterUltBDef))]
    public sealed class SampleCharacterUltB : UltimateSkill
    {
        public SampleCharacterUltB()
        {
            base.TargetType = TargetType.SingleEnemy;
            base.GunName = GunNameID.GetGunFromId(4158);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            EnemyUnit enemy = selector.GetEnemy(base.Battle);
            yield return new DamageAction(base.Owner, enemy, this.Damage, base.GunName, GunType.Single);
            
            //Only apply the status effect if the enemy is still alive after the attack. 
            if (enemy.IsAlive)
            {
                yield return new ApplyStatusEffectAction<Weak>(enemy, 0, base.Value1, 0, 0, 0.2f);
                yield break;
            }
        }
    }
}
