using System;

namespace EasterRts.Utilities.Attributes {

    // Remove it and replace usages with OpenedBoxAttribute.
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class OpenedBoxDrawerAttribute : Attribute {}
}
