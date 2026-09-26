using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using System.Linq;
using LBoL.Core.Stations;
using LBoL.Core.Cards;
using System;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardyoumiDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.Order = 5;
			config.FindInBattle = false;
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 3;
			config.UpgradedValue1 = 5;

			config.Colors = [ManaColor.Black, ManaColor.Red];
			config.Cost = new ManaGroup { Black = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "jill 07km";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardyoumiDef))]
	public sealed class cardyoumi : lvalonmemecard.memecard
	{
		public bool remove = false;
		public int used
		{
			get
			{
				return DeckCounter ?? 0;
			}
		}
		public override void Initialize()
		{
			base.Initialize();
			if (Config.Type == CardType.Tool)
			{
				throw new InvalidOperationException("Deck counter enabled card 'cardyoumi' must not be Tool.");
			}
			DeckCounter = new int?(0);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			bool hasother = GameRun.BaseDeck.Any(c => c is cardkotk);
			if (GameRun.CurrentStation.Type == StationType.Boss && !hasother)
			{
				yield return new ForceKillAction(Battle.Player, Battle.Player);
			}
			if (!IsUpgraded)
			{
				if (DeckCounter >= Value1 - 1)
				{
					yield return new ForceKillAction(Battle.Player, Battle.Player);
				}
			}
			else
			{
				if (DeckCounter >= Value1 - 1)
				{
					GameRun.SetHpAndMaxHp(Convert.ToInt32(Math.Round((double)Battle.Player.Hp / 2, MidpointRounding.AwayFromZero)), Battle.Player.MaxHp, false);
				}
			}
			Battle.RequestDebugAction(new InstantWinAction(), "youmi win");
			yield break;
		}
		public override IEnumerable<BattleAction> AfterUseAction()
		{
			DeckCounter += 1;
			Card deckCardByInstanceId = Battle.GameRun.GetDeckCardByInstanceId(InstanceId);
			if (deckCardByInstanceId != null)
			{
				deckCardByInstanceId.DeckCounter = DeckCounter;
			}
			return base.AfterUseAction();
		}


		public override IEnumerable<BattleAction> AfterFollowPlayAction()
		{
			DeckCounter += 1;
			Card deckCardByInstanceId = Battle.GameRun.GetDeckCardByInstanceId(InstanceId);
			if (deckCardByInstanceId != null)
			{
				deckCardByInstanceId.DeckCounter = DeckCounter;
			}
			return base.AfterFollowPlayAction();
		}
	}
}


