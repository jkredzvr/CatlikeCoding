using UnityEditor;
using UnityEngine;


//Lets unity know to use our class instead of 
//the default editor for Line components.
[CustomEditor(typeof(Line))]
public class LineInspector : Editor
{


    private void OnSceneGUI()
    {

        //target is a special variable set to object being drawn when OnSceneGUI is called
        Line line = target as Line;

        //Handles operates in the world space, while points are in the local space.
        //Must explicitly convert the points into world space, otherwise transform changes on the 
        //points (scale, move, rotate), wont affect the line

        //Initial Code - Handles takes the points vector and plots it directly to the WorldSpace.  Does not take into account the Line gameobject's position relative to 
        //World space. 
        //Handles.color = Color.white;
        //will plot directly to world space
        //Handles.DrawLine(line.p0, line.p1);


        //create a handle transform using the line's global transform position
        Transform handleTransform = line.transform;
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        ////Take the local positions of p0 and p1 relative to the line gameobject and transform it to global coordinates
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);


        Handles.color = Color.white;
        //plot the positions of the point in the global space
        Handles.DrawLine(p0, p1);

        //Updates the line points to be moved in the editor
        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            //Allows for any chnages to be undone
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }
        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {

            //changes can be undone
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }
}