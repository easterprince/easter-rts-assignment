using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace EasterRts.Editor.Drawers {

    public class OpenedBoxDrawer : OdinAttributeDrawer<OpenedBoxAttribute> {

        protected override void DrawPropertyLayout(GUIContent label) {
            SirenixEditorGUI.BeginBox(label);
            CallNextDrawer(null);
            SirenixEditorGUI.EndBox();
        }
    }
}
