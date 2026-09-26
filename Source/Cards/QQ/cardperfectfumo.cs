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
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoL.Core.StatusEffects;

namespace lvalonmeme.Cards
{
	public sealed class cardperfectfumoDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Sakuya";

			config.Value1 = 1;
			config.Value2 = 1;

			config.IsXCost = true;

			config.Colors = [ManaColor.Blue, ManaColor.White];
			config.Cost = new ManaGroup { Blue = 1, White = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Skill;

			config.Keywords = Keyword.Exile;

			config.Mana = new ManaGroup() { Philosophy = 1 };

			config.RelativeKeyword = Keyword.TempMorph;
			config.UpgradedRelativeKeyword = Keyword.TempMorph;

			config.RelativeCards = [nameof(PerfectServant)];
			config.UpgradedRelativeCards = [nameof(PerfectServant) + "+"];
			config.RelativeEffects = [nameof(sememe), nameof(ExtraTurn)];
			config.UpgradedRelativeEffects = [nameof(sememe), nameof(ExtraTurn)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "酒醉的蝴蝶";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardperfectfumoDef))]
	public sealed class cardperfectfumo : lvalonmemecard.memecard
	{
		public override ManaGroup GetXCostFromPooled(ManaGroup pooledMana)
		{
			return new ManaGroup
			{
				White = pooledMana.White,
				Blue = pooledMana.Blue,
				Philosophy = pooledMana.Philosophy
			};
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return BuffAction<PerfectServantUSe>(SynergyAmount(consumingMana, ManaColor.Blue) * Value2);
			yield return BuffAction<PerfectServantWSe>(SynergyAmount(consumingMana, ManaColor.White, 2) * Value1);

			yield return PerformAction.Effect(Battle.Player, "ExtraTime");
			yield return PerformAction.Sfx("ExtraTurnLaunch");
			yield return PerformAction.Animation(Battle.Player, "spell", 1.6f);
			yield return BuffAction<ExtraTurn>(1);
			yield return new RequestEndPlayerTurnAction();
			yield break;
		}
	}
}


