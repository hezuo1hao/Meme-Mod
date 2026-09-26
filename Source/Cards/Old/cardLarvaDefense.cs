using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Linq;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.Core.StatusEffects;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.StatusEffects.Others;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardLarvaDefenseDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Value1 = 1;

			config.Value2 = 6;
			config.UpgradedValue2 = 8;

			config.Block = 20;
			config.UpgradedBlock = 24;

			config.Colors = [ManaColor.Green];
			config.Cost = new ManaGroup { Any = 2, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Defense;

			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeCards = [nameof(LarvaDefense), nameof(LarvaFriend)];
			config.UpgradedRelativeCards = [nameof(LarvaDefense) + "+", nameof(LarvaFriend)];
			config.RelativeEffects = [nameof(seold), nameof(Weak), nameof(Vulnerable)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(Weak), nameof(Vulnerable)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "tojorin";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardLarvaDefenseDef))]
	public sealed class cardLarvaDefense : lvalonmemecard.oldcard
	{
		public override bool Triggered
		{
			get
			{
				if (Battle != null)
				{
					return Battle.HandZone.Any((Card card) => card is LarvaFriend && card.Summoned);
				}
				return false;
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			foreach (BattleAction battleAction in DebuffAction<Weak>(Battle.AllAliveEnemies, 0, Value1, 0, 0, true, 0.1f))
			{
				yield return battleAction;
			}
			foreach (BattleAction battleAction in DebuffAction<Vulnerable>(Battle.AllAliveEnemies, 0, Value1, 0, 0, true, 0.1f))
			{
				yield return battleAction;
			}
			yield return DefenseAction(true);
			if (TriggeredAnyhow)
			{
				foreach (BattleAction battleAction in DebuffAction<Poison>(Battle.AllAliveEnemies, Value2, 0, 0, 0, true, 0.1f))
				{
					yield return battleAction;
				}
			}
			yield break;
		}
	}
}


