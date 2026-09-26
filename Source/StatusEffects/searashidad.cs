using System.Collections.Generic;
using System.Linq;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using lvalonmeme.Cards;
using lvalonmeme.Packs;
using lvalonmeme.Patches;

namespace lvalonmeme.StatusEffects
{
	public sealed class searashidadDef : lvalonmemesetemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.IsStackable = false;
			config.HasCount = true;
			config.RelativeEffects = [nameof(Grace), nameof(TempFirepower), nameof(TempSpirit)];
			return config;
		}
	}

	[EntityLogic(typeof(searashidadDef))]
	public sealed class searashidad : StatusEffect
	{
		List<Card> thecards = [];
		protected override void OnAdded(Unit unit)
		{
			Count = 0;
			thecards = [];
			HandleOwnerEvent(Battle.CardsAddingToHand, OnAddCard1);
			HandleOwnerEvent(Battle.CardsAddingToDrawZone, OnAddCard2);
			ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
			ReactOwnerEvent(Battle.CardPlayed, OnCardUsed);
			ReactOwnerEvent(Battle.Player.TurnEnded, OnTurnEnded);
		}

		private IEnumerable<BattleAction> OnTurnEnded(UnitEventArgs args)
		{
			yield return new RemoveStatusEffectAction(this);
		}

		private void OnAddCard2(CardsAddingToDrawZoneEventArgs args)
		{
			if (Highlight) return;
			OnAddCard(args.Cards);
		}

		private void OnAddCard1(CardsEventArgs args)
		{
			if (Highlight) return;
			OnAddCard(args.Cards);
		}
		private void OnAddCard(Card[] cards)
		{
			thecards.AddRange(cards);
			Count += cards.Length;
		}

		private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
		{
			if (Battle.BattleShouldEnd || args.Card is cardarashidad)
			{
				if (GameRun.Packs.Contains(nameof(packoldDef)[..^3]) && CustomGameEventManager.GetList("oldban").Intersect(thecards.Select(c => c.Id)).Any() && !Highlight)
				{ // old card re-add deduction at last
					Count -= CustomGameEventManager.GetList("oldban").Intersect(thecards.Select(c => c.Id)).Count();
				}
				Highlight = true;
				yield break;
			}
			if (thecards.Any(c => c.InstanceId == args.Card.InstanceId))
			{
				thecards.Remove(args.Card);
				Count--;
			}
			if (Count == 0)
			{
				NotifyActivating();
				yield return BuffAction<Grace>(Level);
				yield return BuffAction<TempFirepower>(Level);
				yield return BuffAction<TempSpirit>(Level);
				yield return new RemoveStatusEffectAction(this);
			}
			yield break;
		}
	}
}