using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardDropToKnifeDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Sakuya";

			config.Colors = [ManaColor.White, ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 1, White = 1, Blue = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.RelativeCards = [nameof(DropToKnife), nameof(Knife)];
			config.UpgradedRelativeCards = [nameof(DropToKnife) + "+", nameof(Knife)];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "breedo";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardDropToKnifeDef))]
	public sealed class cardDropToKnife : lvalonmemecard.oldcard
	{
		public override bool DiscardCard => true;

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			int count = Battle.HandZone.Count;
			yield return new DiscardManyAction(Battle.HandZone);
			if (!Battle.BattleShouldEnd && count > 0)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<Knife>(count, false), AddCardsType.Normal);
			}
		}
	}
}


