using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader.Attributes;
using lvalonmeme.Cards.Template;
using lvalonmeme.Packs;
using lvalonmeme.StatusEffects;
using System.Collections.Generic;

namespace lvalonmeme.Cards
{
	public sealed class cardBladePowerDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Sakuya";

			config.Value1 = 2;
			config.UpgradedValue1 = 3;
			config.Value2 = 1;

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { White = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Skill;

			config.RelativeCards = [nameof(BladePower)];
			config.UpgradedRelativeCards = [nameof(BladePower) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(Firepower)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(Firepower)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Unfinished = true;
			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardBladePowerDef))]
	public sealed class cardBladePower : lvalonmemecard.oldcard
	{
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new DrawManyCardAction(Value2);
            yield break;
        }
        private IEnumerable<BattleAction> EnterHandReactor()
		{
			if (Battle.BattleShouldEnd || Zone != CardZone.Hand)
			{
				yield break;
			}
			NotifyActivating();
			yield return new ExileCardAction(this);
			yield return BuffAction<Firepower>(Value1, 0, 0, 0, 0.2f);
			yield break;
		}
		protected override void OnEnterBattle(BattleController battle)
		{
			if (Zone == CardZone.Hand && Battle.HandZone.Count == 1)
			{
				React(EnterHandReactor());
			}
		}
		public override IEnumerable<BattleAction> OnDraw()
		{
			if (Battle.HandZone.Count != 1)
			{
				return null;
			}
			return EnterHandReactor();
		}
		public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
		{
			if (!Battle.BattleShouldEnd && srcZone == CardZone.Hand && dstZone != CardZone.Hand)
			{
				DeltaValue1 = 0;
			}
			if (dstZone != CardZone.Hand || Battle.HandZone.Count != 1)
			{
				return null;
			}
			return EnterHandReactor();
		}
	}
}


