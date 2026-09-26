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
using LBoL.Base.Extensions;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardbbDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.Mana = new ManaGroup { Philosophy = 3 };
			config.UpgradedMana = new ManaGroup { Philosophy = 6 };

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { Any = 3, White = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.RelativeCards = [nameof(PatchouliLibrary), nameof(YukariFriend), nameof(OutInGap)];
			config.UpgradedRelativeCards = [nameof(PatchouliLibrary), nameof(YukariFriend), nameof(OutInGap), nameof(ZhenmiaowanAbility), nameof(DanceAroundLake), nameof(MoonPurify), nameof(Jiangshen)];

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Keywords = Keyword.Exile | Keyword.Retain;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;

			config.Illustrator = "並非ZUN";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbbDef))]
	public sealed class cardbb : lvalonmemecard.memecard
	{
		public override Interaction Precondition()
		{
			List<Card> list2 = (from card in Battle.HandZone
								where card != this
								select card).ToList();
			if (!list2.Empty())
			{
				return new SelectHandInteraction(0, Battle.MaxHand, list2);
			}
			return null;
		}

		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (precondition != null)
			{
				IReadOnlyList<Card> readOnlyList = ((SelectHandInteraction)precondition).SelectedCards;
				if (readOnlyList.Count > 0)
				{
					yield return new ExileManyCardAction(readOnlyList);
				}
			}

			yield return new GainManaAction(Mana);

			List<Card> cards1 = [
				Library.CreateCard<PatchouliLibrary>(),
				Library.CreateCard<YukariFriend>(),
				Library.CreateCard<OutInGap>(),
			];
			SelectCardInteraction interaction = new(0, cards1.Count, cards1, SelectedCardHandling.DoNothing)
			{
				Source = this
			};
			yield return new InteractionAction(interaction, false);
			foreach (Card tmp in interaction.SelectedCards)
			{
				yield return new AddCardsToHandAction(tmp);
			}
			interaction = null;

			if (IsUpgraded)
			{
				List<Card> cards2 = [
				Library.CreateCard<ZhenmiaowanAbility>(),
				Library.CreateCard<DanceAroundLake>(),
				Library.CreateCard<MoonPurify>(),
				Library.CreateCard<Jiangshen>()
				];
				SelectCardInteraction interaction2 = new(0, cards2.Count, cards2, SelectedCardHandling.DoNothing)
				{
					Source = this
				};
				yield return new InteractionAction(interaction2, false);
				foreach (Card tmp in interaction2.SelectedCards)
				{
					yield return new AddCardsToHandAction(tmp);
				}
				interaction2 = null;
			}

			yield return new ApplyStatusEffectAction<sebb>(Battle.Player, 1, null, null, null, 0f, true);
			yield break;
		}
	}
}


