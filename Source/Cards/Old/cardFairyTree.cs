using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardFairyTreeDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Value1 = 1;
			config.Mana = new ManaGroup() { Philosophy = 1 };

			config.Colors = [ManaColor.Green];
			config.Cost = new ManaGroup { Green = 2 };
			config.UpgradedCost = new ManaGroup { Any = 0 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;

			config.RelativeKeyword = Keyword.FriendCard;
			config.UpgradedRelativeKeyword = Keyword.FriendCard;

			config.RelativeCards = [nameof(FairyTree)];
			config.UpgradedRelativeCards = [nameof(FairyTree) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "breedo";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardFairyTreeDef))]
	public sealed class cardFairyTree : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			// if (Battle.Player.TryGetStatusEffect(out SE.magicalburstdef.magicalburst tmp) && tmp is mimaextensions.mimase magicalburst)
			//     {
			//         magicalburst.truecounter = 0;
			//         yield return BuffAction<SE.magicalburstdef.magicalburst>(0, 1, 0, 0, 0.2f);
			//     }
			if (Battle.Player.TryGetStatusEffect(out seFairyTree se))
			{
				yield return BuffAction<seFairyTree>(Value1, 0, 0, Value1, 0.2f);
				se.Count++;
			}
			else
			{
				yield return BuffAction<seFairyTree>(Value1, 0, 0, Value1, 0.2f);
			}
			yield break;
		}
	}
}


