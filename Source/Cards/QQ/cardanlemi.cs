using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using System.Linq;
using LBoL.Core.Randoms;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardanlemiDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Colors = [ManaColor.Red];
			config.Cost = new ManaGroup { Red = 3 };
			config.UpgradedCost = new ManaGroup { Red = 1 };
			config.Rarity = Rarity.Rare;
			config.Value1 = 3;
			config.Value2 = 5;

			config.Type = CardType.Skill;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "TOP-Exerou";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardanlemiDef))]
	public sealed class cardanlemi : lvalonmemecard.memecard
	{
		protected override int BaseValue3 { get; set; } = 1;
		protected override int BaseUpgradedValue3 { get; set; } = 1;
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			List<Card> list = Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot, false), Value1, (CardConfig config) => config.Cost.Amount == Value2 && config.Type == CardType.Ability).ToList();
			if (list.Count > 0)
			{
				MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(list, false, false, false)
				{
					Source = this
				};
				yield return new InteractionAction(interaction, false);
				Card selectedCard = interaction.SelectedCard;
				yield return new AddCardsToHandAction(new Card[]
				{
					selectedCard
				});
				interaction = null;
			}
			yield break;
		}
	}
}


