using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.StatusEffects.Neutral.MultiColor;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardggDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 3;

			config.Colors = [ManaColor.Colorless];
			config.Cost = new ManaGroup { Any = 3, Colorless = 2 };
			config.Rarity = Rarity.Rare;
			config.Mana = new ManaGroup { Colorless = 1 };

			config.Type = CardType.Ability;

			config.RelativeKeyword = Keyword.Purified | Keyword.Morph;
			config.UpgradedRelativeKeyword = Keyword.Purified | Keyword.Morph;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "シャドウバース";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardggDef))]
	public sealed class cardgg : lvalonmemecard.memecard
	{
		protected override ManaGroup vMana2 { get; set; } = ManaGroup.Anys(1);
		protected override ManaGroup vUpgradedMana2 { get; set; } = ManaGroup.Anys(1);
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new GainManaAction(Mana);
			if (IsUpgraded)
			{
				yield return new DrawManyCardAction(Value1);
			}
			yield return new ApplyStatusEffectAction<segg>(Battle.Player, 1, null, null, null, 0f, true);
			yield return new ApplyStatusEffectAction<ZhenmiaowanAbilitySe>(Battle.Player, 1, null, null, null, 0f, true);
			yield break;
		}
	}
}


