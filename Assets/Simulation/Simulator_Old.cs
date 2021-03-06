﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Simulation
{
    class Simulator_Old
    {
        private int numberOfOrganisms, numberOfIterations, iterationLength, numberOfMutations, minComplexity, maxComplexity, lastID = 0, minMotorForce, maxMotorForce, parentProbabilityScale, fitnesDeterminationScale, minDistance, randScale;
        float oldNodeChoiceThresh;
        FromChromosome fromchromosome;
        System.Random r;
        List<Pair<Organism,double>> Organisms = new List<Pair<Organism,double>>();
        //NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Simulator_Old(int noo, int noi, int il, int nom, int minc, int maxc, int minmf, int maxmf, int fds, int pps, float olcf, int md, int rs)
        {
            numberOfOrganisms = noo;
            numberOfIterations = noi;
            iterationLength = il;
            numberOfMutations = nom;//(int)0.02 * numberOfOrganisms * Organisms[0].First.Chromosome.Length;
            minComplexity = minc;
            maxComplexity = maxc;
            minMotorForce = minmf;
            maxMotorForce = maxmf;
            parentProbabilityScale = pps;
            fitnesDeterminationScale = fds;
            oldNodeChoiceThresh = olcf;
            minDistance = md;
            randScale = rs;
            r = new System.Random();
            for (int i=0; i < numberOfOrganisms; i++)
            {
                byte[] chromosome = GenerateChromosome(minComplexity,maxComplexity,minMotorForce,maxMotorForce,oldNodeChoiceThresh, minDistance, randScale, r);
                Organisms.Add(new Pair<Organism, double> (new Organism(++lastID, chromosome , r), 0));
            }
        }

        public void RunIteration()
        {

            // Fitness Determination - calculating score basing on difference between target and current position of organism after one iteration
            //logger.Info("STAGE 1 - Fitness Determination");
            //Debug.Log("STAGE 1 - Fitness Determination");
            for (var i = 0; i < Organisms.Count; i++)
            {
                //Organisms[i].Second = Math.Pow(Organisms[i].First.Move(iterationLength),fitnesDeterminationScale);//Math.Pow(1 /Organisms[i].First.Move(iterationLength),5); //Count the fitness score
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
                p.Second = NthRoot(p.Second, parentProbabilityScale);
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
                    Offspring.Add(new Pair<Organism, double>(crossedPair.First, 0));
                    Offspring.Add(new Pair<Organism, double>(crossedPair.Second, 0));
                    Crossed.Add(new Pair<int, int>(tmp1.ID, tmp2.ID));
                    crossingsLeft--;
                }
            }

            // Offspring Mutation (Mutations are rare)
            //logger.Info("STAGE 4 - OffSpring Mutation");
            //Debug.Log("STAGE 4 - OffSpring Mutation");
            int counter = numberOfMutations+1;
            while(--counter > 0)
            {
                int rnd = r.Next(0, Offspring.Count);
                Organism o = Offspring[rnd].First;
                Debug.Log(BitConverter.ToString(o.Chromosome));
                //logger.Info("Mutation of organism " + o.ID + " performed!");
                Offspring.RemoveAt(rnd);                
                Offspring.Add(new Pair<Organism, double>(Mutation(o, r), 0));
                Organism o1 = Offspring.FirstOrDefault(i => (i.First.ID == o.ID)).First;
                Debug.Log(BitConverter.ToString(o1.Chromosome));
            }

            Organisms.Clear();
            Organisms.AddRange(Offspring);

        }

        public static byte[] GenerateChromosome(int minComplexity, int maxComplexity, int minMotorForce, int maxMotorForce, float oldNodeChoiceThresh, int minDistance, int randScale, System.Random random)
        {
            List<byte[]> IDs = new List<byte[]>();
            List<Pair<int, int>> pairs = new List<Pair<int, int>>();
            bool inc = false;
            int lastID = 1;
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            int randomNumber = random.Next(minComplexity, maxComplexity);
            float[] lastPosition = new float[3] { 0.0f, 0.0f, 0.0f };
            float[] lastCreatedPosition = new float[3] { 0.0f, 0.0f, 0.0f };
            for (int i = 0; i < randomNumber; i++)
            {
                // HEADER (10 bytes)
                // random IDs
                //double id1choice, id2choice;
                double choice;
                int chosenID1 = 0, chosenID2 = 0;
                bool firstType, secondType;
                //do
                //{
                //    id1choice = random.NextDouble();
                //    id2choice = random.NextDouble();
                //    if (id1choice > oldNodeChoiceThresh && IDs.Count > 0) // 0 to 1 typycally 0.85 because it has to be rare to choose both form old nodes
                //    {
                //        chosenID1 = (int)Math.Floor(random.NextDouble() * IDs.Count) + 1;
                //    }
                //    else
                //    {
                //        chosenID1 = lastID;
                //        inc = true;
                //    }
                //    if (id2choice > oldNodeChoiceThresh && IDs.Count > 0) // 0 to 1 typycally 0.85 because it has to be rare to choose both form old nodes
                //    {
                //        chosenID2 = (int)Math.Floor(random.NextDouble() * IDs.Count) + 1;
                //    }
                //    else
                //    {
                //        if (inc)
                //        {
                //            lastID++;
                //            inc = false;
                //        }
                //        chosenID2 = lastID;
                //    }
                //} while ((chosenID1 == chosenID2) || pairs.Any(p => p.First == chosenID1 && p.Second == chosenID2 || p.First == chosenID2 && p.Second == chosenID1));
                if (i == 0)
                {
                    chosenID1 = 1;
                    chosenID2 = 2;
                }
                else
                {
                    do
                    {
                        choice = random.NextDouble();
                        chosenID1 = (int)Math.Floor(random.NextDouble() * IDs.Count) + 1;
                        if (choice > 0.70)
                        {
                            chosenID2 = (int)Math.Floor(random.NextDouble() * IDs.Count) + 1;
                        }
                        else
                        {
                            chosenID2 = ++lastID;
                        }
                    }
                    while (chosenID1 == chosenID2 || pairs.Any(p => p.First == chosenID1 && p.Second == chosenID2 || p.First == chosenID2 && p.Second == chosenID1));
                }
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
                    byte[] joint1 = IDs.ElementAt(chosenID1 - 1);
                    stream.Append(joint1);
                    lastPosition = new float[] { System.BitConverter.ToSingle(joint1, 0), System.BitConverter.ToSingle(joint1, 4), System.BitConverter.ToSingle(joint1, 8) };
                }
                else
                {
                    System.IO.MemoryStream tmp = new System.IO.MemoryStream();
                    float x = (float)(lastPosition[0] + randScale * random.NextDouble() + minDistance); // typycally random scale 4, minimum distance 1
                    tmp.Append(BitConverter.GetBytes(x)); // Position x-axis (4 bytes)
                    float y = (float)(lastPosition[1] + randScale * random.NextDouble() + minDistance);
                    tmp.Append(BitConverter.GetBytes(y)); // Position y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Position z-axis (4 bytes)
                    lastPosition = new float[] { x, y, 0.0f };
                    lastCreatedPosition = lastPosition;
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Rotation z-axis (4 bytes)
                    byte[] tmpArray = tmp.ToArray();
                    IDs.Add(tmpArray);
                    stream.Append(tmpArray);
                }
                if (firstType)
                {
                    stream.Append(BitConverter.GetBytes(random.Next(minMotorForce, maxMotorForce))); //Maximum Motor Force (4 bytes)
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
                    byte[] joint2 = IDs.ElementAt(chosenID2 - 1);
                    stream.Append(joint2);
                    //lastPosition = new float[] { System.BitConverter.ToSingle(joint2, 0), System.BitConverter.ToSingle(joint2, 4), System.BitConverter.ToSingle(joint2, 8) };
                }
                else
                {
                    System.IO.MemoryStream tmp = new System.IO.MemoryStream();
                    //tmp.Append(BitConverter.GetBytes((float)(lastPosition[0] + 4 * random.NextDouble() - 1))); // Position x-axis (4 bytes)
                    //tmp.Append(BitConverter.GetBytes((float)(lastPosition[1] + 4 * random.NextDouble()))); // Position y-axis (4 bytes)
                    //tmp.Append(BitConverter.GetBytes(0.0f)); // Position z-axis (4 bytes)
                    float x = (float)(lastPosition[0] + randScale * random.NextDouble() + minDistance);
                    tmp.Append(BitConverter.GetBytes(x)); // Position x-axis (4 bytes)
                    float y = (float)(lastPosition[1] + randScale * random.NextDouble() + minDistance);
                    tmp.Append(BitConverter.GetBytes(y)); // Position y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Position z-axis (4 bytes)
                    lastPosition = new float[] { x, y, 0.0f };

                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation x-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes(0.0f)); // Rotation y-axis (4 bytes)
                    tmp.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Rotation z-axis (4 bytes)
                    byte[] tmpArray = tmp.ToArray();
                    IDs.Add(tmpArray);
                    stream.Append(tmpArray);
                }

                if (secondType)
                {
                    stream.Append(BitConverter.GetBytes(random.Next(minMotorForce, maxMotorForce))); //Maximum Motor Force (4 bytes)
                    stream.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Lower Angle (!= Upper Angle) (4bytes)
                    stream.Append(BitConverter.GetBytes((float)(360 * random.NextDouble()))); // Upper Angle (4bytes)
                    //float firstAngle = (float)(360 * random.NextDouble());
                    //float secondAngle = (float)(360 * random.NextDouble());
                    //if (firstAngle > secondAngle)
                    //{
                    //    stream.Append(BitConverter.GetBytes(secondAngle)); // Lower Angle (4bytes)
                    //    stream.Append(BitConverter.GetBytes(firstAngle));  // Upper Angle (4bytes)
                    //}
                    //else
                    //{
                    //    stream.Append(BitConverter.GetBytes(firstAngle));  // Lower Angle (4bytes)
                    //    stream.Append(BitConverter.GetBytes(secondAngle)); // Upper Angle (4bytes)
                    //}

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
                Vector3 currentPosition = new Vector3((float)cp[0], (float)cp[1], (float)cp[2]);
                Organisms.Add(new Pair<Organism, double>(new Organism(ID, chromosome, currentPosition), 0));
            }
            //foreach (Pair<Organism, double> o in Organisms)
            //{
            //    Console.WriteLine("[" + o.First.ID + "]" + "Movement Ability: (" + o.First.MovementAbility[0].ToString("n3") + ", " + o.First.MovementAbility[1].ToString("n3") + ", " + o.First.MovementAbility[2].ToString("n3") + ")");
            //}
        }

    }
}
