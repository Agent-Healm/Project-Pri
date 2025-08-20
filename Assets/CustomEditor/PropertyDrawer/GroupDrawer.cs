using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Codice.CM.SEIDInfo;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GroupAttribute))]
public class GroupDrawer : PropertyDrawer
{
    // protected static bool isDrawn;
    protected static float globalHeight = 0f;
    protected static Split tree;
    protected static Dictionary<string, Rect> outRects = new();
    
    // public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    // {

    // }
    // public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    // {
    //     return 40f;
    // }

    protected void SetRect(Rect position, string groupName)
    {
        // position.height = 40;
        // Debug.Log($"My rect: {position}");
        // var prop = s_dict[groupName];
        // float fieldWidth = position.width / prop.Count;
        // for (int i = 0; i < prop.Count; i++)
        // {
        //     var rect = new Rect(position.x + i * fieldWidth,
        //                         position.y,
        //                         fieldWidth,
        //                         EditorGUIUtility.singleLineHeight);
        //     prop[i].rect = rect;
        //     EditorGUI.DrawRect(rect, new(0, 0.2f, 0.8f, 0.5f));
        // }
        Debug.Log($"Debug for tree: {tree.children.Count}");
        globalHeight = ComputeTotalHeight(tree);
        var rect = position;
        rect.height = globalHeight;
        GroupLayout.Compute(tree, rect, outRects);
    }

    protected void SetupTree(SerializedProperty property)
    {
        // Debug.Log("Invoked GetNumberOf FIeld");
        FieldInfo[] fieldInfos = property.serializedObject.targetObject.GetType()
        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        // Debug.Log($"Type : {property.serializedObject.targetObject.GetType()}");
        foreach (var fieldInfo in fieldInfos)
        {
            // Debug.Log($"Field : {fieldInfo}");
            var attrs = fieldInfo.GetCustomAttributes(false);
            foreach (var attr in attrs)
            {
                // Debug.Log($"This field has {attr.GetType()}");
                if (attr is HorizontalLayout2Attribute horizontalAttr)
                {
                    // Debug.Log("Field name : " + fieldInfo.Name);
                    string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
                    if (tree == null)
                    {
                        tree = GroupLayout.Horizontal(horizontalAttr.GroupName);
                    }

                    Debug.Log($"Added {path} into tree, group: {horizontalAttr.GroupName}");
                    tree.children.Add(new Leaf(path));
                    break;
                }
                else if (attr is VerticalLayout2Attribute vertiAttr)
                {

                    if (tree == null)
                    {
                        tree = GroupLayout.Vertical(vertiAttr.GroupName);
                    }

                    string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
                    var subGroup = vertiAttr.GroupName.Split("/");
                    // Debug.Log($"Split group: {subGroup[0]}, {subGroup[1]}");  
                    // Debug.Log($"Split group result: {subGroup.Length}");
                    Split currentSplit = tree;
                    // List<Node> childrenTemp;
                    // bool isFound = false;

                    if (subGroup[0].Equals(tree.groupName))
                    {
                        for (int i = 1; i < subGroup.Length; i++)
                        {
                            // Debug.Log($"{i}| Starting at split: {currentSplit.groupName}");
                            var subGroupName = subGroup[i];

                            var existing = currentSplit.children
                            .OfType<Split>()
                            .FirstOrDefault(split => split.groupName == subGroupName);

                            // Debug.Log($"{i}| split at {existing?.groupName}");

                            if (existing == null)
                            {
                                // Debug.Log($"{i}| {subGroupName} doesnt exist, creating");
                                existing = GroupLayout.Vertical(subGroupName);
                                currentSplit.children.Add(existing);
                            }
                            currentSplit = existing;


                        }
                        currentSplit.children.Add(new Leaf(path));
                    }
                    else
                    {
                        Debug.Log($"new vertical");

                        // currentSplit.children.Add(new Leaf(path));
                        Debug.Log($"Added {path} into vertical tree, group: {vertiAttr.GroupName}");
                        tree.children.Add(new Leaf(path));
                        break;
                    }

                }
            }
        }
    }


