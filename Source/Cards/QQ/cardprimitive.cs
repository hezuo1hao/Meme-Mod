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
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardprimitiveDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 15;
			config.Value2 = 5;

			config.Colors = [ManaColor.Green, ManaColor.White];
			config.Cost = new ManaGroup { Any = 2, Green = 1, White = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1, Hybrid = 1, HybridColor = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "ななもと";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardprimitiveDef))]
	public sealed class cardprimitive : lvalonmemecard.memecard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<Firepower>(Battle.Player, Value1, null, null, null, 0f, true);
			if (!Battle.Player.HasStatusEffect<seedging>())
			{
				yield return new ApplyStatusEffectAction<seedging>(Battle.Player, Value2, null, null, null, 0f, true);
			}
			yield break;
		}
	}
}


