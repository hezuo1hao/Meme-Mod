using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Neutral.Red;
using System;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardMeilingBlockDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;

			config.Block = 8;
			config.Shield = 8;

			config.Colors = [ManaColor.Red];
			config.Cost = new ManaGroup { Any = 1, Red = 2 };
			config.UpgradedCost = new ManaGroup { Any = 1, Red = 1 };
			config.Rarity = Rarity.Uncommon;

			config.Type = CardType.Defense;

			config.RelativeCards = [nameof(MeilingBlock)];
			config.UpgradedRelativeCards = [nameof(MeilingBlock) + "+"];
			config.RelativeEffects = [nameof(seold)];
			config.UpgradedRelativeEffects = [nameof(seold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "失灵方程";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardMeilingBlockDef))]
	public sealed class cardMeilingBlock : lvalonmemecard.oldcard
	{
		public override int AdditionalBlock
		{
			get
			{
				if (Battle != null)
				{
					return PlayerFirepowerPositive;
				}
				return 0;
			}
		}
		public override int AdditionalShield
		{
			get
			{
				if (Battle != null)
				{
					return PlayerFirepowerPositive;
				}
				return 0;
			}
		}
		private int PlayerFirepowerPositive
		{
			get
			{
				return Math.Max(0, Battle.Player.TotalFirepower);
			}
		}
	}
}


