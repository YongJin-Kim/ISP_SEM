using System;
using System.Diagnostics;

namespace SEC.Nanoeye.FourierTransform
{
	/// <summary>
	/// <p>A set of statistical utilities for complex number arrays</p>
	/// <p>Comments? Questions? Bugs? Tell Ben Houston at ben@exocortex.org</p>
	/// <p>Version: March 22, 2002</p>
	/// </summary>
	public class ComplexStats
	{
		//---------------------------------------------------------------------------------------------

		private ComplexStats() {
		}

		//---------------------------------------------------------------------------------------------
		//--------------------------------------------------------------------------------------------
		//--------------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate the sum
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public  ComplexF		Sum( ComplexF[] data ) {
			Debug.Assert( data != null );
			return	SumRecursion( data, 0, data.Length );
		}
		static private ComplexF		SumRecursion( ComplexF[] data, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= data.Length, "end = " + end + " and data.Length = " + data.Length );
			if( ( start + 1 ) == end ) {
				return	data[ start ];
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumRecursion( data, start, middle ) + SumRecursion( data, middle, end );
			}
		}

		/// <summary>
		/// Calculate the sum
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public  Complex		Sum( Complex[] data ) {
			Debug.Assert( data != null );
			return	SumRecursion( data, 0, data.Length );
		}
		static private Complex		SumRecursion( Complex[] data, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= data.Length, "end = " + end + " and data.Length = " + data.Length );
			if( ( start + 1 ) == end ) {
				return	data[ start ];
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumRecursion( data, start, middle ) + SumRecursion( data, middle, end );
			}
		}

		//--------------------------------------------------------------------------------------------
		//--------------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate the sum of squares
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public ComplexF		SumOfSquares( ComplexF[] data ) {
			Debug.Assert( data != null );
			return	SumOfSquaresRecursion( data, 0, data.Length );
		}
		static private ComplexF		SumOfSquaresRecursion( ComplexF[] data, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= data.Length, "end = " + end + " and data.Length = " + data.Length );
			if( ( start + 1 ) == end ) {
				ComplexF c = data[ start ];
				return	c * c;
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumOfSquaresRecursion( data, start, middle ) + SumOfSquaresRecursion( data, middle, end );
			}
		}

		/// <summary>
		/// Calculate the sum of squares
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public Complex		SumOfSquares( Complex[] data ) {
			Debug.Assert( data != null );
			return	SumOfSquaresRecursion( data, 0, data.Length );
		}
		static private Complex		SumOfSquaresRecursion( Complex[] data, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= data.Length, "end = " + end + " and data.Length = " + data.Length );
			if( ( start + 1 ) == end ) {
				Complex c = data[ start ];
				return	c * c;
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumOfSquaresRecursion( data, start, middle ) + SumOfSquaresRecursion( data, middle, end );
			}
		}

		//--------------------------------------------------------------------------------------------
		//--------------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate the mean (average)
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public ComplexF		Mean( ComplexF[] data ) {
			return	ComplexStats.Sum( data ) / data.Length;
		}

		/// <summary>
		/// Calculate the mean (average)
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public Complex		Mean( Complex[] data ) {
			return	ComplexStats.Sum( data ) / data.Length;
		}

		/// <summary>
		/// Calculate the variance
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public ComplexF	Variance( ComplexF[] data ) {
			Debug.Assert( data != null );
			if( data.Length == 0 ) {
				throw new DivideByZeroException( "length of data is zero" );
			}
			return	ComplexStats.SumOfSquares( data ) / data.Length - ComplexStats.Sum( data );
		}
		/// <summary>
		/// Calculate the variance 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public Complex	Variance( Complex[] data ) {
			Debug.Assert( data != null );
			if( data.Length == 0 ) {
				throw new DivideByZeroException( "length of data is zero" );
			}
			return	ComplexStats.SumOfSquares( data ) / data.Length - ComplexStats.Sum( data );
		}

		/// <summary>
		/// Calculate the standard deviation
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public ComplexF	StdDev( ComplexF[] data ) {
			Debug.Assert( data != null );
			if( data.Length == 0 ) {
				throw new DivideByZeroException( "length of data is zero" );
			}
			return	ComplexMath.Sqrt( ComplexStats.Variance( data ) );
		}
		/// <summary>
		/// Calculate the standard deviation 
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		static public Complex	StdDev( Complex[] data ) {
			Debug.Assert( data != null );
			if( data.Length == 0 ) {
				throw new DivideByZeroException( "length of data is zero" );
			}
			return	ComplexMath.Sqrt( ComplexStats.Variance( data ) );
		}

		//--------------------------------------------------------------------------------------------
		//--------------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate the root mean squared (RMS) error between two sets of data.
		/// </summary>
		/// <param name="alpha"></param>
		/// <param name="beta"></param>
		/// <returns></returns>
		static public float	RMSError( ComplexF[] alpha, ComplexF[] beta ) {
			Debug.Assert( alpha != null );
			Debug.Assert( beta != null );
			Debug.Assert( beta.Length == alpha.Length );

			return (float) Math.Sqrt( SumOfSquaredErrorRecursion( alpha, beta, 0, alpha.Length ) );
		}
		static private float	SumOfSquaredErrorRecursion( ComplexF[] alpha, ComplexF[] beta, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= alpha.Length, "end = " + end + " and alpha.Length = " + alpha.Length );
			Debug.Assert( beta.Length == alpha.Length );
			if( ( start + 1 ) == end ) {
				ComplexF delta = beta[ start ] - alpha[ start ];
				return	( delta.Re * delta.Re ) + ( delta.Im * delta.Im );
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumOfSquaredErrorRecursion( alpha, beta, start, middle ) + SumOfSquaredErrorRecursion( alpha, beta, middle, end );
			}
		}

		/// <summary>
		/// Calculate the root mean squared (RMS) error between two sets of data.
		/// </summary>
		/// <param name="alpha"></param>
		/// <param name="beta"></param>
		/// <returns></returns>
		static public double	RMSError( Complex[] alpha, Complex[] beta ) {
			Debug.Assert( alpha != null );
			Debug.Assert( beta != null );
			Debug.Assert( beta.Length == alpha.Length );

			return Math.Sqrt( SumOfSquaredErrorRecursion( alpha, beta, 0, alpha.Length ) );
		}
		static private double	SumOfSquaredErrorRecursion( Complex[] alpha, Complex[] beta, int start, int end ) {
			Debug.Assert( 0 <= start, "start = " + start );
			Debug.Assert( start < end, "start = " + start + " and end = " + end );
			Debug.Assert( end <= alpha.Length, "end = " + end + " and alpha.Length = " + alpha.Length );
			Debug.Assert( beta.Length == alpha.Length );
			if( ( start + 1 ) == end ) {
				Complex delta = beta[ start ] - alpha[ start ];
				return	( delta.Re * delta.Re ) + ( delta.Im * delta.Im );
			}
			else {
				int middle = ( start + end ) >> 1;
				return	SumOfSquaredErrorRecursion( alpha, beta, start, middle ) + SumOfSquaredErrorRecursion( alpha, beta, middle, end );
			}
		}


	}
}
