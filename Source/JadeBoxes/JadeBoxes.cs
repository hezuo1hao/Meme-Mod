using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Mythic;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.StatusEffects.Enemy.Seija;
using LBoL.EntityLib.StatusEffects.Koishi;
using LBoL.Presentation;
using LBoL.Presentation.Units;
using LBoLEntitySideloader.Attributes;
using lvalonmeme.Exhibits;
using UnityEngine;


namespace lvalonmeme.JadeBoxes
{
	// public class JadeBoxEnableMemes
	// {
	// 	public sealed class JadeBoxEnableMemesDef : lvalonmemejadeboxtemplate
	// 	{
	// 		public override JadeBoxConfig MakeConfig()
	// 		{
	// 			var config = DefaultConfig();
	// 			return config;
	// 		}


	// 		[EntityLogic(typeof(JadeBoxEnableMemesDef))]
	// 		public sealed class JadeBoxEnableMemes : JadeBox
	// 		{
	// 		}
	// 	}
	// }
	// public class JadeBoxEnableOld
	// {
	// 	public sealed class JadeBoxEnableOldDef : lvalonmemejadeboxtemplate
	// 	{
	// 		public override JadeBoxConfig MakeConfig()
	// 		{
	// 			var config = DefaultConfig();
	// 			return config;
	// 		}


	// 		[EntityLogic(typeof(JadeBoxEnableOldDef))]
	// 		public sealed class JadeBoxEnableOld : JadeBox
	// 		{
	// 			protected override void OnAdded()
	// 			{
	// 				HandleGameRunEvent(GameRun.StationEntered, OnStationEntered);
	// 			}
	// 			protected override void OnGain(GameRunController gameRun)
	// 			{
	// 				GameMaster.Instance.StartCoroutine(RemoveFromPool(gameRun));
	// 			}
	// 			private IEnumerator RemoveFromPool(GameRunController gameRun)
	// 			{
	// 				if (BepinexPlugin.oldcard.Value)
	// 				{
	// 					List<Card> card = new List<Card>{
	// 					Library.CreateCard<cardold>()
	// 				};
	// 					string[] arr = toolbox.banlistgetter();
	// 					bool inban = false;
	// 					foreach (string s in arr)
	// 					{
	// 						if (s.ToLowerInvariant() == card[0].Id.ToLowerInvariant())
	// 						{
	// 							inban = true;
	// 							break;
	// 						}
	// 					}
	// 					if (!inban)
	// 					{
	// 						gameRun.AddDeckCards(card, false, null);
	// 					}
	// 				}
	// 				yield return null;
	// 			}
	// 			public void OnStationEntered(StationEventArgs args)
	// 			{
	// 				var gameRun = GameMaster.Instance.CurrentGameRun;
	// 				EntryStation entryStation = args.Station as EntryStation;
	// 				if (entryStation != null && GameRun.Stages.IndexOf(entryStation.Stage) == 0)
	// 				{
	// 					List<Card> list = (from card in gameRun.BaseDeck
	// 									   where CustomGameEventManager.GetList("oldban").Contains(card.Id)
	// 									   select card).ToList();
	// 					foreach (Card card in list)
	// 					{
	// 						switch (card.Id)
	// 						{
	// 							case nameof(YaoguaiBuster):
	// 								gameRun.RemoveDeckCard(card, true);
	// 								gameRun.AddDeckCard(Library.CreateCard<cardYaoguaiBuster>());
	// 								break;
	// 							case nameof(IceLance):
	// 								gameRun.RemoveDeckCard(card, true);
	// 								gameRun.AddDeckCard(Library.CreateCard<cardIceLance>());
	// 								break;
	// 						}
	// 					}
	// 				}
	// 			}

