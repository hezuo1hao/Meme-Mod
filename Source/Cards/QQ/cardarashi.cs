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
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Red;
using lvalonmeme.StatusEffects;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardarashiDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 2;
			config.UpgradedValue1 = 3;

			config.Colors = [ManaColor.Red];
			config.Cost = new ManaGroup { Any = 2, Red = 3 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.Keywords = Keyword.Retain;
			config.UpgradedKeywords = Keyword.Retain;

			config.RelativeCards = [nameof(MeihongPower)];
			config.UpgradedRelativeCards = [nameof(MeihongPower)];
			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "Doro";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardarashiDef))]
	public sealed class cardarashi : lvalonmemecard.memecard
	{
		public override IEnumerable<BattleAction> OnRetain()
		{
			if (Zone == CardZone.Hand)
			{
				NotifyActivating();
				yield return new ApplyStatusEffectAction<Firepower>(Battle.Player, Value1, null, null, null, 0f, true);
			}
			yield break;
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (Battle.Player.HasStatusEffect<Firepower>())
			{
				yield return new ApplyStatusEffectAction<TempFirepower>(Battle.Player, Battle.Player.StatusEffects.Where(se => se is Firepower).First().Level, null, null, null, 0f, true);
			}
			yield break;
		}
	}
}


