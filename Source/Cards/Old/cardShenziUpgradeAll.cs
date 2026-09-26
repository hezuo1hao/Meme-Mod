using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.White;
using LBoL.Core.Battle.BattleActions;
using System.Linq;
using LBoL.Core.Cards;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardShenziUpgradeAllDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { Any = 3, White = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1, White = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(ShenziUpgradeAll)];
			config.UpgradedRelativeCards = [nameof(ShenziUpgradeAll) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "阿荣";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardShenziUpgradeAllDef))]
	public sealed class cardShenziUpgradeAll : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			List<Card> list = (from c in Battle.EnumerateAllCards()
							   where c.CanUpgradeAndPositive && c != this
							   select c).ToList();
			if (list.Count > 0)
			{
				yield return new UpgradeCardsAction(list);
			}
			yield break;
		}
	}
}


