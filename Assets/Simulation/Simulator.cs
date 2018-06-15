using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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
        System.Random r;
        List<Pair<Organism,double>> Organisms = new List<Pair<Organism,double>>();
        //NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Simulator(int noo, int noi, int il, float[] t)
        {
            numberOfOrganisms = noo;
            numberOfIterations = noi;
            iterationLength = il;
            target = t;
            r = new System.Random();
            for (int i=0; i < numberOfOrganisms; i++)
            {
                byte[] chromosome = GenerateChromosome(r);
                Organisms.Add(new Pair<Organism, double> (new Organism(++lastID, chromosome , r), double.PositiveInfinity ));
            }
            numberOfMutations = 1;//(int)0.02 * numberOfOrganisms * Organisms[0].First.Chromosome.Length;
        }

        public void RunIteration()
        {

            // Fitness Determination - calculating score basing on difference between target and current position of organism after one iteration
            //logger.Info("STAGE 1 - Fitness Determination");
            //Debug.Log("STAGE 1 - Fitness Determination");
            for (var i = 0; i < Organisms.Count; i++)
            {
                Organisms[i].Second = Math.Pow(1 /Organisms[i].First.Move(target[0], target[1], target[2]),5); //Count the fitness score
                //Debug.Log(Organisms[i].First.ID + ": " + Organisms[i].Second.ToString("n20")); //Display fitness score of an organism
                Debug.Log(Organisms[i].First.ID.ToString() + "   " + BitConverter.ToString(Organisms[i].First.Chromosome)); //Display ID and Chromosome
            }
            foreach (Pair<Organism, double> o in Organisms)
            {
                //logger.Info("ID: " + o.First.ID + "    Fitness Score: " + o.Second.ToString());
            }
            // Parent Selection
            //logger.Info("STAGE 2 - Parent Selection");
            // Debug.Log("STAGE 2 - Parent Selection");
            // Create selection probability list with ordered data
            List<Pair<Organism, double>> ProbabilityList = new List<Pair<Organism, double>>();
            foreach(Pair<Organism,double> o in Organisms)
            {
                ProbabilityList.Add(new Pair<Organism,double>(o.First, o.Second));
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
            List<Pair<Organism, double>> Parents = new List<Pair<Organism, double>>();
            while(Parents.Count != numberOfOrganisms/2)
            {
                double random = r.NextDouble();
                foreach(Pair<Organism,double> t in ProbabilityList)
                {
                    if (random < t.Second)
                    {
                        var found = Organisms.FirstOrDefault(v => v.First.ID == t.First.ID);
                        Parents.Add(new Pair<Organism,double>(t.First, found.Second));
                        ProbabilityList.Remove(t);
                        break;
                    }
                }
            }

            // scaling probability values for parent list
            Parents = Parents.OrderByDescending(x => x.Second).ToList();
            foreach (Pair<Organism, double> p in Parents)
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
            //logger.Info("IDs of chosen organisms: ");
            //Show ID's of organisms that were chosen
            foreach (Pair<Organism, double> t in Parents)
            {
                //logger.Info(t.First.ID + "   ");
            }
            // Offspring Production
            //logger.Info("STAGE 3 - OffSpring Production");
            //Debug.Log("STAGE 3 - OffSpring Production");
            List<Pair<Organism, double>> Offspring = new List<Pair<Organism, double>>();
            List<Pair<int, int>> Crossed = new List<Pair<int, int>>();
            int crossingsLeft = numberOfOrganisms/2;
            // Choose Reproduction Pairs
            while (crossingsLeft > 0)
            {
                double random = r.NextDouble();
                double random2 = r.NextDouble();
                Organism tmp1 = null, tmp2 = null;
                foreach (Pair<Organism, double> p in Parents)
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
                    //logger.Info("Crossing organisms {0} and {1}", tmp1.ID, tmp2.ID);
                    Pair<Organism, Organism> crossedPair = Crossover(tmp1, tmp2, r);
                    Offspring.Add(new Pair<Organism, double>(crossedPair.First, double.PositiveInfinity));
                    Offspring.Add(new Pair<Organism, double>(crossedPair.Second, double.PositiveInfinity));
                    Crossed.Add(new Pair<int, int>(tmp1.ID, tmp2.ID));
                    crossingsLeft--;
                }
            }

            // Offspring Mutation (Mutations are rare)
            //logger.Info("STAGE 4 - OffSpring Mutation");
            //Debug.Log("STAGE 4 - OffSpring Mutation");
            int counter = numberOfMutations+1;
            while(--counter>0)
            {
                int rnd = r.Next(0, Offspring.Count);
                Organism o = Offspring[rnd].First;
                Debug.Log(BitConverter.ToString(o.Chromosome));
                //logger.Info("Mutation of organism " + o.ID + " performed!");
                Offspring.RemoveAt(rnd);                
                Offspring.Add(new Pair<Organism, double>(Mutation(o, r), double.PositiveInfinity));
                Organism o1 = Offspring.FirstOrDefault(i => (i.First.ID == o.ID)).First;
                Debug.Log(BitConverter.ToString(o1.Chromosome));
            }

            Organisms.Clear();
            Organisms.AddRange(Offspring);

        }

        public static byte[] GenerateChromosome(System.Random random)
        {
            List<byte[]> IDs = new List<byte[]>();
            List<Pair<int, int>> pairs = new List<Pair<int, int>>();
            int lastID = 1;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            int randomNumber = random.Next(2, 100);
            //Console.WriteLine(randomNumber*82);
            float[] lastPosition = new float[3] { 0.0f, 0.0f, 0.0f };
            for (int i = 0; i < randomNumber; i++)
            {
                // HEADER (10 bytes)
                // random IDs
                double id1choice, id2choice;
                int chosenID1 = 0, chosenID2 = 0;
                bool firstType, secondType;
                do
                {
                    id1choice = random.NextDouble();
                    id2choice = random.NextDouble();
                    chosenID1 = (id1choice > 0.85 && IDs.Count > 0) ? (int)Math.Floor(random.NextDouble() * IDs.Count) + 1 : lastID++;
                    chosenID2 = (id2choice > 0.85 && IDs.Count > 0) ? (int)Math.Floor(random.NextDouble() * IDs.Count) + 1 : lastID;
                } while ((chosenID1 == chosenID2) || pairs.Any(p => p.First == chosenID1 && p.Second == chosenID2 || p.First == chosenID2 && p.Second == chosenID1));
                pairs.Add(new Pair<int, int>(chosenID1, chosenID2));

                stream.Append(BitConverter.GetBytes(chosenID1)); //Id of second Joint (4 bytes)
                firstType = random.Next(0, 2) == 1 ? true : false;
                stream.Append(BitConverter.GetBytes(firstType)); //Type of joint (1 byte)
                stream.Append(BitConverter.GetBytes(chosenID2)); //Id of second Joint (4 bytes)
                secondType = random.Next(0, 2) == 1 ? true : false;
                stream.Append(BitConverter.GetBytes(secondType)); //Type of joint (1 byte)


                // BODY (72 bytes)
                // First Joint
                if (chosenID1 <= IDs.Count)
                {
                    stream.Append(IDs.ElementAt(chosenID1 - 1));
                }
                else
                {
                    System.IO.MemoryStream tmp = new System.IO.MemoryStream();
                    tmp.Append(BitConverter.GetBytes((float)(lastPosition[0] + 4 * random.NextDouble() - 1))); // Position x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(lastPosition[1] + 4 * random.NextDouble()))); // Position y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Position z-axis (4 bytes)

                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Rotation z-axis (4 bytes)
                    byte[] tmpArray = tmp.ToArray();
                    IDs.Add(tmpArray);
                    stream.Append(tmpArray);
                }
                if (firstType)
                {
                    stream.Append(BitConverter.GetBytes(random.Next(10, 10000))); //Maximum Motor Force (4 bytes)
                    stream.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Lower Angle (!= Upper Angle) (4bytes)
                    stream.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Upper Angle (4bytes)
                }
                else
                {
                    stream.Append(BitConverter.GetBytes(0)); //Maximum Motor Force (4 bytes)
                    stream.Append(BitConverter.GetBytes(0.0f)); // Lower Angle (!= Upper Angle) (4bytes)
                    stream.Append(BitConverter.GetBytes(0.0f)); // Upper Angle (4bytes)
                }

                if (chosenID2 <= IDs.Count)
                {
                    stream.Append(IDs.ElementAt(chosenID2 - 1));
                }
                else
                {
                    System.IO.MemoryStream tmp = new System.IO.MemoryStream();
                    tmp.Append(BitConverter.GetBytes((float)(lastPosition[0] + 4 * random.NextDouble() - 1))); // Position x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(lastPosition[1] + 4 * random.NextDouble()))); // Position y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Position z-axis (4 bytes)

                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Rotation z-axis (4 bytes)
                    byte[] tmpArray = tmp.ToArray();
                    IDs.Add(tmpArray);
                    stream.Append(tmpArray);
                }

                if (secondType)
                {
                    stream.Append(BitConverter.GetBytes(2210));//random.Next(10, 10000))); //Maximum Motor Force (4 bytes)
                    float firstAngle = (float)(360 * random.NextDouble());
                    float secondAngle = (float)(360 * random.NextDouble());
                    if (firstAngle > secondAngle)
                    {
                        stream.Append(BitConverter.GetBytes(secondAngle)); // Lower Angle (4bytes)
                        stream.Append(BitConverter.GetBytes(firstAngle));  // Upper Angle (4bytes)
                    }
                    else
                    {
                        stream.Append(BitConverter.GetBytes(firstAngle));  // Lower Angle (4bytes)
                        stream.Append(BitConverter.GetBytes(secondAngle)); // Upper Angle (4bytes)
                    }

                }
                else
                {
                    stream.Append(BitConverter.GetBytes(0)); //Maximum Motor Force (4 bytes)
                    stream.Append(BitConverter.GetBytes(0.0f)); // Lower Angle (!= Upper Angle) (4bytes)
                    stream.Append(BitConverter.GetBytes(0.0f)); // Upper Angle (4bytes)
                }
            }
            return stream.ToArray();
        }

        /// <summary>
        /// Chromosome crossover function.
        /// </summary>
        /// <param name="O1">Organism 1</param>
        /// <param name="O2">Organism 2</param>
        /// <param name="r">Random generator reference</param>
        /// <returns>Pair of Organisms</returns>
        public Pair<Organism, Organism> Crossover(Organism O1, Organism O2, System.Random r)
        {
            int random = (O1.Chromosome.Length < O2.Chromosome.Length) ? r.Next(0, O1.Chromosome.Length / 82) * 82 : r.Next(0, O2.Chromosome.Length / 82) * 82;
            byte[] Chromosome1 = new byte[O2.Chromosome.Length];
            byte[] Chromosome2 = new byte[O1.Chromosome.Length];
            O1.Chromosome.Take(random).ToArray().CopyTo(Chromosome1, 0);
            O2.Chromosome.Skip(random).Take(Chromosome2.Length - random).ToArray().CopyTo(Chromosome1, random);
            O2.Chromosome.Take(random).ToArray().CopyTo(Chromosome2, 0);
            O1.Chromosome.Skip(random).Take(Chromosome1.Length - random).ToArray().CopyTo(Chromosome2, random);
            return new Pair<Organism, Organism>(new Organism(++lastID, Chromosome1, r), new Organism(++lastID, Chromosome2, r));
        }

        /// <summary>
        /// Mutating selected organism chormosome.
        /// </summary>
        /// <param name="o">Organism object</param>
        /// <param name="r">Random generator reference</param>
        /// <returns>Organism with mutated chromosome</returns>
        public Organism Mutation (Organism o, System.Random r)
        {
            var tmpChromosome = new BitArray(o.Chromosome);
            int mut = r.Next(0, tmpChromosome.Length);
            tmpChromosome[mut] = !tmpChromosome[mut];
            byte[] newChromosome = new byte[tmpChromosome.Length / 8];
            tmpChromosome.CopyTo(newChromosome, 0);
            return new Organism(o.ID, newChromosome, r);
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

        public void SaveOrganisms(int iteration)
        {
            List<Organism> organismList = new List<Organism>();
            foreach (Pair<Organism, double> o in Organisms)
            {
                organismList.Add(o.First);
            }
            string jsonString = JsonConvert.SerializeObject(organismList, Formatting.Indented);
            string path = @".\organisms";
            Directory.CreateDirectory(path);
            string filePath = path + @"\iteration_" + iteration.ToString() + ".txt";
            File.WriteAllText(filePath, jsonString);
        }

        public void LoadOrganisms(int iteration)
        {
            string path = @".\organisms\iteration_" + iteration.ToString() + ".txt";
            string jsonString = File.ReadAllText(path);
            Organisms.Clear();
            var jsonobject = (JArray)JsonConvert.DeserializeObject(jsonString);
            foreach (var o in jsonobject.Children())
            {
                int ID = (int)o["ID"];
                byte[] chromosome = (byte[])o["Chromosome"];
                var cp = o["CurrentPosition"];
                double[] currentPosition = new double[] { (double)cp[0], (double)cp[1], (double)cp[2] };
                var ma = o["MovementAbility"];
                double[] movementAbility = new double[] { (double)ma[0], (double)ma[1], (double)ma[2] };
                Organisms.Add(new Pair<Organism, double>(new Organism(ID, chromosome, currentPosition, movementAbility), double.PositiveInfinity));
            }
            foreach (Pair<Organism, double> o in Organisms)
            {
                Console.WriteLine("[" + o.First.ID + "]" + "Movement Ability: (" + o.First.MovementAbility[0].ToString("n3") + ", " + o.First.MovementAbility[1].ToString("n3") + ", " + o.First.MovementAbility[2].ToString("n3") + ")");
            }
        }

    }
}
