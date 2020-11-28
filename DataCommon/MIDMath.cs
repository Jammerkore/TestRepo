using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// Math routines (not in the C#.Math class
	/// </summary>
	public class MIDMath
	{
		static private Random _random;
        static private bool _disableRandom = false;

        static MIDMath()
        {
            string val = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
            if (val != null
                && val.ToLower() == "true")
            {
                val = MIDConfigurationManager.AppSettings["DisableRandom"];
                if (val != null
                && val.ToLower() == "true")
                {
                    _disableRandom = true;
                }
            }
        }

		/// <summary>
		/// Calculates the Greatest Common Divisor of two integers.
		/// </summary>
		/// <param name="aInteger1">First of two integers</param>
		/// <param name="aInteger2">Second of two integers</param>
		/// <returns></returns>
		static public int GreatestCommonDivisor (int aInteger1, int aInteger2)
		{
			int gcd = 1;
			int _minimum;
			int _maximum;
			if (Math.Abs(aInteger1) > Math.Abs(aInteger2))
			{
				_minimum = Math.Abs(aInteger2);
				_maximum = Math.Abs(aInteger1);
			}
			else
			{
				_minimum = Math.Abs(aInteger1);
				_maximum = Math.Abs(aInteger2);
			}
			int remainder = _minimum;
			while (remainder > 0)
			{
				gcd = remainder;
				remainder = _maximum % _minimum;
				_maximum = _minimum;
				_minimum = remainder;
			}
			return gcd;
		}

		/// <summary>
		/// Calculates the Least Common Multiple of two integers
		/// </summary>
		/// <param name="aInteger1">First of two integers</param>
		/// <param name="aInteger2">Second of two integers</param>
		/// <returns></returns>
		static public int LeastCommonMultiple (int aInteger1, int aInteger2)
		{
			return ((aInteger1 * aInteger2) / GreatestCommonDivisor(aInteger1, aInteger2));
		}

		/// <summary>
		/// Calculates Std Deviation from an ArrayList of doubles
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		static public double GetStandardDeviation(ArrayList num) 
		{
			double SumOfSqrs = 0;
			double avg = GetAvg(num);
			for (int i=0; i<num.Count; i++)
			{
				SumOfSqrs += Math.Pow(((double)num[i] - avg), 2);
			}
			double n = (double)num.Count;
			return Math.Sqrt(SumOfSqrs/(n-1));
		}

		/// <summary>
		/// Calculates Std Deviation from an array of doubles
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		static public double GetStandardDeviation(double[] num)
		{
			double Sum = 0.0, SumOfSqrs = 0.0;
			for (int i=0; i<num.Length; i++)
			{
				Sum += num[i];
				SumOfSqrs += Math.Pow(num[i], 2);
			}
			double topSum = (num.Length * SumOfSqrs) - (Math.Pow(Sum, 2));
			double n = (double)num.Length;
			return Math.Sqrt( topSum / (n * (n-1)) );
		}
		
		/// <summary>
		/// Calculates the average from ann array of doubles
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		static public double GetAvg(double[] num)
		{
			double sum = 0.0;
			for (int i=0; i<num.Length; i++)
			{
				sum += num[i];
			}
			double avg = sum / System.Convert.ToDouble(num.Length);

			return avg;
		}

		/// <summary>
		/// Calculates the average from ann array of doubles
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		static public double GetAvg(int[] num)
		{
			double sum = 0.0;
			for (int i=0; i<num.Length; i++)
			{
				sum += num[i];
			}
			double avg = sum / System.Convert.ToDouble(num.Length);

			return avg;
		}

		/// <summary>
		/// Calculates the average from ann ArrayList of doubles
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		static public double GetAvg(ArrayList num)
		{
			double sum = 0.0;
			for (int i=0; i<num.Count; i++)
			{
				sum += (double)num[i];
			}
			double avg = sum / System.Convert.ToDouble(num.Count);

			return avg;
		}
		static private bool firstTimeIsNumber = true; 
		static private Regex objNotNumberPattern;
		static private Regex objTwoDotPattern;
		static private Regex objTwoMinusPattern;
		static private String strValidRealPattern;
		static private String strValidIntegerPattern;
		static private Regex objNumberPattern;

		static private Regex objNotPositivePattern;
		static private Regex objPositivePattern;
		// begin MID Track 4708 Size Performance Slow
		static private string lastStringNumber = string.Empty;
		static private bool lastStringNumberStatus = false;
		static private string lastStringPositiveNumber = string.Empty;
		static private bool lastStringPositiveNumberStatus = false;
		// end MID Track 4708 Size Performance Slow

		static private void CreateRegExNumberPatterns()
		{
			firstTimeIsNumber = false;
			objNotNumberPattern=new Regex("[^0-9.-]");
			objTwoDotPattern=new Regex("[0-9]*[.][0-9]*[.][0-9]*");
			objTwoMinusPattern=new Regex("[0-9]*[-][0-9]*[-][0-9]*");
			strValidRealPattern="^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
			strValidIntegerPattern="^([-]|[0-9])[0-9]*$";
			objNumberPattern =new Regex("(" + strValidRealPattern +")|(" + strValidIntegerPattern + ")");

			objNotPositivePattern=new Regex("[^0-9.]");
			objPositivePattern=new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");

		}
		/// <summary>
		/// Determines if a string represents a number
		/// </summary>
		/// <param name="strNumber">String representation of the potential number</param>
		/// <returns>True: if the string represents a number; False otherwise</returns>
		static public bool IsNumber(string strNumber)
		{
 			if (firstTimeIsNumber)
			{
                CreateRegExNumberPatterns();
			}
			// begin MID Track 4708 Size Performance Slow
			//return !objNotNumberPattern.IsMatch(strNumber) &&
			//	!objTwoDotPattern.IsMatch(strNumber) &&
			//	!objTwoMinusPattern.IsMatch(strNumber) &&
			//	objNumberPattern.IsMatch(strNumber);
			if (strNumber == string.Empty)
			{
				return false;
			}
			if (lastStringNumber != strNumber)
			{
				lastStringNumber = strNumber;
				lastStringNumberStatus =
                    objNumberPattern.IsMatch(lastStringNumber) &&
	    			!objTwoDotPattern.IsMatch(lastStringNumber) &&
				    !objTwoMinusPattern.IsMatch(lastStringNumber);
			}
			return lastStringNumberStatus;
			// end MID Track 4708 Size Performance Slow
		}

		/// <summary>
		/// Determine if a string contains a positive number (integer or real)
		/// </summary>
		/// <param name="strNumber">String containing the potential number</param>
		/// <returns>True: if string contains a positive number; False otherwise</returns>
		static public bool IsPositiveNumber(string strNumber)
		{
			if (firstTimeIsNumber)
			{
				CreateRegExNumberPatterns();
			}

			// begin MID Track 4708 Size Performance Slow
			//return !objNotPositivePattern.IsMatch(strNumber) &&
			//	objPositivePattern.IsMatch(strNumber)  &&
			//	!objTwoDotPattern.IsMatch(strNumber);
			if (lastStringPositiveNumber != strNumber)
			{
				lastStringPositiveNumber = strNumber;
				if (lastStringNumber != lastStringPositiveNumber)
				{
					lastStringPositiveNumberStatus = IsNumber(lastStringPositiveNumber);
				}
				else
				{
					lastStringPositiveNumberStatus = lastStringNumberStatus;
				}
				if (lastStringPositiveNumberStatus)
				{
					// It is a number! So check to see that there is no negative sign
					//lastStringPositiveNumberStatus =
					//	!objNotPositivePattern.IsMatch(lastStringPositiveNumber)
					//	&& objPositivePattern.IsMatch(lastStringPositiveNumber);
					lastStringPositiveNumberStatus =
						!objNotPositivePattern.IsMatch(lastStringPositiveNumber);
				}
			}
			return lastStringPositiveNumberStatus;
			// end MID Track 4708 Size Performance Slow
		}

		/// <summary>
		/// returns the minimum number that is not equal to zero.
		/// </summary>
		/// <param name="num1"></param>
		/// <param name="num2"></param>
		/// <returns></returns>
		static public int MinNotEqualZero(int num1, int num2)
		{
			int result;
			if (num1 != 0 && num2 != 0)
				result = Math.Min(num1, num2);
			else if (num1 != 0)
				result = num1;
			else if (num2 != 0)
				result = num2;
			else 
				result = 0;

			return result;
		}

		/// <summary>
		/// returns the minimum number that is not equal to zero.
		/// </summary>
		/// <param name="num1"></param>
		/// <param name="num2"></param>
		/// <returns></returns>
		static public double MinNotEqualZero(double num1, double num2)
		{
			double result;
			if (num1 != 0 && num2 != 0)
				result = Math.Min(num1, num2);
			else if (num1 != 0)
				result = num1;
			else if (num2 != 0)
				result = num2;
			else 
				result = 0;

			return result;
		}

    	static public int GregorianHashCode(DateTime aDate)
		{
			return (aDate.Year << 9) + (aDate.Month << 5) + aDate.Day; 
		}

        // begin TT#1074 - MD - Jellis Group Allocation - Inventory Min Max Broken
        /// <summary>
        /// returns a long with aInt_1 in bits 32-63 and aInt_0 in bits 0-31 (neither can be negative)
        /// </summary>
        /// <param name="aInt_0">Integer to put in bits 0-31</param>
        /// <param name="aInt_1">Integer to put in bits 32-63</param>
        /// <returns></returns>
        static public long PackIntegers(int aInt_0, int aInt_1)
        {
            return ((((long)aInt_1) << 32) + (long)aInt_0);
        }
        /// <summary>
        /// returns a 2-dimensional integer array with bits 0-31 of the long in position 0 and bits 32-63 in position 1
        /// </summary>
        /// <param name="aIntegerPackedLong">Long to split into 2 integers</param>
        /// <returns>2-dimensional integer array: int[0] = bits 0-31, int[1] = bits 32-63</returns>
        static public int[] UnPackLong (long aLongInteger)
        {
            int[] ints = new int[2];
            ints[0] = (int)aLongInteger;
            ints[1] = (int)(aLongInteger >> 32);
            return ints;
        }
        // end TT#1074 - MD - Jellis Group Allocation - Inventory Min Max Broken

		/// <summary>
		/// Gets random number.
		/// </summary>
		/// <returns>Random Double</returns>
		static public double GetRandomDouble() 
		{
			try
			{
				if (_random == null)
				{
					_random = new Random((int)DateTime.Now.Ticks);
				}

                if (_disableRandom)
                {
                    return 1.0;
                }
                else
                {
                    return _random.NextDouble();
                }
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets random number.
		/// </summary>
		/// <returns>Random positive integer.</returns>
		static public int GetRandomInteger()
		{
			try
			{
				if (_random == null)
				{
					_random = new Random((int)DateTime.Now.Ticks);
				}

				return _random.Next();
			}
			catch
			{
				throw;
			}
		}
		// begin MID Track 4024 Header ID is invalid as file name
		static private bool firstTimeFileNameString = true; 
		static private Regex objFileNamePattern;
		//static private String fileNameStringPattern = "^[^\\/:*?\"<>|]+$";

		static private void CreateRegExFileNamePatterns()
		{
			firstTimeFileNameString = false;
			objFileNamePattern=new Regex("^[^\\" + Include.HeaderNameExcludedCharacters + "]+$");
			// NOTE: for some reason Regex cannot see the '\' in the include string; think it is because it
			// is treating it as an escape character, so added it again here
		}
		/// <summary>
		/// Determines if a string can be used as a file name in Windows
		/// </summary>
		/// <param name="aFileName">String representation of the potential file name</param>
		/// <returns>True: if the string is a valid file name; False otherwise</returns>
		static public bool ValidFileName(string aFileName)
		{
			if (firstTimeFileNameString)
			{
				CreateRegExFileNamePatterns();
			}
			return objFileNamePattern.IsMatch(aFileName);
		}
        // end MID Track 4024 Header ID is invalid as file name

		// BEgin TT#503 - protect the Monitor logs from special characters
		static public string ValidAndReplaceFileName(string aFileName)
		{
			if (firstTimeFileNameString)
			{
				CreateRegExFileNamePatterns();
			}
			//===========================
			// Invalid characters found
			//===========================
			if (!objFileNamePattern.IsMatch(aFileName))
			{
				foreach (char c in objFileNamePattern.ToString())
				{
					aFileName = aFileName.Replace(c, '_');
				}
			}

			return aFileName;
		}
		// End TT#503 - protect the Monitor logs from special characters
        // begin TT#555 - Total Sales is an aggregate variable not on the database
        /// <summary>
        /// Converts an "InFix" or "Algebraic" expression to a "PostFix" or "Reverse Polish" expression
        /// </summary>
        /// <param name="aInFixExpression">The InFix (Algebraic) expression to convert</param>
        /// <returns>String containing the corresponding "PostFix" (Reverse Polish) expression.</returns>
        static public string ConvertInFixToPostFixNotation(string aInFixExpression)
        {
            StringBuilder postFixExpression = new StringBuilder();
            Stack<string> operatorStack = new Stack<string>();
            char[] delimiter = { ' ' };
            string[] inFixTokens = aInFixExpression.Split(delimiter);
            foreach (string inFixToken in inFixTokens)
            {
                ProcessInFixToken(inFixToken, ref operatorStack, ref postFixExpression);
            }
            string poppedOperator;
            while (operatorStack.Count > 0)
            {
                poppedOperator = operatorStack.Pop();
                if (poppedOperator == "(")
                {
                    throw new ArithmeticException("Unbalanced parenthesis in [" + aInFixExpression + "]");
                }
                postFixExpression.Append(operatorStack.Pop() + " ");
            }
            return postFixExpression.ToString();
        }
        /// <summary>
        /// Processes a token (or string of tokens without intervening spaces) from an InFix Expression 
        /// </summary>
        /// <param name="aInFixToken">Token or string of tokens from an InFix Expression with all spaces removed</param>
        /// <param name="aOperatorStack">Stack containing previously processed operators waiting to be put in the PostFixExpression</param>
        /// <param name="aPostFixExpression">Post Fix Expression that is being built</param>
        static private void ProcessInFixToken(string aInFixToken, ref Stack<string> aOperatorStack, ref StringBuilder aPostFixExpression)
        {
            if (aInFixToken.Length == 1)
            {
                switch (aInFixToken)
                {
                    case ("("):
                        {
                            aOperatorStack.Push(aInFixToken);
                            break;
                        }
                    case ("+"):
                    case ("-"):
                    case ("*"):
                    case ("/"):
                    case ("%"):
                    case ("^"):
                        {
                            ProcessOperator(aInFixToken, ref aOperatorStack, ref aPostFixExpression);
                            break;
                        }
                    case (")"):
                        {
                            string poppedToken;
                            bool continuePopping = true;
                            while (continuePopping)
                            {
                                if (aOperatorStack.Count > 0)
                                {
                                    poppedToken = aOperatorStack.Pop();
                                    if (poppedToken == "(")
                                    {
                                        continuePopping = false;
                                    }
                                    else
                                    {
                                        aPostFixExpression.Append(poppedToken + " ");
                                    }
                                }
                                else
                                {
                                    throw new ArithmeticException("Unbalanced parenthesis [(] in InFix (algebraic) expression");
                                }
                            }
                            break;
                        }
                    default:
                        {
                            if (char.IsDigit(aInFixToken[0]))
                            {
                                aPostFixExpression.Append(aInFixToken + " ");
                                break;
                            }
                            throw new ArithmeticException("Invalid token [" + aInFixToken + "] in an InFix or algebraic expression");
                        }
                }
            }
            else
            {
                int i = 0;
                int j;
                while (i < aInFixToken.Length)
                {
                    if (aInFixToken[i] == '@')
                    {
                        j = i + 1;
                        while (j < aInFixToken.Length
                               && char.IsDigit(aInFixToken[j]))
                        {
                            j++;
                        }
                        aPostFixExpression.Append(aInFixToken.Substring(i, j - i) + " ");
                        i = j;
                    }
                    else if (char.IsDigit(aInFixToken[i]))
                    {
                        j = i + 1;
                        while (j < aInFixToken.Length
                               && char.IsDigit(aInFixToken[j]))
                        {
                            j++;
                        }
                        aPostFixExpression.Append(aInFixToken.Substring(i, j - i) + " ");
                        i = j;
                    }
                    else
                    {
                        ProcessInFixToken(aInFixToken[i].ToString(), ref aOperatorStack, ref aPostFixExpression);
                        i++;
                    }
                }
            }
        }
        /// <summary>
        /// Processes an operator by placing it either on the stack or in the post fix expression
        /// </summary>
        /// <param name="aOperator">Operator to be processed</param>
        /// <param name="aOperatorStack">Stack containing previously processed operators</param>
        /// <param name="aPostFixExpression">Post Fix Expression being built.</param>
        static private void ProcessOperator(string aOperator, ref Stack<string> aOperatorStack, ref StringBuilder aPostFixExpression)
        {
            string peekOperator;
            int peekOperatorPrecedence;
            if (aOperatorStack.Count == 0)
            {
                aOperatorStack.Push(aOperator);
            }
            else
            {
                peekOperator = aOperatorStack.Peek();
                peekOperatorPrecedence = GetOperatorPrecedence(peekOperator[0]);
                int processOperatorPrecedence = GetOperatorPrecedence(aOperator[0]);
                while (processOperatorPrecedence < peekOperatorPrecedence)
                {
                    aPostFixExpression.Append(aOperatorStack.Pop() + " ");
                    peekOperator = aOperatorStack.Peek();
                    peekOperatorPrecedence = GetOperatorPrecedence(aOperator[0]);
                }
                aOperatorStack.Push(aOperator);
            }
        }
        /// <summary>
        /// Determines the precedence of the given operator
        /// </summary>
        /// <param name="aOperator">Operator whose precedence is to be determined</param>
        /// <returns>Precedence of the given operator (the larger the number the higher the precedence).</returns>
        static private int GetOperatorPrecedence(char aOperator)
        {
            switch (aOperator)
            {
                case ')':
                    {
                        return -2;
                    }
                case '(':
                    {
                        return -1;
                    }
                case '-':
                    {
                        return 6;
                    }
                case '+':
                    {
                        return 6;
                    }
                case '*':
                    {
                        return 8;
                    }
                case '/':
                    {
                        return 8;
                    }
                case '%':
                    {
                        return 8;
                    }
                case '^':
                    {
                        return 9;
                    }
                default:
                    {
                        throw new ArgumentException("Cannot determine precedence of unknown token [" + aOperator.ToString() + "]");
                    }
            }
        }
        // end TT#555 - Total Sales is an aggregate variable not on the database
 	}
}
