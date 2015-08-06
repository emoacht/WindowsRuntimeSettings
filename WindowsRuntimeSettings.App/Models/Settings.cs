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

		public static Settings Current => _current;
		private static readonly Settings _current = new Settings();

		[Roaming]
		public string Name
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}

		[Roaming]
		public int Age
		{
			get { return GetValue<int>(); }
			set { SetValue(value); }
		}

		public RaceType Race
		{
			get { return GetValue(RaceType.Human); }
			set { SetValue(value); }
		}

		[Roaming]
		public Schedule Schedule
		{
			get { return GetValue<Schedule>(); }
			set { SetValue(value); }
		}

		[CryptFile]
		[Roaming]
		public Point WayPoint
		{
			get { return GetValue<Point>(); }
			set { SetValue(value); }
		}

		[CryptFile]
		public Color SymbolColor
		{
			get { return GetValue(Colors.RoyalBlue); }
			set { SetValue(value); }
		}

		[CryptVault]
		public string SecretPhrase
		{
			get { return GetValue<string>(); }
			set { SetValue(value); }
		}
	}
}