
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ All rights reserved./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using WanzyeeStudio.Editrix.Extension;
using WanzyeeStudio.Extension;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Toolkit{

	/// <summary>
	/// Select filtered objects of specified type from the current selection.
	/// </summary>
	/// 
	/// <remarks>
	/// Used to easily edit multiple objects with built-in importer by selecting folders and filter it.
	/// This will look into all assets if nothing selected currently.
	/// Click menu "Assets/Select Filtered/Filter Selection..." to do with an editor window.
	/// Or directly use sub menu items to select by type or asset label.
	/// </remarks>
	/// 
	/// <remarks>
	/// The original idea is from
	/// <a href="http://wiki.unity3d.com/index.php?title=TextureImportSettings" target="_blank">
	/// <c>TextureImportSettings</c></a> and
	/// <a href="http://forum.unity3d.com/threads/77816/" target="_blank">
	/// <c>ChangeAudioImportSettings</c></a>.
	/// They became obsolete and stop updating since Unity do the multi-edit.
	/// But still one thing convenient lost, directly edit from selected asset folder.
	/// Obvious difference if you wanna select into multiple folder or hundreds of assets in a folder.
	/// This's used to simplify operation to select.
	/// </remarks>
	/*
	 * GUI callback is the only way to show dynamic menu in Unity editor scope.
	 * So make the window instance work in two different mode.
	 */
	public class SelectionFilter : EditorWindow{

		#region Menu

		/// <summary>
		/// Select none, with hotkey Ctrl-Alt-A.
		/// </summary>
		[MenuItem("Edit/Selection/None %&a")]
		public static void SelectNone(){
			Selection.objects = new Object[0];
		}

		/// <summary>
		/// Open a window of <c>SelectionFilter</c> to filter selection by specified type and mode.
		/// </summary>
		[MenuItem("Assets/Select Filtered/Filter Selection...", false, 30)]
		public static void OpenWindow(){
			GetWindow<SelectionFilter>(true);
		}

		/// <summary>
		/// Show the context menu to select all assets of specified type deep in current selection.
		/// </summary>
		[MenuItem("Assets/Select Filtered/Filter by Type", false, 100)]
		public static void FilterByType(){

			if(!FilterByTypeValid()) throw new InvalidOperationException("No asset in the project.");

			_options = GetSelectedTypes().ToDictionary((type) => type.FullName, (type) => GetSelected(type));

			CreateInstance<SelectionFilter>().ShowPopup();

		}

		/// <summary>
		/// Check if <c>FilterByType()</c> valid, i.e., any asset type found.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Assets/Select Filtered/Filter by Type", true)]
		private static bool FilterByTypeValid(){
			return GetSelectedTypes().Any();
		}
		
		/// <summary>
		/// Show the context menu to select all assets of specified label deep in current selection.
		/// </summary>
		[MenuItem("Assets/Select Filtered/Filter by Label", false, 100)]
		public static void FilterByLabel(){

			if(!FilterByLabelValid()) throw new InvalidOperationException("No asset label is set.");

			var _labels = EditrixUtility.GetAllAssetLabels(GetSelected(typeof(Object)));
			_options = _labels.ToDictionary((label) => label, (label) => GetSelected(typeof(Object), label));

			CreateInstance<SelectionFilter>().ShowPopup();

		}

		/// <summary>
		/// Check if <c>FilterByLabel()</c> valid, i.e., any asset label existed.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Assets/Select Filtered/Filter by Label", true)]
		private static bool FilterByLabelValid(){
			return EditrixUtility.GetAllAssetLabels(GetSelected(typeof(Object))).Any();
		}

		#endregion


		#region Static Fields

		/// <summary>
		/// Tooltip message shown on the top of window.
		/// </summary>
		private const string _MSG_TOOLTIP = "Filter objects in current selection.\nUse menu Edit/Selection as aid.";
		
		/// <summary>
		/// Warning message for invalid selection with nothing.
		/// </summary>
		private const string _MSG_INVALID = "Please select something or change mode to find valid type.";
		
		/// <summary>
		/// The title of window.
		/// </summary>
		private static readonly GUIContent _title = new GUIContent("Selection Filter");

		/// <summary>
		/// The option items with corresponding targets.
		/// </summary>
		private static Dictionary<string, Object[]> _options;

		#endregion


		#region Static Methods

		/// <summary>
		/// Show the option menu.
		/// </summary>
		private static void ShowOptionMenu(){

			var _menu = new GenericMenu();

			foreach(var _option in _options) _menu.AddItem(_option.Key, () => Selection.objects = _option.Value);
			
			_options = null;

			_menu.ShowAsContext();

		}

		/// <summary>
		/// Get filtered objects from current selection by specified type, label, and selection mode.
		/// </summary>
		/// 
		/// <remarks>
		/// Filter from all assets if nothing selected.
		/// Return <c>UnityEngine.GameObject</c> instead if the type is <c>UnityEngine.Component</c>.
		/// </remarks>
		/// 
		/// <returns>The filtered selection.</returns>
		/// <param name="type">Type to filter.</param>
		/// <param name="label">Asset label to filter.</param>
		/// <param name="mode">Selection mode to filter.</param>
		/*
		 * Use type of GameObject to filter deep in asset folder doesn't work.
		 * We have to filter Object first then check type to get prefabs.
		 */
		public static Object[] GetSelected(Type type, string label = "", SelectionMode mode = SelectionMode.DeepAssets){

			var _objects = Selection.objects;
			if(0 == _objects.Length) Selection.objects = EditrixUtility.GetAllAssets();

			var _object = typeof(GameObject).IsAssignableFrom(type);
			var _component = typeof(Component).IsAssignableFrom(type);

			IEnumerable<Object> _all;

			if(!_object && !_component) _all = Selection.GetFiltered(type, mode);
			else if(SelectionMode.DeepAssets != mode) _all = Selection.GetFiltered(typeof(GameObject), mode);
			else _all = Selection.GetFiltered(typeof(Object), mode).OfType<GameObject>().Cast<Object>();

			if(_component) _all = _all.Where((obj) => null != (obj as GameObject).GetComponent(type));
			if(!string.IsNullOrEmpty(label)) _all = _all.Where((obj) => AssetDatabase.GetLabels(obj).Contains(label));

			Selection.objects = _objects;
			return _all.OrderBy((obj) => EditrixUtility.GetObjectOrder(obj)).ToArray();

		}

		/// <summary>
		/// Get the types of all the selected objects, include base types.
		/// Also include components' type attached on each <c>UnityEngine.GameObject</c>.
		/// </summary>
		/// <returns>The selected types.</returns>
		/// <param name="label">Label.</param>
		/// <param name="mode">Mode.</param>
		private static List<Type> GetSelectedTypes(string label = "", SelectionMode mode = SelectionMode.DeepAssets){
			
			var _objects = GetSelected(typeof(GameObject), label, mode).Cast<GameObject>();
			var _components = _objects.SelectMany((obj) => obj.GetComponents<Component>());

			var _all = _components.Cast<Object>().Union(GetSelected(typeof(Object), label, mode));
			var _types = _all.SelectMany((obj) => obj.GetType().GetParents()).Distinct();

			var _result = _types.OrderBy((type) => GetTypeOrder(type)).ToList();
			_result.Remove(typeof(object));

			return _result;

		}

		/// <summary>
		/// Get an order <c>string</c> of given type for sorting.
		/// </summary>
		/// 
		/// <remarks>
		/// The order of type is:
		/// 	0. UnityEngine.Object.
		/// 	1. UnityEngine types, but not scene object type.
		/// 	2. UnityEditor types, e.g., MonoScript.
		/// 	3. Other custom types, but not scene object type.
		/// 	4. GameObject, main scene object type.
		/// 	5. Component, another scene object type.
		/// 	6. Other custom Component, MonoBehaviour.
		/// </remarks>
		/// 
		/// <returns>The order.</returns>
		/// <param name="type">Type.</param>
		/// 
		private static string GetTypeOrder(Type type){

			if(typeof(Object) == type) return "0";

			var _name = type.FullName;

			if(typeof(GameObject).IsAssignableFrom(type)) return "4" + _name;

			else if(typeof(MonoBehaviour).IsAssignableFrom(type)) return "6" + _name;

			else if(typeof(Component).IsAssignableFrom(type)) return "5" + _name;

			else if(_name.StartsWith("UnityEngine.")) return "1" + _name;

			else if(_name.StartsWith("UnityEditor.")) return "2" + _name;

			else return "3" + _name;

		}

		#endregion


		#region Fields
		
		/// <summary>
		/// Flag to refresh, set by initialization or callbacks.
		/// Checked to update menu when <c>OnGUI()</c>.
		/// </summary>
		private bool _refresh;
		
		/// <summary>
		/// Types of all objects selected by current selection mode.
		/// </summary>
		private List<Type> _types = new List<Type>();
		
		/// <summary>
		/// The type names shown on popup menu.
		/// </summary>
		private string[] _pops;
		
		/// <summary>
		/// The index of current type.
		/// </summary>
		[Obfuscation(Exclude = true)]
		[SerializeField]
		[Tooltip("Index of current type.")]
		private int _index;

		/// <summary>
		/// The asset label to filter.
		/// </summary>
		[Obfuscation(Exclude = true)]
		[SerializeField]
		[Tooltip("Asset label to filter.")]
		private string _label = "";

		/// <summary>
		/// The selection mode to filter.
		/// </summary>
		[Obfuscation(Exclude = true)]
		[SerializeField]
		[Tooltip("Selection mode to filter.")]
		private SelectionMode _mode = SelectionMode.DeepAssets;

		/// <summary>
		/// The objects filtered from selection.
		/// </summary>
		[Obfuscation(Exclude = true)]
		[SerializeField]
		[Tooltip("Objects filtered from selection.")]
		private Object[] _filtered = {};

		/// <summary>
		/// The GUI scroll position.
		/// </summary>
		/*
		 * The height of this layout is fixed.
		 * But still scroll in case EditorGUIUtility.singleLineHeight changed some day.
		 */
		private Vector2 _scroll;

		#endregion


		#region Message Methods
		
		/// <summary>
		/// OnEnable, set window size and update type menu.
		/// Register callbacks to check changed to update type menu and repaint.
		/// </summary>
		private void OnEnable(){

			if(null == _options) minSize = new Vector2(273f, 300f);
			else position = new Rect(position.size, new Vector2(10f, 10f));

			titleContent = _title;
			Refresh();

			Undo.undoRedoPerformed += Refresh;
			Selection.selectionChanged += Refresh;

			EditrixUtility.projectChanged += Refresh;
			EditrixUtility.hierarchyChanged += Refresh;

			EditrixUtility.playmodeChanged += Close;

		}

		/// <summary>
		/// OnDisable, deregister callbacks.
		/// </summary>
		private void OnDisable(){
			
			Undo.undoRedoPerformed -= Refresh;
			Selection.selectionChanged -= Refresh;
			
			EditrixUtility.projectChanged -= Refresh;
			EditrixUtility.hierarchyChanged -= Refresh;
			
			EditorApplication.delayCall -= Repaint;
			EditrixUtility.playmodeChanged -= Close;

		}
		
		/// <summary>
		/// OnGUI, draw the main GUI with tooltip, options, and filter button.
		/// </summary>
		private void OnGUI(){

			if(null == _options){

				if(_refresh) UpdateMenu();
				_refresh = false;

				EditorGUIUtility.hierarchyMode = true;
				Undo.RecordObject(this, "Change Filter");

				DrawTooltip();
				DrawHeader();
				DrawContent();
				DrawFooter();

			}else if(EventType.Repaint == Event.current.type){

				ShowOptionMenu();
				SendEvent(Event.KeyboardEvent("~"));
				EditorApplication.delayCall += Close;

			}
			
		}

		#endregion


		#region Methods

		/// <summary>
		/// Refresh callback method to update type menu and repaint.
		/// </summary>
		private void Refresh(){

			_refresh = true;

			EditorApplication.delayCall -= Repaint;
			EditorApplication.delayCall += Repaint;

		}

		/// <summary>
		/// Update the types and popup menu by current selection.
		/// Set <c>null</c> as invalid flag if nothing selected.
		/// </summary>
		private void UpdateMenu(){

			var _type = (_index < _types.Count) ? _types[_index] : null;

			_types = GetSelectedTypes(_label, _mode);

			_pops = _types.Select((type) => type.GetPrettyName(true)).ToArray();

			_index = (null != _type && _types.Contains(_type)) ? _types.IndexOf(_type) : 0;
			
			_filtered = (0 < _types.Count) ? GetSelected(_types[_index], _label, _mode) : new Object[0];

		}

		/// <summary>
		/// Show the label popup menu.
		/// </summary>
		/// <param name="rect">Rect.</param>
		private void ShowLabelMenu(Rect rect){

			var _menu = new GenericMenu();

			_menu.AddItem("[ None ]", UpdateLabel, "", string.IsNullOrEmpty(_label));
			
			var _labels = EditrixUtility.GetAllAssetLabels(_filtered);
			if(0 < _labels.Length) _menu.AddSeparator("");

			foreach(var _new in _labels) _menu.AddItem(_new, UpdateLabel, _new, _new == _label);

			_menu.DropDown(rect);

		}

		/// <summary>
		/// Set <c>_label</c> then update the menu.
		/// </summary>
		/// <param name="label">Label.</param>
		private void UpdateLabel(string label){
			_label = label;
			UpdateMenu();
		}

		#endregion


		#region Draw Methods

		/// <summary>
		/// Draw the tooltip, click to open documentation.
		/// </summary>
		private void DrawTooltip(){

			EditorGUILayout.HelpBox(_MSG_TOOLTIP, MessageType.Info, true);

			if(GUI.Button(GUILayoutUtility.GetLastRect(), "", GUIStyle.none)) Help.BrowseURL("https://git.io/viqRj");

		}

		/// <summary>
		/// Draw the header, includes all options.
		/// </summary>
		private void DrawHeader(){

			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();

			var _enabled = GUI.enabled;
			GUI.enabled = 0 < _types.Count;
			_index = EditorGUILayout.Popup("Type", _index, _pops);
			GUI.enabled = _enabled;

			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Label");

			var _rect = GUILayoutUtility.GetRect(new GUIContent(_label), EditorStyles.popup);
			if(GUI.Button(_rect, _label, EditorStyles.popup)) ShowLabelMenu(_rect);

			GUILayout.EndHorizontal();
			_mode = (SelectionMode)EditorGUILayout.EnumPopup("Selection Mode", _mode);

			if(EditorGUI.EndChangeCheck()) UpdateMenu();
			EditorGUILayout.Space();

		}

		/// <summary>
		/// Draw the preview list, or warning in need.
		/// </summary>
		private void DrawContent(){

			if(0 == _types.Count){
				EditorGUILayout.HelpBox(_MSG_INVALID, MessageType.Warning, true);
				return;
			}
			
			EditorGUI.indentLevel++;
			_scroll = GUILayout.BeginScrollView(_scroll);

			EditorGUILayout.PropertyField(new SerializedObject(this).FindProperty("_filtered"), true);

			GUILayout.EndScrollView();
			EditorGUI.indentLevel--;

			EditorGUILayout.Space();

		}

		/// <summary>
		/// Draw the footer, includes select button.
		/// </summary>
		private void DrawFooter(){

			GUILayout.FlexibleSpace();

			var _enabled = GUI.enabled;
			GUI.enabled = 0 < _types.Count;

			if(GUILayout.Button("Filter Selection", GUILayout.Height(EditorGUIUtility.singleLineHeight * 2f))){
				Selection.objects = _filtered;
				Close();
			}

			GUI.enabled = _enabled;

		}

		#endregion

	}

}
