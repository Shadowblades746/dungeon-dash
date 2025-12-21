using _Scripts;
using UnityEditor; // use the unity editor library
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(AbstractDungeonGenerator), true)] // makes all child classes use the custom editor
    // create custom editor for the abstract dungeon generator class
    public class RandomDungeonGeneratorEditor : UnityEditor.Editor
    {
        private AbstractDungeonGenerator _generator; // access the abstract dungeon generator reference

        // method for the generator to run before the game starts
        private void Awake()
        {
            _generator = (AbstractDungeonGenerator)target; // reference to the generator
        } 

        // button to display class fields in the inspector
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // call base method to display all fields from class
            if(GUILayout.Button("Create Dungeon")) // allow to create custom button in the inspector
            {
                _generator.GenerateDungeon(); // if button is clicked then generate dungeon
            } 
        }
    }
}