    public void DrawField2(SerializedProperty property, Node node)
    {
        if (node is Leaf leaf)
        {
            int colorIndex = UnityEngine.Random.Range(0, 41);
            var rect = outRects[leaf.id];
            // Debug.Log($"Rect height: {rect.height}");
            EditorGUI.DrawRect(rect, new(0, 0.025f * (colorIndex + 1), 0.025f * (colorIndex + 1), 1));
            EditorGUI.PropertyField(rect, property);
            return;
        }
        var split = node as Split;
        int numberOfChildren = split.children.Count;
        if (numberOfChildren == 0) return;

        for (int i = 0; i < numberOfChildren; i++)
        {
            DrawField2(property, split.children[i]);
        }
    }
    public void DrawField(SerializedProperty property, Node node, Rect rect, float spacing = 0f)
    {
        if (node is Leaf leaf)
        {
            // int colorIndex = UnityEngine.Random.Range(0, 41);
            // EditorGUI.DrawRect(rect, new(0, 0.025f * (colorIndex + 1), 0.025f * (colorIndex + 1), 1));
            // EditorGUI.PropertyField(rect, property);
            return;
        }

        var split = node as Split;
        int numberOfChildren = split.children.Count;
        if (numberOfChildren == 0) return;

        float[] w = split.weights != null && split.weights.Length == numberOfChildren
        ? split.weights
        : Enumerable.Repeat(1f, numberOfChildren).ToArray();

        float totalWeight = w.Sum();
        float totalSpacing = spacing * (numberOfChildren - 1);

        if (split.groupType == GroupType.H)
        {
            float usableWidth = rect.width - totalSpacing;
            float x = rect.x;
            for (int i = 0; i < numberOfChildren; i++)
            {
                float wi = usableWidth * (w[i] / totalWeight);
                var r = new Rect(x, rect.y, wi, rect.height);
                DrawField(property, split.children[i], r, spacing);
                x += wi + spacing;
            }
        }
        else
        {
            float usableHeight = rect.height - totalSpacing;
            float y = rect.y;
            for (int i = 0; i < numberOfChildren; i++)
            {
                float hi = usableHeight * (w[i] / totalWeight);
                var r = new Rect(rect.x, y, rect.width, hi);
                DrawField(property, split.children[i], r, spacing);
                y += hi + spacing;
            }
        }
    }

    private float ComputeTotalHeight(Node node)
    {
        if (node is Leaf leaf)
        {
            Debug.Log($"height for: {leaf.id}");
            return EditorGUIUtility.singleLineHeight;
        }
        var split = node as Split;
        int numberOfChildren = split.children.Count;
        if (numberOfChildren == 0) return 0;

        if (split.groupType == GroupType.H)
        {
            float maxHeight = 0f;
            for (int i = 0; i < numberOfChildren; i++)
            {
                float childHeight = ComputeTotalHeight(split.children[i]);
                maxHeight = Mathf.Max(maxHeight, childHeight);
            }
            return maxHeight;
        }
        else if (split.groupType == GroupType.V)
        {
            float sumHeight = 0f;
            for (int i = 0; i < numberOfChildren; i++)
            {
                sumHeight += ComputeTotalHeight(split.children[i]);
            }
            return sumHeight;
        }

        return 0f;
    }

}

public enum GroupType { H, V, }
public abstract class Node { }
public sealed class Leaf : Node
{
    public string id;

    public Leaf(string id)
    {
        this.id = id;
    }
}

public sealed class Split : Node
{
    public string groupName;
    public GroupType groupType;
    public List<Node> children;
    public float[] weights; // optional, same length as children (defaults to equal)
    public Split(GroupType groupType, IEnumerable<Node> children, float[] weights = null)
    {
        this.groupType = groupType;
        this.children = children.ToList();
        this.weights = weights;
    }
    public Split(string groupName, GroupType groupType)
    {
        this.groupName = groupName;
        this.groupType = groupType;
        children = new();
    }
}

public static class GroupLayout
{
    public static Leaf Field(string id) => new(id);
    public static Split Horizontal(string groupName, params Node[] children) => new(GroupType.H, children);
    public static Split Horizontal(string groupName) => new(groupName, GroupType.H);
    public static Split Vertical(string groupName, params Node[] children) => new(GroupType.V, children);
    public static Split Vertical(string groupName) => new(groupName, GroupType.V);

    public static void Compute(Node node, Rect rect, Dictionary<string, Rect> outRects, float spacing = 0f)
    {
        if (node is Leaf leaf)
        {
            outRects[leaf.id] = rect;
            return;
        }

        var s = node as Split;
        int n = s.children.Count;
        if (n == 0) return;

        float[] w = s.weights != null && s.weights.Length == n
        ? s.weights
        : Enumerable.Repeat(1f, n).ToArray();

        float totalW = w.Sum();
        float totalSpacing = spacing * (n * 1);

        if (s.groupType == GroupType.H)
        {
            float usableWidth = rect.width - totalSpacing;
            float x = rect.x;
            for (int i = 0; i < n; i++)
            {
                float wi = usableWidth * (w[i] / totalW);
                var r = new Rect(x, rect.y, wi, rect.height);
                Compute(s.children[i], r, outRects, spacing);
                x += wi + spacing;
            }
        }
        else
        {
            float usableHeight = rect.height - totalSpacing;
            float y = rect.y;
            for (int i = 0; i < n; i++)
            {
                float hi = usableHeight * (w[i] / totalW);
                var r = new Rect(rect.x, y, rect.width, hi);
                Compute(s.children[i], r, outRects, spacing);
                y += hi + spacing;
            }
        }
    }
}