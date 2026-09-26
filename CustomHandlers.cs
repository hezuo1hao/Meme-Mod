using System;
using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.Presentation;
using LBoLEntitySideloader.CustomHandlers;
using lvalonmeme.Cards;
using lvalonmeme.Patches;

namespace lvalonmeme
{
	public class CustomHandlers
	{
		private static bool youmiplayed;

		public static void OnDeckCardsAdding(CardsEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			BattleController battle = currentGameRun.Battle;
			if (currentGameRun.Packs.Contains(nameof(Packs.packoldDef)[..^3]))
			{
				for (int num = args.Cards.Length - 1; num >= 0; num--)
				{
					Card val = args.Cards[num];
					if (CustomGameEventManager.GetList("oldban").Contains(val.Id))
					{
						string text = "card" + val.Id;
						Card val2 = Library.TryCreateCard(text, val.IsUpgraded, val.UpgradeCounter);
						args.Cards[num] = val2;
					}
				}
			}
			string[] arr = toolbox.banlistgetter();
			List<Card> list = args.Cards.Where(c => BepinexPlugin.banlistoverride.Value && arr.Contains(c.Id.ToLowerInvariant())).ToList();
			foreach (Card card in list)
			{
				if (!currentGameRun.Packs.Contains(nameof(Packs.packoldDef)[..^3]) || !CustomGameEventManager.GetList("oldban").Contains(card.Id))
				{
					args.Cards = args.Cards.Where(c => c != card).ToArray();
				}
			}
		}

		public static void OnDeckCardsAdded(CardsEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			BattleController battle = currentGameRun.Battle;
			bool flag = currentGameRun.BaseDeck.Any(card => card is cardyoumi) && currentGameRun.BaseDeck.Any(card => card is cardkotk);
			IEnumerable<Card> enumerable = currentGameRun.BaseDeck;
			if (battle != null)
			{
				enumerable = enumerable.Concat(battle.EnumerateAllCards());
			}
			foreach (Card item in enumerable)
			{
				if (item is cardyoumi)
				{
					if (flag)
					{
						CardConfig.FromId("cardyoumi").ImageId = "cardyoumi" + 2;
						CardConfig.FromId("cardyoumi").UpgradeImageId = "cardyoumi" + 2;
					}
					else
					{
						CardConfig.FromId("cardyoumi").ImageId = "cardyoumi";
						CardConfig.FromId("cardyoumi").UpgradeImageId = "cardyoumi";
					}
				}
				else if (item is cardkotk)
				{
					if (flag)
					{
						CardConfig.FromId("cardkotk").ImageId = "cardkotk" + 2;
						CardConfig.FromId("cardkotk").UpgradeImageId = "cardkotk" + 2;
						CardConfig.FromId("cardkotk").Illustrator = "Hajin";
					}
					else
					{
						CardConfig.FromId("cardkotk").ImageId = "cardkotk";
						CardConfig.FromId("cardkotk").UpgradeImageId = "cardkotk";
						CardConfig.FromId("cardkotk").Illustrator = "哈基双朋友";
					}
				}
			}
		}

		public static void OnDeckCardsRemoving(CardsEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			BattleController battle = currentGameRun.Battle;
		}

		public static void OnDeckCardsRemoved(CardsEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			BattleController battle = currentGameRun.Battle;
			bool flag = currentGameRun.BaseDeck.Any(card => card is cardyoumi) && currentGameRun.BaseDeck.Any(card => card is cardkotk);
			IEnumerable<Card> enumerable = currentGameRun.BaseDeck;
			if (battle != null)
			{
				enumerable = enumerable.Concat(battle.EnumerateAllCards());
			}
			foreach (Card item in enumerable)
			{
				if (item is cardyoumi)
				{
					if (flag)
					{
						CardConfig.FromId("cardyoumi").ImageId = "cardyoumi" + 2;
						CardConfig.FromId("cardyoumi").UpgradeImageId = "cardyoumi" + 2;
					}
					else
					{
						CardConfig.FromId("cardyoumi").ImageId = "cardyoumi";
						CardConfig.FromId("cardyoumi").UpgradeImageId = "cardyoumi";
					}
				}
				else if (item is cardkotk)
				{
					if (flag)
					{
						CardConfig.FromId("cardkotk").ImageId = "cardkotk" + 2;
						CardConfig.FromId("cardkotk").UpgradeImageId = "cardkotk" + 2;
						CardConfig.FromId("cardkotk").Illustrator = "Hajin";
					}
					else
					{
						CardConfig.FromId("cardkotk").ImageId = "cardkotk";
						CardConfig.FromId("cardkotk").UpgradeImageId = "cardkotk";
						CardConfig.FromId("cardkotk").Illustrator = "哈基双朋友";
					}
				}
			}
		}

