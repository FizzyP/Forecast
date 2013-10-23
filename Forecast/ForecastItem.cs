using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forecast
{
    public class ForecastItem : List<ModelAtom>
    {
        public string name;

        public double TotalProb
        {
            get
            {
                double total = 0.0;
                foreach (ModelAtom m in this)
                    total += m.Prob;
                return total;
            }
        }

        public ForecastItem simplify(int suggestedSimplifiedModelSize)
        {
            ModelAtom.sortByValue(this);

            ForecastItem simplerModel = new ForecastItem();
            List<ModelAtom> buffer = new List<ModelAtom>();
            double delta = this.Count() / (double)suggestedSimplifiedModelSize;
            double nextBinBoundary = delta;
            int count = 0;
            foreach (ModelAtom m in this)
            {
                buffer.Add(m);
                count++;
                if (count >= nextBinBoundary || count == this.Count())
                {
                    //  Combine all the models in the buffer and add them to the output.
                    simplerModel.Add(ModelAtom.combine(buffer));
                    //  Reset for the next bucket.
                    buffer.Clear();
                    //  Update nextBinBoundar
                    nextBinBoundary += delta;
                }
            }

            return simplerModel;
        }

        public static ForecastItem product(ForecastItem a, ForecastItem b)
        {
            ForecastItem item = new ForecastItem();
            foreach (ModelAtom m in a)
            {
                foreach (ModelAtom n in b)
                {
                    item.Add(ModelAtom.product(m, n));
                }
            }
            return item;
        }


        public ForecastItem() { }

        public static ForecastItem simplifiedProduct(ForecastItem a, ForecastItem b, int precision)
        {
            ForecastItem sp = product(a, b);
            return sp.simplify(precision);
        }

        public ForecastItem(ForecastItem copyMe)
        {
            this.name = copyMe.name;
            this.AddRange(copyMe);
        }

        public double HighestProbability
        {
            get {
                if (this.Count == 0)
                    return 0.0;

                ModelAtom mostLikely = this[0];
                foreach (ModelAtom m in this)
                {
                    if (m.Prob > mostLikely.Prob)
                        mostLikely = m;
                }

                return mostLikely.Value;
            }
        }


        public double getValueAtPercentile(double percentile)
        {
            if (this.Count == 0)
                return 0.0;
            else if (this.Count == 1)
                return this[0].Value;

            ModelAtom.sortByValue(this);
            double prob = percentile / 100.0;
            double totalProb = 0.0;
            double prevVal = 0.0;
            foreach (ModelAtom m in this)
            {
                if (m.Prob == 0.0)
                    continue;
                double newTotalProb = totalProb + m.Prob;
                if (totalProb <= prob && prob <= newTotalProb)
                {
                    //  Linearly interpolate between the values.
                    double t = (newTotalProb - prob) / (newTotalProb - totalProb);
                    return t * prevVal + (1 - t) * m.Value;
                }
                prevVal = m.Value;
                totalProb = newTotalProb;
            }

            return this[this.Count - 1].Value;
        }


        public double Mean
        {
            get
            {
                double total = 0.0;
                foreach (ModelAtom m in this)
                {
                    total += m.Prob * m.Value;
                }
                return total;
            }
        }

        public double ValueAtPeakDensity
        {
            get{
                List<double>
                densities = getDensityEstimate();
                double max = 0.0;
                double value = 0.0;
                for (int i=0; i < densities.Count; i++)
                {
                    if (densities[i] > max)
                    {
                        max = densities[i];
                        value = this[i].Value;
                    }
                }
                return value;
            }
        }

        public List<double> getDensityEstimate()
        {
            List<double> densities = new List<double>();
            double prevVal = 0.0; // reduced[0].Value;

            double totalProb = 0.0;

            for (int i = 0; i < this.Count; ++i)
            {
                ModelAtom a = this[i];

                double density;
                if (i == 0 || a.Value - prevVal == 0)
                {
                    //  If there is no next value
                    if (i == this.Count - 1 || a.Value == this[i + 1].Value)
                    {
                        density = 0;
                    }
                    else
                    {
                        density = a.Prob / Math.Abs(a.Value - this[i + 1].Value);
                    }
                }
                else if (i == this.Count - 1 || a.Value == this[i + 1].Value)
                {
                    density = a.Prob / Math.Abs(a.Value - prevVal);
                }
                else
                {
                    density = 0.5f * (a.Prob / Math.Abs(a.Value - prevVal) + a.Prob / Math.Abs(a.Value - this[i + 1].Value));
                    densities.Add(density);
                }
                totalProb += a.Prob;
                prevVal = a.Value;
            }

            return densities;
        }
    }
}
