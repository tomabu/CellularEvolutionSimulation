using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simulation
{
    class Simulator
    {
        private int numberOfOrganisms;
        private int numberOfIterations;
        private int iterationLength;
        private int numberOfMutations;
        private int lastID = 0;
        private float[] target;
        Random r;
        List<Pair<IOrganism,double>> Organisms = new List<Pair<IOrganism,double>>();
        public Simulator(int noo, int noi, int il, float[] t)
        {
            numberOfOrganisms = noo;
            numberOfIterations = noi;
            iterationLength = il;
            target = t;
            r = new Random();
            for (int i=0; i < numberOfOrganisms; i++)
            {
                Organisms.Add(new Pair<IOrganism, double> (new Organism(++lastID, r), double.PositiveInfinity ));
            }
            numberOfMutations = 1;//(int)0.02 * numberOfOrganisms * Organisms[0].First.Chromosome.Length;
        }

        public void RunIteration()
        {

            // Fitness Determination - calculating score basing on difference between target and current position of organism after one iteration
            Console.WriteLine("STAGE 1 - Fitness Determination");
            for (var i = 0; i < Organisms.Count; i++)
            {
                Organisms[i].Second = Math.Pow(1 /Organisms[i].First.Move(target[0], target[1], target[2]),5); //Count the fitness score
                //Console.WriteLine(Organisms[i].First.ID + ": " + Organisms[i].Second.ToString("n20")); //Display fitness score of an organism
                Console.WriteLine(Organisms[i].First.ID.ToString() + "   " + BitConverter.ToString(Organisms[i].First.Chromosome)); //Display ID and Chromosome
            }

            // Parent Selection
            Console.WriteLine("STAGE 2 - Parent Selection");
            // Create selection probability list with ordered data
            List<Pair<IOrganism, double>> ProbabilityList = new List<Pair<IOrganism, double>>();
            foreach(Pair<IOrganism,double> o in Organisms)
            {
                ProbabilityList.Add(new Pair<IOrganism,double>(o.First, o.Second));
            }
            ProbabilityList = ProbabilityList.OrderByDescending(x => x.Second).ToList();
            // Sum all values (to the power of three to increase difference between probabilities later)
            double sum = 0;
            for (var i = 0; i < ProbabilityList.Count; i++)
            {
                sum += ProbabilityList[i].Second;
            }
            // Normalize scores in probability list
            double tmp = 0;
                for (var i = 0; i < ProbabilityList.Count; i++)
            {
                tmp += ProbabilityList[i].Second / sum;
                ProbabilityList[i].Second =  tmp;
            }
            // Select half of population as parents
            List<Pair<IOrganism, double>> Parents = new List<Pair<IOrganism, double>>();
            while(Parents.Count != numberOfOrganisms/2)
            {
                double random = r.NextDouble();
                foreach(Pair<IOrganism,double> t in ProbabilityList)
                {
                    if (random < t.Second)
                    {
                        var found = Organisms.FirstOrDefault(v => v.First.ID == t.First.ID);
                        Parents.Add(new Pair<IOrganism,double>(t.First, found.Second));
                        ProbabilityList.Remove(t);
                        break;
                    }
                }
            }

            // scaling probability values for parent list
            Parents = Parents.OrderByDescending(x => x.Second).ToList();
            foreach (Pair<IOrganism, double> p in Parents)
            {
                p.Second = NthRoot(p.Second, 3);
            }
            sum = 0;
            for (var i = 0; i < Parents.Count; i++)
            {
                sum += Parents[i].Second;
            }
            tmp = 0;
            for (var i = 0; i < Parents.Count; i++)
            {
                tmp += Parents[i].Second / sum;
                Parents[i].Second = tmp;
            }

            //Show ID's of organisms that were chosen
            foreach (Pair<IOrganism, double> t in Parents)
            {
                Console.Write(t.First.ID.ToString()+"   "); //Display ID
                //Console.Write(t.Second + "   "); //Display cumulative probability of the Organism
                Console.WriteLine(BitConverter.ToString(t.First.Chromosome)); //Display Chromosome
            }
            // Offspring Production
            Console.WriteLine("STAGE 3 - OffSpring Production");
            List<Pair<IOrganism, double>> Offspring = new List<Pair<IOrganism, double>>();
            List<Pair<int, int>> Crossed = new List<Pair<int, int>>();
            int crossingsLeft = numberOfOrganisms/2;
            // Choose Reproduction Pairs
            while (crossingsLeft > 0)
            {
                double random = r.NextDouble();
                double random2 = r.NextDouble();
                IOrganism tmp1 = null, tmp2 = null;
                foreach (Pair<IOrganism, double> p in Parents)
                {
                    if (tmp1 == null && random < p.Second)
                    {
                        tmp1 = p.First;
                    }
                    if (tmp2 == null && random2 < p.Second)
                    {
                        tmp2 = p.First;
                    }
                }
                Pair<int, int> foundItem = null;
                if(Crossed.Count>0) foundItem = Crossed.FirstOrDefault(i => (i.First == tmp1.ID && i.Second == tmp2.ID) || (i.First == tmp2.ID && i.Second == tmp1.ID));
                if (tmp1 != null && tmp2 != null && tmp1 != tmp2 && foundItem == null)
                {
                    Pair<IOrganism, IOrganism> crossedPair = Crossover(tmp1, tmp2, r);
                    Offspring.Add(new Pair<IOrganism, double>(crossedPair.First, double.PositiveInfinity));
                    Offspring.Add(new Pair<IOrganism, double>(crossedPair.Second, double.PositiveInfinity));
                    Crossed.Add(new Pair<int, int>(tmp1.ID, tmp2.ID));
                    crossingsLeft--;
                }
            }

            // Offspring Mutation (Mutations are rare)
            Console.WriteLine("STAGE 4 - OffSpring Mutation");
            int counter = numberOfMutations+1;
            while(--counter>0)
            {
                int rnd = r.Next(0, Offspring.Count);
                IOrganism o = Offspring[rnd].First;
                Console.WriteLine(BitConverter.ToString(o.Chromosome));
                Offspring.RemoveAt(rnd);                
                Offspring.Add(new Pair<IOrganism, double>(Mutation(o, r), double.PositiveInfinity));
                IOrganism o1 = Offspring.FirstOrDefault(i => (i.First.ID == o.ID)).First;
                Console.WriteLine(BitConverter.ToString(o1.Chromosome));
            }

            Organisms.Clear();
            Organisms.AddRange(Offspring);

        }
        /// <summary>
        /// Chromosome crossover function.
        /// </summary>
        /// <param name="O1">Organism 1</param>
        /// <param name="O2">Organism 2</param>
        /// <param name="r">Random generator reference</param>
        /// <returns>Pair of Organisms</returns>
        public Pair<IOrganism,IOrganism> Crossover (IOrganism O1, IOrganism O2, Random r)
        {
            int random = r.Next(0, O1.Chromosome.Length);
            byte[] Chromosome1 = new byte[O1.Chromosome.Length];
            byte[] Chromosome2 = new byte[O2.Chromosome.Length];
            O1.Chromosome.CopyTo(Chromosome1, 0);
            O2.Chromosome.CopyTo(Chromosome2, 0);
            byte[] tmp = new byte[Chromosome1.Length];
            Chromosome1.Take(random).ToArray().CopyTo(tmp, 0);
            Chromosome2.Skip(random).Take(Chromosome2.Length - random).ToArray().CopyTo(tmp, random);
            Chromosome2.Take(random).ToArray().CopyTo(Chromosome1, 0);
            tmp.CopyTo(Chromosome2, 0);
            return new Pair<IOrganism, IOrganism>(new Organism(Chromosome1, ++lastID, r), new Organism(Chromosome2, ++lastID, r));
        }

        /// <summary>
        /// Mutating selected organism chormosome.
        /// </summary>
        /// <param name="o">Organism object</param>
        /// <param name="r">Random generator reference</param>
        /// <returns>Organism with mutated chromosome</returns>
        public IOrganism Mutation (IOrganism o, Random r)
        {
            var tmpChromosome = new BitArray(o.Chromosome);
            int mut = r.Next(0, tmpChromosome.Length);
            tmpChromosome[mut] = !tmpChromosome[mut];
            byte[] newChromosome = new byte[tmpChromosome.Length / 8];
            tmpChromosome.CopyTo(newChromosome, 0);
            return new Organism(newChromosome, o.ID, r);
        }

        public int GetNumberOfIterations()
        {
            return numberOfIterations;
        }

        /// <summary>
        /// Counts the n-th root of a number
        /// </summary>
        /// <param name="A">Number</param>
        /// <param name="n">Root degree</param>
        /// <returns>N-th root of the number</returns>
        public static double NthRoot(double A, int n)
        {
            double _n = (double)n;
            double[] x = new double[2];
            x[0] = A;
            x[1] = A / _n;
            while (Math.Abs(x[0] - x[1]) > .000000000000001)
            {
                x[1] = x[0];
                x[0] = (1 / _n) * (((_n - 1) * x[1]) + (A / Math.Pow(x[1], _n - 1)));

            }
            return x[0];
        }

    }
}
