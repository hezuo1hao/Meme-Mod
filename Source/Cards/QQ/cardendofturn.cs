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
using LBoL.EntityLib.Cards.Character.Marisa;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.StatusEffects.Others;
using lvalonmeme.Packs;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using System;
using LBoL.Core.StatusEffects;

namespace lvalonmeme.Cards
{
	public sealed class cardendofturnDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.FindInBattle = false;
			config.Owner = null;

			config.Value1 = 1;

			config.Colors = [ManaColor.White, ManaColor.Blue, ManaColor.Black];
			config.Cost = new ManaGroup { Any = 0 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.IsUpgradable = false;

			config.Keywords = Keyword.Forbidden;

			config.RelativeCards = [nameof(HuiyeSuperExtraTurn)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "ev";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardendofturnDef))]
	public sealed class cardendofturn : lvalonmemecard.memecard
	{
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleStarting, OnBattleStarting);
			ReactBattleEvent(Battle.Player.TurnEnding, OnTurnEnding);
		}

		private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
		{
			if (GameRun.BaseDeck.Any(c => c is HuiyeSuperExtraTurn)
			&& !Battle.Player.StatusEffects.Any(s => s is SuperExtraTurn)
			&& Battle.EnumerateAllCardsButPlayingAreas().Any(c => c is HuiyeSuperExtraTurn))
			{
				Card card = Battle.EnumerateAllCardsButPlayingAreas().FirstOrDefault(c => c is HuiyeSuperExtraTurn);
				if (card != null && card is HuiyeSuperExtraTurn)
				{
					yield return new PlayCardAction(card);
				}
				if (Battle.Player.TryGetStatusEffect(out SuperExtraTurn se))
				{
					se.Status = TurnStatus.NaturalTurn;
					se.Limit = 1;
				}
			}
			yield return new RemoveCardAction(this);
		}

		private IEnumerable<BattleAction> OnBattleStarting(GameEventArgs args)
		{
			if (Zone != CardZone.Exile)
			{
				yield return new ExileCardAction(this);
			}
		}
	}
}


