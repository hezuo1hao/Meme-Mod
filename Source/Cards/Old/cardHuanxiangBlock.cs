using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.StatusEffects.Reimu;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardHuanxiangBlockDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Reimu";

			config.Value1 = 3;
			config.UpgradedValue1 = 4;

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { Any = 1, White = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(HuanxiangBlock)];
			config.UpgradedRelativeCards = [nameof(HuanxiangBlock) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardHuanxiangBlockDef))]
	public sealed class cardHuanxiangBlock : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<HuanxiangBlockSe>(Value1, 0, 0, 0, 0.2f);
			yield break;
		}
	}
}


