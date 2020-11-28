using System;
using System.Diagnostics;
using System.Globalization;


namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Summary description for MIDTimer.
	/// </summary>
	public class MIDTimer
	{

		private DateTime _startTime;
		private DateTime _stopTime;
        //private TimeSpan _elaspedTime;

		/// <summary>
		/// Start time of MIDTimer.
		/// </summary>
		public DateTime StartTime
		{
			get	{ return _startTime; }
			set { _startTime = value; }
		}
		/// <summary>
		/// Stop time of MIDTimer.
		/// </summary>
		public DateTime StopTime
		{
			get	{ return _stopTime; }
			set { _stopTime = value; }
		}
		/// <summary>
		/// Elasped time of MIDTimer based upon STOP time.
		/// </summary>
		public TimeSpan ElaspedTime
		{
			get	{ return _stopTime.Subtract(_startTime); }
		}

		// BEGIN TT#739-MD - STodd - delete stores
		/// <summary>
		/// Elasped time of MIDTimer based upon DateTime.Now.
		/// </summary>
		public TimeSpan ElaspedTimeNow
		{
			get { return DateTime.Now.Subtract(_startTime); }
		}
		// END TT#739-MD - STodd - delete stores
		
		/// <summary>
		/// Elasped time in a string format.
		/// </summary>
		public string ElaspedTimeString
		{
			get	{ return System.Convert.ToString(ElaspedTime, CultureInfo.CurrentUICulture); }
		}


		public MIDTimer()
		{

		}

		/// <summary>
		/// Starts the timer and sets the StartTime
		/// </summary>
		public void Start()
		{
			_startTime = System.DateTime.Now;
		}

		/// <summary>
		/// Stops the timer and sets the StopTime.
		/// </summary>
		public void Stop()
		{
			_stopTime = System.DateTime.Now;
		}

		/// <summary>
		/// Stops the timer and writes out a debug message with the tag in front.
		/// </summary>
		/// <param name="tag"></param>
		public void Stop(string tag)
		{
			Stop();
			Write(tag);
		}

		// BEGIN TT#739-MD - STodd - delete stores
		public TimeSpan AverageTime(int count)
		{
			var eNow = this.ElaspedTimeNow;
			TimeSpan avgTime = TimeSpan.FromTicks(eNow.Ticks / count);
			return avgTime;
		}
		// END TT#739-MD - STodd - delete stores

		/// <summary>
		/// Writes out the elaspe time as a string.
		/// </summary>
		public void Write()
		{
			Debug.WriteLine(ElaspedTimeString);
		}

		/// <summary>
		/// Writes out elapsed time with tag in front.
		/// </summary>
		/// <param name="tag"></param>
		public void Write(string tag)
		{
			Debug.WriteLine(tag + ": " + ElaspedTimeString);
		}
	}
}
