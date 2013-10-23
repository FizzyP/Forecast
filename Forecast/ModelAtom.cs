using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forecast
{
    public class ModelAtom
    {
        private double prob;
        private double value;

        public double Prob
        {
            get { return prob; }
            set { prob = value; }
        }

        public double Value
        {
            get { return value; }
        }

        public ModelAtom()
        {
            this.prob = 0;
            this.value = 0;
        }

        public ModelAtom(double prob, double value)
        {
            this.prob = prob;
            this.value = value;
        }


        public static ModelAtom product(ModelAtom a, ModelAtom b)
        {
            return new ModelAtom(a.prob * b.prob, a.value + b.value);
        }

        public static ModelAtom combine(List<ModelAtom> atoms)
        {
            ModelAtom m = new ModelAtom();
            foreach (ModelAtom atom in atoms)
            {
                m.prob += atom.prob;
                m.value += atom.value * atom.prob;
            }
            m.value /= m.prob;
            return m;
        }

        public static void sortByValue(List<ModelAtom> atoms)
        {
            atoms.Sort((x, y) => CompareByValue(x,y));
        }

        private static int CompareByValue(ModelAtom a, ModelAtom b)
        {
            if (a.value < b.value) return -1;
            else if (a.value == b.value) return 0;
            else return 1;
        }
    }
}
