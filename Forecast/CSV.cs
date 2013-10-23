using System;
using System.Collections.Generic;

namespace Proto
{
	public class CSV : List<string[]> {

		public CSV() {
		}

		public CSV(CSV csv) {
			foreach (string[] row in csv) {
				Add ((string[]) row.Clone ());
			}
		}


		public int Rows {
			get {
				return Count;
			}
		}

		public int Cols {
			get {
				if (Rows == 0)
					return 0;
				else
					return this [0].Length;
			}
		}

		public int CellCount {
			get {
				return Rows * Cols;
			}
		}


		public void makeRectangular() {
			int maxRowLength = 0;
			foreach (string[] row in this) {
				if (row.Length > maxRowLength)
					maxRowLength = row.Length;
			}

			//	Make all the rows the same length
			for (int i=0; i < this.Count; i++) {
				string[] temp = this [i];
				this[i] = new string[maxRowLength];
				for (int j=0; j < temp.Length; j++) {
					this [i] [j] = temp [j];
				}
				for (int j=temp.Length; j < maxRowLength; j++)
					this[i][j] = null;
			}
		}


	}
}

