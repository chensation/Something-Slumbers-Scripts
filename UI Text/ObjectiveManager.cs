using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{

    public bool InitializeObjective = true;
    public InteractiveTextContainer ObjectiveContainer;

    public static int ProgressTracker = -1;
    public static int TOTAL_PROGRESS_COUNT = 0;
    public static List<string> Objectives = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        if (Objectives.Count == 0)
        {

            //read all objectives from txt file
             using (StreamReader objReader = new StreamReader(
                Path.Combine(Application.streamingAssetsPath, "objectives.txt")))
             {
                while (!objReader.EndOfStream)
                 {
                     var currLine = objReader.ReadLine();
                     var values = currLine.Split('|');
                     Objectives.Add(values[1]);
                    TOTAL_PROGRESS_COUNT++;
                 }
             }

        }

        if (InitializeObjective)
        {
            if(ProgressTracker == -1)
                IncrementProgress();

        }
        var objectiveTextBox = GameObject.FindGameObjectWithTag("ObjectiveText").GetComponent<TextBox>();
        objectiveTextBox.updateText(Objectives[ProgressTracker]);
    }

    public void IncrementProgress()
    {
        if(ProgressTracker < TOTAL_PROGRESS_COUNT-1)
        {
            ProgressTracker++;
            ObjectiveContainer.ChangeText(Objectives[ProgressTracker]);

        }
    }

    public void UpdateProgress(int index)
    {

        ProgressTracker = index >= Objectives.Count ? Objectives.Count - 1 : index;
        ObjectiveContainer.ChangeText(Objectives[ProgressTracker]);
    }
    public IEnumerator WaitThenIncrementProgress(float second)
    {
        yield return new WaitForSeconds(second);
        IncrementProgress();

    }

    public IEnumerator WaitThenUpdateProgress(float second, int index)
    {
        yield return new WaitForSeconds(second);
        UpdateProgress(index);

    }
}
