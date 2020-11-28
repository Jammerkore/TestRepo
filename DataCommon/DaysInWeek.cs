using System;

namespace MIDRetail.DataCommon
{
	public class DaysInWeekFlagValues
	{
		public const ushort Sunday		= 0x0040;
		public const ushort Monday		= 0x0001;
		public const ushort Tuesday		= 0x0002;
		public const ushort Wednesday	= 0x0004;
		public const ushort Thursday	= 0x0008;
		public const ushort Friday		= 0x0010;
		public const ushort Saturday	= 0x0020;
	}

	[Serializable]
	public class DaysInWeek
	{
		private ushort _dayInWeekFlags;

		public DaysInWeek()
		{
			_dayInWeekFlags = 0;
		}

		public DaysInWeek(bool aSunday, bool aMonday, bool aTuesday, bool aWednesday, bool aThursday, bool aFriday, bool aSaturday)
		{
			Sunday = aSunday;
			Monday = aMonday;
			Tuesday = aTuesday;
			Wednesday = aWednesday;
			Thursday = aThursday;
			Friday = aFriday;
			Saturday = aSaturday;
		}
	
		public bool Sunday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Sunday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Sunday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Sunday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public bool Monday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Monday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Monday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Monday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		public bool Tuesday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Tuesday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Tuesday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Tuesday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	
		public bool Wednesday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Wednesday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Wednesday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Wednesday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	
		public bool Thursday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Thursday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Thursday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Thursday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	
		public bool Friday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Friday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Friday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Friday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	
		public bool Saturday
		{
			get
			{
				try
				{
					return ((_dayInWeekFlags & DaysInWeekFlagValues.Saturday) > 0);
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
			set
			{
				try
				{
					if (value)
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags | DaysInWeekFlagValues.Saturday);
					}
					else
					{
						_dayInWeekFlags = (ushort)(_dayInWeekFlags & ~DaysInWeekFlagValues.Saturday);
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}
	}
}
