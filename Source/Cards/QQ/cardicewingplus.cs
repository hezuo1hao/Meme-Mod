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
using lvalonmeme.Packs;
using LBoL.EntityLib.PlayerUnits;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using System;
using LBoL.EntityLib.Cards;

namespace lvalonmeme.Cards
{
	public sealed class cardicewingplusDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = nameof(Cirno);

			config.Value1 = 1; // draw
			config.Value2 = 3; // threshold
			config.Mana = new ManaGroup() { Colorless = 1 };

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 0 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Retain | Keyword.Exile;
			config.UpgradedKeywords = Keyword.Retain | Keyword.Exile | Keyword.Replenish;

			config.RelativeEffects = [nameof(Cold), nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(Cold), nameof(sememe)];

			config.RelativeCards = [nameof(IceWing), nameof(cardiwoption1), nameof(cardiwoption2), nameof(cardiwoption3)];
			config.UpgradedRelativeCards = [nameof(IceWing), nameof(cardiwoption1), nameof(cardiwoption2), nameof(cardiwoption3)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "和莱";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardicewingplusDef))]
	public sealed class cardicewingplus : lvalonmemecard.memecard
	{
		public int growcount => Battle != null ? Battle.BattleCardUsageHistory.Count(x => x.Id == nameof(cardicewingplusDef)[..^3]) : 0;
		public int Value6 => 6;
		public ManaGroup Mana3 => new ManaGroup() { Blue = 1 };
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (growcount == Value6 - 1)
			{
				List<Card> list =
                [
                    Library.CreateCard<cardiwoption1>(),
					Library.CreateCard<cardiwoption2>(),
					Library.CreateCard<cardiwoption3>()
				];

				SelectCardInteraction interaction = new SelectCardInteraction(1, 1, list)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				foreach (Card selectedCard in interaction.SelectedCards)
				{
					if (!(selectedCard is OptionCard optionCard))
					{
						continue;
					}

					optionCard.SetBattle(Battle);
					foreach (BattleAction item in optionCard.TakeEffectActions())
					{
						yield return item;
					}
				}
			}
			// 每场战斗中打出三次时，之后打出的冰之翼效果改为原版冰之翼效果
			if (growcount >= Value2) // ice wing
			{
				yield return new GainManaAction(Mana3);
				List<Card> list = Battle.DrawZone.Where((Card card2) => card2.Config.Colors.Contains(ManaColor.Blue)).ToList();
				if (list.Count <= 0)
				{
					yield break;
				}

				List<Card> cards = list.SampleManyOrAll(2, GameRun.BattleRng).ToList();
				if (cards.Count <= 0)
				{
					yield break;
				}

				MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(cards)
				{
					Source = this
				};
				yield return new InteractionAction(interaction);
				Card card = interaction.SelectedCard;
				yield return new MoveCardAction(card, CardZone.Hand);
				cards.Remove(card);
				if (cards.Count > 0)
				{
					yield break;
				}

				foreach (Card item in cards)
				{
					yield return new MoveCardToDrawZoneAction(item, DrawZoneTarget.Bottom);
				}
			}
			else
			{
				// 抽1张牌 获得1点无色
				yield return new DrawManyCardAction(Value1);
				yield return new GainManaAction(Mana);
			}
			yield break;
		}
	}
}


