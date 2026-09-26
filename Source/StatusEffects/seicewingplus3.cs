using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader.Attributes;

namespace lvalonmeme.StatusEffects
{
	public sealed class seicewingplus3Def : lvalonmemesetemplate
	{
		public override StatusEffectConfig MakeConfig()
		{
			StatusEffectConfig config = GetDefaultStatusEffectConfig();
			config.Type = StatusEffectType.Positive;
			config.RelativeEffects = [nameof(Cold)];
			return config;
		}
	}

	[EntityLogic(typeof(seicewingplus3Def))]
	public sealed class seicewingplus3 : StatusEffect
	{
		protected override void OnAdded(Unit unit)
		{
			ReactOwnerEvent(Owner.TurnStarting, OnTurnStarting);
		}

		private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
		{
			// 每回合开始时随机一个敌人获得寒冷
			for (int i = 0; i < Level; i++)
			{
				if (Battle.BattleShouldEnd) { yield break; }
				Unit unit = Battle.RandomAliveEnemy;
				if (unit == null) { yield break; }
				yield return new ApplyStatusEffectAction<Cold>(unit);
			}
		}
	}
}