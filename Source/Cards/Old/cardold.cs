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
using LBoL.Core.Randoms;
using lvalonmeme.Patches;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardoldDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 3;
			config.UpgradedValue1 = 5;
			config.Mana = new ManaGroup { Any = 0 };

			config.Colors = [ManaColor.Blue, ManaColor.Green];
			config.Cost = new ManaGroup { Any = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Ethereal;
			config.RelativeKeyword = Keyword.TempMorph | Keyword.Ethereal;
			config.UpgradedRelativeKeyword = Keyword.TempMorph;
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "ノア";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "oldcard");
			return config;
		}
	}

	[EntityLogic(typeof(cardoldDef))]
	public sealed class cardold : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			Card[] cards = Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot, false), Value1, (CardConfig config) => config.Id != Id && CustomGameEventManager.GetList("old").Contains(config.Id));
			MiniSelectCardInteraction interaction = new(cards, false, false, false)
			{
				Source = this
			};
			yield return new InteractionAction(interaction, false);
			Card selectedCard = interaction.SelectedCard;
			selectedCard.SetTurnCost(Mana);
			if (!IsUpgraded)
			{
				selectedCard.IsEthereal = true;
			}
			yield return new AddCardsToHandAction(new Card[]
			{
				selectedCard
			});
			yield break;
		}
	}
}


