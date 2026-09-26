using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;
using LBoL.EntityLib.PlayerUnits;
using LBoL.EntityLib.Cards;

namespace lvalonmeme.Cards
{
	public sealed class cardiwoption2Def : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = false;
			config.HideMesuem = true;
			config.Owner = nameof(Cirno);

			config.TargetType = TargetType.SingleEnemy;

			config.Value1 = 2; // mana count

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 0 };
			config.Rarity = Rarity.Common;

			config.Mana = new ManaGroup() { Blue = 2 };

			config.Type = CardType.Ability;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.RelativeCards = [nameof(cardicewingplus)];
			config.UpgradedRelativeCards = [nameof(cardicewingplus)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "和莱";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardiwoption2Def))]
	public sealed class cardiwoption2 : OptionCard
	{
		// 从回合开始多抽1张牌
		public override IEnumerable<BattleAction> TakeEffectActions()
		{
			yield return new ApplyStatusEffectAction<seicewingplus2>(Battle.Player, Value1);
		}
	}
}


