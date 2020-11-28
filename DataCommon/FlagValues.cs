using System;

namespace MIDRetail.DataCommon
{
	public class ComputationInfoFlagValues
	{
		public const byte None			= 0x00;
		public const byte CompChanged	= 0x01; // Cell has been changed by a computation
		public const byte UserChanged	= 0x02; // Cell has been changed by a user
		public const byte CompLocked	= 0x04; // Cell has been locked by a computation
		//Begin Track #5752 - JScott - Calculation Time
		public const byte AutoTotalsProcessed	= 0x08; // Cell has been processed for Autototals
		//End Track #5752 - JScott - Calculation Time
	}

	public class ComparativeExtensionFlagValues
	{
		public const byte None				= 0x00;
		public const byte DisplayOnlyInited	= 0x01;	// DisplayOnlyInited Value
		public const byte IneligibleInited	= 0x02;	// IneligibleInited Value
		public const byte ClosedInited		= 0x04;	// ClosedInited Value
		public const byte ProtectedInited	= 0x08;	// ProtectedInited Value
		public const byte HiddenInited		= 0x10;	// HiddenInited Value
		public const byte CellByCubeInited	= 0x20;	// CellByCubeInited Value
	}

	public class CubeAttributesFlagValues
	{
		public const ushort GroupTotal		= 0x0001; // GroupTotal Cube
		public const ushort StoreTotal		= 0x0002; // StoreTotal Cube
	}

	public class PlanCubeAttributesFlagValues : CubeAttributesFlagValues
	{
		public const ushort Basis			= 0x0004; // Basis Cube
		public const ushort Plan			= 0x0008; // Plan Cube
		public const ushort Chain			= 0x0010; // Chain Cube
		public const ushort Store			= 0x0020; // Store Cube
		public const ushort WeekDetail		= 0x0040; // WeekDetail Cube
		public const ushort PeriodDetail	= 0x0080; // PeriodDetail Cube
		public const ushort DateTotal		= 0x0100; // DateTotal Cube
		public const ushort LowLevelTotal	= 0x0200; // ChainTotal Cube
	}
}
