using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace EvolutionEngine
{
    class ObjectSerializer
    {
        private static SaveData data;

        public void SaveModel(string fileName, Organism organism)
        {
            data = new SaveData(fileName);
            //data.tra
        }

        public void LoadModel(GameObject obj, string fileName)
        {
            data = SaveData.Load(Application.streamingAssetsPath + "\\" + fileName + ".uml");
            
        }

        public static void SaveOrganism(GameObject obj)
        {
            UnityEngine.Object prefab = PrefabUtility.CreatePrefab("Assets/Resources/" + obj.name + ".prefab", obj);
        }


    }
}
