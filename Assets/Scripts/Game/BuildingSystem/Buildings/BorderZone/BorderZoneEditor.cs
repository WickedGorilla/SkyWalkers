#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(BorderZone))]
public class BorderZone2DEditor : Editor
{
    private BoxBoundsHandle _boxBoundsHandle;

    private BorderZone Target => (BorderZone)target;

    private void OnSceneGUI()
    {
        Draw();
    }

    private void Draw()
    {
        Transform transform = Target.transform;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        Matrix4x4 rotatedMatrix = Handles.matrix * Matrix4x4.TRS(position, rotation, Vector3.one);

        using (new Handles.DrawingScope(rotatedMatrix))
        {
            CopyColliderPropertiesToHandle();

            EditorGUI.BeginChangeCheck();
            _boxBoundsHandle.DrawHandle();

            if (!EditorGUI.EndChangeCheck())
                return;

            Undo.RecordObject(Target, $"Modify {ObjectNames.NicifyVariableName(target.GetType().Name)}");
            CopyHandlePropertiesToCollider();
        }
    }

    private void CopyColliderPropertiesToHandle()
    {
        _boxBoundsHandle.center = TransformColliderCenterToHandleSpace(Target.transform, Target.Center);
        _boxBoundsHandle.size = Vector2.Scale(Target.Size, Target.transform.lossyScale);
    }

    private void CopyHandlePropertiesToCollider()
    {
        Type type = typeof(BorderZone);
        FieldInfo centerField = type.GetField("_center", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo sizeField = type.GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);

        Vector2 center = TransformHandleCenterToColliderSpace(Target.transform, _boxBoundsHandle.center);
        centerField.SetValue(target, center);

        Vector2 size = Vector2.Scale(_boxBoundsHandle.size, InvertScaleVector(Target.transform.lossyScale));
        size = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        sizeField.SetValue(target, size);
    }

    private Vector2 TransformHandleCenterToColliderSpace(Transform transform, Vector2 handleCenter)
        => transform.localToWorldMatrix.inverse * (Handles.matrix * (Vector3)handleCenter);

    private Vector2 TransformColliderCenterToHandleSpace(Transform transform, Vector2 colliderCenter)
        => (Vector2)(Handles.inverseMatrix * (transform.localToWorldMatrix * (Vector3)colliderCenter));

    private Vector2 InvertScaleVector(Vector2 scaleVector)
    {
        for (int axis = 0; axis < 2; ++axis)
            scaleVector[axis] = scaleVector[axis] == 0f ? 0f : 1f / scaleVector[axis];

        return scaleVector;
    }

    private void OnEnable()
    {
        CreateBoxBoundsHandle();
    }

    private void CreateBoxBoundsHandle()
    {
        _boxBoundsHandle = new BoxBoundsHandle
        {
            wireframeColor = Color.red,
            handleColor = Color.green
        };
    }
}
#endif
