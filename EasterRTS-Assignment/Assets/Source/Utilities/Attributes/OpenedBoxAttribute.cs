using System;

namespace EasterRts.Utilities.Attributes {

    [AttributeUsage(
        validOn: AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, 
        AllowMultiple = false, Inherited = true
    )]
    public class OpenedBoxAttribute : Attribute {}
}
