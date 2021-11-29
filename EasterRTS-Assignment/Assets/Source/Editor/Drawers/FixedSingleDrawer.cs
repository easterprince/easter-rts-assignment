using EasterRts.Utilities.FixedFloats;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace EasterRts.Editor.Drawers {

    [CustomPropertyDrawer(typeof(FixedSingle))]
    public class FixedSingleDrawer : OdinValueDrawer<FixedSingle> {

        protected override void DrawPropertyLayout(GUIContent label) {

            FixedSingle targetValue = FixedSingle.FromRawValue(ValueEntry.SmartValue.RawValue);
            var stringValue = targetValue.ToString();

            var rectangle = EditorGUILayout.GetControlRect();
            stringValue = EditorGUI.TextField(rectangle, label, stringValue, EditorStyles.numberField);

            if (ValueEntry.IsEditable) {
                FixedSingle.TryParse(stringValue, out targetValue);
                ValueEntry.SmartValue = FixedSingle.FromRawValue(targetValue.RawValue);
            }
        }
    }
}