	// 			private void UpgradeAllCards()
	// 			{
	// 				try
	// 				{
	// 					//always upgrae all card in the deck in case several card are added at once
	// 					Debug.Log("cards in deck: " + GameRun.BaseDeck.Count);
	// 					foreach (var card in GameRun.BaseDeck)
	// 					{
	// 						//check if card is not upgraded
	// 						if (card.CanUpgrade && card.CanUpgradeAndPositive && !card.IsUpgraded)
	// 						{
	// 							card.Upgrade();
	// 						}
	// 					}
	// 				}
	// 				catch (Exception e)
	// 				{
	// 					Debug.LogError(" exception in UpgradeAllCards: " + e.Message + e.StackTrace);
	// 				}
	// 			}
	// 		}
	// 	}
	// }
	public class JadeBox5Color
	{
		public sealed class JadeBox5ColorDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}
			[EntityLogic(typeof(JadeBox5ColorDef))]
			public sealed class JadeBox5Color : JadeBox
			{
				protected override void OnGain(GameRunController gameRun)
				{
					GameRun.BaseMana = new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 1 };
				}
				protected override void OnEnterBattle()
				{
					if (BepinexPlugin.color5support.Value && GameRun.CurrentStation.Act == 1 && GameRun.Stages.IndexOf(GameRun.CurrentStation.Stage) == 0 && GameRun.CurrentStation.Level == 1)
					{
						ReactBattleEvent(Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(OnBattleStated));
					}
				}

