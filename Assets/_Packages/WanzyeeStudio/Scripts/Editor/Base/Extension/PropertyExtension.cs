
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Extension{
	
	/// <summary>
	/// Extension methods for <c>UnityEditor.SerializedProperty</c>.
	/// </summary>
	public static class PropertyExtension{

		#region Common

		/// <summary>
		/// Check if the property, serializedObject, and targetObject, all exist, otherwise throw an exception.
		/// </summary>
		/// <param name="property">Property.</param>
		public static void CheckValid(this SerializedProperty property){

			if(null == property) throw new ArgumentNullException("property");

			string _miss;
			var _object = property.serializedObject;

			if(null == _object) _miss = "property.serializedObject";
			else if(null == _object.targetObject) _miss = "property.serializedObject.targetObject";
			else return;

			var _format = "Missing '{0}' of SerializedProperty '{1}'.";
			throw new MissingReferenceException(string.Format(_format, _miss, property.propertyPath));

		}

		/// <summary>
		/// Get the parent <c>UnityEditor.SerializedProperty</c> contains this.
		/// </summary>
		/// 
		/// <remarks>
		/// Return <c>null</c> if this is a root within target object.
		/// </remarks>
		/// 
		/// <returns>The parent.</returns>
		/// <param name="property">Property.</param>
		/// 
		public static SerializedProperty GetParent(this SerializedProperty property){

			property.CheckValid();

			var _path = property.propertyPath;
			if(!_path.Contains(".")) return null;

			_path = _path.Remove(_path.LastIndexOf(_path.EndsWith("]") ? ".Array.data[" : "."));
			return property.serializedObject.FindProperty(_path);
			
		}

		/// <summary>
		/// Get the tooltip of the property, return <c>string.Empty</c> if none.
		/// </summary>
		/// 
		/// <remarks>
		/// The built-in <c>SerializedProperty.tooltip</c> never works with unknown reason.
		/// This uses <c>EditorGUI.BeginProperty()</c> to fetch the tooltip, must be called in <c>OnGUI()</c>.
		/// </remarks>
		/// 
		/// <returns>The tooltip.</returns>
		/// <param name="property">Property.</param>
		/// 
		public static string GetTooltip(this SerializedProperty property){

			property.CheckValid();

			var _result = EditorGUI.BeginProperty(new Rect(), GUIContent.none, property).tooltip;
			EditorGUI.EndProperty();

			return _result ?? "";

		}

		#endregion


		#region Array

		/// <summary>
		/// Check if the property is a valid array property.
		/// </summary>
		/// <param name="property">Property.</param>
		public static void CheckArray(this SerializedProperty property){

			property.CheckValid();
			if(property.isArray && SerializedPropertyType.String != property.propertyType) return;

			var _format = "SerializedProperty '{0}' isn't an array.";
			throw new NotSupportedException(string.Format(_format, property.propertyPath));

		}

		/// <summary>
		/// Insert an array element at the specified index and return it.
		/// </summary>
		/// 
		/// <remarks>
		/// Optional to copy values from the original element at the index, like the Unity's default behavior.
		/// Otherwise Leave all the values empty.
		/// </remarks>
		/// 
		/// <returns>Element property.</returns>
		/// <param name="property">Property.</param>
		/// <param name="index">Index.</param>
		/// <param name="copy">If set to <c>true</c> copy.</param>
		/// 
		public static SerializedProperty Insert(this SerializedProperty property, int index = -1, bool copy = false){

			property.CheckArray();
			if(0 > index || index > property.arraySize) index = property.arraySize;

			property.InsertArrayElementAtIndex(copy ? index : 0);
			if(!copy) property.MoveArrayElement(0, index);

			property.serializedObject.ApplyModifiedProperties();
			return property.GetArrayElementAtIndex(index);

		}

		/// <summary>
		/// Add the source <c>collection</c> to the array property.
		/// </summary>
		/// <param name="property">Property.</param>
		/// <param name="collection">Collection.</param>
		public static void AddRange(this SerializedProperty property, IEnumerable<Object> collection){

			property.CheckArray();

			foreach(var _element in collection){
				property.arraySize++;
				property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = _element;
			}

			property.serializedObject.ApplyModifiedProperties();

		}

		#endregion

	}

}
