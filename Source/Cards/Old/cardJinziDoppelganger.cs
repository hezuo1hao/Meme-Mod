using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardJinziDoppelgangerDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.UpgradedValue1 = 2;
			config.Mana = new ManaGroup() { Any = 1 };

			config.Colors = [ManaColor.Blue, ManaColor.Black, ManaColor.Green];
			config.Cost = new ManaGroup { Any = 2, Blue = 1, Black = 1, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeKeyword = Keyword.NaturalTurn;
			config.UpgradedRelativeKeyword = Keyword.NaturalTurn;
			config.RelativeCards = [nameof(JinziDoppelganger)];
			config.UpgradedRelativeCards = [nameof(JinziDoppelganger) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "三折塔";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardJinziDoppelgangerDef))]
	public sealed class cardJinziDoppelganger : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<SuperExtraTurn>(Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


