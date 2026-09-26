using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardallfriendsDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Colors = [ManaColor.Green, ManaColor.Blue];
			config.Cost = new ManaGroup { Green = 3, Blue = 2 };
			config.Rarity = Rarity.Rare;
			config.Value1 = 1;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(GatherFairy)];
			config.UpgradedRelativeCards = [nameof(GatherFairy), nameof(DayaojingFriend), nameof(MaidFriend), nameof(LeidiFriend), nameof(LarvaFriend), nameof(LilyFriend), nameof(LunaFriend), nameof(StarFriend), nameof(SunnyFriend), nameof(ClownpieceFriend)];
			config.UpgradedRelativeKeyword = Keyword.Replenish;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardallfriendsDef))]
	public sealed class cardallfriends : lvalonmemecard
	{
		protected override bool ismeme { get; set; } = true;
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new ApplyStatusEffectAction<seallfriends>(Battle.Player, new int?(Value1), null, null, null, 0f, true);
			if (IsUpgraded)
			{
				List<Card> cards = [
				Library.CreateCard<DayaojingFriend>(),
				Library.CreateCard<MaidFriend>(),
				Library.CreateCard<LeidiFriend>(),
				Library.CreateCard<LarvaFriend>(),
				Library.CreateCard<LilyFriend>(),
				Library.CreateCard<LunaFriend>(),
				Library.CreateCard<StarFriend>(),
				Library.CreateCard<SunnyFriend>(),
				Library.CreateCard<ClownpieceFriend>()
			];
				List<Card> cards2 = [];
				foreach (Card card in cards)
				{
					card.Summon();
					card.IsReplenish = true;
					cards2.Add(card);
				}
				yield return new AddCardsToDrawZoneAction(cards, DrawZoneTarget.Random, AddCardsType.Normal);
			}
			yield break;
		}
	}
}


