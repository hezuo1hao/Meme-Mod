using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Presentation;
using lvalonmeme.Cards;

namespace VanillaTweaks
{
	// [OverwriteVanilla]
	// public sealed class QiannianShenqiCardDefinition : CardTemplate
	// {
	//     public override IdContainer GetId()
	//     {
	//         return nameof(QiannianShenqiCard);
	//     }
	//     [DontOverwrite]
	//     public override CardImages LoadCardImages()
	//     {
	//         return null;
	//     }
	//     [DontOverwrite]
	//     public override LocalizationOption LoadLocalization()
	//     {
	//         return null;
	//     }

	//     [DontOverwrite]
	//     public override CardConfig MakeConfig()
	//     {
	//         return null;
	//     }
	//     [EntityLogic(typeof(QiannianShenqiCardDefinition))]
	//     public sealed class QiannianShenqiCard : Card
	//     {
	//         protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
	//         {
	//             lvalonmeme.BepinexPlugin.log.LogDebug("lvalonmeme im in actions");
	//             StatusEffect se0 = Battle.Player.StatusEffects.FirstOrDefault((StatusEffect se) => se is QiannianShenqiSe);
	//             lvalonmeme.BepinexPlugin.log.LogDebug("lvalonmeme i got se " + se0.Id);
	//             if (se0 != null)
	//             {
	//                 if (se0.Level != Value1)
	//                 {
	//                     se0.Level = Value1;
	//                 }

	//                 ((QiannianShenqiSe)se0).LoseLifeVersion = true;
	//                 lvalonmeme.BepinexPlugin.log.LogDebug("lvalonmeme i set lose life vesion to " + ((QiannianShenqiSe)se0).LoseLifeVersion);
	//             }
	//             lvalonmeme.BepinexPlugin.log.LogDebug("lvalonmeme im done");
	//             EnemyUnit enemyUnit = Battle.EnemyGroup.FirstOrDefault((EnemyUnit u) => u is Seija && u.IsAlive);
	//             if (enemyUnit == null)
	//             {
	//                 yield break;
	//             }

	//             StatusEffect statusEffect = enemyUnit.StatusEffects.FirstOrDefault((StatusEffect se) => se is QiannianShenqiSe);
	//             if (statusEffect != null)
	//             {
	//                 if (statusEffect.Level != Value1)
	//                 {
	//                     statusEffect.Level = Value1;
	//                 }

	//                 ((QiannianShenqiSe)statusEffect).LoseLifeVersion = true;
	//             }
	//         }
	//     }
	// }
	[OverwriteVanilla]
	public sealed class ColdHeartedDefinition : CardTemplate
	{
		public override IdContainer GetId()
		{
			// must return the Id of the entity which is going to be overwritten
			return nameof(ColdHearted);
		}

		// don't overwrite attribute makes the Sideloader ignore the method and leave the entity component the same
		[DontOverwrite]
		public override CardImages LoadCardImages()
		{
			return null;
		}
		[DontOverwrite]
		public override LocalizationOption LoadLocalization()
		{
			return null;
		}

		[DontOverwrite]
		public override CardConfig MakeConfig()
		{
			return null;
		}
		[EntityLogic(typeof(ColdHeartedDefinition))]
		public sealed class ColdHearted : Card
		{
			protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
			{
				yield return BuffAction<ColdHeartedSe>(0, 0, 0, 0, 0.2f);
				if (GameMaster.Instance.CurrentGameRun.Packs.Contains(nameof(lvalonmeme.Packs.packoldDef)[..^3]))
				{
					List<Card> list = [];
					for (int i = 0; i < Value1; i++)
					{
						list.Add(Library.CreateCard(nameof(cardIceLance)));
					}
					foreach (Card iceLance in list)
					{
						iceLance.SetTurnCost(Mana);
						iceLance.IsEthereal = true;
						iceLance.IsExile = true;
					}
					yield return new AddCardsToHandAction(list, AddCardsType.Normal);
				}
				else
				{
					List<IceLance> list = Library.CreateCards<IceLance>(Value1, false).ToList();
					foreach (IceLance iceLance in list)
					{
						iceLance.SetTurnCost(Mana);
						iceLance.IsEthereal = true;
						iceLance.IsExile = true;
					}
					yield return new AddCardsToHandAction(list, AddCardsType.Normal);
				}
				yield break;
			}
		}
	}
}