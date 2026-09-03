using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DuplicatePositionFinder : EditorWindow
{
    private string nameFilter = "asteroid_mod";
    private float tolerance = 0.01f;
    private List<List<Transform>> duplicateGroups = new();
    private List<Transform> lastMatches = new();
    private Vector2 scroll;

    [MenuItem("Tools/重複座標チェッカー")]
    private static void Open()
    {
        GetWindow<DuplicatePositionFinder>("重複座標チェッカー");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("名前フィルタ(部分一致・大文字小文字無視)");
        nameFilter = EditorGUILayout.TextField(nameFilter);

        EditorGUILayout.LabelField("座標の許容誤差");
        tolerance = EditorGUILayout.FloatField(tolerance);

        if (GUILayout.Button("シーン内を検索"))
        {
            Search();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"該当オブジェクト数: {lastMatches.Count}");
        using (new EditorGUI.DisabledScope(lastMatches.Count == 0))
        {
            if (GUILayout.Button("該当オブジェクトにRigidbodyを追加(重力OFF)"))
            {
                AddRigidbodies();
            }
            if (GUILayout.Button("該当オブジェクトのRigidbodyを削除"))
            {
                RemoveRigidbodies();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"重複候補グループ数: {duplicateGroups.Count}");

        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (var group in duplicateGroups)
        {
            EditorGUILayout.LabelField($"--- 座標 {FormatPos(group[0].position)} 付近 ({group.Count}個) ---", EditorStyles.boldLabel);
            foreach (var t in group)
            {
                if (t == null) continue;
                if (GUILayout.Button(GetHierarchyPath(t), EditorStyles.label))
                {
                    Selection.activeGameObject = t.gameObject;
                    EditorGUIUtility.PingObject(t.gameObject);
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void Search()
    {
        duplicateGroups.Clear();

        var matches = new List<Transform>();
        Scene scene = EditorSceneManager.GetActiveScene();
        foreach (var root in scene.GetRootGameObjects())
        {
            CollectMatches(root.transform, matches);
        }
        lastMatches = matches;

        var remaining = new List<Transform>(matches);
        while (remaining.Count > 0)
        {
            var current = remaining[0];
            remaining.RemoveAt(0);

            var group = new List<Transform> { current };
            for (int i = remaining.Count - 1; i >= 0; i--)
            {
                if (Vector3.Distance(current.position, remaining[i].position) <= tolerance)
                {
                    group.Add(remaining[i]);
                    remaining.RemoveAt(i);
                }
            }

            if (group.Count > 1)
            {
                duplicateGroups.Add(group);
            }
        }

        Debug.Log($"[重複座標チェッカー] 名前フィルタ='{nameFilter}' 該当オブジェクト数={matches.Count} 重複グループ数={duplicateGroups.Count}");
    }

    private void AddRigidbodies()
    {
        int added = 0;
        int alreadyHad = 0;

        foreach (var t in lastMatches)
        {
            if (t == null) continue;

            var rb = t.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = Undo.AddComponent<Rigidbody>(t.gameObject);
                added++;
            }
            else
            {
                Undo.RecordObject(rb, "Disable Rigidbody Gravity");
                alreadyHad++;
            }

            rb.useGravity = false;
            EditorUtility.SetDirty(t.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"[重複座標チェッカー] Rigidbody追加={added}件 既存につき重力OFFのみ変更={alreadyHad}件");
    }

    private void RemoveRigidbodies()
    {
        int removed = 0;

        foreach (var t in lastMatches)
        {
            if (t == null) continue;

            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Undo.DestroyObjectImmediate(rb);
                removed++;
                EditorUtility.SetDirty(t.gameObject);
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log($"[重複座標チェッカー] Rigidbody削除={removed}件");
    }

    private void CollectMatches(Transform t, List<Transform> matches)
    {
        if (t.name.ToLower().Contains(nameFilter.ToLower()))
        {
            matches.Add(t);
        }

        for (int i = 0; i < t.childCount; i++)
        {
            CollectMatches(t.GetChild(i), matches);
        }
    }

    private static string GetHierarchyPath(Transform t)
    {
        var sb = new StringBuilder(t.name);
        var parent = t.parent;
        while (parent != null)
        {
            sb.Insert(0, parent.name + "/");
            parent = parent.parent;
        }
        return sb.ToString();
    }

    private static string FormatPos(Vector3 v) => $"({v.x:F3}, {v.y:F3}, {v.z:F3})";
}
