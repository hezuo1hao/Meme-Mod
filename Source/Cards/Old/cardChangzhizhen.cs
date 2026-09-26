using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Base.Extensions;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardChangzhizhenDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Reimu";

			config.Value1 = 4;
			config.UpgradedValue1 = 6;

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { White = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeCards = [nameof(Changzhizhen)];
			config.UpgradedRelativeCards = [nameof(Changzhizhen) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Unfinished = true;
			config.Illustrator = "三折塔";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardChangzhizhenDef))]
	public sealed class cardChangzhizhen : lvalonmemecard.oldcard
	{
		public override IEnumerable<BattleAction> OnRetain()
		{
			if (Zone == CardZone.Hand)
			{
				EnemyUnit randomEnemy = Battle.EnemyGroup.Alives.SampleOrDefault(Battle.GameRun.BattleRng);
				if (randomEnemy != null)
				{
					NotifyActivating();
					yield return PerformAction.Effect(randomEnemy, "Changzhi", 0f, "ReimuBoundaryHit", 0f, PerformAction.EffectBehavior.PlayOneShot, 0.3f);
					yield return new DamageAction(Battle.Player, randomEnemy, DamageInfo.Reaction(Value3));
				}
			}
			yield break;
		}
		protected override int BaseValue3 { get; set; } = 4;
		protected override int BaseUpgradedValue3 { get; set; } = 6;
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			BaseValue3 += Value1;
			BaseUpgradedValue3 += Value1;
			yield break;
		}
	}
}


