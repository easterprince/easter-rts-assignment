using EasterRts.Utilities.Attributes;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;

namespace EasterRts.Editor.Processors {

    public class CustomClassAttributesProcessor : OdinAttributeProcessor {

        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes) {
            if (property.GetAttribute<OpenedBoxDrawerAttribute>() != null) {
                attributes.Add(new OpenedBoxAttribute());
            }
        }
    }
}
