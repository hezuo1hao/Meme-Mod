using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using lvalonmeme.SampleCharacterUlt;
namespace lvalonmeme
{
	public class SampleCharacterLoadouts
	{
		public static string UltimateSkillA = nameof(SampleCharacterUltA);
		public static string UltimateSkillB = nameof(SampleCharacterUltB);

		// public static string ExhibitA = nameof(SampleCharacterExhibitA);
		// public static string ExhibitB = nameof(SampleCharacterExhibitB);
		public static List<string> DeckA = [
			nameof(Shoot),
			nameof(Shoot),
			nameof(Boundary),
			nameof(Boundary),
		];

		public static List<string> DeckB = [
			nameof(Shoot),
			nameof(Shoot),
			nameof(Boundary),
			nameof(Boundary),
		];

		public static PlayerUnitConfig playerUnitConfig = new PlayerUnitConfig(
			Id: BepinexPlugin.modUniqueID,
			HasHomeName: true,
			ShowOrder: 8,
			Order: 0,
			UnlockLevel: 0,
			ModleName: "",
			NarrativeColor: "#e58c27",
			IsSelectable: true,
			MaxHp: 80,
			InitialMana: new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 0, Colorless = 0, Philosophy = 0 },
			InitialMoney: 99,
			InitialPower: 0,
			BasicRingOrder: 0,
			LeftColor: ManaColor.Philosophy,
			RightColor: ManaColor.Colorless,
			UltimateSkillA: SampleCharacterLoadouts.UltimateSkillA,
			UltimateSkillB: SampleCharacterLoadouts.UltimateSkillB,
			// ExhibitA: SampleCharacterLoadouts.ExhibitA,
			// ExhibitB: SampleCharacterLoadouts.ExhibitB,
			ExhibitA: null,
			ExhibitB: null,
			DeckA: SampleCharacterLoadouts.DeckA,
			DeckB: SampleCharacterLoadouts.DeckB,
			DifficultyA: 3,
			DifficultyB: 2
		);
	}
}