		public static void StationEntering(StationEventArgs args)
		{
			youmiplayed = false;
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			Station station = args.Station;
			EntryStation val = (EntryStation)(object)((station is EntryStation) ? station : null);
			if (val != null && CollectionsExtensions.IndexOf(currentGameRun.Stages, val.Stage) == 0)
			{
				return;
			}
			ManaGroup baseMana = currentGameRun.BaseMana;
			if (baseMana.HasTrivial || currentGameRun.Player.Exhibits.Any(e => e.Config.Id == "KongbaiKapai"))
			{
				return;
			}
			BepinexPlugin.log.LogWarning("lvalonmeme station entering, no trivial mana and blank card detected");
			Singleton<GameMaster>.Instance.CurrentGameRun.GainExhibitInstantly(Library.CreateExhibit("KongbaiKapai"), false, null);
			baseMana = currentGameRun.BaseMana;
			if (baseMana.Philosophy > 1)
			{
				currentGameRun.TryLoseBaseMana(ManaGroup.Single((ManaColor)7), false);
				return;
			}
			BepinexPlugin.log.LogWarning("lvalonmeme station entering, no trivial mana after blank card");
			baseMana = currentGameRun.BaseMana;
			if (baseMana.Colorless > 0)
			{
				currentGameRun.TryLoseBaseMana(ManaGroup.Single((ManaColor)6), false);
			}
		}

		public static void StationEntered(StationEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			Station station = args.Station;
			EntryStation val = (EntryStation)(object)((station is EntryStation) ? station : null);
			string[] arr = toolbox.banlistgetter();
			List<Card> list = currentGameRun.BaseDeck.Where(c => BepinexPlugin.banlistoverride.Value && arr.Contains(c.Id.ToLowerInvariant())).ToList();
			foreach (Card item in list)
			{
				if (!currentGameRun.Packs.Contains(nameof(Packs.packoldDef)[..^3]) || !CustomGameEventManager.GetList("oldban").Contains(item.Id))
				{
					currentGameRun.RemoveDeckCard(item, false);
				}
			}

			EntryStation entryStation = args.Station as EntryStation;
			if (entryStation != null && currentGameRun.Stages.IndexOf(entryStation.Stage) == 0)
			{
				if (currentGameRun.Packs.Contains(nameof(Packs.packoldDef)[..^3]))
				{
					if (BepinexPlugin.oldcard.Value)
					{
						List<Card> card = [
						Library.CreateCard<cardold>()
					];
						string[] arr2 = toolbox.banlistgetter();
						bool inban = false;
						foreach (string s in arr2)
						{
							if (s.ToLowerInvariant() == card[0].Id.ToLowerInvariant())
							{
								inban = true;
								break;
							}
						}
						if (!inban)
						{
							currentGameRun.AddDeckCards(card, false, null);
						}
					}
					List<Card> list2 = (from card2 in currentGameRun.BaseDeck
										where CustomGameEventManager.GetList("oldban").Contains(card2.Id)
										select card2).ToList();
					foreach (Card card2 in list2)
					{
						switch (card2.Id)
						{
							case nameof(YaoguaiBuster):
								currentGameRun.RemoveDeckCard(card2, true);
								currentGameRun.AddDeckCard(Library.CreateCard<cardYaoguaiBuster>());
								break;
							case nameof(IceLance):
								currentGameRun.RemoveDeckCard(card2, true);
								currentGameRun.AddDeckCard(Library.CreateCard<cardIceLance>());
								break;
						}
					}
				}
			}
		}

		public static void StationRewardGenerating(StationEventArgs args)
		{
			GameRunController currentGameRun = Singleton<GameMaster>.Instance.CurrentGameRun;
			Station station = args.Station;
			if (!currentGameRun.BaseDeck.Any(c => c is cardkotk) && youmiplayed && (int)currentGameRun.CurrentStation.Type != 10)
			{
				station.Rewards.Clear();
			}
		}

		public static void addreactors()
		{
			CHandlerManager.RegisterBattleEventHandler(b => b.BattleStarting, addbattlereactor, null, (GameEventPriority)int.MinValue);
		}

		private static void addbattlereactor(GameEventArgs args)
		{
			PlayerUnit player = Singleton<GameMaster>.Instance.CurrentGameRun.Battle.Player;
			battlereactor((Unit)(object)player);
		}

