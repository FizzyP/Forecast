using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Proto
{
	public class CSVReader : CSV
	{
		protected string csv = string.Empty;
		protected string separator = ",";

		public CSVReader(string csv, string separator = "\",\"")
		{
			this.csv = csv;
			this.separator = separator;
			int maxRowLength = 0;

			foreach (string line in Regex.Split(csv, System.Environment.NewLine).ToList().Where(s => !string.IsNullOrEmpty(s)))
			{
				string[] values = Regex.Split(line, separator);

				for (int i = 0; i < values.Length; i++)
				{
					//Trim values
					values[i] = values[i].Trim('\"');
				}

				this.Add(values);

				if (values.Length > maxRowLength)
					maxRowLength = values.Length;
			}
		}
	}
}

