using System;
using System.Drawing;

namespace Checkers
{
	public class Field
	{
		#region default values
		private int[,] DefField = new int[8, 8]
		{{2, 0, 2, 0, 2, 0, 2, 0},
		 {0, 2, 0, 2, 0, 2, 0, 2},
		 {2, 0, 2, 0, 2, 0, 2, 0},
		 {0, 1, 0, 1, 0, 1, 0, 1},
		 {1, 0, 1, 0, 1, 0, 1, 0},
		 {0,-2, 0,-2, 0,-2, 0,-2},
		 {-2, 0,-2, 0,-2, 0,-2, 0},
		 {0,-2, 0,-2, 0,-2, 0,-2}};
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region private values
		private Color master;
		public int[,] mField = new int[8, 8];
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
		#region public properties
		public Color Master {get{return master;} set{master = value;}}
		// ---------- * ---------------- * --------------- * ------------
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region public functions
		public int Get(Dot d){return mField[d.Y, d.X];}
		public void Set(Dot d, int V){mField[d.Y, d.X] = V;}
		public void ToDefault(){mField = (int[,])DefField.Clone(); master = Color.White;}
		public void ReColor(){master = master == Color.White ? Color.Black : Color.White;}
		public override string ToString()
		{
			string res = ""; 
			for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) res += (char)(mField[i, j] + 50);
			return res;
		}
		public void FromString(string str)
		{
			for(int j = 0; j < 8; j++) for(int i = 0; i < 8; i++) mField[i, j] = (int)str[j * 8 + i] - 50;//
		}
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region special public functions
		public bool FBlack(Dot d){return mField[d.Y, d.X] != 0;}
		public bool Figure(Dot d){return Math.Abs(mField[d.Y, d.X]) > 1;}
		public bool FKing(Dot d){return Math.Abs(mField[d.Y, d.X]) > 8;}
		public bool FErrs(Dot d){return Math.Abs(mField[d.Y, d.X]) > 30;}
		public bool FErre(Dot d){return Math.Abs(mField[d.Y, d.X]) > 80;}
		public Color FColor(Dot d){return mField[d.Y, d.X] > 0 ? Color.White : Color.Black;}
		#endregion
		// ---------- * ---------------- * --------------- * ------------
		#region constructors
		public Field(){this.ToDefault();}
		#endregion
		// ---------- * ---------------- * --------------- * ------------	
	}
}
