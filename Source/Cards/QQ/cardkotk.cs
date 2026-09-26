using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Linq;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Core.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardkotkDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Colors = [ManaColor.Black];
			config.Cost = new ManaGroup { Black = 1 };
			config.UpgradedCost = new ManaGroup { Any = 0 };
			config.Mana = new ManaGroup { Philosophy = 3 };
			config.Rarity = Rarity.Rare;
			config.Value1 = 1;
			config.Value2 = 4;

			config.Type = CardType.Ability;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "哈基双朋友";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardkotkDef))]
	public sealed class cardkotk : lvalonmemecard.memecard
	{
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStarted));
		}
		private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
		{
			if (GameRun.BaseDeck.Any(c => c is cardyoumi))
			{
				yield return new GainManaAction(Mana);
				yield return new ApplyStatusEffectAction<Graze>(Battle.Player, new int?(Value2 + 1), null, null, null, 0f, true);
				yield return new ApplyStatusEffectAction<sekotk>(Battle.Player, 1, null, null, null, 0f, true);
				yield return new RemoveCardAction(this);
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new GainManaAction(Mana);
			if (IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<Graze>(Battle.Player, new int?(Value2), null, null, null, 0f, true);
			}
			yield return new ApplyStatusEffectAction<sekotk>(Battle.Player, 1, null, null, null, 0f, true);
			yield break;
		}
	}
}


