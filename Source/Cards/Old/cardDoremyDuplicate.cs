using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using System.Linq;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards.Neutral.Blue;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardDoremyDuplicateDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.UpgradedValue1 = 2;

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Blue = 2 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeCards = [nameof(DoremyDuplicate)];
			config.UpgradedRelativeCards = [nameof(DoremyDuplicate) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "tojorin";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardDoremyDuplicateDef))]
	public sealed class cardDoremyDuplicate : lvalonmemecard.oldcard
	{
		public override Interaction Precondition()
		{
			List<Card> list = (from hand in Battle.HandZone
							   where hand != this && hand.CanBeDuplicated
							   select hand).ToList();
			if (list.Count <= 0)
			{
				return null;
			}
			return new SelectHandInteraction(1, 1, list);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (precondition != null)
			{
				Card card = ((SelectHandInteraction)precondition).SelectedCards[0];
				List<Card> list = [];
				for (int i = 0; i < Value1; i++)
				{
					list.Add(card.CloneBattleCard());
				}
				yield return new AddCardsToHandAction(list, AddCardsType.Normal);
			}
			yield break;
		}
	}
}


