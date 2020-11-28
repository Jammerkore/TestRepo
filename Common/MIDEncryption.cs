using System;
using System.Collections;

namespace MID.MRS.Common
{
	/// <summary>
	/// Class that provides an encryption/decryption algorithm for encoding valuable information.
	/// </summary>

	public class Encryption
	{
		//=======
		// FIELDS
		//=======

		const int cMaxChars = 140;
		const string cMask1 = "`I:n6CbL>0`10PHA\\>^55ED^hcYj6[jm62Ee[^[52A@OlS1NENXW4hUK4K5BYgEQ5ajm:QUlRX3c4d`Eb:TV3Ci<Ki`?YHK:j]J;W3Dch[4KeaF?Y_ONB^Nag]Q@_EC:<ggEJU\\_e`]G";
		const string cMask2 = "jiK;<Xbnc\\8ZkUZHh=PDjL>f^MTh62WV``M6\\OYSCDea?5gk;D0[9bIMZTK^3hg`4Pi;8TZHSl0VULVXTX]a2kMlFnJ84iOIK5\\^;\\cUXW8<abf>la9J;5]82kc89RCXMV:P^fLgd79I";
		const string cMask3 = "GaJK>8_1B5fA[0ZgBV33UFVe[DaBW90gb@LmA84iYgB2b\\[@8lE\\[gI0:S^hEMH84mNJR\\V:Q8I\\3S3nXQ9ZcC<jfKb\\H:S;N:mc>@\\JF::RM[kVP_@cC]C_`RS6ih`FT==hT>_Gc^CX";
		const string cMask4 = "bhDbgOJPimYkWKT02_mh<W6bII<Kb^i6U<Iea5S=F3T\\=4Z=dSnFO@b;bPINCm];4SEK2^=k<`R92ITE^nU?_RUDH>2A[5iGEIXY]>:6F2:^]Y8Ij3_CHNJPl03c[k?6e;cX_jVOcOh\\";
		const string cMask5 = "kYP5T_6QY;4W_Sa_SM8>n67hn@F9MS7Z0EO6_;UVl]0HGF\\CJF[3fN0dRV5DHl454IVI4ZnGgP@A5^PXR3]6g4cPEYS95@>0XjDm^B?^GC:E>^_c6LaD>l<nT54I7eBWB@W9JQi?cm^T";
		const string cMask6 = "9eJC]HA03HONcbZX6Ja;@ijF]h;5f6D?a1lRnDF>m6TiPhB69X:<JdI?j4Uc<;8k4?[bd8X`3R=9D0dcV5k>j7dkV]Vj^R2blWeA=N@0nI9fW?ii?`=7?i1kI_;7Q]2OPJ<\\:RV7dj>Q";
		const string cMask7 = "PWeD4i^lT4fVS>59D31ER2;Tmg6^8RngQC;Db?SENDS^_?_YJkiT48bNKAF;bnd25n5@3_F5N[f=NKc^8LD<WIcHP\\gPgHQOOPY`9OVYUd5GATnnMW3FYWIZ;\\^Efd<6:G]la2TWdYVb";
		const string cMask8 = "NaQcSA8O;cE009a@94UNE;^i2H:_kM^ImdP><R:@V:T7d^hnM]F2DJ0B3Wa:6HS]4j2:XR4UiEf:VV4BN;B>6[5Rd=;S`09=Ue3h[^EjX_8nRdZKRNZj6EVGeNDJFOdi;YT`S;U_dFTE";
		const string cMask9 = "eN]>a?CQ:\\8O0Nm@gn1ZM7KOU[jZFEXWI?6jLLiEhV]j@dE?1d=?lZ=^2=W5DX:c4O6`G3bJ5[aeHP>`EZXQ8HY]2Gg>dc1?Kif`]Tfm1b:AhFEQne_X0X[ZdG]0:TUAZQJNXM2_cYOE";
		const string cMask10 = ";[_d>2fMLX15HgmP@H=3M_G1YH854;S]ENWWU0`CfASQLI8`6f^^9S<JC]:eiaI@4C^h?Fj@Wi<<<dS??Fm=LdS`B<CfejI4fBL9Y?P_kM6?Fn>N:iURb<b?^lU223J\\O8EhGJTnd>@m";
		const string cCodes = "rIgPRiz1Toq6GhaWm9VCKdxkHyJF8AjUQ47wls@Yp3cBNOn5DtSL!0ueXMZEfbv2";

