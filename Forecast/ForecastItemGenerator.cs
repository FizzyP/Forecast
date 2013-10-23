using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forecast
{
    public abstract class ForecastItemGenerator
    {
        private static ForecastItemGenerator _Fair = new FairForecastItemGenerator();
        public static ForecastItemGenerator Fair
        {
            get { return _Fair; }
        }
        public static ForecastItemGenerator Default
        {
            get { return _Fair; }
        }

        public abstract ForecastItem generateForecastItemFromList(string name, List<double> data, int precision);
    }


    public class FairForecastItemGenerator : ForecastItemGenerator
    {
        public override ForecastItem generateForecastItemFromList(string name, List<double> data, int precision)
        {
            ForecastItem item = new ForecastItem();
            //  Leftmost entry is the name of the contribution
            item.name = name;
            //  Now treat the others as uniformly distributed across percentiles
            double prob = 1.0f / data.Count();

            for (int i = 0; i < data.Count(); ++i)
            {
                item.Add(new ModelAtom(prob, data[i]));
            }
            return item;
        }
    }

    public class WeightedForecastItemGenerator : ForecastItemGenerator
    {
        List<double> weights;

        public WeightedForecastItemGenerator(List<double> weights)
        {
            this.weights = new List<double>(weights);
        }

        public override ForecastItem generateForecastItemFromList(string name, List<double> data, int precision)
        {
            if (data.Count != weights.Count)
            {
                throw new ForecastParsingException("For weighted forecast item interpretation, the item description must have the same number of entries as their are weights.");
            }

            ForecastItem item = new ForecastItem();
            //  Leftmost entry is the name of the contribution
            item.name = name;
            for (int i = 0; i < data.Count(); ++i)
            {
                try
                {
                    item.Add(new ModelAtom(weights[i], data[i]));
                }
                catch
                {
                    throw new ForecastParsingException("Forecast Item description incompatible with current forecast item interpretation.");
                }
            }
            return item;
        }
    }


    public class PolygonalForecastItemGenerator : ForecastItemGenerator
    {
        List<double> weights;

        public PolygonalForecastItemGenerator(List<double> weights)
        {
            if (weights.Count < 2)
                throw new ForecastParsingException("Polygonal item interpretation requires at least two values.");

            this.weights = new List<double>(weights);
        }

        public override ForecastItem generateForecastItemFromList(string name, List<double> data, int precision)
        {
            if (data.Count != weights.Count)
            {
                throw new ForecastParsingException("For polygonal forecast item interpretation, the item description must have the same number of entries as their are weights.");
            }
            for (int i = 1; i < data.Count(); ++i)
            {
                if (data[i-1] >= data[i])
                    throw new ForecastParsingException("Polygonal forecast item interpretation expects a monotone increasing sequence of values.");
            }

            ForecastItem item = new ForecastItem();
            //  Leftmost entry is the name of the contribution
            item.name = name;

            double totalProb = 0.0f;
            double step = (data[data.Count - 1] - data[0]) / precision;
            double currValue = data[0];
            int currLeftEndPtIdx = 0;

            for (int i = 0; i < precision;
                    i++, currValue += step)
            {
                //  See if we need to move on to the next polygonal piece
                if (currValue > data[currLeftEndPtIdx + 1]) {
                    currLeftEndPtIdx++;
                    //  Possible round off error problems if we don't check this?
                    if (currLeftEndPtIdx == data.Count - 1) {
                        currLeftEndPtIdx--;
                    }
                }
                

                double t = (data[currLeftEndPtIdx + 1] - currValue) / (data[currLeftEndPtIdx + 1] - data[currLeftEndPtIdx]);
                double prob = weights[currLeftEndPtIdx + 1] * (1 - t) + t * weights[currLeftEndPtIdx];
                totalProb += prob;

                try
                {
                    item.Add(new ModelAtom(prob, currValue));
                }
                catch
                {
                    throw new ForecastParsingException("Forecast Item description incompatible with current forecast item interpretation.");
                }

                
            }


            //  Go back and correct the total mass
            foreach (ModelAtom m in item)
            {
                m.Prob /= totalProb;
            }

            return item;
        }
    }


}
