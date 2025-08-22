using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Healm.Inspector
{
    [CustomPropertyDrawer(typeof(GroupAttribute))]
    public class GroupDrawer : PropertyDrawer
    {
        protected static bool isDrawn;
        // protected static float s_globalHeight = 0f;
        // protected static float s_offsetHeight = - EditorGUIUtility.standardVerticalSpacing * 0.5f;
        // protected static Split s_tree;
        protected static List<Split> splits = new();
        protected static List<SplitInformation> splitHeight = new();
        protected static Dictionary<string, Rect> outRects = new();

        protected class SplitInformation
        {
            public string groupName;
            public float globalHeight = 0f;
            public float offsetHeight = -EditorGUIUtility.standardVerticalSpacing * 0.5f;

            public SplitInformation(string groupName)
            {
                this.groupName = groupName;
                // globalHeight = 0f;
                // offsetHeight = -EditorGUIUtility.standardVerticalSpacing * 0.5f;
            }
        }


        private string TreeDebug(Node node)
        {
            if (node is Leaf leaf)
            {
                // Debug.Log($"height for: {leaf.id}");
                return $" {leaf.id}, ";
            }
            var split = node as Split;
            int numberOfChildren = split.children.Count;
            if (numberOfChildren == 0) return "";

            if (split.groupType == GroupType.H)
            {
                string myString = $"H [{split.groupName}] ( ";
                for (int i = 0; i < numberOfChildren; i++)
                {
                    myString += TreeDebug(split.children[i]);
                }
                myString += " )";
                return myString;
            }
            else if (split.groupType == GroupType.V)
            {
                string myString = $"V [{split.groupName}] ( ";
                for (int i = 0; i < numberOfChildren; i++)
                {
                    myString += TreeDebug(split.children[i]);
                }
                myString += ") ";
                return myString;
            }

            return "";
        }

        // private void Test()
        // {

        //     var root =
        //     GroupLayout.Horizontal(
        //         "ok",
        //         GroupLayout.Field("x1"),
        //         GroupLayout.Vertical(
        //             "ok",
        //             GroupLayout.Field("x2"),
        //             GroupLayout.Horizontal(
        //                 "ok",
        //                 GroupLayout.Field("x3"),
        //                 GroupLayout.Vertical(
        //                     "ok",
        //                     GroupLayout.Field("x4"),
        //                     GroupLayout.Field("x5")
        //                 )
        //             ),
        //             GroupLayout.Field("x7")
        //         ),
        //         GroupLayout.Field("x6")
        //     );

        //     var rects = new Dictionary<string, Rect>();
        //     GroupLayout.Compute(root, new Rect(0, 0, 1000, 600), rects);

        //     foreach (var kv in rects)
        //         Debug.Log($"{kv.Key} -> {kv.Value}");
        // }

        // private void TreeTest()
        // {
        //     s_tree = 
        //     GroupLayout.Horizontal(
        //         "main",
        //         GroupLayout.Field("x1"),
        //         GroupLayout.Vertical(
        //             "inner",
        //             GroupLayout.Field("x2"),
        //             GroupLayout.Horizontal(
        //                 "inner2",
        //                 GroupLayout.Field("x3"),
        //                 GroupLayout.Vertical(
        //                     "inner3",
        //                     GroupLayout.Field("x4"),
        //                     GroupLayout.Field("x5")
        //                 )
        //             ),
        //             GroupLayout.Field("x7")
        //         ),
        //         GroupLayout.Field("x6")
        //     );
        // }

        protected void SetRect(Rect position, Node node)
        {
            // Debug.Log($"Debug for tree: {s_tree.children.Count}");
            var split = node as Split;
            var rect = position;
            // rect.height = s_globalHeight;
            rect.height = splitHeight.Find(info => info.groupName.Equals(split.groupName)).globalHeight;
            GroupLayout.Compute(node, rect, outRects, spacing: 0);
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
                    if (attr is HorizontalGroupAttribute horizontalAttr)
                    {
                        string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
                        var subGroup = horizontalAttr.GroupName.Split("/");

                        Split splitTemp = splits.Find(split => split.groupName == subGroup[0]);
                        // Debug.Log($"I found {splitTemp?.groupName ?? "nothing"}");

                        if (splitTemp == null)
                        {
                            splitTemp = GroupLayout.Horizontal(horizontalAttr.GroupName);
                            splits.Add(splitTemp);
                        }

                        // if (s_tree == null)
                        // {
                        //     s_tree = GroupLayout.Horizontal(horizontalAttr.GroupName);
                        // }

                        // Split currentSplit = s_tree;
                        Split currentSplit = splitTemp;

                        // if (subGroup[0].Equals(s_tree.groupName))
                        if (subGroup[0].Equals(splitTemp.groupName))
                        {
                            for (int i = 1; i < subGroup.Length; i++)
                            {
                                var subGroupName = subGroup[i];

                                var existing = currentSplit.children
                                .OfType<Split>()
                                .FirstOrDefault(split => split.groupName == subGroupName);
                                if (existing == null)
                                {
                                    existing = GroupLayout.Horizontal(subGroupName);
                                    currentSplit.children.Add(existing);
                                }
                                currentSplit = existing;
                            }
                            currentSplit.children.Add(new Leaf(path));
                        }
                        else
                        {
                            Debug.Log($"New horizontal");
                            currentSplit.children.Add(new Leaf(path));
                            Debug.Log($"Added {path} into hori tree, group: {horizontalAttr.GroupName}");
                            // s_tree.children.Add(new Leaf(path));
                            break;

                        }
                    }
                    else if (attr is VerticalGroupAttribute vertiAttr)
                    {
                        string path = property.propertyPath.Replace(property.name, fieldInfo.Name);
                        var subGroup = vertiAttr.GroupName.Split("/");

                        Split splitTemp = splits.Find(s => s.groupName == subGroup[0]);

                        if (splitTemp == null)
                        {
                            splitTemp = GroupLayout.Vertical(vertiAttr.GroupName);
                            splits.Add(splitTemp);
                        }
                        // if (s_tree == null)
                        // {
                        //     s_tree = GroupLayout.Vertical(vertiAttr.GroupName);
                        // }

                        // Split currentSplit = s_tree;
                        Split currentSplit = splitTemp;

                        // if (subGroup[0].Equals(s_tree.groupName))
                        if (subGroup[0].Equals(currentSplit.groupName))
                        {
                            for (int i = 1; i < subGroup.Length; i++)
                            {
                                var subGroupName = subGroup[i];

                                var existing = currentSplit.children
                                .OfType<Split>()
                                .FirstOrDefault(split => split.groupName == subGroupName);

                                if (existing == null)
                                {
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
                            currentSplit.children.Add(new Leaf(path));
                            Debug.Log($"Added {path} into vertical tree, group: {vertiAttr.GroupName}");
                            break;
                        }
                    }
                }
            }
        }

        protected void DrawField(SerializedProperty property, Node node)
        {
            if (node is Leaf leaf)
            {
                int colorIndex = Random.Range(0, 41);
                var rect = outRects[leaf.id];
                // Debug.Log($"Rect height: {rect.height}");
                // if (rect.height <= EditorGUIUtility.singleLineHeight)
                // {
                //     Debug.Log($"{leaf.id} rect is too short: {rect.height}");
                // }
                var seriProp = property.serializedObject.FindProperty(leaf.id);

                // EditorGUI.DrawRect(rect, new(0, 0.025f * (colorIndex + 1), 0.025f * (colorIndex + 1), 1));
                EditorGUI.PropertyField(rect, seriProp);
                return;
            }
            var split = node as Split;
            int numberOfChildren = split.children.Count;
            if (numberOfChildren == 0) return;

            for (int i = 0; i < numberOfChildren; i++)
            {
                DrawField(property, split.children[i]);
            }
        }

        protected Vector2 ComputeWeights(Node node)
        {
            if (node is Leaf leaf)
            {
                return Vector2.one;
            }
            var split = node as Split;
            int numberOfChildren = split.children.Count;
            if (numberOfChildren == 0) return Vector2.zero;
            if (split.groupType == GroupType.H)
            {
                Vector2 maxWeight = Vector2.zero;
                float[] weights = new float[numberOfChildren];

                for (int i = 0; i < numberOfChildren; i++)
                {
                    var currentWeight = ComputeWeights(split.children[i]);
                    maxWeight.x += currentWeight.x;
                    maxWeight.y = Mathf.Max(maxWeight.y, currentWeight.y);
                    weights[i] = currentWeight.x;
                }

                split.weights = weights;
                return maxWeight;
            }
            else if (split.groupType == GroupType.V)
            {
                Vector2 maxWeight = Vector2.zero;
                float[] weights = new float[numberOfChildren];

                for (int i = 0; i < numberOfChildren; i++)
                {
                    var currentWeight = ComputeWeights(split.children[i]);
                    maxWeight.x = Mathf.Max(maxWeight.x, currentWeight.x);
                    maxWeight.y += currentWeight.y;
                    weights[i] = currentWeight.y;
                }
                split.weights = weights;
                return maxWeight;
            }
            return Vector2.zero;
        }

        protected float ComputeTotalHeight(Node node)
        {
            if (node is Leaf leaf)
            {
                // s_offsetHeight += EditorGUIUtility.standardVerticalSpacing;
                // Debug.Log($"height for: {leaf.id}");
                return EditorGUIUtility.singleLineHeight;
            }
            var split = node as Split;
            int numberOfChildren = split.children.Count;
            if (numberOfChildren == 0) return 0;

            if (split.groupType == GroupType.H)
            {
                // float maxHeight = EditorGUIUtility.singleLineHeight;
                float maxHeight = 0f;
                for (int i = 0; i < numberOfChildren; i++)
                {
                    float childHeight = ComputeTotalHeight(split.children[i]);
                    maxHeight = Mathf.Max(maxHeight, childHeight);
                }
                // return maxHeight + EditorGUIUtility.standardVerticalSpacing;
                return maxHeight;
            }
            else if (split.groupType == GroupType.V)
            {
                float sumHeight = 0f;
                for (int i = 0; i < numberOfChildren; i++)
                {
                    sumHeight += ComputeTotalHeight(split.children[i]);
                }
                return sumHeight + EditorGUIUtility.standardVerticalSpacing * (numberOfChildren - 1);
            }

            return 0f;
        }

        protected float ComputeOffsetHeight(Node node)
        {
            if (node is Leaf leaf)
            {
                // s_offsetHeight +=
                // Debug.Log($"height for: {leaf.id}");
                return EditorGUIUtility.standardVerticalSpacing;
                // return EditorGUIUtility.singleLineHeight;
            }
            var split = node as Split;
            int numberOfChildren = split.children.Count;
            if (numberOfChildren == 0) return 0;

            float maxHeight = 0f;
            for (int i = 0; i < numberOfChildren; i++)
            {
                maxHeight += ComputeOffsetHeight(split.children[i]);
                // Debug.Log($"New height {maxHeight}"); 
            }
            // Debug.Log($"Return: {maxHeight}");
            return maxHeight;
            // if (split.groupType == GroupType.H)
            // {
            //     // float maxHeight = EditorGUIUtility.singleLineHeight;
            //     float maxHeight = 0f;
            //     for (int i = 0; i < numberOfChildren; i++)
            //     {
            //         float childHeight = ComputeTotalHeight(split.children[i]);
            //         maxHeight = Mathf.Max(maxHeight, childHeight);
            //     }
            //     // return maxHeight + EditorGUIUtility.standardVerticalSpacing;
            //     return maxHeight;
            // }
            // else if (split.groupType == GroupType.V)
            // {
            //     float sumHeight = 0f;
            //     for (int i = 0; i < numberOfChildren; i++)
            //     {
            //         sumHeight += ComputeTotalHeight(split.children[i]);
            //     }
            //     return sumHeight;
            // }

            // return 0f;
        }

        protected void OnGUI_Internal(Rect position, SerializedProperty property, GUIContent label)
        {
            var groupName = (attribute as GroupAttribute).GroupName;
            var rootGroupName = groupName.Split("/");
            Split currentSplit = splits.Find(s => s.groupName.Equals(rootGroupName[0]));

            // if (property.name != (s_tree.children[0] as Leaf).id)
            if (property.name != (currentSplit.children[0] as Leaf).id)
            {
                if (!isDrawn)
                {
                    var rect = outRects[property.name];
                    EditorGUI.PropertyField(rect, property, false);
                }
                return;
            }
            isDrawn = false;
            EditorGUIUtility.labelWidth = 50;

            SetRect(position, currentSplit);
            DrawField(property, currentSplit);
            // SetRect(position, s_tree);
            // DrawField(property, s_tree);
            isDrawn = true;
        }

        protected float GetPropertyHeight_Internal(SerializedProperty property)
        {
            // if (splits == null)
            // {
            //     SetupTree(property);
            // }

            var groupName = (attribute as GroupAttribute).GroupName;
            var rootGroup = groupName.Split("/");
            var currentSplit = splits.Find(split => split.groupName.Equals(rootGroup[0]));

            // if (s_tree == null) 
            // Debug.Log($"{rootGroup[0]} -> {currentSplit?.groupName ?? "none"}");
            if (currentSplit == null)
            {
                SetupTree(property);
                // TreeTest();
                // Debug.Log($"Tree debug: {TreeDebug(s_tree)}");
                // s_globalHeight = ComputeTotalHeight(s_tree);
                // ComputeWeights(s_tree);

                // --------------

                // split = splits.Find(split => split.groupName.Equals(rootGroup[0]));
                // Debug.Log($"Tree debug: {TreeDebug(split)}");

                // SplitInformation splitInfo = new(rootGroup[0])
                // {
                //     // s_globalHeight = ComputeTotalHeight(split);
                //     globalHeight = ComputeTotalHeight(split),
                //     // offsetHeight += ComputeOffsetHeight(split);
                // };
                // splitInfo.offsetHeight += ComputeOffsetHeight(split);
                // // Debug.Log($"offset set: {splitInfo.offsetHeight}");
                // // Debug.Log($"offset set: {ComputeOffsetHeight(split)}");
                // splitHeight.Add(splitInfo);

                // ComputeWeights(split);

                currentSplit = splits.Find(split => split.groupName.Equals(rootGroup[0]));

                foreach (var split in splits)
                {
                    // Debug.Log($"Tree debug: {TreeDebug(split)}");

                    SplitInformation splitInfo = new(split.groupName)
                    {
                        globalHeight = ComputeTotalHeight(split),
                    };
                    splitInfo.offsetHeight += ComputeOffsetHeight(split);
                    splitHeight.Add(splitInfo);

                    ComputeWeights(split);
                }
            }
            // Debug.Log($"tree: {s_tree?.children.Count}");
            // if (s_tree.children[0] is Leaf leaf)
            if (currentSplit.children[0] is Leaf leaf)
            {
                if (property.name != leaf.id)
                {
                    return 0f;
                }
            }
            try
            {
                var x = splitHeight.Find(info => info.groupName.Equals(rootGroup[0]));
                return x.globalHeight - x.offsetHeight;
            }
            catch (NullReferenceException)
            {
                Debug.Log($"Null Reference: {rootGroup[0]}");
                Debug.Log($"Current height list");
                foreach (var item in splitHeight)
                {
                    Debug.Log($"{item.groupName}: {item.globalHeight} - {item.offsetHeight}");
                }
            }
            return 40f;
            // Debug.Log($"OFfset height: {x.offsetHeight}");
            // return s_globalHeight - s_offsetHeight;
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
        #region ShortHand
        public static Leaf Field(string id) => new(id);
        public static Split Horizontal(string groupName, params Node[] children) => new(GroupType.H, children);
        public static Split Horizontal(string groupName) => new(groupName, GroupType.H);
        public static Split Vertical(string groupName, params Node[] children) => new(GroupType.V, children);
        public static Split Vertical(string groupName) => new(groupName, GroupType.V);
        #endregion

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
            float totalSpacing = spacing * (n - 1);

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
            else if (s.groupType == GroupType.V)
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

}