		char _base;
		Random _randGen;
		Hashtable _indexToCode;
		Hashtable _codeToIndex;
		BitArray[] _maskBits;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of the Encryption class using the given base.
		/// </summary>
		/// <param name="aBase">
		/// A character that indicates the base of the encryption.  The base is the beginning character of the valid range of characters that can be encrypted.
		/// The valid range is terminated at 64 ascii characters.  For instance, if ":" is the base, only characters in the range of ":" to "y" can be specified
		/// in the string to be encrypted.
		/// </param>

		public Encryption(char aBase)
		{
			int i;

			try
			{
				_base = aBase;

				_randGen = new Random(System.DateTime.Now.Second);

				_maskBits = new BitArray[10];
				_maskBits[0] = new BitArray(ConvertStringToByteArray(cMask1, '0'));
				_maskBits[1] = new BitArray(ConvertStringToByteArray(cMask2, '0'));
				_maskBits[2] = new BitArray(ConvertStringToByteArray(cMask3, '0'));
				_maskBits[3] = new BitArray(ConvertStringToByteArray(cMask4, '0'));
				_maskBits[4] = new BitArray(ConvertStringToByteArray(cMask5, '0'));
				_maskBits[5] = new BitArray(ConvertStringToByteArray(cMask6, '0'));
				_maskBits[6] = new BitArray(ConvertStringToByteArray(cMask7, '0'));
				_maskBits[7] = new BitArray(ConvertStringToByteArray(cMask8, '0'));
				_maskBits[8] = new BitArray(ConvertStringToByteArray(cMask9, '0'));
				_maskBits[9] = new BitArray(ConvertStringToByteArray(cMask10, '0'));

				_indexToCode = new Hashtable();
				_codeToIndex = new Hashtable();

				i = 0;

				foreach (char chr in cCodes)
				{
					_indexToCode.Add(i, chr);
					_codeToIndex.Add(chr, i);
					i++;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//========
		// METHODS
		//========

		/// <summary>
		/// Method that encrypts a given string.
		/// </summary>
		/// <param name="aKey">
		/// The string to encrypt.
		/// </param>
		/// <returns>
		/// An encrypted string.
		/// </returns>

		public string Encrypt(string aKey)
		{
			try
			{
				if (aKey.Length > cMaxChars)
				{
					throw new Exception("String to encode exceeds maximum length of " + System.Convert.ToString(cMaxChars));
				}

				foreach (char chr in aKey)
				{
					if (chr < _base || chr > _base + 63)
					{
						throw new Exception("String contains character '" + chr + "' that is outside of valid range: '" + _base + "' - '" + Convert.ToChar(_base + 63) +"'");
					}
				}

				return Encode(XOREncode(Spin(aKey)));
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Method that decrypts a given encrypted string.
		/// </summary>
		/// <param name="aKey">
		/// The encrypted string to decrypt.
		/// </param>
		/// <returns>
		/// The decrypted string.
		/// </returns>

		public string Decrypt(string aKey)
		{
			try
			{
				return Unspin(XORDecode(Decode(aKey)));
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string Spin(string aString)
		{
			int numSpins;
			string outStr;
			int i;

			try
			{
				numSpins = _randGen.Next(0, 9);
				outStr = (string)aString.Clone();

				for (i = 0; i < numSpins; i++)
				{
					outStr = outStr.Substring(1) + outStr.Substring(0, 1);
				}

				return ConvertToIntToString(numSpins) + outStr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string Unspin(string aString)
		{
			int numSpins;
			string outStr;
			int i;

			try
			{
				numSpins = System.Convert.ToInt32(aString.Substring(0, 1));
				outStr = aString.Substring(1);

				for (i = 0; i < numSpins; i++)
				{
					outStr = outStr.Substring(outStr.Length - 1, 1) + outStr.Substring(0, outStr.Length - 1);
				}

				return outStr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string Encode(byte[] aByteArray)
		{
			string outStr;

			try
			{
				outStr = "";

				foreach (byte idx in aByteArray)
				{
					outStr += (char)_indexToCode[Convert.ToInt32(idx)];
				}

				return outStr;
			}
			catch (Exception)
			{
				throw;
			}
		}
	
		private byte[] Decode(string aKey)
		{
			byte[] outArr;
			int i;

			try
			{
				outArr = new byte[aKey.Length];
				i = 0;

				foreach (char chr in aKey)
				{
					outArr[i] = Convert.ToByte((int)_codeToIndex[chr]);
					i++;
				}

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}
	
		private byte[] XOREncode(string aKey)
		{
			int maskIdx;
			byte[] keyArr;
			byte[] maskArr;
			byte[] outArr;
			int i;

			try
			{
				maskIdx = _randGen.Next(0, 9);

				keyArr = XOR(ConvertStringToByteArray(aKey, _base), maskIdx);
				maskArr = XOR(ConvertIntToByteArray(maskIdx), 0);

				outArr = new byte[keyArr.Length + maskArr.Length];
				i = 0;

				foreach (byte maskByte in maskArr)
				{
					outArr[i] = maskByte;
					i++;
				}

				foreach (byte keyByte in keyArr)
				{
					outArr[i] = keyByte;
					i++;
				}

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string XORDecode(byte[] aKey)
		{
			byte[] maskArr;
			int maskIdx;
			byte[] keyArr;
			int i;

			try
			{
				maskArr = new byte[1];
				maskArr[0] = aKey[0];
				maskIdx = Convert.ToInt32(XOR(maskArr, 0)[0]);

				keyArr = new byte[aKey.Length - 1];

				for (i = 0; i < aKey.Length - 1; i++)
				{
					keyArr[i] = aKey[i + 1];
				}

				return ConvertByteArrayToString(XOR(keyArr, maskIdx), _base);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private byte[] XOR(byte[] aKey, int aMaskIdx)
		{
			byte[] outArr;
			BitArray keyBits;
			BitArray tempMask;

			try
			{
				keyBits = new BitArray(aKey);
				tempMask = (BitArray)_maskBits[aMaskIdx].Clone();
				tempMask.Length = keyBits.Length;
				keyBits = keyBits.Xor(tempMask);

				outArr = new byte[aKey.Length];
				keyBits.CopyTo(outArr, 0);

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private byte[] ConvertStringToByteArray(string aString, char aBase)
		{
			byte[] outArr;
			int i;

			try
			{
				outArr = new byte[aString.Length];

				i = 0;

				foreach (char chr in aString)
				{
					outArr[i] = Convert.ToByte(chr - aBase);
					i++;
				}

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}
	
		private string ConvertByteArrayToString(byte[] aByteArray, char aBase)
		{
			char[] strArr;
			int i;

			try
			{
				strArr = new char[aByteArray.Length];

				for (i = 0; i < aByteArray.Length; i++)
				{
					strArr[i] = Convert.ToChar(aByteArray[i] + aBase);
				}

				return new string(strArr);
			}
			catch (Exception)
			{
				throw;
			}
		}
	
		private byte[] ConvertIntArrayToByteArray(int[] aIntArray)
		{
			byte[] outArr;
			int i;

			try
			{
				outArr = new byte[aIntArray.Length];

				for (i = 0; i < aIntArray.Length; i++)
				{
					outArr[i] = Convert.ToByte(aIntArray[i]);
				}

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private byte[] ConvertIntToByteArray(int aInt)
		{
			byte[] outArr;

			try
			{
				outArr = new byte[1];
				outArr[0] = Convert.ToByte(aInt);

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private int[] ConvertByteArrayToIntArray(byte[] aByteArray)
		{
			int[] outArr;
			int i;

			try
			{
				outArr = new int[aByteArray.Length];
				i = 0;

				foreach (byte inByte in aByteArray)
				{
					outArr[i] = Convert.ToInt32(inByte);
					i++;
				}

				return outArr;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string ConvertToIntToString(int aInt)
		{
			string tempStr;

			try
			{
				tempStr = Convert.ToString(aInt);
				return tempStr.Substring(tempStr.Length - 1, 1);
			}			
			catch (Exception)
			{
				throw;
			}
		}
	}
}
