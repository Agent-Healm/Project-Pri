using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Healm.Inspector
{
    [CustomPropertyDrawer(typeof(HorizontalLayoutAttribute))]
    public class HorizontalLayoutDrawer : GroupDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var attr = attribute as HorizontalLayoutAttribute;

            if (property.name != (s_tree.children[0] as Leaf).id)
            {
                return;
            }
            EditorGUIUtility.labelWidth = 50;
            SetRect(position, attr.GroupName);
            DrawField(property, s_tree);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight_Internal(property);
        }

        private void Test()
        {

            var root =
            GroupLayout.Horizontal(
                "ok",
                GroupLayout.Field("x1"),
                GroupLayout.Vertical(
                    "ok",
                    GroupLayout.Field("x2"),
                    GroupLayout.Horizontal(
                        "ok",
                        GroupLayout.Field("x3"),
                        GroupLayout.Vertical(
                            "ok",
                            GroupLayout.Field("x4"),
                            GroupLayout.Field("x5")
                        )
                    ),
                    GroupLayout.Field("x7")
                ),
                GroupLayout.Field("x6")
            );

            var rects = new Dictionary<string, Rect>();
            GroupLayout.Compute(root, new Rect(0, 0, 1000, 600), rects);

            foreach (var kv in rects)
                Debug.Log($"{kv.Key} -> {kv.Value}");
        }
    }
}