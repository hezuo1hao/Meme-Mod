using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Blue;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardYonglinCardDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 3;
			config.UpgradedValue1 = 5;
			config.Mana = new ManaGroup { Any = 1 };
			config.UpgradedMana = new ManaGroup { Any = 0 };

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 2, Blue = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.Keywords = Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Ethereal;
			config.RelativeCards = [nameof(YonglinCard)];
			config.UpgradedRelativeCards = [nameof(YonglinCard) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardYonglinCardDef))]
	public sealed class cardYonglinCard : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (Battle.Player.TryGetStatusEffect(out seYonglinCard se) && !IsUpgraded)
			{
				se.Count++;
			}
			if (Battle.Player.TryGetStatusEffect(out seYonglinCard0 se0) && IsUpgraded)
			{
				se0.Count++;
			}
			if (!IsUpgraded)
			{
				yield return new ApplyStatusEffectAction<seYonglinCard>(Battle.Player, 1, null, null, null, 0f, true);
			}
			else
			{
				yield return new ApplyStatusEffectAction<seYonglinCard0>(Battle.Player, 1, null, null, null, 0f, true);
			}
			yield break;
		}
	}
}


