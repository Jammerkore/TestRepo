using System;
using System.Globalization;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Common 
{
/// <summary>
/// The MIDFlag class contains static methods used to get or set flag values.
/// </summary>
	public class MIDFlag
	{
		//private MIDFlag() {} added 12.17.03 - RS
		//public class MIDFlag violates FXCop Rule: "Types with only static members should not have public or protected constructors"
		private MIDFlag() {}
		//=======
		// FIELDS
		//=======

		//============
		// CONSTRUCTOR
		//============
   
		//========
		// Methods
		//========
        /// <summary>
        /// MaxMIDFlags identifies the maximum number of flags for a given flag container:
        /// byte, ushort, uint or ulong
        /// </summary>
        /// <param name="Flags">An instance of a flag container: byte, ushort, uint or ulong</param>
        /// <returns>Maximum number of flags in the given container: byte, ushort, uint or ulong</returns>
		public static int MaxMIDFlags(byte Flags)
		{
			return 8;
		}
		/// <summary>
		/// Gets maximum number of possible flags
		/// </summary>
		/// <param name="Flags"></param>
		/// <returns>Maximum number of possible flags</returns>
		public static int MaxMIDFlags(ushort Flags)
		{
			return 16;
		}
		public static int MaxMIDFlags(uint Flags)
		{
			return 32;
		}
		public static int MaxMIDFlags(ulong Flags)
		{
			return 64;
		}
        
		/// <summary>
		/// FlagMask is a private method that builds a mask with exactly one flag "true"
		/// </summary>
		/// <param name="FlagNumber">Flag to mark "true" (relative to 0).</param>
		/// <returns>ulong FlagMask with only the selected flag set to "true"</returns>
		private static ulong FlagMask(int FlagNumber)
		{
			ulong _flagMask = 1;
			return (_flagMask << FlagNumber);
		}
		/// <summary>
		/// GetFlagValue is a bool that returns the value of the selected flag
		/// </summary>
		/// <param name="Flags">Flag Container: byte, ushort, uint or ulong.</param>
		/// <param name="FlagNumber">The selected flag whose value is to be retrieved (relative to 0)</param>
		/// <returns>bool value of the requested flag</returns>
		public static bool GetFlagValue(byte Flags, int FlagNumber)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidByteFlagGetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidByteFlagGetRequest));
			}
			return GetFlagValue((ulong)Flags, FlagNumber);
		}
		public static bool GetFlagValue(ushort Flags, int FlagNumber)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidShortFlagGetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidShortFlagGetRequest));
			}
			return GetFlagValue((ulong)Flags, FlagNumber);
		}
		public static bool GetFlagValue (uint Flags, int FlagNumber)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidIntegerFlagGetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidIntegerFlagGetRequest));
			}
			return GetFlagValue((ulong)Flags, FlagNumber);
		}
		public static bool GetFlagValue (ulong Flags, int FlagNumber)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidLongFlagGetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidLongFlagGetRequest));
			}
			return Convert.ToBoolean(Flags & FlagMask(FlagNumber), CultureInfo.CurrentUICulture);
		}
		/// <summary>
		/// SetFlagValue returns a flag container with the selected flag set as requested and
		/// all other flags left ASIS.
		/// </summary>
		/// <param name="Flags">Flag Container: byte: ushort, uint or ulong</param>
		/// <param name="FlagNumber">The selected flag whose value is to be set (flag is relative to 0)</param>
		/// <param name="FlagValue">bool value (true or false) to which to set the selected flag</param>
		/// <returns></returns>
		public static byte SetFlagValue (byte Flags, int FlagNumber, bool FlagValue)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidByteFlagSetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidByteFlagSetRequest));
			}
			return (byte) SetFlagValue((ulong) Flags, FlagNumber, FlagValue);
		}
		public static ushort SetFlagValue (ushort Flags, int FlagNumber, bool FlagValue)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidShortFlagSetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidShortFlagSetRequest));
			}
			return (ushort) SetFlagValue((ulong) Flags, FlagNumber, FlagValue);
		}
		public static uint SetFlagValue (uint Flags, int FlagNumber, bool FlagValue)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidIntegerFlagSetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidIntegerFlagSetRequest));
			}
			return (uint) SetFlagValue((ulong) Flags, FlagNumber, FlagValue);
		}
		public static ulong SetFlagValue (ulong Flags, int FlagNumber, bool FlagValue)
		{
			if (FlagNumber >= MaxMIDFlags(Flags))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_InvalidLongFlagSetRequest,
					MIDText.GetText(eMIDTextCode.msg_InvalidLongFlagSetRequest));
			}
			if (FlagValue)
			{
				return (ulong)(Flags | FlagMask(FlagNumber));
			}
			else
			{
				return (ulong)(Flags & ~FlagMask(FlagNumber));
			}
		}
	}
}