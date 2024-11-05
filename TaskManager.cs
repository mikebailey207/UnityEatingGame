using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    private static TaskManager Instance;
    public TextMeshProUGUI taskText;

    public void AddTask(string task)
    {
        taskText.text = task;
        StartCoroutine(RemoveTaskAfterDelay(2f)); // Remove task after 2 seconds
    }

    private IEnumerator RemoveTaskAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        taskText.text = ""; // Clear task
    }
}
