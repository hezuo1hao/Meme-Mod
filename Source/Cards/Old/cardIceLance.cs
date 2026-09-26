using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using lvalonmeme.Cards.Template;
using LBoL.Core.Battle;
using LBoL.Core;
using System.Linq;
using lvalonmeme.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.Units;
using lvalonmeme.Packs;

namespace lvalonmeme.Cards
{
	public sealed class cardIceLanceDef : lvalonmemecardtemplate
	{
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = "Cirno";

			config.TargetType = TargetType.SingleEnemy;

			config.Damage = 8;
			config.UpgradedDamage = 9;

			config.Value1 = 3;
			config.UpgradedValue1 = 4;
			config.Value2 = 3;
			config.UpgradedValue2 = 4;

			config.GunName = "冰枪术";
			config.GunNameBurst = "冰枪术B";

			config.Colors = [ManaColor.Blue];
			config.Cost = new ManaGroup { Blue = 1 };
			config.UpgradedCost = new ManaGroup { Any = 1 };
			config.Rarity = Rarity.Common;

			config.Type = CardType.Attack;

			config.Keywords = Keyword.Accuracy;
			config.UpgradedKeywords = Keyword.Accuracy;

			config.RelativeCards = [nameof(IceLance)];
			config.UpgradedRelativeCards = [nameof(IceLance) + "+"];
			config.RelativeEffects = [nameof(seold), nameof(Cold)];
			config.UpgradedRelativeEffects = [nameof(seold), nameof(Cold)];

			config.Pack = nameof(packoldDef)[..^3];

			config.Illustrator = "tojorin";

			config.Index = CardIndexGenerator.GetUniqueIndex(config, "old");
			return config;
		}
	}

	[EntityLogic(typeof(cardIceLanceDef))]
	public sealed class cardIceLance : lvalonmemecard.oldcard
	{
		public override int AdditionalValue1
		{
			get
			{
				if (Battle == null || !Battle.Player.HasStatusEffect<ColdHeartedSe>())
				{
					return 0;
				}
				return 3;
			}
		}
		protected override void OnEnterBattle(BattleController battle)
		{
			HandleBattleEvent(Battle.Player.DamageDealing, new GameEventHandler<DamageDealingEventArgs>(OnPlayerDamageDealing), GameEventPriority.ConfigDefault);
		}
		private void OnPlayerDamageDealing(DamageDealingEventArgs args)
		{
			if (args.ActionSource == this && args.Targets != null)
			{
				if (args.Targets.Any((Unit target) => target.HasStatusEffect<Cold>()))
				{
					args.DamageInfo = args.DamageInfo.MultiplyBy(Value1);
					args.AddModifier(this);
				}
			}
		}
		protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
		{
			string gunName = GunName;
			if (Battle.Player.HasStatusEffect<ColdHeartedSe>())
			{
				gunName = "血脉" + GunName;
			}
			yield return AttackAction(selector, gunName);
			yield break;
		}
	}
}