				private IEnumerable<BattleAction> OnBattleStated(GameEventArgs args)
				{
					if (BepinexPlugin.color5support.Value)
					{
						foreach (Card card in GameMaster.Instance.CurrentGameRun.BaseDeck)
						{
							if (card.CanUpgradeAndPositive)
							{
								card.Upgrade();
							}
						}
						foreach (Card card in Battle.EnumerateAllCards())
						{
							if (card.CanUpgradeAndPositive)
							{
								card.Upgrade();
							}
						}
					}
					yield break;
				}
				//overwrite ExchangeExhibit method to remove rainbow mana because there is no more mana of the original color to remove
				[HarmonyPatch(typeof(Debut), nameof(Debut.ExchangeExhibit))]
				class BanExhibitSwap_Patch
				{
					static void Prefix(Debut __instance)
					{
					}
				}
			}
		}
	}
	public class JadeBox7Color
	{
		public sealed class JadeBox7ColorDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}
			[EntityLogic(typeof(JadeBox7ColorDef))]
			public sealed class JadeBox7Color : JadeBox
			{
				protected override void OnGain(GameRunController gameRun)
				{
					GameRun.BaseMana = new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 1, Colorless = 1, Philosophy = 1 };
				}
				//overwrite ExchangeExhibit method to remove rainbow mana because there is no more mana of the original color to remove
				[HarmonyPatch(typeof(Debut), nameof(Debut.ExchangeExhibit))]
				class BanExhibitSwap_Patch
				{
					static void Prefix(Debut __instance)
					{
					}
				}
			}
		}
	}
	public class JadeBoxSwapBlank
	{
		public sealed class JadeBoxSwapBlankDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}


			[EntityLogic(typeof(JadeBoxSwapBlankDef))]
			public sealed class JadeBoxSwapBlank : JadeBox
			{
				public ManaColor? ManaColor { get; set; }
				protected override void OnGain(GameRunController gameRun)
				{
					bool retain = GameRun.JadeBoxes.Any(jb => jb.Id == nameof(JadeBox5Color) || jb.Id == nameof(JadeBox7Color));
					ManaGroup BaseMana = GameRun.BaseMana;
					ManaColor = GameRun.Player.Exhibits.First(e => e.Config.Rarity == Rarity.Shining).Config.BaseManaColor ?? GameRun.BaseMana.EnumerateComponents().ToList().Sample(gameRun.RootRng);
					//GameRun.BaseMana = new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 1 };
					GameRun.Player.RemoveExhibit(GameRun.Player.Exhibits.First(e => e.Config.Rarity == Rarity.Shining));
					Exhibit rolled = gameRun.RollShiningExhibit(gameRun.ShiningExhibitRng, null, config => config.Id.ToLowerInvariant() == BepinexPlugin.blankid.Value.ToLowerInvariant());
					rolled ??= gameRun.RollShiningExhibit(gameRun.ShiningExhibitRng, null, config => config.Id.ToLowerInvariant() == "kongbaikapai");
					gameRun.GainExhibitInstantly(rolled, false, null);
					//GameMaster.DebugGainExhibit(rolled);
					if (retain)
					{
						GameRun.BaseMana = BaseMana;
					}
					else if (ManaColor != null)
					{
						GameRun.BaseMana -= ManaGroup.FromColor((ManaColor)ManaColor, 1);
					}
				}
			}
		}
	}
	public class JadeBoxBalance
	{
		public sealed class JadeBoxBalanceDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				config.Mana = new ManaGroup { Any = 1 };
				return config;
			}


			[EntityLogic(typeof(JadeBoxBalanceDef))]
			public sealed class JadeBoxBalance : JadeBox
			{
				int topmana = BepinexPlugin.balancetop.Value;
				int botmana = BepinexPlugin.balancebot.Value;
				string[] blacklist = BepinexPlugin.balanceban.Value.Split(',').Select(s => s.ToLowerInvariant()).ToArray();
				public ManaGroup AuraMana { get; set; }
				protected override void OnEnterBattle()
				{
					AuraMana = Mana;
					foreach (Card card in Battle.EnumerateAllCards())
					{
						if (!blacklist.Contains(card.Config.Id.ToLowerInvariant()))
						{
							if (card.ConfigCost.Amount <= botmana && !card.Config.Keywords.HasFlag(Keyword.Basic) && !card.IsXCost)
							{
								card.AuraCost += AuraMana;
							}
							if (card.ConfigCost.Amount >= topmana)
							{
								card.DecreaseBaseCost(ManaGroup.FromComponents(card.Cost.EnumerateComponents().SampleManyOrAll(1, GameRun.BattleRng)));
							}
						}
					}
					HandleBattleEvent(Battle.CardsAddedToDiscard, new GameEventHandler<CardsEventArgs>(OnAddCard));
					HandleBattleEvent(Battle.CardsAddedToHand, new GameEventHandler<CardsEventArgs>(OnAddCard));
					HandleBattleEvent(Battle.CardsAddedToExile, new GameEventHandler<CardsEventArgs>(OnAddCard));
					HandleBattleEvent(Battle.CardsAddedToDrawZone, new GameEventHandler<CardsAddingToDrawZoneEventArgs>(OnAddCardToDraw));
					HandleBattleEvent(Battle.CardTransformed, new GameEventHandler<CardTransformEventArgs>(OnCardTransformed));
				}
				private void OnAddCard(CardsEventArgs args)
				{
					SetMana(args.Cards);
				}
				private void OnAddCardToDraw(CardsAddingToDrawZoneEventArgs args)
				{
					SetMana(args.Cards);
				}
				private void OnCardTransformed(CardTransformEventArgs args)
				{
					SetMana(args.DestinationCard);
				}
				private void SetMana(Card card)
				{
					if (!blacklist.Contains(card.Config.Id.ToLowerInvariant()))
					{
						if (card.ConfigCost.Amount <= botmana && !card.Config.Keywords.HasFlag(Keyword.Basic) && !card.IsXCost)
						{
							card.AuraCost += AuraMana;
						}
						if (card.ConfigCost.Amount >= topmana)
						{
							card.DecreaseBaseCost(ManaGroup.FromComponents(card.Cost.EnumerateComponents().SampleManyOrAll(1, GameRun.BattleRng)));
						}
					}
				}
				private void SetMana(IEnumerable<Card> cards)
				{
					foreach (Card card in cards)
					{
						if (!blacklist.Contains(card.Config.Id.ToLowerInvariant()))
						{
							if (card.ConfigCost.Amount <= botmana && !card.Config.Keywords.HasFlag(Keyword.Basic) && !card.IsXCost)
							{
								card.AuraCost += AuraMana;
							}
							if (card.ConfigCost.Amount >= topmana)
							{
								card.DecreaseBaseCost(ManaGroup.FromComponents(card.Cost.EnumerateComponents().SampleManyOrAll(1, GameRun.BattleRng)));
							}
						}
					}
				}

			}
		}
	}
	public class JadeBoxFav
	{
		public sealed class JadeBoxFavDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}


			[EntityLogic(typeof(JadeBoxFavDef))]
			public sealed class JadeBoxFav : JadeBox
			{
				int countdown = 0;
				protected override void OnGain(GameRunController gameRun)
				{
					countdown = 0;
					GameRun.RemoveGamerunInitialCards();
					if (!GameRun.JadeBoxes.Any(jb => jb.Id == nameof(JadeBox5Color) || jb.Id == nameof(JadeBox7Color)))
					{
						GameRun.BaseMana = new ManaGroup { Any = 0 };
					}
				}
				protected override void OnAdded()
				{
					countdown = 0;
					HandleGameRunEvent(GameRun.StationEntered, delegate (StationEventArgs args)
					{
						EntryStation entryStation = args.Station as EntryStation;
						if (entryStation != null && GameRun.Stages.IndexOf(entryStation.Stage) == 0)
						{
							args.Station.PreDialogs.Add(new StationDialogSource("StartupSelectCard", this));
						}
					});
				}
				[RuntimeCommand("select", "")]
				public IEnumerator Select()
				{
					if (countdown < 9)
					{
						countdown++;
						yield return new WaitForSeconds(0.1f);
					}
					else
					{
						Card[] cards = new Card[0];
						if (BepinexPlugin.favdebug.Value)
						{
							cards = toolbox.CreateAllCardsPoolList(new CardWeightTable(RarityWeightTable.AllOnes, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes, true), null);
						}
						else
						{
							cards = toolbox.CreateAllCardsPoolList(new CardWeightTable(RarityWeightTable.AllOnes, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes, true), config => config.DebugLevel < 1);
						}
						GameRun.UpgradeNewDeckCardOnFlags(cards);
						SelectCardInteraction interaction = new SelectCardInteraction(1, 1, cards, SelectedCardHandling.DoNothing)
						{
							Source = this
						};
						yield return GameRun.InteractionViewer.View(interaction);
						Card card = interaction.SelectedCards[0];
						GameRun.AddDeckCards(new Card[]
						{
						interaction.SelectedCards[0]
						}, false, null);
						if (BepinexPlugin.favpure.Value)
						{
							GameRun.AddDeckCards(new Card[]
							{
							Library.CreateCard<Shoot>(true),
							Library.CreateCard<Shoot>(true),
							Library.CreateCard<Shoot>(true),
							Library.CreateCard<Shoot>(true),
							Library.CreateCard<Shoot>(true),
							Library.CreateCard<Boundary>(true),
							Library.CreateCard<Boundary>(true),
							Library.CreateCard<Boundary>(true),
							Library.CreateCard<Boundary>(true),
							Library.CreateCard<Boundary>(true),
							});
						}
						if (!GameRun.JadeBoxes.Any(jb => jb.Id == nameof(JadeBox5Color) || jb.Id == nameof(JadeBox7Color)))
						{
							ManaGroup mana = card.CostToMana(true);
							mana.Philosophy += card.Config.Cost.Any;
							mana.Colorless -= card.Config.Cost.Any;
							if (mana.Amount < 5)
							{ // add till 5 base mana
								mana.Philosophy += 5 - mana.Amount;
							}
							if (!mana.HasTrivial || card.Cost.Hybrid > 0)
							{
								// int rewardAndShopCardColorLimitFlag = GameRun.RewardAndShopCardColorLimitFlag + 1;
								// GameRun.RewardAndShopCardColorLimitFlag = rewardAndShopCardColorLimitFlag;
								if (mana.Philosophy == 0)
								{
									mana.Colorless--;
									mana.Philosophy++;
								}
								if (!GameRun.Player.Exhibits.Any(e => e.Config.Id == nameof(KongbaiKapai)))
								{
									//GameMaster.Instance.CurrentGameRun.GainExhibitInstantly(Library.CreateExhibit(nameof(KongbaiKapai)), true, null);
									GameMaster.DebugGainExhibit(Library.CreateExhibit(nameof(KongbaiKapai)));
									mana.Philosophy--;
								}
							}
							if (!BepinexPlugin.favpure.Value)
							{
								mana.Philosophy += 1;
							}
							GameRun.BaseMana = mana;
							if (!GameRun.Player.Exhibits.Any(e => e.Config.Id == nameof(XianzheShi)))
							{
								GameRun.GainExhibitInstantly(Library.CreateExhibit(nameof(XianzheShi)), false, null);
								GameRun.LoseExhibit(GameRun.Player.Exhibits.First(e => e.Config.Id == nameof(XianzheShi)), false, true);
							}
							else
							{
								GameRun.GainExhibitInstantly(Library.CreateExhibit(nameof(LongjingYu)), false, null);
								GameRun.LoseExhibit(GameRun.Player.Exhibits.First(e => e.Config.Id == nameof(LongjingYu)), false, true);
							}
						}
						yield return new WaitForSeconds(0.5f);
					}
					yield break;
				}
				[HarmonyPatch(typeof(Debut), nameof(Debut.ExchangeExhibit))]
				class BanExhibitSwap_Patch
				{
					static void Prefix(Debut __instance)
					{
						var run = GameMaster.Instance.CurrentGameRun;
						if (run.JadeBoxes.Any(jb => jb.Id == nameof(JadeBoxFav)))
						{
							Exhibit ex = run.Player.Exhibits[0];
							ManaColor valueOrDefault = ex.Config.BaseManaColor.GetValueOrDefault();
							//ex.Config.BaseManaColor * ex.Config.BaseManaAmount;
							if (!run.BaseMana.HasColor(valueOrDefault))
							{
								for (int i = 0; i < ex.Config.BaseManaAmount; i++)
								{
									if (run.BaseMana.Amount >= 1)
									{
										if (run.BaseMana.Philosophy > 0)
										{
											run.LoseBaseMana(ManaGroup.Single(ManaColor.Philosophy));
										}
										else
										{
											run.LoseBaseMana(ManaGroup.Single(run.BaseMana.EnumerateComponents().ToList().Sample(run.RootRng)));
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
	public class JadeBoxDouble
	{
		public sealed class JadeBoxDoubleDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}
			[EntityLogic(typeof(JadeBoxDoubleDef))]
			public sealed class JadeBoxDouble : JadeBox
			{
				// bool enlarged = false;
				List<Unit> blacklist = [];
				private readonly List<(Card, CardUsingEventArgs)> AttackEchoArgs = [];
				protected override void OnEnterBattle()
				{
					ReactBattleEvent(Battle.Player.TurnStarted, OnTurnStarted);
					HandleBattleEvent(Battle.CardUsing, OnCardUsing);
					ReactBattleEvent(Battle.CardUsed, OnCardUsed);
					ReactBattleEvent(Battle.Player.TurnStarting, OnBattleStarting);
					HandleBattleEvent(Battle.EnemySpawned, OnEnemySpawned);
				}
				private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
				{

					if (!Battle.BattleShouldEnd && BepinexPlugin.doublemana.Value)
					{
						NotifyActivating();
						yield return new GainManaAction(GameRun.BaseMana);
					}
					yield break;
				}
				private void OnCardUsing(CardUsingEventArgs args)
				{
					if (BepinexPlugin.doubleplay.Value)
					{
						Card token = args.Card.CloneTwiceToken();
						token.IsPlayTwiceToken = true;
						token.PlayTwiceSourceCard = args.Card;
						AttackEchoArgs.Add((token, args.Clone()));
					}
				}
				private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
				{
					if (args.Card is QiannianShenqiCard)
					{
						StatusEffect se0 = Battle.Player.StatusEffects.FirstOrDefault((StatusEffect se) => se is QiannianShenqiSe);
						if (se0 != null)
						{
							if (se0.Level != 2)
							{
								se0.Level = 2;
							}

							((QiannianShenqiSe)se0).LoseLifeVersion = true;
						}
					}
					foreach ((Card card, CardUsingEventArgs aargs) in AttackEchoArgs)
					{
						yield return new PlayTwiceAction(card, aargs);
					}
					AttackEchoArgs.Clear();
					yield break;
				}
				private IEnumerable<BattleAction> OnBattleStarting(GameEventArgs args)
				{
					if (!Battle.Player.TryGetStatusEffect(out QiannianShenqiSe se0))
					{
						yield return new ApplyStatusEffectAction<QiannianShenqiSe>(Battle.Player, 2, null, 10, 10, 0f, true);
					}
					if (Battle.Player.TurnCounter == 1)
					{
						NotifyActivating();

						//apply buffs to all enemies on the first turn
						foreach (var enemy in Battle.AllAliveEnemies)
						{
							if (blacklist.Contains(enemy))
							{
								continue;
							}
							UnitView uv = enemy.View as UnitView;
							uv?.transform.DOScale(1.4f, 1.4f).SetEase(Ease.OutQuad);
							GameRun.SetEnemyHpAndMaxHp(enemy.Hp * 2, enemy.MaxHp * 2, enemy, true);
							blacklist.Add(enemy);
						}
					}
					yield break;
				}
				private void OnEnemySpawned(UnitEventArgs args)
				{
					if (blacklist.Contains(args.Unit))
					{
						return;
					}
					var enemy = args.Unit as EnemyUnit;
					UnitView uv = enemy.View as UnitView;
					uv?.transform.DOScale(1.4f, 1.4f).SetEase(Ease.OutQuad);
					GameRun.SetEnemyHpAndMaxHp(enemy.Hp * 2, enemy.MaxHp * 2, enemy, true);
					blacklist.Add(args.Unit);
				}

				protected override void OnGain(GameRunController gameRun)
				{
					//UpgradeAllCards();
					//GameMaster.Instance.StartCoroutine(RemoveFromPool(gameRun));
					GameMaster.Instance.StartCoroutine(gainproof(gameRun));
				}

				protected override void OnAdded()
				{
					// UnitView uv = GameRun.Player.View as UnitView;
					// uv?.transform.DOScale(1.25f, 1.25f).SetEase(Ease.OutQuad);
					HandleGameRunEvent(GameRun.StationEntered, OnStationEntered);
					//card upgrade event needs to be added in OnAdded or else it will be gone after a reload
					//HandleGameRunEvent(GameRun.DeckCardsAdded, OnDeckCardAdded);
				}
				public void OnStationEntered(StationEventArgs args)
				{
					var gameRun = GameMaster.Instance.CurrentGameRun;
					// if (!enlarged && BepinexPlugin.doublelife.Value)
					// {
					//     gameRun.Player.GetView<UnitView>().transform.DOScale(5f, 5f).SetEase(Ease.OutQuad);
					//     enlarged = true;
					// }
					EntryStation entryStation = args.Station as EntryStation;
					if (entryStation != null && GameRun.Stages.IndexOf(entryStation.Stage) == 0)
					{
						if (BepinexPlugin.doublelife.Value)
						{
							gameRun.SetHpAndMaxHp(gameRun.Player.Hp * 2, gameRun.Player.MaxHp * 2);
						}
						// if (BepinexPlugin.doublepower.Value)
						// {
						//     GameMaster.Instance.StartCoroutine(gainproof(gameRun));
						//     //GameMaster.DebugGainExhibit(Library.CreateExhibit(nameof(exmacros)));
						//     //gameRun.GainExhibitInstantly(Library.CreateExhibit(nameof(exmacros)), true, null);
						// }
					}
					// if (BepinexPlugin.doublepower.Value)
					// {
					//     gameRun.Player.Us.MaxPowerLevel *= 2;
					//     gameRun.Player.Us.UsRepeatableType = UsRepeatableType.FreeToUse;
					// }
				}

				public void OnDeckCardAdded(CardsEventArgs args)
				{
					try
					{
						UpgradeAllCards();

					}
					catch (Exception e)
					{
						Debug.LogError(" exception in OnDeckCardAdded: " + e.Message + e.StackTrace);
					}
				}

				private void UpgradeAllCards()
				{
					try
					{
						//always upgrae all card in the deck in case several card are added at once
						Debug.Log("cards in deck: " + GameRun.BaseDeck.Count);
						foreach (var card in GameRun.BaseDeck)
						{
							//check if card is not upgraded
							if (card.CanUpgrade && card.CanUpgradeAndPositive && !card.IsUpgraded)
							{
								card.Upgrade();
							}
						}
					}
					catch (Exception e)
					{
						Debug.LogError(" exception in UpgradeAllCards: " + e.Message + e.StackTrace);
					}
				}

				private IEnumerator RemoveFromPool(GameRunController gameRun)
				{
					//remove exhibits that become useless with universal access to upgraded cards
					var exhibit = new HashSet<Type> { typeof(Chaidao), typeof(Fengrenji),
						typeof(Jiaobu), typeof(Shoubiao), typeof(Shouyinji), typeof(Zixingche)};

					gameRun.ExhibitPool.RemoveAll(e => exhibit.Contains(e));
					yield return null;
				}

				private IEnumerator gainproof(GameRunController gameRun)
				{
					if (BepinexPlugin.doublepower.Value)
					{
						//GameMaster.DebugGainExhibit(Library.CreateExhibit(nameof(exmacros)));
						gameRun.GainExhibitInstantly(Library.CreateExhibit(nameof(exmacros)), true, null);
					}
					yield break;
				}
			}


		}
	}
	public class JadeBoxMonke
	{
		public sealed class JadeBoxMonkeDef : lvalonmemejadeboxtemplate
		{
			public override JadeBoxConfig MakeConfig()
			{
				var config = DefaultConfig();
				return config;
			}
			[EntityLogic(typeof(JadeBoxMonkeDef))]
			public sealed class JadeBoxMonke : JadeBox
			{
				HashSet<Card> toremovecard;
				HashSet<StatusEffect> toremovese;
				HashSet<Exhibit> toremoveexhibit;
				List<string> SEmana =
                [
                    nameof(MoodPeace),
					nameof(MoodEpiphany),
					nameof(Burst)
				];
				List<string> cardblacklist =
                [
                    nameof(Potion)
				];
				List<string> shiningmanablacklist =
                [
                    nameof(HuiyeBaoxiang),
					nameof(QipaiYouhua),
					nameof(YizangnuoWuzhi),
					nameof(QicaiLianhua),
					nameof(HuashanBaiyaosheng),
				];
				protected override void OnEnterBattle()
				{
					toremovecard = [];
					toremovese = [];
					toremoveexhibit = [];
					HandleBattleEvent(Battle.ManaGaining, OnManaGaining);
					ReactBattleEvent(Battle.ManaGained, OnManaGained);
					HandleBattleEvent(Battle.Predraw, OnPredraw);
					ReactBattleEvent(Battle.CardDrawn, OnCardDrawn);
				}
				private IEnumerable<BattleAction> lose()
				{

					foreach (Card card in toremovecard)
					{
						if (Battle.EnumerateAllCards().Contains(card))
						{
							yield return new RemoveCardAction(card);
						}
					}
					toremovecard.Clear();
					foreach (StatusEffect se in toremovese)
					{
						if (Battle.Player.StatusEffects.Contains(se))
						{
							yield return new RemoveStatusEffectAction(se);
						}
					}
					toremovese.Clear();
					foreach (Exhibit exhibit in toremoveexhibit)
					{
						if (Battle.Player.Exhibits.Contains(exhibit))
						{
							GameRun.LoseExhibit(exhibit, true, true);
						}
					}
					toremoveexhibit.Clear();

				}
				private IEnumerable<BattleAction> OnManaGained(ManaEventArgs args)
				{
					foreach (BattleAction action in lose())
					{
						yield return action;
					}
				}
				private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
				{
					foreach (BattleAction action in lose())
					{
						yield return action;
					}
				}

				private void OnPredraw(CardEventArgs args)
				{
					if (args.Cause == ActionCause.TurnStart || args.ActionSource == this)
					{
						return;
					}
					if (args.Cause == ActionCause.CardUse)
					{
						if (Battle.DiscardZone.Any(c => c.Id == nameof(SakuyaDraw)) || Battle.ExileZone.Any(c => c.Id == nameof(SakuyaDraw)))
						{
							int todraw = 0;
							foreach (Card card in Battle.DiscardZone.Concat(Battle.ExileZone).Where(c => c.Id == nameof(SakuyaDraw)))
							{
								todraw += card.Value1 - 1;
								toremovecard.Add(card);
							}
							if (todraw > 0)
							{
								React(new DrawManyCardAction(todraw));
							}
							return;
						}
					}
					if (args.Cause == ActionCause.Card)
					{
						if (args.ActionSource is Card card)
						{
							if (cardblacklist.Contains(card.Id))
							{
								return;
							}
							// React(new RemoveCardAction(card));
							toremovecard.Add(card);
							return;

						}
					}
					if (args.Cause == ActionCause.Exhibit)
					{
						if (args.ActionSource is Exhibit exhibit)
						{
							if (shiningmanablacklist.Contains(exhibit.Id))
							{
								return;
							}
							if (exhibit.Config.Rarity != Rarity.Shining)
							{
								toremoveexhibit.Add(exhibit);
								// GameRun.LoseExhibit(exhibit, true, true);
								return;
							}
						}
					}
					if (args.Cause == ActionCause.StatusEffect)
					{
						if (args.ActionSource is StatusEffect statusEffect)
						{
							if (statusEffect.Id == nameof(MoodEpiphany))
							{
								args.CancelBy(this);
								return;
							}
							if (statusEffect.Id == nameof(Burst))
							{
								if (Battle.Player.TryGetStatusEffect(out BurstDrawSe burstdraw))
								{
									toremovese.Add(burstdraw);
								}
								return;
							}
							if (statusEffect.Id == nameof(MoodPassion))
							{
								if (Battle.Player.TryGetStatusEffect(out PassionDrawSe passiondraw))
								{
									toremovese.Add(passiondraw);
								}
								return;
							}
							// React(new RemoveStatusEffectAction(statusEffect));
							toremovese.Add(statusEffect);
							return;
						}
					}
					args.CancelBy(this);
				}

				private void OnManaGaining(ManaEventArgs args)
				{
					if (args.Cause == ActionCause.TurnStart)
					{
						return;
					}
					if (args.Cause == ActionCause.Card)
					{
						if (args.ActionSource is Card card)
						{
							if (cardblacklist.Contains(card.Id))
							{
								return;
							}
							// React(new RemoveCardAction(card));
							toremovecard.Add(card);
							return;
						}
					}
					if (args.Cause == ActionCause.Exhibit)
					{
						if (args.ActionSource is Exhibit exhibit)
						{
							if (shiningmanablacklist.Contains(exhibit.Id))
							{
								return;
							}
							if (exhibit.Config.Rarity != Rarity.Shining)
							{
								toremoveexhibit.Add(exhibit);
								// GameRun.LoseExhibit(exhibit, true, true);
								return;
							}
						}
					}
					if (args.Cause == ActionCause.StatusEffect)
					{
						if (args.ActionSource is StatusEffect statusEffect)
						{
							if (SEmana.Contains(statusEffect.Id))
							{
								return;
							}
							// React(new RemoveStatusEffectAction(statusEffect));
							toremovese.Add(statusEffect);
							return;
						}
					}
					args.CancelBy(this);
				}
			}
		}
	}
}