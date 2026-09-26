using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.Units;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardDeepFreezeDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.TargetType = TargetType.AllEnemies;

			config.Damage = 10;
			config.UpgradedDamage = 18;

			config.GunName = "冰尖之舞";
			config.GunNameBurst = "冰尖之舞B";

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 1, Blue = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1, Blue = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Attack;

			config.RelativeCards = [nameof(DeepFreeze)];
			config.UpgradedRelativeCards = [nameof(DeepFreeze) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(Cold), nameof(seDeepFreeze)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(Cold), nameof(seDeepFreeze)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "四動自半";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardDeepFreezeDef))]
	public sealed class cardDeepFreeze : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
			{
				if (Battle.BattleShouldEnd)
				{
					yield break;
				}
				if (!enemy.HasStatusEffect<Cold>())
				{
					yield return DebuffAction<Cold>(enemy, 0, 0, 0, 0, true, 0.1f);
				}
				else
				{
					if (enemy.HasStatusEffect<seDeepFreeze>())
					{
						yield return DebuffAction<Cold>(enemy, 0, 0, 0, 0, true, 0.1f);
					}
					else
					{
						yield return DebuffAction<seDeepFreeze>(enemy, 0, 0, 0, 0, true, 0.1f);
					}
				}
			}
			yield break;
		}
	}
}


