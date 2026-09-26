using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Adventure;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardsanaeseductionDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 5;
			config.Value2 = 3;

			config.Mana = new ManaGroup { Any = 1 };

			config.Colors = [ManaColor.Green];
			config.Cost = new ManaGroup { Any = 2, Green = 2 };
			config.UpgradedCost = new ManaGroup { Any = 1, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.RelativeCards = [nameof(NewsNegative)];
			config.UpgradedRelativeCards = [nameof(NewsNegative)];
			config.RelativeEffects = [nameof(sesanaeseduction), nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sesanaeseduction), nameof(sememe)];
			config.RelativeKeyword = Keyword.TempMorph;
			config.UpgradedRelativeKeyword = Keyword.TempMorph;

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "宮瀬まひろ";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardsanaeseductionDef))]
	public sealed class cardsanaeseduction : lvalonmemecard.memecard
	{
		protected override int BaseValue3 { get; set; } = 999;
		protected override int BaseUpgradedValue3 { get; set; } = 999;
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
		}
		private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
		{
			if (!Battle.Player.HasStatusEffect<sesanaeseduction>())
			{
				yield return new ApplyStatusEffectAction<sesanaeseduction>(Battle.Player, 1, null, null, null, 0f, true);
			}
			yield break;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<TempFirepower>(Battle.Player, Value1, null, null, null, 0f, true);
			yield return new ApplyStatusEffectAction<TempSpirit>(Battle.Player, Value1, null, null, null, 0f, true);
			yield return new DrawManyCardAction(Value2);
			yield return new ApplyStatusEffectAction<semanaunfreeze>(Battle.Player, Value2, null, null, null, 0f, true);
			EnemyUnit target = Battle.RandomAliveEnemy;
			yield return new ApplyStatusEffectAction<TempFirepowerNegative>(target, Value3, null, null, null, 0f, true);
			yield return new ApplyStatusEffectAction<seedging>(Battle.Player, 1, null, null, null, 0f, true);
			yield break;
		}
	}
}


