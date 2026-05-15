#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class TileHorizontalAlignerWindow : EditorWindow
{
    [SerializeField] private Transform _root;
    [SerializeField] private string _prefix = "Tile_Horizontal_";
    [SerializeField] private int _anchorNumber = 1;
    [SerializeField] private float _spacingZ = -5.45f;

    private static readonly Regex NumberRegex = new Regex(@"(\d+)$");

    [MenuItem("Tools/Epic/Tile Horizontal Aligner")]
    private static void Open()
    {
        GetWindow<TileHorizontalAlignerWindow>("Tile Aligner");
    }

    private void OnGUI()
    {
        _root = (Transform)EditorGUILayout.ObjectField("Root", _root, typeof(Transform), true);
        _prefix = EditorGUILayout.TextField("Name Prefix", _prefix);
        _anchorNumber = EditorGUILayout.IntField("Anchor Number", _anchorNumber);
        _spacingZ = EditorGUILayout.FloatField("Spacing Z", _spacingZ);

        EditorGUILayout.Space();

        if (GUILayout.Button("Log Preview"))
        {
            LogPreview();
        }

        if (GUILayout.Button("Apply Align"))
        {
            ApplyAlign();
        }
    }

    private void LogPreview()
    {
        List<Transform> tiles = FindTiles();

        if (!TryFindAnchor(tiles, out Transform anchor, out int anchorNumber))
        {
            Debug.LogError($"Anchor not found. Anchor Number: {_anchorNumber}");
            return;
        }

        float anchorZ = anchor.localPosition.z;

        foreach (Transform tile in tiles)
        {
            int number = GetNumber(tile.name);
            float targetZ = anchorZ + (number - anchorNumber) * _spacingZ;

            Debug.Log($"{tile.name} / CurrentZ:{tile.localPosition.z:0.###} -> TargetZ:{targetZ:0.###}");
        }
    }

    private void ApplyAlign()
    {
        List<Transform> tiles = FindTiles();

        if (!TryFindAnchor(tiles, out Transform anchor, out int anchorNumber))
        {
            Debug.LogError($"Anchor not found. Anchor Number: {_anchorNumber}");
            return;
        }

        Undo.SetCurrentGroupName("Align Tile Horizontal");
        int undoGroup = Undo.GetCurrentGroup();

        float anchorZ = anchor.localPosition.z;

        foreach (Transform tile in tiles)
        {
            int number = GetNumber(tile.name);
            float targetZ = anchorZ + (number - anchorNumber) * _spacingZ;

            Undo.RecordObject(tile, "Align Tile Horizontal");

            Vector3 localPosition = tile.localPosition;
            localPosition.z = targetZ;
            tile.localPosition = localPosition;

            EditorUtility.SetDirty(tile);
        }

        Undo.CollapseUndoOperations(undoGroup);
        Debug.Log($"Aligned {tiles.Count} tiles. Anchor:{anchor.name}, SpacingZ:{_spacingZ}");
    }

    private List<Transform> FindTiles()
    {
        List<Transform> tiles = new List<Transform>();

        if (_root == null)
        {
            Debug.LogError("Root is null.");
            return tiles;
        }

        Transform[] children = _root.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (!child.name.StartsWith(_prefix))
            {
                continue;
            }

            if (GetNumber(child.name) < 0)
            {
                continue;
            }

            tiles.Add(child);
        }

        tiles.Sort((a, b) => GetNumber(a.name).CompareTo(GetNumber(b.name)));
        return tiles;
    }

    private bool TryFindAnchor(List<Transform> tiles, out Transform anchor, out int anchorNumber)
    {
        foreach (Transform tile in tiles)
        {
            int number = GetNumber(tile.name);

            if (number == _anchorNumber)
            {
                anchor = tile;
                anchorNumber = number;
                return true;
            }
        }

        anchor = null;
        anchorNumber = -1;
        return false;
    }

    private int GetNumber(string objectName)
    {
        Match match = NumberRegex.Match(objectName);

        if (!match.Success)
        {
            return -1;
        }

        return int.Parse(match.Groups[1].Value);
    }
}
#endif