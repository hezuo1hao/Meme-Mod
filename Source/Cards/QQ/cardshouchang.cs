using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoLEntitySideloader.Resource;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardshouchangDef : lvalonmemecardtemplate
	{
		public override CardImages LoadCardImages()
		{
			var imgs = new CardImages(BepinexPlugin.embeddedSource);
			imgs.AutoLoad(this, ".png", hasUpgradeImage: true);
			return imgs;
		}
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Colors = [ManaColor.White, ManaColor.Green];
			config.Cost = new ManaGroup { Any = 1, White = 1, Green = 3 };
			config.UpgradedCost = new ManaGroup { Any = 1, Hybrid = 2, HybridColor = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;
			config.UpgradedKeywords = Keyword.Replenish;
			config.RelativeCards = [nameof(LilyFriend), nameof(LunaFriend), nameof(LarvaFriend)];
			config.UpgradedRelativeCards = [nameof(LilyFriend) + "+", nameof(LunaFriend) + "+", nameof(LarvaFriend)];

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.UpgradeImageId = $"{GetId()}{CardImages.upgradeString}";

			config.Illustrator = "7-Mirano / 授昌朋友";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardshouchangDef))]
	public sealed class cardshouchang : lvalonmemecard.memecard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			List<Card> cards = [
				Library.CreateCard<LunaFriend>(),
				Library.CreateCard<LilyFriend>(),
				Library.CreateCard<LarvaFriend>()
			];
			List<Card> cards2 = [];
			foreach (Card card in cards)
			{
				if (IsUpgraded && card.Id != nameof(LarvaFriend))
				{
					card.Upgrade();
				}
				card.Summon();
				cards2.Add(card);
			}
			foreach (Card card in cards2)
			{
				yield return new AddCardsToHandAction(card);
			}
			yield break;
		}
	}
}


