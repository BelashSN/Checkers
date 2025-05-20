using System;

namespace Checkers
{
	public class Dot
	{
		#region private values
		private int mX, mY;
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region constructors
		public Dot(){mX = -1; mY = -1;}
		public Dot(int nX, int nY){mX = nX; mY = nY;}
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region public functions
		public override string ToString(){return ((char)('a' + mX)).ToString() + (mY + 1).ToString();}
		public void Set(int nX, int nY){mX = nX; mY = nY;}
		public static Dot FromArray(int ID){int nY = ID / 8; int nX = ID - (nY * 8); return new Dot(nX, nY);}
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region public properties
		public bool Successful {get{return mX > -1 && mX < 8 && mY > -1 && mY < 8;}}
		public int X {get{return mX;}}
		public int Y {get{return mY;}}
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region public operators
		public static Dot operator>>(Dot d, int n) {return new Dot(d.X + Math.Abs(n), d.Y + n);}
		public static Dot operator<<(Dot d, int n) {return new Dot(d.X - Math.Abs(n), d.Y + n);}
		public static bool operator==(Dot d1, Dot d2) {return ((d1.X == d2.X) && (d1.Y == d2.Y));}
		public static bool operator!=(Dot d1, Dot d2) {return ((d1.X != d2.X) || (d1.Y != d2.Y));}
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
	}
}
