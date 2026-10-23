using UnityEditor;
using UnityEngine;

public class ObjectDuplicator : EditorWindow
{
    public int numDuplicates = 3;  // number of duplicates to create
    public float minDistance = 2f; // minimum distance between each duplicate
    public float maxDistance = 4f; // maximum distance between each duplicate


    [MenuItem("Window/Object Duplicator/Show")]
    public static void ShowWindow()
    {
        ObjectDuplicator window = (ObjectDuplicator)EditorWindow.GetWindow(typeof(ObjectDuplicator));
window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Duplicate"))
        {
            Duplicate();
        }

        GUILayout.Label("Number of duplicates:");
        numDuplicates = EditorGUILayout.IntField(numDuplicates);

        GUILayout.Label("Minimum distance:");
        minDistance = EditorGUILayout.FloatField(minDistance);

        GUILayout.Label("Maximum distance:");
        maxDistance = EditorGUILayout.FloatField(maxDistance);
		if (!float.TryParse(minDistance.ToString(), out float result) || !float.TryParse(maxDistance.ToString(), out result))
        {
            EditorGUILayout.HelpBox("Distance fields must contain only numbers!", MessageType.Error);
            return;
        }

        if (!int.TryParse(numDuplicates.ToString(), out int result2))
        {
            EditorGUILayout.HelpBox("Number of duplicates field must contain only numbers!", MessageType.Error);
            return;
        }
		if (minDistance > maxDistance)
        {
            EditorGUILayout.HelpBox("Minimum distance cannot be greater than maximum distance.", MessageType.Error);
        }
    }

    public void Duplicate()
{
    // get the selected game object
    GameObject selectedObject = Selection.activeGameObject;
    if (selectedObject == null)
    {
        Debug.LogWarning("No game object selected.");
        return;
    }
	if (minDistance > maxDistance)
    {
        float temp = minDistance;
        minDistance = maxDistance;
        maxDistance = temp;
		Debug.LogError("You are not suppose to do that. I edited the value to fix this and make sure when you input the value, max always higher than min");
		return;
    }
    // create duplicates
    for (int i = 0; i < numDuplicates; i++)
    {
        // instantiate a copy of the selected game object
        GameObject copy = Instantiate(selectedObject);

        // rename the copy
        copy.name = selectedObject.name + " " + (i + 1);

        // set the position of the copy randomly within the specified range
        float x = Random.Range(minDistance, maxDistance);
        float y = selectedObject.transform.position.y; // Keep the original y position
        float z = Random.Range(minDistance, maxDistance);
        Vector3 offset = new Vector3(x, y, z);
        copy.transform.position = selectedObject.transform.position + offset;

        // set the parent of the copy
        copy.transform.SetParent(selectedObject.transform.parent);

        // duplicate all the components on the selected game object
        foreach (Component component in selectedObject.GetComponents<Component>())
        {
            // Check if the component already exists on the copy
            if (!copy.GetComponent(component.GetType()))
            {
                Component copyComponent = copy.AddComponent(component.GetType());
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(copyComponent);
            }
        }

        // duplicate all the child objects of the selected game object
        DuplicateChildObjects(selectedObject.transform, copy.transform);
    }
}


    private void DuplicateChildObjects(Transform source, Transform dest)
{
    // create duplicates of all the child objects
    foreach (Transform child in source)
    {
        GameObject copy = Instantiate(child.gameObject);
        copy.transform.SetParent(dest);
        copy.transform.localPosition = child.localPosition;
        copy.transform.localRotation = child.localRotation;
        copy.transform.localScale = child.localScale;

        // duplicate only the components that don't already exist on the destination object
        foreach (Component component in child.GetComponents<Component>())
        {
            if (!copy.GetComponent(component.GetType()))
            {
                Component copyComponent = copy.AddComponent(component.GetType());
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentValues(copyComponent);
            }
        }

        // duplicate all the child objects of the child object
        DuplicateChildObjects(child, copy.transform);
    }
}
}