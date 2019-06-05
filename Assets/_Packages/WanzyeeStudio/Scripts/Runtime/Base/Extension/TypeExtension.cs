
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WanzyeeStudio.Extension{

	/// <summary>
	/// Extension methods for <c>System.Type</c>.
	/// </summary>
	public static class TypeExtension{
		
		#region Name

		/// <summary>
		/// The dictionary of built-in types with pretty name.
		/// </summary>
		private static Dictionary<Type, string> _builtinNames = new Dictionary<Type, string>(){
			{typeof(void), "void"},
			{typeof(bool), "bool"},
			{typeof(byte), "byte"},
			{typeof(char), "char"},
			{typeof(decimal), "decimal"},
			{typeof(double), "double"},
			{typeof(float), "float"},
			{typeof(int), "int"},
			{typeof(long), "long"},
			{typeof(object), "object"},
			{typeof(sbyte), "sbyte"},
			{typeof(short), "short"},
			{typeof(string), "string"},
			{typeof(uint), "uint"},
			{typeof(ulong), "ulong"},
			{typeof(ushort), "ushort"}
		};

		/// <summary>
		/// Get a pretty readable name of the type, even generic, optional to use the full name.
		/// </summary>
		/// 
		/// <remarks>
		/// This doesn't handle anonymous types.
		/// </remarks>
		/// 
		/// <returns>The pretty name.</returns>
		/// <param name="type">Type.</param>
		/// <param name="full">If set to <c>true</c> use the full name.</param>
		/*
		 * Reference page below, add array type name, used if a list nests with array.
		 * http://stackoverflow.com/q/6402864
		 * http://stackoverflow.com/q/1533115
		 */
		public static string GetPrettyName(this Type type, bool full = false){
			
			if(null == type) throw new ArgumentNullException("type");

			if(type.IsArray){
				var _type = type.GetElementType().GetPrettyName(full);
				return string.Format("{0}[{1}]", _type, new string(',', type.GetArrayRank() - 1));
			}

			if(!full){

				if(_builtinNames.ContainsKey(type)) return _builtinNames[type];
				if(!type.Name.Contains("`")) return type.Name;

				var _type = typeof(Nullable<>);
				var _nullable = (type != _type && type.IsGenericType && _type == type.GetGenericTypeDefinition());
				if(_nullable) return type.GetGenericArguments()[0].GetPrettyName() + "?";

			}

			var _generic = type.IsGenericType && !Regex.IsMatch(type.FullName, @"(\A|\.|\+)\W");
			return _generic ? GetPrettyNameInternalGeneric(type, full) : (full ? type.FullName : type.Name);

		}

		/// <summary>
		/// Get a pretty name of generic type, sub method for <c>GetPrettyName()</c>.
		/// Change to type name format and wrap argument types with angle brackets.
		/// </summary>
		/// <returns>The pretty name.</returns>
		/// <param name="type">Type.</param>
		/// <param name="full">If set to <c>true</c> full name.</param>
		private static string GetPrettyNameInternalGeneric(Type type, bool full){
			
			var _name = full ? type.FullName : type.Name;
			if(_name.Contains("[[")) _name = _name.Remove(_name.IndexOf("[["));

			var _arguments = type.GetGenericArguments();
			var _args = _arguments.Select((typ) => typ.IsGenericParameter ? "" : typ.GetPrettyName(full));
			var _skip = _args.Count();

			foreach(var _match in Regex.Matches(_name, @"`\d+").Cast<Match>().Reverse()){

				var _take = int.Parse(_match.Value.Substring(1));
				_skip -= _take;

				var _types = string.Join(", ", _args.Skip(_skip).Take(_take).ToArray());
				_name = _name.Remove(_match.Index, _match.Length).Insert(_match.Index, "<" + _types + ">");

			}

			return Regex.Replace(_name, @" (?=[,>])", "");

		}

		#endregion


		#region Member

		/// <summary>
		/// Get the named public or nonpublic nested type of the specified type.
		/// </summary>
		/// <returns>The nested type.</returns>
		/// <param name="type">Type.</param>
		/// <param name="name">Type name.</param>
		/// <param name="isStatic">If to get a static type.</param>
		public static Type GetNestedType(this Type type, string name, bool isStatic){
			
			if(null == type || string.IsNullOrEmpty(name)) return null;

			return type.GetNestedType(name, GetBinding(isStatic));

		}

		/// <summary>
		/// Get the named public or nonpublic <c>FieldInfo</c> of the specified type.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="type">Type.</param>
		/// <param name="name">Field name.</param>
		/// <param name="isStatic">If to get a static field.</param>
		/// <param name="fieldType">Field type.</param>
		public static FieldInfo GetField(this Type type, string name, bool isStatic, Type fieldType = null){

			if(null == type || string.IsNullOrEmpty(name)) return null;

			var _result = type.GetField(name, GetBinding(isStatic));

			return (null == _result || null == fieldType || _result.FieldType == fieldType) ? _result : null;

		}

		/// <summary>
		/// Get the named public or nonpublic <c>PropertyInfo</c> of the specified type.
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="type">Type.</param>
		/// <param name="name">Property name.</param>
		/// <param name="isStatic">If to get a static property.</param>
		/// <param name="propertyType">Property type.</param>
		/// <param name="indexTypes">Index types.</param>
		public static PropertyInfo GetProperty(
			this Type type,
			string name,
			bool isStatic,
			Type propertyType = null,
			params Type[] indexTypes
		){

			if(null == type || string.IsNullOrEmpty(name)) return null;

			return type.GetProperty(name, GetBinding(isStatic), null, propertyType, indexTypes ?? new Type[0], null);

		}

		/// <summary>
		/// Get the named public or nonpublic <c>MethodInfo</c> of the specified type.
		/// </summary>
		/// <returns>The method.</returns>
		/// <param name="type">Type.</param>
		/// <param name="name">Method name.</param>
		/// <param name="isStatic">If to get a static method.</param>
		/// <param name="returnType">Return type.</param>
		/// <param name="paramTypes">Parameter types.</param>
		public static MethodInfo GetMethod(
			this Type type,
			string name,
			bool isStatic,
			Type returnType = null,
			params Type[] paramTypes
		){

			if(null == type || string.IsNullOrEmpty(name)) return null;

			var _result = type.GetMethod(name, GetBinding(isStatic), null, paramTypes ?? new Type[0], null);

			return (null == _result || null == returnType || _result.ReturnType == returnType) ? _result : null;

		}

		/// <summary>
		/// Get the <c>BindingFlags</c> to get a public or nonpublic member.
		/// </summary>
		/// <returns>The binding flags.</returns>
		/// <param name="isStatic">If to get a static member.</param>
		private static BindingFlags GetBinding(bool isStatic){
			
			var _flag = isStatic ? BindingFlags.Static : BindingFlags.Instance;

			return _flag | BindingFlags.Public | BindingFlags.NonPublic;

		}

		#endregion


		#region Hierarchy
		
		/// <summary>
		/// Get the parent hierarchy array, sorted from self to root type.
		/// </summary>
		/// <returns>The parent hierarchy array.</returns>
		/// <param name="type">Type.</param>
		public static Type[] GetParents(this Type type){
			
			var _result = new List<Type>();

			for(var _type = type; null != _type; _type = _type.BaseType) _result.Add(_type);

			return _result.ToArray();
			
		}

		/// <summary>
		/// Get all child types, excluding self, optional to find deep or directly inheritance only.
		/// </summary>
		/// <returns>The child types.</returns>
		/// <param name="type">Type.</param>
		/// <param name="deep">If set to <c>true</c> deep.</param>
		public static Type[] GetChildren(this Type type, bool deep = false){

			var _all = AppDomain.CurrentDomain.GetAssemblies().SelectMany((dll) => dll.GetTypes());

			if(deep) return _all.Where((typ) => typ.IsSubclassOf(type)).ToArray();

			else return _all.Where((typ) => typ.BaseType == type).ToArray();

		}
		
		/// <summary>
		/// Return the element type of an array or list type, otherwise <c>null</c>.
		/// </summary>
		/// <returns>The element type.</returns>
		/// <param name="type">Type.</param>
		/*
		 * http://stackoverflow.com/q/906499
		 */
		public static Type GetItemType(this Type type){

			if(!typeof(IList).IsAssignableFrom(type)) return null;

			if(type.IsArray) return type.GetElementType();

			var _interfaces = type.GetInterfaces().Where((typ) => typ.IsGenericType);

			var _type = _interfaces.FirstOrDefault((typ) => typ.GetGenericTypeDefinition() == typeof(IEnumerable<>));

			return (null == _type) ? null : _type.GetGenericArguments()[0];

		}

		#endregion


		#region Instance

		/// <summary>
		/// Get the default value of the type, just like <c>default(T)</c>.
		/// </summary>
		/// <returns>The default value.</returns>
		/// <param name="type">Type.</param>
		public static object GetDefault(this Type type){

			if(null == type) throw new ArgumentNullException("type");

			if(!type.IsValueType || null != Nullable.GetUnderlyingType(type)) return null;

			else return Activator.CreateInstance(type);

		}

		/// <summary>
		/// Determine if able to create an instance of the type.
		/// </summary>
		/// 
		/// <remarks>
		/// Optional to throw an exception message or just return <c>false</c> if invalid.
		/// This only checks some basic conditions and might be not precise.
		/// </remarks>
		/// 
		/// <remarks>
		/// The current conditions below:
		/// 	1. Return <c>false</c> only if it's interface, abstract, generic definition, delegate.
		/// 	2. Recurse to check the element type of an array type.
		/// 	3. Recurse to check the generic arguments of a list or dictionary type.
		/// </remarks>
		/// 
		/// <returns><c>true</c>, if creatable, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="exception">Flag to throw an exception or return <c>false</c>.</param>
		/// 
		public static bool IsCreatable(this Type type, bool exception = false){
			
			var _error = GetCreatableError(type);

			if(null != _error && exception) throw new ArgumentException(_error, "type");
			if(null != _error) return false;
			
			if(type.IsArray) return type.GetElementType().IsCreatable(exception);
			if(!type.IsGenericType) return true;
			
			var _definition = type.GetGenericTypeDefinition();
			if(typeof(List<>) != _definition && typeof(Dictionary<,>) != _definition) return true;

			return type.GetGenericArguments().All((arg) => arg.IsCreatable(exception));

		}

		/// <summary>
		/// Get the error for <c>IsCreatable()</c>, <c>null</c> if passed.
		/// </summary>
		/// <returns>The error.</returns>
		/// <param name="type">Type.</param>
		private static string GetCreatableError(Type type){

			if(null == type) return "Argument cannot be null.";

			if(type.IsInterface) return "Can't create interface.";
			if(type.IsAbstract) return "Can't create abstract type.";

			if(type.IsGenericTypeDefinition) return "Can't create generic definition.";
			if(typeof(Delegate).IsAssignableFrom(type)) return "Can't create delegete.";

			return null;

		}

		#endregion

	}

}
