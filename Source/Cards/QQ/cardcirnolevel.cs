using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardcirnolevelDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Value1 = 1;
			config.Value2 = 1;
			config.UpgradedValue2 = 2;

			config.Colors = [ManaColor.White, ManaColor.Red];
			config.Cost = new ManaGroup { White = 1, Red = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(ShengtianKick), nameof(ShopDefense)];
			config.UpgradedRelativeCards = [nameof(ShengtianKick) + "+", nameof(ShopDefense) + "+"];

			config.RelativeEffects = [nameof(secirnolevel), nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(secirnolevel), nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "这么小块的尸块";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardcirnolevelDef))]
	public sealed class cardcirnolevel : lvalonmemecard.memecard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			yield return new AddCardsToHandAction(Library.CreateCards<ShengtianKick>(Value1, IsUpgraded), AddCardsType.Normal);
			yield return new AddCardsToHandAction(Library.CreateCards<ShopDefense>(Value1, IsUpgraded), AddCardsType.Normal);
			yield return new ApplyStatusEffectAction<secirnolevel>(Battle.Player, Value2, null, null, null, 0f, true);
			yield break;
		}
	}
}