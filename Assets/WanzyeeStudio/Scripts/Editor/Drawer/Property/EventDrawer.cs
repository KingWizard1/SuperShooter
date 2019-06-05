
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using WanzyeeStudio.Extension;
using WanzyeeStudio.Editrix.Extension;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Drawer{
	
	/// <summary>
	/// <c>UnityEditor.CustomPropertyDrawer</c> for <c>UnityEngine.Events.UnityEvent</c>.
	/// </summary>
	/// 
	/// <remarks>
	/// Unity doesn't allow to reorder the event list by default with unknown reason.
	/// And not support to select between multiple components of the same type.
	/// This overrides the original drawer to make the <c>UnityEditorInternal.ReorderableList</c> draggable.
	/// And modifies the <c>UnityEditor.GenericMenu</c> items to identify each component.
	/// </remarks>
	/// 
	/// <remarks>
	/// Note, this works by reflection, it might fail if Unity changes the code in the future.
	/// </remarks>
	/*
	 * Type of UnityEventBase has been used by UnityEditorInternal.UnityEventDrawer.
	 * According to built-in mechanism, We actually can't override it.
	 * 
	 * This tries to hack it for as more applicability as possible.
	 * And declare the basic attributes in case the hacking failed.
	 */
	[CustomPropertyDrawer(typeof(UnityEventBase), true)]
	[CustomPropertyDrawer(typeof(UnityEvent), true)]
	[CustomPropertyDrawer(typeof(UnityEvent<BaseEventData>), true)]
	internal class EventDrawer : UnityEventDrawer{

		#region Utility Fields

		/// <summary>
		/// Utility type to find the drawer for specified type.
		/// </summary>
		private static readonly Type _utilityType = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor");

		/// <summary>
		/// Method to build the drawer type dictionary.
		/// </summary>
		private static readonly MethodInfo _buildMethod = _utilityType.GetMethod(
			"BuildDrawerTypeForTypeDictionary",
			true,
			typeof(void)
		);

		/// <summary>
		/// Field to get the drawer type set dictionary to replace.
		/// </summary>
		private static readonly FieldInfo _setsField = _utilityType.GetField(
			"s_DrawerTypeForType",
			true
		);

		/// <summary>
		/// Field to get the drawer type.
		/// </summary>
		private static readonly FieldInfo _drawerField = _utilityType.GetNestedType("DrawerKeySet", false).GetField(
			"drawer",
			false,
			typeof(Type)
		);

		#endregion


		#region Static Methods

		/// <summary>
		/// Replace the default drawer type for <c>UnityEngine.Events.UnityEventBase</c> to this.
		/// </summary>
		[InitializeOnLoadMethod]
		private static void ReplaceDrawerTypes(){

			try{

				_buildMethod.Invoke(null, null);
				var _sets = _setsField.GetValue(null) as IDictionary;

				var _types = _sets.Keys.OfType<Type>(
					).Where((type) => typeof(UnityEventBase).IsAssignableFrom(type)
					).Where((type) => typeof(UnityEventDrawer) == _drawerField.GetValue(_sets[type])
				).ToArray();
				
				foreach(var _type in _types){
					var _set = _sets[_type];
					_drawerField.SetValue(_set, typeof(EventDrawer));
					_sets[_type] = _set;
				}

			}catch{}

		}

		#endregion


		#region Drawer Fields

		/// <summary>
		/// Field to get the reorderable list to setup.
		/// </summary>
		private static readonly FieldInfo _listField = typeof(UnityEventDrawer).GetField(
			"m_ReorderableList",
			false,
			typeof(ReorderableList)
		);

		/// <summary>
		/// Field to get the event for building menu.
		/// </summary>
		private static readonly FieldInfo _eventField = typeof(UnityEventDrawer).GetField(
			"m_DummyEvent",
			false,
			typeof(UnityEventBase)
		);

		/// <summary>
		/// Method to build the popup menu to select the component.
		/// </summary>
		private static readonly MethodInfo _popupMethod = typeof(UnityEventDrawer).GetMethod(
			"BuildPopupList",
			true,
			typeof(GenericMenu),
			new[]{
				typeof(Object),
				typeof(UnityEventBase),
				typeof(SerializedProperty)
			}
		);

		/// <summary>
		/// Field to get all the menu items to rename.
		/// </summary>
		private static readonly FieldInfo _itemsField = typeof(GenericMenu).GetField(
			"menuItems",
			false,
			typeof(ArrayList)
		);

		/// <summary>
		/// Field to get the menu item content to rename.
		/// </summary>
		private static readonly FieldInfo _contentField = typeof(GenericMenu).GetNestedType("MenuItem", false).GetField(
			"content",
			false,
			typeof(GUIContent)
		);

		#endregion


		#region Fields

		/// <summary>
		/// The stored property paths which has been initialized.
		/// </summary>
		private readonly List<string> _inited = new List<string>();

		#endregion


		#region Methods

		/// <summary>
		/// Set the property's list reorderable and foldable, then replace the drawing callbacks.
		/// Called when <c>GetPropertyHeight()</c> be invoked since it's the first GUI entry.
		/// </summary>
		/// <param name="property">Property.</param>
		/*
		 * Use base.GetPropertyHeight() to ensure the list is created.
		 */
		private void Initialize(SerializedProperty property){

			if(_inited.Contains(property.propertyPath)) return;
			_inited.Add(property.propertyPath);

			try{

				base.GetPropertyHeight(property, GUIContent.none);

				var _list = _listField.GetValue(this) as ReorderableList;
				_list.draggable = true;

				WrapHeaderCallback(_list, property);
				WrapElementCallback(_list);

				ReorderableListExpander.Wrap(_list);

			}catch{}

		}

		/// <summary>
		/// Wrap <c>drawHeaderCallback</c> to display the tooltip.
		/// </summary>
		/// <param name="list">List.</param>
		/// <param name="property">Property.</param>
		private void WrapHeaderCallback(ReorderableList list, SerializedProperty property){

			var _tooltip = property.GetTooltip();
			if("" == _tooltip) return;

			var _label = new GUIContent("", _tooltip);
			list.drawHeaderCallback += (rect) => EditorGUI.LabelField(rect, _label, GUIStyle.none);

		}

		/// <summary>
		/// Wrap <c>drawElementCallback</c> to replace the target menu button.
		/// </summary>
		/// <param name="list">List.</param>
		private void WrapElementCallback(ReorderableList list){

			var _callback = list.drawElementCallback;

			list.drawElementCallback = (rect, index, isActive, isFocused) => {
				try{
					ReplaceTargetMenu(rect, list.serializedProperty.GetArrayElementAtIndex(index));
				}catch{
					list.drawElementCallback = _callback;
				}
			};

			list.drawElementCallback += _callback;

		}

		/// <summary>
		/// Check if to replace the target menu button, to show renamed items to identify components.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="element">Element.</param>
		private void ReplaceTargetMenu(Rect rect, SerializedProperty element){

			var _target = element.FindPropertyRelative("m_Target").objectReferenceValue;
			if(!(_target is GameObject || _target is Component)) return;

			rect.xMin = rect.x + (rect.width * 0.3f) + 5f;
			rect.yMax = rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

			if(!GUI.Button(rect, GUIContent.none, GUIStyle.none)) return;

			var _event = _eventField.GetValue(this);
			var _menu = _popupMethod.Invoke(null, new object[]{_target, _event, element}) as GenericMenu;

			RenameTargetMenu(_menu);
			_menu.DropDown(rect);

		}

		/// <summary>
		/// Rename the items of the specified menu with target component index.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/*
		 * Skip the first 2 items, which are "No Function" and a separator.
		 */
		private void RenameTargetMenu(GenericMenu menu){

			var _items = (_itemsField.GetValue(menu) as ArrayList).Cast<object>();
			var _contents = _items.Skip(2).Select((item) => _contentField.GetValue(item) as GUIContent);

			var _group = _contents.GroupBy((content) => content.text.Remove(content.text.IndexOf('/')));
			var _firsts = _group.Select((contents) => contents.First().text).ToList();

			var _index = -1;
			foreach(var _content in _contents){

				if(_firsts.Contains(_content.text)) _index++;
				_content.text = string.Format("[{0}] {1}", _index, _content.text);

			}

		}

		/// <summary>
		/// Get the height of the property.
		/// </summary>
		/// <returns>The property height.</returns>
		/// <param name="property">Property.</param>
		/// <param name="label">Label.</param>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label){

			Initialize(property);

			return base.GetPropertyHeight(property, label);

		}

		#endregion

	}

}
