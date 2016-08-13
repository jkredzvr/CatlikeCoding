using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    //# steps between points for showing velocity
    private const int stepsPerCurve = 10;
    private const float directionScale = 0.5f;
    private const float handleSize = .04f;
    private const float pickSize = .06f;

    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private int selectedIndex = -1;

    //Handles whats seen in the inspector
    public override void OnInspectorGUI() {
        //keep drawing the default inspector
        //DrawDefaultInspector();
        spline = target as BezierSpline;
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            spline.Loop = loop;
        }
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount)
        {
            DrawSelectedPointInspector();
        }
        if (GUILayout.Button("Add Curve")) {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    private void DrawSelectedPointInspector() {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)
            EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }

    //Handles whats seen in the scene
    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        //Initialize the points
        Vector3 p0 = ShowPoint(0);

        //Loop through bezier splines
        for (int i = 1; i < spline.ControlPointCount; i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);

            //Draw lines between points
            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            //reset end point of spline to be new start of next spline
            p0 = p3;
        }
        ShowDirections();     
        //Old method of drawing tangent lines between two points
        //Handles.color = Color.white;
        //Vector3 lineStart = curve.GetPoint(0f);
        //Handles.color = Color.green;
        //Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0F));
        //for (int i = 1; i <= lineSteps; i++) {
        //    //Draw # of lineSteps tangent lines between two points
        //    //lineend is the interpolated point between p0 and p1 in lineSteps
        //    Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
        //    Handles.color = Color.white;
        //    Handles.DrawLine(lineStart, lineEnd);
        //    Handles.color = Color.green;
        //    Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection(i / (float)lineSteps));
        //    lineStart = lineEnd;
        //}
    }

   

    //Crowded with all those transform handles. We could only show a handle for the active point. Then then other points can suffice with dots.
    private Vector3 ShowPoint(int index) {
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        
        //This fixes the size of the handles so it can be scaled with zooming in and out.
        float size = HandleUtility.GetHandleSize(point);

        if (index == 0) {
            size *= 2f;
        }

        //handle becomes white dot
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        //if the current point is the selected handle
        if (Handles.Button(point, handleRotation, size*handleSize, size*pickSize, Handles.DotCap))
        {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index,handleTransform.InverseTransformPoint(point));
            }
        }
        
        return point;
    }

    //Old Show point that made all handles into lines
    //private Vector3 ShowPoint(int index)
    //{
    //    Vector3 point = handleTransform.TransformPoint(spline.points[index]);
    //    EditorGUI.BeginChangeCheck();
    //    point = Handles.DoPositionHandle(point, handleRotation);
    //    if (EditorGUI.EndChangeCheck())
    //    {
    //        Undo.RecordObject(spline, "Move Point");
    //        EditorUtility.SetDirty(spline);
    //        spline.points[index] = handleTransform.InverseTransformPoint(point);
    //    }
    //    return point;
    //}

    private void ShowDirections() {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve*spline.CurveCount;
        for (int i = 1; i <= steps; i++) {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    
}