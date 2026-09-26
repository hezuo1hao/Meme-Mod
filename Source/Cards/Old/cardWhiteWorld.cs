using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.Marisa;
using LBoL.Core.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardWhiteWorldDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.Value1 = 3;
			config.Mana = new ManaGroup() { Any = 1 };

			config.Colors = [ManaColor.White];
			config.Cost = new ManaGroup { Any = 2, White = 1 };
			config.Rarity = Rarity.Rare;

			config.Type = CardType.Ability;

			config.RelativeCards = [nameof(WhiteWorld)];
			config.UpgradedRelativeCards = [nameof(WhiteWorld) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(ManaFreezed)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(ManaFreezed)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "breedo";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardWhiteWorldDef))]
	public sealed class cardWhiteWorld : lvalonmemecard.oldcard
	{
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			if (IsUpgraded)
			{
				yield return BuffAction<Firepower>(Value1, 0, 0, 0, 0.2f);
				yield return BuffAction<Spirit>(Value1, 0, 0, 0, 0.2f);
			}

            int count = 1;
			if (Battle.Player.TryGetStatusEffect<seWhiteWorld>(out var whiteWorld))
				count = whiteWorld.Count++;

            yield return new ApplyStatusEffectAction<seWhiteWorld>(Battle.Player, Value1, null, count, null, 0f, true);
			yield break;
		}
	}
}


