using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardmathexhibitDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.FindInBattle = false;
			config.Owner = null;

			config.Value1 = 3;

			config.Colors = [ManaColor.Red, ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 10, Red = 1, Blue = 1 };
			config.UpgradedCost = new ManaGroup { Any = 4, Red = 1, Blue = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

            config.Keywords = config.UpgradedKeywords = Keyword.Disposable;
            config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "saay_heeey";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardmathexhibitDef))]
	public sealed class cardmathexhibit : lvalonmemecard.memecard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<semathexhibit>();
			yield break;
		}
		public override IEnumerable<BattleAction> AfterUseAction()
		{
			Card deckCardByInstanceId = GameRun.GetDeckCardByInstanceId(InstanceId);
			if (deckCardByInstanceId != null)
			{
				GameRun.RemoveDeckCard(deckCardByInstanceId, triggerVisual: false);
			}
			yield return new RemoveCardAction(this);
		}
		public override IEnumerable<BattleAction> AfterFollowPlayAction()
		{
			Card deckCardByInstanceId = GameRun.GetDeckCardByInstanceId(InstanceId);
			if (deckCardByInstanceId != null)
			{
				GameRun.RemoveDeckCard(deckCardByInstanceId, triggerVisual: false);
			}
			yield return new RemoveCardAction(this);
		}
	}
}


