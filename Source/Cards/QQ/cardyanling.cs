using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using System;
using LBoL.Core.Units;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardyanlingDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;
			config.GunName = "FrozenOrb";

			config.Colors = [ManaColor.Blue, ManaColor.Green];
			config.Cost = new ManaGroup { Any = 1, Blue = 1, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Damage = 0;

			config.Type = CardType.Attack;
			config.TargetType = TargetType.SingleEnemy;

			config.Keywords = Keyword.Exile;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "阿部司";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardyanlingDef))]
	public sealed class cardyanling : lvalonmemecard.memecard
	{
		protected override string GetBaseDescription()
		{
			if (GameRun != null)
			{
				return base.ExtraDescription1;
			}
			else
			{
				return base.GetBaseDescription();
			}
		}
		public override int AdditionalDamage
		{
			get
			{
				if (GameRun != null)
				{
					return Convert.ToInt32(Math.Round(333 * (1 - Math.Exp(-0.01f * GameRun.ReloadTimes)), MidpointRounding.AwayFromZero));
				}
				else
				{
					return 0;
				}
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			EnemyUnit target = selector.GetEnemy(Battle);
			yield return AttackAction(target);
			yield break;
		}
	}
}