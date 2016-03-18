using Android.App;
using Android.Widget;
using Android.OS;
using Android.Runtime;
using System;
using System.Threading.Tasks;

namespace hockeycry.Droid
{
	[Activity (Label = "hockeycry", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Register the crash manager before Initializing the trace writer
			HockeyApp.CrashManager.Register (this, "13496511fff84394846cfc16c0efbe41"); 

			//Register to with the Update Manager
			//HockeyApp.UpdateManager.Register (this, "13496511fff84394846cfc16c0efbe41");

			// Initialize the Trace Writer
			HockeyApp.TraceWriter.Initialize ();

			// Wire up Unhandled Expcetion handler from Android
			AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) => 
			{
				// Use the trace writer to log exceptions so HockeyApp finds them
				HockeyApp.TraceWriter.WriteTrace(args.Exception);
			};

			// Wire up the .NET Unhandled Exception handler
			AppDomain.CurrentDomain.UnhandledException +=
				(sender, args) => HockeyApp.TraceWriter.WriteTrace(args.ExceptionObject);

			// Wire up the unobserved task exception handler
			TaskScheduler.UnobservedTaskException += 
				(sender, args) => HockeyApp.TraceWriter.WriteTrace(args.Exception);
			
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);
				if(count > 3)
					throw new HockeyAppSampleException("You intentionally caused a crash!");
			};
		}
	}

	public class HockeyAppSampleException : System.Exception
	{
		public HockeyAppSampleException(string msg) : base(msg)
		{
		}
	}
}


