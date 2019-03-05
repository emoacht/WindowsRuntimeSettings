using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace WindowsRuntimeSettings.App.Models
{
	public class Settings : SettingsBase
	{
		private Settings()
		{ }

		public static Settings Current { get; } = new Settings();

		[Roaming]
		public string Name
		{
			get => GetValue<string>();
			set => SetValue(value);
		}

		[Roaming]
		public int Age
		{
			get => GetValue<int>();
			set => SetValue(value);
		}

		public RaceType Race
		{
			get => GetValue(RaceType.Human);
			set => SetValue(value);
		}

		[Roaming]
		public Schedule Schedule
		{
			get => GetValue<Schedule>();
			set => SetValue(value);
		}

		[CryptFile]
		[Roaming]
		public Point WayPoint
		{
			get => GetValue<Point>();
			set => SetValue(value);
		}

		[CryptFile]
		public Color SymbolColor
		{
			get => GetValue(Colors.RoyalBlue);
			set => SetValue(value);
		}

		[CryptVault]
		public string SecretPhrase
		{
			get => GetValue<string>();
			set => SetValue(value);
		}
	}
}