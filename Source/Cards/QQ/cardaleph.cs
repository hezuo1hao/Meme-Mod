using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Randoms;
using LBoL.Presentation;
using LBoL.Core.Cards;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardalephDef : lvalonmemecardtemplate
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

			config.Type = CardType.Skill;

			config.Illustrator = "問他";

            config.Keywords = config.UpgradedKeywords = Keyword.Disposable;
            config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardalephDef))]
	public sealed class cardaleph : lvalonmemecard.memecard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			Exhibit ex = null;
			foreach (Exhibit exhibit in GameRun.Player.Exhibits)
			{
				if (exhibit.Config.Owner != null)
				{
					ex = exhibit;
					break;
				}
			}
			Exhibit rolled = GameRun.RollShiningExhibit(GameRun.ShiningExhibitRng, null, (ExhibitConfig config) => string.IsNullOrWhiteSpace(config.Owner));
			if (ex != null)
			{
				GameRun.LoseExhibit(ex, false, true);
				while (GameRun.Player.HasExhibit(rolled))
				{
					rolled = GameRun.RollShiningExhibit(GameRun.ShiningExhibitRng, null, (ExhibitConfig config) => string.IsNullOrWhiteSpace(config.Owner));
				}
				if (rolled != null)
				{
					GameMaster.DebugGainExhibit(rolled);
				}
			}
			rolled = null;
			if (IsUpgraded)
			{
				rolled = GameRun.RollNormalExhibit(GameRun.ExhibitRng, new ExhibitWeightTable(new RarityWeightTable(0.5f, 0.33f, 0.17f, 0f), AppearanceWeightTable.NotInShop), null);
				while (GameRun.Player.HasExhibit(rolled))
				{
					rolled = GameRun.RollShiningExhibit(GameRun.ShiningExhibitRng, null, (ExhibitConfig config) => string.IsNullOrWhiteSpace(config.Owner));
				}
				if (rolled != null)
				{
					GameMaster.DebugGainExhibit(rolled);
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

		// Token: 0x060009FB RID: 2555 RVA: 0x000151F2 File Offset: 0x000133F2
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


