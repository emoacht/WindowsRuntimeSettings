using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using WindowsRuntimeSettings.App.Models;

namespace WindowsRuntimeSettings.App
{
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			this.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Run();
		}

		private void Run()
		{
			Settings.Current.Name = "Elrond";
			Settings.Current.Age = 12000;
			Settings.Current.Race = RaceType.Elf;

			Settings.Current.Schedule = new Schedule
			{
				Transportation = TransportationType.Airplane,
				DestinationName = "North pole",
				DepatureTime = DateTime.Today.AddHours(6).AddMinutes(32),
				TravelTime = TimeSpan.FromHours(3.5),
				Distance = 100,
			};

			Settings.Current.WayPoint = new Point(3.5, 127.6);
			Settings.Current.SymbolColor = Colors.Aqua;

			Settings.Current.SecretPhrase = "Na-ha-je";

			Debug.WriteLine($"Name -> {Settings.Current.Name}");
			Debug.WriteLine($"Age -> {Settings.Current.Age}");
			Debug.WriteLine($"Race -> {Settings.Current.Race}");

			Debug.WriteLine($"Schedule (Transportation) -> {Settings.Current.Schedule.Transportation}");
			Debug.WriteLine($"Schedule (DepatureTime) -> {Settings.Current.Schedule.DepatureTime:yyyy/MM/dd HH:mm}");

			Debug.WriteLine($"WayPoint -> {Settings.Current.WayPoint}");
			Debug.WriteLine($"SymbolColor -> {Settings.Current.SymbolColor}");

			Debug.WriteLine($"SecretPhrase -> {Settings.Current.SecretPhrase}");
		}
	}
}