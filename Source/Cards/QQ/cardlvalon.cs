using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.EntityLib.Adventures;
using LBoL.Presentation;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Resource;
using lvalonmeme.Cards.Template;
using lvalonmeme.Packs;
using lvalonmeme.StatusEffects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace lvalonmeme.Cards
{
	public sealed class cardlvalonDef : lvalonmemecardtemplate
	{
		public override CardImages LoadCardImages()
		{
			var imgs = new CardImages(BepinexPlugin.embeddedSource);
			imgs.AutoLoad(this, ".png", hasUpgradeImage: true);
			return imgs;
		}
		public override CardConfig MakeConfig()
		{
			CardConfig config = GetCardDefaultConfig();
			config.IsPooled = true;
			config.HideMesuem = false;
			config.Owner = null;
			config.FindInBattle = false;

			config.Colors = [ManaColor.Blue];
			config.Rarity = Rarity.Rare;
			config.Mana = new ManaGroup { Philosophy = 1 };

			config.Type = CardType.Skill;
			config.Keywords = Keyword.Forbidden | Keyword.Ethereal;
			config.UpgradedKeywords = Keyword.Forbidden | Keyword.Replenish | Keyword.Plentiful;

			config.RelativeEffects = [nameof(sememe)];
			config.UpgradedRelativeEffects = [nameof(sememe)];

			config.Pack = nameof(packmemeDef)[..^3];

			config.Illustrator = "ジェット虚無僧 / minusT";

			config.UpgradeImageId = $"{GetId()}{CardImages.upgradeString}";

			config.Index = CardIndexGenerator.GetUniqueIndex(config);
			return config;
		}
	}

	[EntityLogic(typeof(cardlvalonDef))]
	public sealed class cardlvalon : lvalonmemecard.memecard
	{
		public static int CanReward;
        public override ManaGroup? PlentifulMana
		{
			get
			{
				if (IsUpgraded)
				{
					return new ManaGroup?(Mana);
				}
				else
				{
					return new ManaGroup { Philosophy = 0 };
				}
			}
		}
        public override void Initialize()
        {
            base.Initialize();
            SetKeyword(Keyword.Plentiful, false);
        }
		protected override void OnEnterBattle(BattleController battle)
		{
			HandleBattleEvent(Battle.BattleEnding, OnBattleEnding);
		}

		private void OnBattleEnding(GameEventArgs args)
		{
			if (Zone != CardZone.Exile)
                CanReward++;
		}
    }
    [HarmonyPatch(typeof(Stage), nameof(Stage.GetEnemyCardReward))]
    class Stage_GetEnemyCardReward_PostPatch
    {
        public static int canReward = -1;
        static bool Prefix(Stage __instance)
		{
            if (canReward == -1)
                canReward = cardlvalon.CanReward;

            if (canReward > 0)
            {
                canReward--;
                Singleton<GameMaster>.Instance.CurrentGameRun.CurrentStation.AddReward(__instance.GetEnemyCardReward());
            }
            return true;
		}
        static void Postfix(Stage __instance)
        {
			canReward = -1;
        }
    }
    [HarmonyPatch(typeof(Stage), nameof(Stage.GetEliteEnemyCardReward))]
    class Stage_GetEliteEnemyCardReward_PostPatch
    {
        public static int canReward = -1;
        static void Postfix(Stage __instance)
        {
            if (canReward == -1)
                canReward = cardlvalon.CanReward;

            if (canReward > 0)
            {
                canReward--;
                Singleton<GameMaster>.Instance.CurrentGameRun.CurrentStation.AddReward(__instance.GetEliteEnemyCardReward());
            }
        }
    }
    [HarmonyPatch(typeof(Stage), nameof(Stage.GetBossCardReward))]
    class Stage_GetBossCardReward_PostPatch
    {
        public static int canReward = -1;
        static void Postfix(Stage __instance)
        {
            if (canReward == -1)
                canReward = cardlvalon.CanReward;

            if (canReward > 0)
            {
                canReward--;
                Singleton<GameMaster>.Instance.CurrentGameRun.CurrentStation.AddReward(__instance.GetBossCardReward());
            }
        }
    }
    [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
    class GameRunController_EnterBattle_PostPatch
    {
        static void Postfix(Stage __instance)
        {
            cardlvalon.CanReward = 0;
            Stage_GetEnemyCardReward_PostPatch.canReward = -1;
            Stage_GetEliteEnemyCardReward_PostPatch.canReward = -1;
            Stage_GetBossCardReward_PostPatch.canReward = -1;
        }
    }
}


