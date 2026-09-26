using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.Core.Cards;
using lvalonmeme.StatusEffects;
using LBoL.Core.Stations;
using LBoL.Core.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardsanaeconfessionDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.FindInBattle = false;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 5;
			config.Value2 = 1;

			config.Colors = [ManaColor.Green];
			config.Cost = new ManaGroup { Any = 2, Green = 3 };
			config.Rarity = Rarity.Rare;

			config.Mana = new ManaGroup { Any = 1 };

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(LoveLetter)];
			config.UpgradedRelativeCards = [nameof(LoveLetter)];
			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "yamasan";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardsanaeconfessionDef))]
	public sealed class cardsanaeconfession : lvalonmemecard.memecard
	{
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
		}
		private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
		{
			if (GameRun.CurrentStation.Type == StationType.Boss || GameRun.CurrentStation.Type == StationType.EliteEnemy)
			{
				yield return new ApplyStatusEffectAction<Firepower>(Battle.Player, Value2, null, null, null, 0f, true);
				yield return new ApplyStatusEffectAction<Spirit>(Battle.Player, Value2, null, null, null, 0f, true);
				yield return new RemoveCardAction(this);
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			IEnumerable<Card> cards = Library.CreateCards<LoveLetter>(Value1, false);
			if (IsUpgraded)
			{
				foreach (Card card in cards)
				{
					card.AuraCost -= Mana;
				}
			}
			yield return new AddCardsToDiscardAction(cards, AddCardsType.Normal);
			if (!Battle.Player.HasStatusEffect<sesanaeconfession>())
			{
				yield return new ApplyStatusEffectAction<sesanaeconfession>(Battle.Player, Value1, null, null, null, 0f, true);
			}
			yield break;
		}
	}
}


