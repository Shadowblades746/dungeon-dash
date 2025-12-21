using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Data
{
    [CreateAssetMenu(fileName = "RandomWalkParameters_", menuName = "PCG/RandomWalkData")] // header to use in create menu of inspector
    // parent class of "scriptableObject" allows to create through create menu in the inspector 
    public class RandomWalkSo : ScriptableObject
    { 
        public bool randomStartPos = true; // sets the start randomly each iteration
        public int walkLen = 10, iterations = 10; // sets the iterations and walk length
    }
}
