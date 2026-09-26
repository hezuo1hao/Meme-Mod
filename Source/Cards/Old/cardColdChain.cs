using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.Core.Cards;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardColdChainDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Value1 = 1;
			config.UpgradedValue1 = 2;

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Blue = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile;

			config.RelativeKeyword = Keyword.Retain;
			config.UpgradedRelativeKeyword = Keyword.Retain;

			config.RelativeCards = [nameof(ColdChain)];
			config.UpgradedRelativeCards = [nameof(ColdChain) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Unfinished = true;
			config.Illustrator = "阿荣";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardColdChainDef))]
	public sealed class cardColdChain : lvalonmemecard.oldcard
	{
		public override Interaction Precondition()
		{
			IReadOnlyList<Card> drawZoneToShow = Battle.DrawZoneToShow;
			if (drawZoneToShow.Count <= 0)
			{
				return null;
			}
			return new SelectCardInteraction(0, Value1, drawZoneToShow, SelectedCardHandling.DoNothing);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			SelectCardInteraction selectCardInteraction = (SelectCardInteraction)precondition;
			IReadOnlyList<Card> readOnlyList = (selectCardInteraction != null) ? selectCardInteraction.SelectedCards : null;
			if (readOnlyList != null && readOnlyList.Count > 0)
			{
				foreach (Card card in readOnlyList)
				{
					card.IsRetain = true;
					yield return new MoveCardAction(card, CardZone.Hand);
				}
			}
			yield break;
		}
	}
}


