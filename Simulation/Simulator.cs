using System;
using System.Collections.Generic;
using System.Linq;
using Eppy;

namespace Simulation
{
    class Simulator
    {
        private int numberOfOrganisms;
        private int numberOfIterations;
        private int iterationLength;
        private int lastID = 0;
        private Tuple<int, int, int> target;
        List<Tuple<IOrganism, float>> Organisms = new List<Tuple<IOrganism, float>>();
        public Simulator(int noo, int noi, int il, Tuple<int,int,int> t)
        {
            numberOfOrganisms = noo;
            numberOfIterations = noi;
            iterationLength = il;
            target = t;
            Random r = new Random();
            for (int i=0; i < numberOfOrganisms; i++)
            {
                Organisms.Add(new Tuple<IOrganism,float>(new Organism(lastID++,r),float.PositiveInfinity));
            }
        }

        public void RunIteration()
        {
            // Fitness Determination - calculating difference between target and current position of organism after one iteration
            Console.WriteLine("STAGE 1 - Fitness Determination");
            for (var i = 0; i < Organisms.Count; i++)
            {
                Organisms[i] = new Tuple<IOrganism, float>(Organisms[i].Item1, Organisms[i].Item1.Move(target.Item1, target.Item2, target.Item3));
                Console.WriteLine(Organisms[i].Item2.ToString("n3"));
            }

            // Parent Selection
            Console.WriteLine("STAGE 2 - Parent Selection");
            // Order Results
            List<Tuple<IOrganism,float>> orderedList = Organisms.OrderByDescending(x => x.Item2).ToList();
            // Create selection probability list
            List<Tuple<IOrganism, float>> probabilityList = new List<Tuple<IOrganism, float>>();
            // Sum all values (to the power of three to increase difference between probabilities later)
            float sum = 0.0f;
            for (var i = 0; i < orderedList.Count; i++)
            {
                sum += (float)Math.Pow((double)orderedList[i].Item2,3);
            }
            // Add to cumultive probability list new tuples with normalized second value
            float cumulative = 0.0f;
            for (var i = 0; i < orderedList.Count; i++)
            {
                cumulative += (float)Math.Pow((double)orderedList[i].Item2, 3) / sum;
                probabilityList.Add(new Tuple<IOrganism, float>(orderedList[i].Item1, cumulative));
            }
            // Select half of population as parents
            Organisms.Clear();
            Organisms = new List<Tuple<IOrganism, float>>();
            Random r = new Random();
            for (var i = 0; i < numberOfOrganisms/2; i++)
            {
                double tmp = r.NextDouble();
                foreach(Tuple<IOrganism,float> t in probabilityList)
                {
                    if (tmp < t.Item2)
                    {
                        Organisms.Add(t);
                        probabilityList.Remove(t);
                        break;
                    }
                }
            }

            //foreach (Tuple<IOrganism, float> t in Organisms)
            //{
            //    Console.WriteLine(t.Item1.ID.ToString());
            //}

            // Offspring Production



            // Offspring Mutation

        }

        public int GetNumberOfIterations()
        {
            return numberOfIterations;
        }
    }
}
