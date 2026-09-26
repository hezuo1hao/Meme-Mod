using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Linq;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.Core.Cards;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardFairyWrathDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.TargetType = TargetType.AllEnemies;

			config.Damage = 16;
			config.UpgradedDamage = 20;

			config.Value1 = 1;

			config.GunName = "自然之怒";
			config.GunNameBurst = "自然之怒B";

			config.Colors = [ManaColor.Green];
			config.Cost = new ManaGroup { Any = 1, Green = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Attack;

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeCards = [nameof(FairyWrath), nameof(SummerFlower), nameof(DayaojingFriend)];
			config.UpgradedRelativeCards = [nameof(FairyWrath) + "+", nameof(SummerFlower) + "+", nameof(DayaojingFriend)];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "Alioth Studio";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardFairyWrathDef))]
	public sealed class cardFairyWrath : lvalonmemecard.oldcard
	{
		public override bool Triggered
		{
			get
			{
				if (Battle != null)
				{
					return Battle.HandZone.Any((Card card) => card is DayaojingFriend && card.Summoned);
				}
				return false;
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return AttackAction(selector, GunName);
			if (Battle.BattleShouldEnd)
			{
				yield break;
			}
			if (TriggeredAnyhow)
			{
				yield return new AddCardsToHandAction(Library.CreateCards<SummerFlower>(Value1, IsUpgraded), AddCardsType.Normal);
			}
			yield break;
		}
	}
}


