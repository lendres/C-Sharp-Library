public static object CreateGeneric(Type generic, Type innerType, params object[] args)
{
	System.Type specificType = generic.MakeGenericType(new System.Type[] {innerType});
	return Activator.CreateInstance(specificType, args);
}

// To create a genric List of string, use the following code:
CreateGeneric(typeof(List<>), typeof(string));