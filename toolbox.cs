using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace lvalonmeme
{
    public abstract class toolbox
    {
        public static string[] banlistgetter()
        {
            string[] arr2 = BepinexPlugin.cardban.Value.Split(',');
            string[] arr = new string[] { };
            string arrstring = "";
            if (BepinexPlugin.cardwhite.Value != "")
            {
                arr2 = BepinexPlugin.cardwhite.Value.Split(',');
                for (int i = 0; i < arr2.Length; i++)
                {
                    arr2[i] = arr2[i].ToLowerInvariant();
                }
                Card[] cards = toolbox.CreateAllCardsPoolList(new CardWeightTable(RarityWeightTable.AllOnes, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes, true), null);
                foreach (Card card in cards)
                {
                    if (!arr2.Contains(card.Id.ToLowerInvariant()))
                    {
                        arrstring += card.Id.ToLowerInvariant() + ",";
                    }
                }
                arr = arrstring.Split(',');
            }
            else
            {
                arr = BepinexPlugin.cardban.Value.Split(',');
            }
            return arr;
        }

        static public Card[] RollCardsCustomIgnore(RandomGen rng, CardWeightTable weightTable, int count, ManaGroup? manaLimit = null, bool colorLimit = false, bool applyFactors = false, bool battleRolling = false, bool ensureCount = false, Predicate<Card> filter = null)
        {
            GameRunController gr = GameMaster.Instance?.CurrentGameRun;
            if (gr == null)
                throw new InvalidOperationException("Rolling cards when run is not started.");

            UniqueRandomPool<Type> innitialPool = gr.CreateValidCardsPool(weightTable, manaLimit, colorLimit, applyFactors, battleRolling, null);

            UniqueRandomPool<Card> filteredPool = [];

            foreach (RandomPoolEntry<Type> e in innitialPool)
            {
                Card card = Library.CreateCard(e.Elem);
                if (filter(card))
                {
                    card.GameRun = gr;
                    filteredPool.Add(card, e.Weight);
                }
            }

            return filteredPool.SampleMany(rng, count, ensureCount);
        }
        static public Card[] RollCardsCustom(RandomGen rng, CardWeightTable weightTable, int count, ManaGroup? manaLimit = null, bool colorLimit = false, bool applyFactors = false, bool battleRolling = false, bool ensureCount = false, Predicate<Card> filter = null)
        {
            GameRunController gr = GameMaster.Instance?.CurrentGameRun;
            if (gr == null)
                throw new InvalidOperationException("Rolling cards when run is not started.");

            UniqueRandomPool<Type> innitialPool = gr.CreateValidCardsPool(weightTable, manaLimit, colorLimit, applyFactors, battleRolling, null);

            UniqueRandomPool<Card> filteredPool = [];

            foreach (RandomPoolEntry<Type> e in innitialPool)
            {
                Card card = Library.CreateCard(e.Elem);
                if (filter(card) && gr.BaseMana.CanAfford(card.Cost))
                {
                    card.GameRun = gr;
                    filteredPool.Add(card, e.Weight);
                }
            }

            return filteredPool.SampleMany(rng, count, ensureCount);
        }
        static public Card[] RepeatableAllCards(RandomGen rng, CardWeightTable weightTable, int count, bool ensureCount = false, Predicate<Card> filter = null)
        {
            GameRunController gr = GameMaster.Instance?.CurrentGameRun;
            if (gr == null)
                throw new InvalidOperationException("Rolling cards when run is not started.");

            UniqueRandomPool<Type> innitialPool = CreateAllCardsPool(weightTable, null);

            RepeatableRandomPool<Card> filteredPool = [];

            foreach (RandomPoolEntry<Type> e in innitialPool)
            {
                Card card = Library.CreateCard(e.Elem);
                if (filter(card))
                {
                    card.GameRun = gr;
                    filteredPool.Add(card, e.Weight);
                }
            }

            return filteredPool.SampleMany(rng, count, ensureCount);
        }
        static public Card[] UniqueAllCards(RandomGen rng, CardWeightTable weightTable, int count, bool ensureCount = false, Predicate<Card> filter = null)
        {
            GameRunController gr = GameMaster.Instance?.CurrentGameRun;
            if (gr == null)
                throw new InvalidOperationException("Rolling cards when run is not started.");

            UniqueRandomPool<Type> innitialPool = CreateAllCardsPool(weightTable, null);

            UniqueRandomPool<Card> filteredPool = [];

            foreach (RandomPoolEntry<Type> e in innitialPool)
            {
                Card card = Library.CreateCard(e.Elem);
                if (filter(card))
                {
                    card.GameRun = gr;
                    filteredPool.Add(card, e.Weight);
                }
            }

            return filteredPool.SampleMany(rng, count, ensureCount);
        }

        static public UniqueRandomPool<Type> CreateAllCardsPool(CardWeightTable weightTable, [MaybeNull] Predicate<CardConfig> filter = null)
        {
            var gr = GameMaster.Instance.CurrentGameRun;
            var charExSet = new HashSet<string>(gr.Player.Exhibits.Where(e => e.OwnerId != null).Select(e => e.OwnerId));
            UniqueRandomPool<Type> uniqueRandomPool = [];
            foreach (var item4 in EnumerateALLCardTypes())
            {
                Type item = item4.Item1;
                CardConfig item2 = item4.Item2;
                if (filter != null && !filter(item2))
                {
                    continue;
                }
                uniqueRandomPool.Add(item, 1);
            }
        
            return uniqueRandomPool;
        }
        static public Card[] CreateAllCardsPoolList(CardWeightTable weightTable, [MaybeNull] Predicate<CardConfig> filter = null)
        {
            return CreateAllCardsPool(weightTable, filter).ToList().Select(e => Library.CreateCard(e.Elem)).ToArray();
            // var gr = GameMaster.Instance.CurrentGameRun;
            // var charExSet = new HashSet<string>(gr.Player.Exhibits.Where(e => e.OwnerId != null).Select(e => e.OwnerId));
            // UniqueRandomPool<Type> uniqueRandomPool = new UniqueRandomPool<Type>();
            // foreach (var item4 in EnumerateALLCardTypes())
            // {
            //     Type item = item4.Item1;
            //     CardConfig item2 = item4.Item2;
            //     if (filter != null && !filter(item2))
            //     {
            //         continue;
            //     }
            // }

            //return uniqueRandomPool.ToList().Select(e => Library.CreateCard(e.Elem)).ToArray();
        }
        public static IEnumerable<(Type, CardConfig)> EnumerateALLCardTypes()
        {
            foreach (CardConfig item in CardConfig.AllConfig())
            {
                Type type2 = TypeFactory<Card>.TryGetType(item.Id);
                if ((object)type2 != null)
                {
                    yield return (type2, item);
                }
                // }
            }
        }
        public static IEnumerable<(Type, ExhibitConfig)> EnumerateALLExhibitTypes()
        {
            foreach (ExhibitConfig item in ExhibitConfig.AllConfig())
            {
                Type type = TypeFactory<Exhibit>.TryGetType(item.Id);
                if ((object)type != null)
                {
                    yield return (type, item);
                }
            }
        }
        static public UniqueRandomPool<Type> CreateAllExhibitsPool(ExhibitWeightTable weightTable, [MaybeNull] Predicate<ExhibitConfig> filter = null)
        {
            UniqueRandomPool<Type> uniqueRandomPool = [];
            foreach (var item4 in EnumerateALLExhibitTypes())
            {
                ExhibitConfig item2 = item4.Item2;
                if (filter != null && !filter(item2))
                {
                    continue;
                }
            }

            return uniqueRandomPool;
        }

        static public Card createcardwithidBACKUP(String id)
        {
            UniqueRandomPool<Type> uniqueRandomPool = [];
            foreach (var item4 in EnumerateALLCardTypes())
            {
                Type item = item4.Item1;
                CardConfig item2 = item4.Item2;
                if (item2.Id == id)
                {
                    uniqueRandomPool.Add(item, 1);
                }
            }
            UniqueRandomPool<Card> filteredPool = [];
            foreach (RandomPoolEntry<Type> e in uniqueRandomPool)
            {
                Card card = Library.CreateCard(e.Elem);
                filteredPool.Add(card, e.Weight);
            }
            Card rez = filteredPool.Sample(RandomGen.InitGen);
            //Card rez = Library.CreateCard();
            return rez;
        }

        //v2
        public static Card createcardwithid(String id)
        {
            return TypeFactory<Card>.CreateInstance(TypeFactory<Card>.TryGetType(id));
        }


        /* ONREVIVE

        private int oghp;
        private int ogmax;
        private int ogdmg;

        protected override void OnEnterBattle(BattleController battle)
        {
           base.HandleBattleEvent<DamageEventArgs>(base.Battle.Player.DamageTaking, new GameEventHandler<DamageEventArgs>(this.OnPlayerDamageTaking));
           base.HandleBattleEvent<DamageEventArgs>(base.Battle.Player.DamageReceived, new GameEventHandler<DamageEventArgs>(this.OnPlayerDamageReceived));
        }

        private void OnPlayerDamageTaking(DamageEventArgs args)
        {
           oghp = Owner.Hp;
           ogmax = Owner.MaxHp;
           ogdmg = args.DamageInfo.Damage.RoundToInt();
        }

        private void OnPlayerDamageReceived(DamageEventArgs args)
        {
           if (Owner.Hp == Owner.MaxHp && ogdmg > 0 && ((oghp - ogdmg != Owner.Hp - ogdmg) || ogdmg >= ogmax))
           {
               base.NotifyActivating();
           }
        }

        private void OnPlayerDamageTaking(DamageEventArgs args)
        {
           oghp = base.Battle.Player.Hp;
           ogmax = base.Battle.Player.MaxHp;
           ogdmg = args.DamageInfo.Damage.RoundToInt();
        }
        private void OnPlayerDamageReceived(DamageEventArgs args)
        {
           if (base.Battle.Player.Hp == base.Battle.Player.MaxHp && ogdmg > 0 && ((oghp - ogdmg != base.Battle.Player.Hp - ogdmg) || ogdmg >= ogmax))
           {
               base.NotifyActivating();
           }
        }

    How to make an Hybrid color card:
    config.Cost = new ManaGroup() { ..., Hybrid = amount, HybridColor = (int) color_code };

    color_code: 
    0: Azorius (White/Blue)
    1: Orzhov (White/Black)
    2: Boros (White/Red)
    3: Selesnya (White/Green)
    4: Dimir (Blue/Black)
    5: Izzet (Blue/Red)
    6: Simic (Blue/Green)
    7: Rakdos (Black/Red)
    8: Golgari (Black/Green)
    9: Gruul (Red/Green)

    Notation:
    x: MtG Guild (Color 1/Color 2)
    x being the number that has to be selected for the HybridColor field.

    Notes:
    It seems that a card can only have one type of Hybrid mana.
    */

        //checklist: save suppressed damage, and a card that deals suppressed damage
        //checklist: kms, lose 2 evil spirit (if evil spirit reaches -1, die), INNATE
        //checklist: end turn retain mana, ice block after death
        //checklist: lose life gain mana
        //checklist: lose life 4 times add a token
        //checklist: for each card used lose life
        //checklist: gain mb equal to life
    }
}