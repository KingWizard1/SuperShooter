
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ All rights reserved./  (_(__(S)   |___*/

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

using WanzyeeStudio.Editrix.Extension;

namespace WanzyeeStudio.Editrix{

	/// <summary>
	/// Helper to make a <c>UnityEditorInternal.ReorderableList</c> foldable.
	/// </summary>
	public class ReorderableListExpander{

		#region Static Fields

		/// <summary>
		/// Used to determine if a list is wrapped before.
		/// </summary>
		private static readonly ReorderableList.HeaderCallbackDelegate _hook = (rect) => {};

		/// <summary>
		/// The common list settings to display folded layout.
		/// </summary>
		private static readonly ReorderableList _folded = new ReorderableList(null, typeof(object)){

			draggable = false,
			
			drawElementCallback = (rect, index, isActive, isFocused) => {},
			drawElementBackgroundCallback = (rect, index, isActive, isFocused) => {},

			elementHeightCallback = (index) => 0f,
			elementHeight = 0,

			drawFooterCallback = (rect) => {},
			footerHeight = 0
			
		};

		#endregion


		#region Static Methods

		/// <summary>
		/// Copy the layout settings from a list to another.
		/// </summary>
		/// <param name="source">Source list.</param>
		/// <param name="target">Target list.</param>
		private static void CopyLayouts(ReorderableList source, ReorderableList target){

			target.draggable = source.draggable;

			target.drawElementCallback = source.drawElementCallback;
			target.drawElementBackgroundCallback = source.drawElementBackgroundCallback;

			target.elementHeightCallback = source.elementHeightCallback;
			target.elementHeight = source.elementHeight;

			target.drawFooterCallback = source.drawFooterCallback;
			target.footerHeight = source.footerHeight;

		}

		/// <summary>
		/// Create a <c>UnityEditorInternal.ReorderableList</c> with common settings.
		/// </summary>
		/// 
		/// <remarks>
		/// I.e., the property label as the header, click to fold or expand the list, basic add and remove buttons.
		/// </remarks>
		/// 
		/// <returns>The reorderable list.</returns>
		/// <param name="property">Property.</param>
		/// <param name="drawElementCallback">Draw element callback.</param>
		/// <param name="elementHeightCallback">Element height callback.</param>
		/*
		 * The reference of reorderable list:
		 * http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/
		 * 
		 * Set draggable again since it becomes false if the property is not editable.
		 */
		public static ReorderableList Create(
			SerializedProperty property,
			ReorderableList.ElementCallbackDelegate drawElementCallback = null,
			ReorderableList.ElementHeightCallbackDelegate elementHeightCallback = null
		){

			if(null == property) throw new ArgumentNullException("property");

			var _result = new ReorderableList(property.serializedObject, property, true, true, true, true);
			_result.draggable = true;

			_result.drawElementCallback = drawElementCallback;
			_result.elementHeightCallback = elementHeightCallback;

			Wrap(_result);
			return _result;

		}

		/// <summary>
		/// Wrap the specified list's layout callbacks and settings to support folding.
		/// </summary>
		/// 
		/// <remarks>
		/// This only supports a list with <c>ReorderableList.serializedProperty</c>.
		/// And draw the header with the property name and tooltip if no <c>ReorderableList.drawHeaderCallback</c>.
		/// </remarks>
		/// 
		/// <param name="list">List.</param>
		/// 
		public static void Wrap(ReorderableList list){

			if(null == list) throw new ArgumentNullException("list");

			if(null == list.serializedProperty) throw new NotSupportedException("For serialized property list only.");

			if(null != list.drawHeaderCallback && list.drawHeaderCallback.GetInvocationList().Contains(_hook)) return;

			new ReorderableListExpander(list)._list.drawHeaderCallback += _hook;

		}

		#endregion


		#region Fields
		
		/// <summary>
		/// The list to fold and expand.
		/// </summary>
		private readonly ReorderableList _list;

		/// <summary>
		/// The original list settings to display expanded layout.
		/// </summary>
		private readonly ReorderableList _expanded = new ReorderableList(null, typeof(object));

		/// <summary>
		/// The default header label.
		/// </summary>
		private GUIContent _label;

		#endregion


		#region Methods

		/// <summary>
		/// Initialize this, and setup the list's layout callbacks and layout settings.
		/// </summary>
		/// <param name="list">List.</param>
		private ReorderableListExpander(ReorderableList list){

			_list = list;
			CopyLayouts(_list, _expanded);

			if(null == _list.drawHeaderCallback) _list.drawHeaderCallback = DrawDefaultHeader;
			_list.drawHeaderCallback += DrawExpandHeader;

			ApplyLayouts();

		}

		/// <summary>
		/// Draw the header with the property name and tooltip.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/*
		 * I don't know why did the EditorGUI.BeginProperty() give me the previous property's label?
		 */
		private void DrawDefaultHeader(Rect rect){

			if(null == _label){
				var _property = _list.serializedProperty;
				_label = new GUIContent(ObjectNames.NicifyVariableName(_property.name), _property.GetTooltip());
			}

			EditorGUI.LabelField(rect, _label);

		}

		/// <summary>
		/// Draw the header with the foldout toggle.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/*
		 * First resize to align the folding arrow and area.
		 * Then resize to avoid covering the remove button of EventTrigger.
		 */
		private void DrawExpandHeader(Rect rect){

			rect.xMin -= 6f;
			rect.xMax += 5f;

			if(_list.serializedProperty.serializedObject.targetObject is EventTrigger) rect.xMax -= 25f;

			var _expand = _list.serializedProperty.isExpanded;
			if(EditorGUI.Foldout(rect, _expand, "", true) == _expand) return;

			_list.serializedProperty.isExpanded = !_expand;
			ApplyLayouts();

		}

		/// <summary>
		/// Check the expand state to apply the layout settings.
		/// </summary>
		/*
		 * Inspector might repaint with the expanded list height even after folding, repeat repainting to correct.
		 */
		private void ApplyLayouts(){

			var _expand = _list.serializedProperty.isExpanded;
			CopyLayouts(_expand ? _expanded : _folded, _list);

			var _inspector = EditorWindow.focusedWindow;
			if(null != _inspector) RepeatCall(_inspector.Repaint, 3);

		}

		/// <summary>
		/// Repeat delay call.
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="count">Count.</param>
		private void RepeatCall(Action action, int count){
			action.Invoke();
			if(1 < count) EditorApplication.delayCall += () => RepeatCall(action, count - 1);
		}

		#endregion

	}

}
