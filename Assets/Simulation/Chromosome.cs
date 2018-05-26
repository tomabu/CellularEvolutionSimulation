using System;
using System.Linq;

namespace Simulation
{
    class Chromosome
    {
        public byte[] fullCode { get; }
        public Feature[] features { get; }
        public Chromosome(byte[] chromosome)
        {
            fullCode = new byte[chromosome.Length];
            Array.Copy(chromosome, 0, fullCode, 0, chromosome.Length);
            int numberOfFeatures = chromosome.Length / 82;
            features = new Feature[numberOfFeatures];
            for (int i = 0; i < numberOfFeatures; i++)
            {
                features[i] = new Feature(chromosome.Skip(i * 82).Take(82).ToArray());
            }

        }
    }

    public struct Feature
    {
        public byte[] featureCode;
        public int firstID, secondID, firstMaxMotor, secondMaxMotor;
        public bool firstType, secondType;
        public float firstPosX, firstPosY, firstPosZ, firstRotX, firstRotY, firstRotZ, firstLowAng, firstUppAng, secondPosX, secondPosY, secondPosZ, secondRotX, secondRotY, secondRotZ, secondLowAng, secondUppAng;

        public Feature(byte[] feature)
        {
            featureCode = new byte[82];
            Array.Copy(feature, featureCode, 82);
            firstID = BitConverter.ToInt32(feature, 0);
            firstType = BitConverter.ToBoolean(feature, 4);
            secondID = BitConverter.ToInt32(feature, 5);
            secondType = BitConverter.ToBoolean(feature, 9);
            firstPosX = BitConverter.ToSingle(feature, 10);
            firstPosY = BitConverter.ToSingle(feature, 14);
            firstPosZ = BitConverter.ToSingle(feature, 18);
            firstRotX = BitConverter.ToSingle(feature, 22);
            firstRotY = BitConverter.ToSingle(feature, 26);
            firstRotZ = BitConverter.ToSingle(feature, 30);
            firstMaxMotor = BitConverter.ToInt32(feature, 34);
            firstLowAng = BitConverter.ToSingle(feature, 38);
            firstUppAng = BitConverter.ToSingle(feature, 42);
            secondPosX = BitConverter.ToSingle(feature, 46);
            secondPosY = BitConverter.ToSingle(feature, 50);
            secondPosZ = BitConverter.ToSingle(feature, 54);
            secondRotX = BitConverter.ToSingle(feature, 58);
            secondRotY = BitConverter.ToSingle(feature, 62);
            secondRotZ = BitConverter.ToSingle(feature, 66);
            secondMaxMotor = BitConverter.ToInt32(feature, 70);
            secondLowAng = BitConverter.ToSingle(feature, 74);
            secondUppAng = BitConverter.ToSingle(feature, 78);
        }
    }
}
