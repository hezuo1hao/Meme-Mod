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
using LBoL.EntityLib.Cards.Character.Marisa;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.StatusEffects.Others;
using lvalonmeme.Packs;
using LBoL.EntityLib.Cards.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Marisa;
using LBoL.EntityLib.StatusEffects.Neutral.Black;

namespace lvalonmeme.Cards
{
	public sealed class cardtfmDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.Value2 = 5;

			config.Shield = 99;
			config.UpgradedShield = 999;

			config.Colors = [ManaColor.Black];
			config.Cost = new ManaGroup { Any = 2, Black = 3 };

			config.Mana = new ManaGroup() { Any = 1 };

			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(TrueMoon), nameof(WizardStudy)];
			config.UpgradedRelativeCards = [nameof(TrueMoon), nameof(WizardStudy)];
			config.RelativeEffects = [nameof(ManaFreezed), nameof(Amulet), nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(ManaFreezed), nameof(Amulet), nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardtfmDef))]
	public sealed class cardtfm : lvalonmemecard.memecard
	{
		public int Value9 => IsUpgraded ? 99 : 9;
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<Amulet>(Value1);
			yield return BuffAction<Firepower>(Value2);
			yield return BuffAction<Spirit>(Value2);
			yield return DebuffAction<ManaFreezed>(Battle.Player, Mana.Any);

			yield return DefenseAction(true);
			yield return BuffAction<Firepower>(Value9);
			yield return BuffAction<UseCardToLoseGame>(0, 0, 0, Value1);
			yield break;
		}
	}
}


