using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoL.Core.Cards;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardTaijiLiangyiDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Reimu";

			config.Value1 = 1;

			config.Colors = [ManaColor.Red];
			config.Cost = new ManaGroup { Any = 1, Red = 2 };
			config.UpgradedCost = new ManaGroup { Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(TaijiLiangyi), nameof(YinyangCard)];
			config.UpgradedRelativeCards = [nameof(TaijiLiangyi) + "+", nameof(YinyangCard)];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "四動自半";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardTaijiLiangyiDef))]
	public sealed class cardTaijiLiangyi : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new AddCardsToHandAction(new Card[]
			{
				Library.CreateCard<YinyangCard>()
			});
			yield return BuffAction<TaijiLiangyiSe>(Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


