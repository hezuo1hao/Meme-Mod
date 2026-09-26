using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using System.Linq;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards.Character.Sakuya;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardCleanRoomDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Sakuya";

			config.Value1 = 2;
			config.Value2 = 1;
			config.UpgradedValue2 = 2;

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { White = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;

			config.RelativeCards = [nameof(CleanRoom)];
			config.UpgradedRelativeCards = [nameof(CleanRoom) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "千羽";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardCleanRoomDef))]
	public sealed class cardCleanRoom : lvalonmemecard.oldcard
	{
		Card oneTargetHand = null;
		public override Interaction Precondition()
		{
			List<Card> list = (from hand in Battle.HandZone
							   where hand != this
							   select hand).ToList();
			if (list.Count == 1)
			{
				oneTargetHand = list[0];
			}
			if (list.Count <= 1)
			{
				return null;
			}
			return new SelectHandInteraction(1, 1, list);
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
            if (precondition != null)
            {
                Card card = ((SelectHandInteraction)precondition).SelectedCards[0];
                if (card != null)
                {
                    yield return new DiscardAction(card);
                }
            }
            else if (oneTargetHand != null)
            {
                yield return new DiscardAction(this.oneTargetHand);
                oneTargetHand = null;
            }
            yield return new DrawManyCardAction(Value1);
			yield return UpgradeRandomHandAction(Value2, CardType.Unknown);
			yield break;
		}
	}
}


