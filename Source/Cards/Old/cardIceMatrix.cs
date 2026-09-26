using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.Cirno;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardIceMatrixDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Value1 = 3;

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Any = 3, Blue = 2 };
			config.UpgradedCost = new ManaGroup { Any = 2, Blue = 2 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(IceMatrix)];
			config.UpgradedRelativeCards = [nameof(IceMatrix) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(Cold), nameof(FrostArmor)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(Cold), nameof(FrostArmor)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Unfinished = true;
			config.Illustrator = "Alioth Studio";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardIceMatrixDef))]
	public sealed class cardIceMatrix : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<IceMatrixSe>(Value1, 0, 0, 0, 0.2f);
			foreach (BattleAction battleAction in DebuffAction<Cold>(Battle.AllAliveEnemies, 0, 0, 0, 0, true, 0.1f))
			{
				yield return battleAction;
			}
			yield break;
		}
	}
}


