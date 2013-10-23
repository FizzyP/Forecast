using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proto;

namespace Forecast
{
    public class ForecastModel : List<ForecastItem>
    {
        private ForecastItemGenerator itemGenerator = ForecastItemGenerator.Default;

        private ForecastModel() { }

        public ForecastModel(CSV csv, int precision)
        {

            foreach (string[] row in CommentStrippedCsv(csv))
            {
                List<double> data = new List<double>();
                //  Leftmost entry is the name of the contribution
                string name = row[0];
                for (int i = 1; i < row.Count(); ++i)
                {
                    //  Read up until the first empty cell
                    if (row[i] == "")
                        break;

                    data.Add(Double.Parse(row[i]));
                }
                ForecastItem item = itemGenerator.generateForecastItemFromList(name, data, precision);
                this.Add(item);
            }
        }

        public IEnumerable<string[]> CommentStrippedCsv(CSV csv)
        {
            foreach (string[] row in csv)
            {
                //  Ignore rows that are empty or start with an empty cell.
                if (row.Length == 0 || row[0] == "") continue;
                if (row[0][0] == '#')
                {
                    interpretCommand(row);
                    continue;
                }
                yield return row;
            }
        }

        private void interpretCommand(string[] command)
        {
            string commandName = command[0].Substring(1);
            switch (commandName)
            {
                case "set":
                    readSetCommand(command);
                    break;

                default:
                    throw new ForecastParsingException("Unrecognized # directive \"" + command[0] + "\".");
                    break;
            }
        }

        private void readSetCommand(string[] command)
        {
            if (command.Length < 1)
                throw new ForecastParsingException("#set directive missing target in next cell.");

            switch (command[1])
            {
                case "interpretation":
                    readSetInterpretationCommand(command, 2);
                    break;

                default:
                    throw new ForecastParsingException("Unrecognized #set directive: \"" + command[1] + "\".");
                    break;
            }
        }

        private void readSetInterpretationCommand(string[] command, int idx)
        {
            if (command.Length < 1)
                throw new ForecastParsingException("#set interpretation directive missing value in next cell.");

            string keyword = command[idx];

            switch (keyword)
            {
                case "default":
                    itemGenerator = ForecastItemGenerator.Default;
                    break;

                case "fair":
                    itemGenerator = ForecastItemGenerator.Fair;
                    break;

                case "weighted":
                    itemGenerator = readSetInterpretationWeightedCommand(command, idx + 1);
                    break;

                case "polygonal":
                    itemGenerator = readSetInterpretationPolygonalCommand(command, idx + 1);
                    break;

                default:
                    throw new ForecastParsingException("Unrecognized #set interpretation directive: \"" + keyword + "\".");
                    break;
            }
        }

        private ForecastItemGenerator readSetInterpretationWeightedCommand(string[] command, int idx)
        {
            List<double> weights = new List<double>();
            for (int i = idx; i < command.Length; ++i)
            {
                if (command[i] == "")
                    break;

                try
                {
                    weights.Add(Double.Parse(command[i]));
                }
                catch
                {
                    throw new ForecastParsingException("Set interpretation weighted command expects floating point weight values.");
                }
            }
            return new WeightedForecastItemGenerator(weights);
        }

        private ForecastItemGenerator readSetInterpretationPolygonalCommand(string[] command, int idx)
        {
            List<double> weights = new List<double>();
            for (int i = idx; i < command.Length; ++i)
            {
                if (command[i] == "")
                    break;

                try
                {
                    weights.Add(Double.Parse(command[i]));
                }
                catch
                {
                    throw new ForecastParsingException("Set interpretation weighted command expects floating point weight values.");
                }
            }
            return new PolygonalForecastItemGenerator(weights);
        }

        public ForecastItem reduce(int precision)
        {
            return reduce(precision, precision);
        }


        public ForecastItem reduce(int precision, int precisionOut)
        {
            if (this.Count() == 0) return new ForecastItem();
            if (this.Count() == 1)
            {
                return new ForecastItem(this[0]);
            }

            //  At least two
            ForecastItem m = ForecastItem.simplifiedProduct(this[0], this[1], precision);
            for (int i= 2; i < this.Count(); ++i)
            {
                m = ForecastItem.simplifiedProduct(m, this[i], precision);
            }
            return m.simplify(precisionOut);
        }
    }
}
