using System.Collections.Generic;
using System.Linq;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using lvalonmeme.StatusEffects;

namespace lvalonmeme.Cards.Template
{
	public static class CardIndexGenerator
	{
		private static readonly List<ManaColor> offColors = BepinexPlugin.offColors;
		private static int? initial_offset = null;
		private static HashSet<int> uniqueIds = [];

		public const int milx1 = (int)1E7;

		public static HashSet<int> UniqueIds
		{
			get
			{
				if (uniqueIds == null)
					uniqueIds = [];
				return uniqueIds;

			}
		}

		internal static void PromiseClearIndexSet() => EntityManager.AddPostLoadAction(() => uniqueIds = null);
		public static int Initial_offset
		{
			get
			{
				if (initial_offset == null)
				{
					int millions = 0;
					if (UniqueTracker.Instance.configIndexes.TryGetValue(typeof(CardConfig), out var indexSet))
					{
						millions = indexSet.Where(i => i >= milx1).DefaultIfEmpty().Max() / milx1;
					}
					millions += 1;
					initial_offset = millions * milx1;
				}
				return initial_offset.Value;
			}
		}
		//(Card card) => card is mimaextensions.mimacard mimascard && mimascard is mimaextensions.mimacard.passivecard).ToList();
		// public static int GetUniqueIndex(Card card) {
		//     int rez = GetUniqueIndex(card.Config);
		//     if (card is lvalonmemecard lcard && lcard is lvalonmemecard.oldcard){
		//         rez += 300000;
		//     }
		//     return rez;
		// }
		public static int GetUniqueIndex(CardConfig config, string str)
		{
			int rez = GetUniqueIndex(config);
			switch (str)
			{
				// case "old":
				//     rez += 40000000;
				//     break;
				case "oldcard":
					rez -= 30000000;
					break;
				default:
					break;
			}
			return rez;
		}

		public static int GetUniqueIndex(CardConfig config)
		{
			/*
            Generate a unique card ID based on the original offset, the card's rarity, colors and the type:

            Example:
            initial offset = 50000000
            rarity = Common
            color = Blue (Main Color)
            cost = 3
            type = Attack 

            ID = 50123101

            0: Blue is a main color (0=Main Color, 1=Off-Color)
            1: Common (Basic=0, Common=1, Uncommon=2, Rare=2)
            2: Blue (White=1, Blue=2, Black=3, Red=4, Green=5, Colorless=6, Multicolor=9)
            3: Card's cost (Unplayable, X-cost and Cost > 9 will return 9)
            1: Attack (Attack = 1, Defense = 2, Skill = 3, Ability = 4, Teammate = 5)
            01: Amount of cards with the same value whose IDs were set before.   

            additional info:
            +300000 for old cards
            +500000 for banned cards
            */

			int id = Initial_offset;
			switch (config.Owner)
			{
				case null:
					id += 20000000;
					break;
				case "Koishi":
					id += 10000000;
					break;
				case "Cirno":
					id += 9000000;
					break;
				case "Sakuya":
					id += 8000000;
					break;
				case "Marisa":
					id += 7000000;
					break;
				case "Reimu":
					id += 2000000;
					break;
				default:
					break;
			}
			//custom keyword check
			id += config.RelativeEffects.Contains(nameof(seold)) ? 40000000 : 0;

			//Rarity
			id += config.Keywords.HasFlag(Keyword.Basic) ? 0 : (int)(config.Rarity + 1) * 100000;

			//Color
			int color = config.Colors?.Count > 1 ? 9 : 0;
			int? colorFirst = (int?)(config.Colors?.FirstOrDefault());
			colorFirst ??= 9;
			int colorFinal = color * 10000 + colorFirst.Value * 1000;
			id += colorFinal;

			//Cost
			int cost = (config.IsXCost || config.Keywords.HasFlag(Keyword.Forbidden) || config.Cost.Total > 9) ? 9 : config.Cost.Total;
			id += cost * 1000;

			//Type
			id += (int)config.Type * 100;

			//Cards with similar parameters
			if (UniqueTracker.Instance.configIndexes.TryGetValue(typeof(CardConfig), out var indexSet))
				while (indexSet.Contains(id)) id++;

			return id;
		}
	}
}