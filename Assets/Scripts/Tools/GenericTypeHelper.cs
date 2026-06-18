using System;

namespace Tools
{
    public static class GenericTypeHelper
    {
        public static bool TryExtractGenericArgumentsFromBase(Type derivedType, Type openGenericBase, out Type[] genericTypes)
        {
            genericTypes = null;

            while (derivedType != null && derivedType != typeof(object))
            {
                if (derivedType.IsGenericType && derivedType.GetGenericTypeDefinition() == openGenericBase)
                {
                    genericTypes = derivedType.GetGenericArguments();
                    return true;
                }

                derivedType = derivedType.BaseType;
            }

            return false;
        }
    }
}

