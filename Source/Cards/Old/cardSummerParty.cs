using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardSummerPartyDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.IsXCost = true;

			config.Value1 = 1;
			config.Value2 = 3;
			config.Mana = new ManaGroup() { Green = 2 };
			config.UpgradedMana = new ManaGroup() { Green = 1 };

			config.Colors = [ManaColor.Blue, ManaColor.Green];
			config.Cost = new ManaGroup { Blue = 1, Green = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.RelativeKeyword = Keyword.Synergy | Keyword.FriendCard;
			config.UpgradedRelativeKeyword = Keyword.Synergy | Keyword.FriendCard;

			config.RelativeCards = [nameof(SummerParty)];
			config.UpgradedRelativeCards = [nameof(SummerParty) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(FrostArmor)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(FrostArmor)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardSummerPartyDef))]
	public sealed class cardSummerParty : lvalonmemecard.oldcard
	{
		protected override ManaGroup vMana2 { get; set; } = ManaGroup.Blues(1);
		protected override ManaGroup vUpgradedMana2 { get; set; } = ManaGroup.Blues(1);
		public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
		{
			return new ManaGroup
			{
				Blue = pooledMana.Blue,
				Green = pooledMana.Green,
				Philosophy = pooledMana.Philosophy
			};
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			Card[] array = Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.OnlyFriend, false), Value1, null);
			foreach (Card card in array)
			{
				card.Summon();
			}
			if (!IsUpgraded)
			{
				yield return new AddCardsToHandAction(array, AddCardsType.Normal);
			}
			int blue = SynergyAmount(consumingMana, ManaColor.Blue, 1);
			int green = SynergyAmount(consumingMana, ManaColor.Green, IsUpgraded ? 1 : 2);
			if (blue > 0)
			{
				for (int i = 0; i < blue; i++)
				{
					yield return new ApplyStatusEffectAction<FrostArmor>(Battle.Player, Value2, null, null, null, 0f, true);
				}
			}
			if (green > 0)
			{
				for (int i = 0; i < green; i++)
				{
					Card[] array2 = Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.OnlyFriend, false), Value1, null);
					foreach (Card card in array2)
					{
						card.Summon();
					}
					if (Battle.HandZone.Count >= Battle.MaxHand)
					{
						yield return new AddCardsToDrawZoneAction(array2, DrawZoneTarget.Random, AddCardsType.Normal);
					}
					else
					{
						yield return new AddCardsToHandAction(array2, AddCardsType.Normal);
					}
				}
			}
			yield break;
		}
	}
}


