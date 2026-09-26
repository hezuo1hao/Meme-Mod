using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.Core.StatusEffects;
using lvalonmeme.Packs;
using LBoL.Core.Units;

namespace lvalonmeme.Cards
{
	public sealed class cardMystiaSingDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 5;
			config.UpgradedValue1 = 8;
			config.Value2 = 5;
			config.UpgradedValue2 = 10;

			config.Colors = [ManaColor.Black];
			config.Cost = new ManaGroup { Any = 1, Black = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Exile;
			config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
			config.RelativeCards = [nameof(MystiaSing)];
			config.UpgradedRelativeCards = [nameof(MystiaSing) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "vedjoti";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardMystiaSingDef))]
	public sealed class cardMystiaSing : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			// foreach (BattleAction battleAction in DebuffAction<TempFirepowerNegative>(selector.GetUnits(Battle), Value1, 0, 0, 0, true, 0.2f))
			// {
			// 	yield return battleAction;
			// }
			foreach (Unit unit in Battle.AllAliveEnemies)
			{
				yield return DebuffAction<TempFirepowerNegative>(unit, Value1);
			}
			yield return new GainPowerAction(Value2);
			yield break;
		}
	}
}


