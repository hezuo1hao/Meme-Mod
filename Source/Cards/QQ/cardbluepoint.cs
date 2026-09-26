using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using System.Linq;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardbluepointDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.FindInBattle = false;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 10;
			config.Colors = [ManaColor.Colorless];
			config.Cost = new ManaGroup { Any = 3 };
			config.UpgradedCost = new ManaGroup { Any = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Ethereal;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "有可能是万书言";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardbluepointDef))]
	public sealed class cardbluepoint : lvalonmemecard.memecard
	{
		protected override void OnEnterBattle(BattleController battle)
		{
			ReactBattleEvent(Battle.BattleEnding, new EventSequencedReactor<GameEventArgs>(OnBattleEnding));
		}

		private IEnumerable<BattleAction> OnBattleEnding(GameEventArgs args)
		{
			List<Card> list = (from card in Battle.EnumerateAllCardsButExile()
							   where card == this
							   select card).ToList();
			if (list.Count > 0)
			{
				NotifyActivating();
				Battle.Stats.BluePoint += Value1 * list.Count;
			}
			yield break;
		}
	}
}


