using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Manager))]
public class ManagerEditor : Editor {

    bool capableOfSave;

    private static bool expandNN = true;
    private static bool expandTraining = true;
    private static bool expandCar = true;
    private static bool expandCourse = true;

    public override void OnInspectorGUI()
    {
        
        
        Manager script = (Manager)target;


        expandNN = EditorGUILayout.Foldout(expandNN, "Neural Network Settings");
        if (expandNN)
        {
            EditorGUI.indentLevel++;

            script.mutationRate = EditorGUILayout.Slider("Mutation Rate: ", script.mutationRate, 0, 1);
            script.populationDirectParentPortion = EditorGUILayout.Slider("Parent Portion of Population: ", script.populationDirectParentPortion, 0, 1);

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("layers");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();


            GUILayout.BeginHorizontal();
            script.carSaveName = EditorGUILayout.TextField("NN Save Name: ", script.carSaveName);
            if (!capableOfSave)
                GUI.enabled = false;

            if (GUILayout.Button("Save Most Fit"))
            {
                NeuralNet.SaveNN(script.currentFittest.GetNet(), script.carSaveName + ".txt");
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            script.carLoadName = EditorGUILayout.TextField("NN Load Name: ", script.carLoadName);
            if (GUILayout.Button("Run From File") && Application.isPlaying)
            {
                NeuralNet nn = new NeuralNet(NeuralNet.LoadNN(script.carSaveName + ".txt"));
                script.RunSoloCar(nn);
                capableOfSave = false;
                Repaint();
            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }


        expandCar = EditorGUILayout.Foldout(expandCar, "Car Settings");
        if (expandCar)
        {
            EditorGUI.indentLevel++;

            script.maxSpeed = EditorGUILayout.FloatField("Car Speed: ", script.maxSpeed);
            script.rotationSpeed = EditorGUILayout.FloatField("Car Rotation Speed: ", script.rotationSpeed);
            script.drawSensors = EditorGUILayout.Toggle("Car Sensor Length: ", script.drawSensors);
            script.sensorLengths = EditorGUILayout.FloatField("Car Sensor Length: ", script.sensorLengths);
            script.hide = EditorGUILayout.Toggle("Hide Crashed Cars: ", script.hide);
            script.carPrefab = (GameObject)EditorGUILayout.ObjectField(script.carPrefab, typeof(GameObject), true);
            script.aliveMat = (Material)EditorGUILayout.ObjectField(script.aliveMat, typeof(Material), true);
            script.leadMat = (Material)EditorGUILayout.ObjectField(script.leadMat, typeof(Material), true);
            script.crashedMat = (Material)EditorGUILayout.ObjectField(script.crashedMat, typeof(Material), true);


            EditorGUI.indentLevel--;

        }


        expandCourse = EditorGUILayout.Foldout(expandCourse, "Course Settings");
        if (expandCourse)
        {
            EditorGUI.indentLevel++;

            script.obstacleMoveSpeed = EditorGUILayout.FloatField("Obstacle Speed: ", script.obstacleMoveSpeed);
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("movingObstacles");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();


            EditorGUI.indentLevel--;

        }


        expandTraining = EditorGUILayout.Foldout(expandTraining, "Training Settings");
        if (expandTraining)
        {
            EditorGUI.indentLevel++;

            script.timeScale = EditorGUILayout.FloatField("Time Scale: ", script.timeScale);
            script.populationSize = EditorGUILayout.IntField("Population: ", script.populationSize);
            script.fitnessUpdateFrequency = EditorGUILayout.FloatField("Fitness Update Frequency: ", script.fitnessUpdateFrequency);
            script.generationTime = EditorGUILayout.FloatField("Max Generation Time: ", script.generationTime);
            script.delayBetweenGenerations = EditorGUILayout.FloatField("Delay Between Generations: ", script.delayBetweenGenerations);


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Train New") && Application.isPlaying)
            {
                script.RunPopulation();
                capableOfSave = true;
                Repaint();
            }
            if (GUILayout.Button("Train from File") && Application.isPlaying)
            {
                script.RunPopulation(NeuralNet.LoadNN(script.carSaveName + ".txt"));
                capableOfSave = true;
                Repaint();
            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }





    }
    
}
