using System;
using System.Collections.Generic;
using LibVLCSharp.Shared;
using Xamarin.Forms;

namespace Cadi.UI.Views
{
	public partial class RearviewPage : ContentPage
	{

		private const string VIDEO_URL = "rtp://127.0.0.1:1234/test";

		LibVLC _libVlc;
		public RearviewPage()
		{
			InitializeComponent();

			_libVlc = new LibVLC();

		}

		protected override void OnAppearing()
		{
			try
			{
				RearviewPlayer.MediaPlayer = new MediaPlayer(new Media(_libVlc, VIDEO_URL, FromType.FromLocation));
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
