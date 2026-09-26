using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;

namespace lvalonmeme.StatusEffects
{
    public sealed class seYonglinCardDef : lvalonmemesetemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasCount = true;
            config.Order = 11;
            return config;
        }
    }

    [EntityLogic(typeof(seYonglinCardDef))]
    public sealed class seYonglinCard : StatusEffect
    {
        public ManaGroup Mana
        {
            get
            {
                return ManaGroup.Anys(1);
            }
        }
        public bool hold = true;
        protected override void OnAdded(Unit unit)
        {
            Count = Level;
            Highlight = true;
            ReactOwnerEvent(Battle.CardUsed, OnCardUsed);
            ReactOwnerEvent(Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnTurnStarting));
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            Card card = args.Card;
            if (Battle.BattleShouldEnd || !card.CanBeDuplicated || Count == 0 || hold)
            {
                hold = false;
                yield break;
            }
            NotifyActivating();
            Card card2 = card.CloneBattleCard();
            card2.SetTurnCost(Mana);
            card2.IsExile = true;
            card2.IsEthereal = true;
            List<Card> cards =
            [
                card2
            ];
            yield return new AddCardsToHandAction(cards, AddCardsType.Normal);
            if (Count > 0) {
                Count--;
            }
            if (Count == 0)
            {
                Highlight = false;
            }
            yield break;
        }
        private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
        {
            if (Battle.BattleShouldEnd)
            {
                yield break;
            }
            Count = Level;
            Highlight = true;
            yield break;
        }
    }
}