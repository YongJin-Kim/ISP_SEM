using System;
using System.Diagnostics;


namespace SEC.Nanoeye.FourierTransform {

	/// <summary>
	/// <p>Various mathematical functions for complex numbers.</p>
	/// <p>Comments? Questions? Bugs? Tell Ben Houston at ben@exocortex.org</p>
	/// <p>Version: March 22, 2002</p>
	/// </summary>
	public class ComplexMath {
		
		//---------------------------------------------------------------------------------------------------

		private ComplexMath() {
		}

		//---------------------------------------------------------------------------------------------------

		/// <summary>
		/// Swap two complex numbers
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		static public void Swap( ref Complex a, ref Complex b ) {
			Complex temp = a;
			a = b;
			b = temp;
		}

		/// <summary>
		/// Swap two complex numbers
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		static public void Swap( ref ComplexF a, ref ComplexF b ) {
			ComplexF temp = a;
			a = b;
			b = temp;
		}
		
		//---------------------------------------------------------------------------------------------------

		static private double	_halfOfRoot2	= 0.5 * Math.Sqrt( 2 );

		/// <summary>
		/// Calculate the square root of a complex number
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		static public ComplexF	Sqrt( ComplexF c ) {
			double	x	= c.Re;
			double	y	= c.Im;

			double	modulus	= Math.Sqrt( x*x + y*y );
			int		sign	= ( y < 0 ) ? -1 : 1;

			c.Re		= (float)( _halfOfRoot2 * Math.Sqrt( modulus + x ) );
			c.Im	= (float)( _halfOfRoot2 * sign * Math.Sqrt( modulus - x ) );

			return	c;
		}

		/// <summary>
		/// Calculate the square root of a complex number
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		static public Complex	Sqrt( Complex c ) {
			double	x	= c.Re;
			double	y	= c.Im;

			double	modulus	= Math.Sqrt( x*x + y*y );
			int		sign	= ( y < 0 ) ? -1 : 1;

			c.Re		= (double)( _halfOfRoot2 * Math.Sqrt( modulus + x ) );
			c.Im	= (double)( _halfOfRoot2 * sign * Math.Sqrt( modulus - x ) );

			return	c;
		}

		//---------------------------------------------------------------------------------------------------

		/// <summary>
		/// Calculate the power of a complex number
		/// </summary>
		/// <param name="c"></param>
		/// <param name="exponent"></param>
		/// <returns></returns>
		static public ComplexF	Pow( ComplexF c, double exponent ) {
			double	x	= c.Re;
			double	y	= c.Im;
			
			double	modulus		= Math.Pow( x*x + y*y, exponent * 0.5 );
			double	argument	= Math.Atan2( y, x ) * exponent;

			c.Re		= (float)( modulus * System.Math.Cos( argument ) );
			c.Im = (float)( modulus * System.Math.Sin( argument ) );

			return	c;
		}

		/// <summary>
		/// Calculate the power of a complex number
		/// </summary>
		/// <param name="c"></param>
		/// <param name="exponent"></param>
		/// <returns></returns>
		static public Complex	Pow( Complex c, double exponent ) {
			double	x	= c.Re;
			double	y	= c.Im;
			
			double	modulus		= Math.Pow( x*x + y*y, exponent * 0.5 );
			double	argument	= Math.Atan2( y, x ) * exponent;

			c.Re		= (double)( modulus * System.Math.Cos( argument ) );
			c.Im = (double)( modulus * System.Math.Sin( argument ) );

			return	c;
		}
		
		//---------------------------------------------------------------------------------------------------

	}
}
