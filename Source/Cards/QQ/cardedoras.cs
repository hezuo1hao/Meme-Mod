using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Battle.BattleActions;
using System;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardedorasDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;
			config.FindInBattle = false;

			config.Colors = [ManaColor.White, ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 1, Hybrid = 1, HybridColor = 0 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.Keywords = config.UpgradedKeywords = Keyword.Disposable;
            config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "依穆_YiMu";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardedorasDef))]
	public sealed class cardedoras : lvalonmemecard.memecard
	{
		DateTime start = new(2024, 12, 28);
		static int GetRoundedDaysBetweenNowAnd(DateTime specifiedDate)
		{
			TimeSpan difference = DateTime.Now - specifiedDate;
			return (int)Math.Round(difference.TotalDays);
		}

		public int days
		{
			get
			{
				return GetRoundedDaysBetweenNowAnd(start);
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new GainMoneyAction(days);
			if (IsUpgraded)
			{
				MapNodeWidget[,] mapNodeWidgets = UiManager.GetPanel<MapPanel>()._mapNodeWidgets;
				int up = mapNodeWidgets.GetUpperBound(0);
				int up2 = mapNodeWidgets.GetUpperBound(1);
				for (int i = mapNodeWidgets.GetLowerBound(0); i <= up; i++)
				{
					for (int j = mapNodeWidgets.GetLowerBound(1); j <= up2; j++)
					{
						MapNodeWidget mapNodeWidget = mapNodeWidgets[i, j];
						if (mapNodeWidget != null && mapNodeWidget.X == UiManager.GetPanel<MapPanel>().CurrentWidget.X + 1 && (mapNodeWidget.MapNode.StationType == LBoL.Core.Stations.StationType.Enemy || mapNodeWidget.MapNode.StationType == LBoL.Core.Stations.StationType.Gap || mapNodeWidget.MapNode.StationType == LBoL.Core.Stations.StationType.Adventure))
						{
							mapNodeWidget.MapNode.StationType = LBoL.Core.Stations.StationType.Shop;
							mapNodeWidget.SetIconDefault();
						}
					}
				}
			}
			yield break;
		}
		public override IEnumerable<BattleAction> AfterUseAction()
		{
			Card deckCardByInstanceId = base.GameRun.GetDeckCardByInstanceId(base.InstanceId);
			if (deckCardByInstanceId != null)
			{
				base.GameRun.RemoveDeckCard(deckCardByInstanceId, false);
			}
			yield return new RemoveCardAction(this);
			yield break;
		}
		public override IEnumerable<BattleAction> AfterFollowPlayAction()
		{
			Card deckCardByInstanceId = base.GameRun.GetDeckCardByInstanceId(base.InstanceId);
			if (deckCardByInstanceId != null)
			{
				base.GameRun.RemoveDeckCard(deckCardByInstanceId, false);
			}
			yield return new RemoveCardAction(this);
			yield break;
		}
	}
}


