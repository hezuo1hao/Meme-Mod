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
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Red;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards.Character.Marisa;

namespace lvalonmeme.Cards
{
	public sealed class cardarashidadDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.Value2 = 10;

			config.Cost = new ManaGroup { Any = 5 };
			config.Rarity = Rarity.Rare;

			config.Mana = new ManaGroup { Any = 1 };

			config.Type = CardType.Ability;

			config.Keywords = Keyword.Initial | Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Initial | Keyword.Ethereal;

			config.RelativeKeyword = Keyword.TempMorph;
			config.UpgradedRelativeKeyword = Keyword.TempMorph;

			config.RelativeCards = [nameof(MeihongPower), nameof(HuoliQuankai), nameof(JinziDoppelganger)];
			config.UpgradedRelativeCards = [nameof(cardarashi), nameof(HuoliQuankai), nameof(ReimuSilence), nameof(JinziDoppelganger)];
			config.RelativeEffects = [nameof(sememe), nameof(Grace), nameof(TempFirepower), nameof(TempSpirit)];
			config.UpgradedRelativeEffects = [nameof(sememe), nameof(Grace), nameof(TempFirepower), nameof(TempSpirit)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "我不到啊";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardarashidadDef))]
	public sealed class cardarashidad : lvalonmemecard.memecard
	{
		public ManaGroup Mana0 => new ManaGroup { Any = 0 };
		public override Interaction Precondition()
		{
			List<Card> options =
            [
                !IsUpgraded ? Library.CreateCard<MeihongPower>() : Library.CreateCard<cardarashi>(),
                Library.CreateCard<HuoliQuankai>(),
            ];
			if (IsUpgraded)
			{
				options.Add(Library.CreateCard<ReimuSilence>());
			}
			options.Add(Library.CreateCard<JinziDoppelganger>());
			return new SelectCardInteraction(Value1, Value1, options, SelectedCardHandling.DoNothing);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<searashidad>(Value2);
			SelectCardInteraction selectCardInteraction = (SelectCardInteraction)precondition;
			List<Card> cards = selectCardInteraction.PendingCards.ToList();
			Card card = selectCardInteraction.SelectedCards[0];
			List<Card> remains = cards.Where(c => c != card).ToList();
			card.SetTurnCost(Mana0);
			yield return new AddCardsToHandAction(card);
			foreach (Card card2 in remains)
			{
				card2.SetTurnCost(Mana);
			}
			yield return new AddCardsToDrawZoneAction(remains, DrawZoneTarget.Random);
			yield break;
		}
	}
}