		private static void battlereactor(Unit unit)
		{
			GameRunController gamerun = Singleton<GameMaster>.Instance.CurrentGameRun;
			unit.ReactBattleEvent(gamerun.Battle.CardsAddedToDiscard, args => battlefieldyeet(args));
			unit.ReactBattleEvent(gamerun.Battle.CardsAddedToExile, args => battlefieldyeet(args));
			unit.ReactBattleEvent(gamerun.Battle.CardsAddedToHand, args => battlefieldyeet(args));
			unit.ReactBattleEvent(gamerun.Battle.CardsAddedToDrawZone, args => battlefieldyeet2(args));
			unit.ReactBattleEvent(gamerun.Battle.CardUsed, args => CardUsed(args));
			IEnumerable<BattleAction> battlefieldyeet(CardsEventArgs args)
			{
				List<BattleAction> list = [];
				if (gamerun.Packs.Contains(nameof(Packs.packoldDef)[..^3]))
				{
					string[] source = toolbox.banlistgetter();
					bool value = BepinexPlugin.banlistoverride.Value;
					foreach (Card item in args.Cards.Where(card => CustomGameEventManager.GetList("oldban").Contains(card.Id)))
					{
						list.Add(new RemoveCardAction(item));
						string text = "card" + item.Id;
						if (!value || !source.Contains(text.ToLowerInvariant()))
						{
							List<Card> list2 = [Library.TryCreateCard(text, item.IsUpgraded, item.UpgradeCounter)];
							Card val = Library.TryCreateCard(text, item.IsUpgraded, item.UpgradeCounter);
							CardZone zone = item.Zone;
							CardZone val2 = zone;
							switch (val2 - 2)
							{
								case 0:
									list.Add(new AddCardsToHandAction(list2, 0));
									break;
								case (CardZone)1:
									list.Add(new AddCardsToDiscardAction(list2, 0));
									break;
								case (CardZone)2:
									list.Add(new AddCardsToExileAction((Card[])(object)new Card[1] { val }));
									break;
							}
						}
					}
				}
				else
				{
					string[] arr = toolbox.banlistgetter();
					List<Card> list3 = args.Cards.Where(c => BepinexPlugin.banlistoverride.Value && arr.Contains(c.Id.ToLowerInvariant())).ToList();
					foreach (Card item2 in list3)
					{
						if (!gamerun.Packs.Contains(nameof(Packs.packoldDef)[..^3]) || !CustomGameEventManager.GetList("oldban").Contains(item2.Id))
						{
							list.Add(new RemoveCardAction(item2));
						}
					}
				}
				return list;
			}
			IEnumerable<BattleAction> battlefieldyeet2(CardsAddingToDrawZoneEventArgs args)
			{
				List<BattleAction> list = [];
				if (gamerun.Packs.Contains(nameof(Packs.packoldDef)[..^3]))
				{
					string[] source = toolbox.banlistgetter();
					bool value = BepinexPlugin.banlistoverride.Value;
					foreach (Card item3 in args.Cards.Where(card => CustomGameEventManager.GetList("oldban").Contains(card.Id)))
					{
						list.Add(new RemoveCardAction(item3));
						string text = "card" + item3.Id;
						if (!value || !source.Contains(text.ToLowerInvariant()))
						{
							List<Card> list2 = [Library.TryCreateCard(text, item3.IsUpgraded, item3.UpgradeCounter)];
							Card val = Library.TryCreateCard(text, item3.IsUpgraded, item3.UpgradeCounter);
							if ((int)item3.Zone == 1)
							{
								list.Add(new AddCardsToDrawZoneAction(list2, (DrawZoneTarget)2, 0));
							}
						}
					}
				}
				else
				{
					string[] arr = toolbox.banlistgetter();
					List<Card> list3 = args.Cards.Where(c => BepinexPlugin.banlistoverride.Value && arr.Contains(c.Id.ToLowerInvariant())).ToList();
					foreach (Card item4 in list3)
					{
						if (!gamerun.Packs.Contains(nameof(Packs.packoldDef)[..^3]) || !CustomGameEventManager.GetList("oldban").Contains(item4.Id))
						{
							list.Add(new RemoveCardAction(item4));
						}
					}
				}
				return list;
			}
			static IEnumerable<BattleAction> CardUsed(CardUsingEventArgs args)
			{
				if (args.Card is cardyoumi)
				{
					youmiplayed = true;
				}
				return [];
			}
		}
	}
}
