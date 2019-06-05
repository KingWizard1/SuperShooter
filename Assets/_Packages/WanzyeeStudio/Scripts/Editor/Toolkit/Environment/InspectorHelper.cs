
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

using WanzyeeStudio.Extension;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Toolkit{
	
	/// <summary>
	/// Open new Inspector to edit specified object, and toggle Inspector states.
	/// </summary>
	/// 
	/// <remarks>
	/// Use <c>UnityEngine.Object</c> context menu "Inspect" to show single object in a new locked Inspector.
	/// And "Select" to select it, helpful when open multiple locked Inspector windows.
	/// Also able for <c>UnityEngine.Component</c>, useful to edit multiple on different <c>UnityEngine.GameObject</c>.
	/// </remarks>
	/// 
	/// <remarks>
	/// Menu "Window/View/Inspect Selected", with hotkey Ctrl-I, to show selected objects in a new locked Inspector.
	/// Menu "Window/View/Toggle Inspector Mode", with hotkey Alt-I, to toggle debug mode of an Inspector.
	/// Menu "Window/View/Toggle Inspector Lock", with hotkey Ctrl-Shift-I, to toggle lock state of an Inspector.
	/// Toggle the one with mouse over, or focused, or the single one if multiple, otherwise do nothing.
	/// </remarks>
	/// 
	public static class InspectorHelper{

		#region Menu

		/// <summary>
		/// Open new locked Inspector window to show the <c>UnityEngine.Object</c>.
		/// </summary>
		/// <param name="command">Command.</param>
		[MenuItem("CONTEXT/Object/Inspect", false, 10000)]
		private static void ObjectInspect(MenuCommand command){
			Inspect(command.context);
		}

		/// <summary>
		/// Check if <c>ObjectInspect()</c> valid, reflection existing.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		/// <param name="command">Command.</param>
		[MenuItem("CONTEXT/Object/Inspect", true)]
		private static bool ObjectInspectValid(MenuCommand command){
			return null != _lockProp;
		}

		/// <summary>
		/// Ping and select the <c>UnityEngine.Object</c>.
		/// </summary>
		/// <param name="command">Command.</param>
		[MenuItem("CONTEXT/Object/Select", false, 10000)]
		private static void ObjectSelect(MenuCommand command){
			
			var _component = command.context as Component;
			var _object = (null == _component) ? command.context : _component.gameObject;

			EditorGUIUtility.PingObject(_object);
			Selection.activeObject = _object;

		}

		/// <summary>
		/// Open new locked Inspector window to show current selected objects, with hotkey Ctrl-I.
		/// </summary>
		[MenuItem("Window/View/Inspect Selected %i", false, 200)]
		public static void InspectSelected(){
			if(null == Selection.activeObject) throw new InvalidOperationException("Nothing is selected.");
			Inspect(Selection.objects);
		}

		/// <summary>
		/// Check if <c>InspectSelected()</c> valid, reflection existing and anything selected.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Inspect Selected %i", true)]
		private static bool InspectSelectedValid(){
			return null != _lockProp && null != Selection.activeObject;
		}

		/// <summary>
		/// Toggle Inspector debug mode, with hotkey Alt-I.
		/// </summary>
		[MenuItem("Window/View/Toggle Inspector Mode &i", false, 200)]
		public static void ToggleInspectorMode(){

			var _inspector = GetCurrent();
			var _mode = GetMode(_inspector);

			SetMode(_inspector, (InspectorMode.Normal == _mode) ? InspectorMode.Debug : InspectorMode.Normal);

		}

		/// <summary>
		/// Check if <c>ToggleInspectorMode()</c> valid, reflection valid and toggleable window found.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Toggle Inspector Mode &i", true)]
		private static bool ToggleInspectorModeValid(){
			return null != _modeField && null != _modeMethod && null != GetCurrent(false);
		}

		/// <summary>
		/// Toggle Inspector lock state, with hotkey Ctrl-Shift-I.
		/// </summary>
		[MenuItem("Window/View/Toggle Inspector Lock %#i", false, 200)]
		public static void ToggleInspectorLock(){
			var _inspector = GetCurrent();
			SetLocked(_inspector, !GetLocked(_inspector));
		}

		/// <summary>
		/// Check if <c>ToggleInspectorLock()</c> valid, reflection valid and toggleable window found.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Toggle Inspector Lock %#i", true)]
		private static bool ToggleInspectorLockValid(){
			return null != _lockProp && null != GetCurrent(false);
		}

		#endregion


		#region Fields

		/// <summary>
		/// Type of Inspector window.
		/// </summary>
		private static readonly Type _windowType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor");

		/// <summary>
		/// Field to get the mode of an Inspector.
		/// </summary>
		private static readonly FieldInfo _modeField = _windowType.GetField(
			"m_InspectorMode",
			false,
			typeof(InspectorMode)
		);

		/// <summary>
		/// Method to set the mode of an Inspector.
		/// </summary>
		private static readonly MethodInfo _modeMethod = _windowType.GetMethod(
			"SetMode",
			false,
			typeof(void),
			typeof(InspectorMode)
		);

		/// <summary>
		/// Property to lock an Inspector.
		/// </summary>
		private static readonly PropertyInfo _lockProp = _windowType.GetProperty(
			"isLocked",
			false,
			typeof(bool)
		);

		#endregion


		#region Methods

		/// <summary>
		/// Get the current Inspector which is mouse hovered or focused, or the single instance.
		/// Otherwise optional to throw an exception or return <c>null</c>.
		/// </summary>
		/// <returns>Inspector.</returns>
		/// <param name="exception">Flag to throw an exception or return <c>null</c>.</param>
		private static EditorWindow GetCurrent(bool exception = true){

			if(CheckInspector(EditorWindow.mouseOverWindow, false)) return EditorWindow.mouseOverWindow;
			if(CheckInspector(EditorWindow.focusedWindow, false)) return EditorWindow.focusedWindow;

			var _windows = Resources.FindObjectsOfTypeAll(_windowType);

			if(1 == _windows.Length) return _windows[0] as EditorWindow;
			if(!exception) return null;

			if(0 == _windows.Length) throw new InvalidOperationException("There's no Inspector window.");
			else throw new AmbiguousMatchException("There're multiple Inspector, please hover or focus one.");

		}

		/// <summary>
		/// Check if the specified window is an Inspector, optional to throw an exception or return <c>false</c>.
		/// </summary>
		/// <returns><c>true</c> if is inspector; otherwise, <c>false</c>.</returns>
		/// <param name="inspector">Inspector.</param>
		/// <param name="exception">Flag to throw an exception or return <c>false</c>.</param>
		private static bool CheckInspector(EditorWindow inspector, bool exception = true){

			if(null == _windowType) throw new MissingMemberException("Type of UnityEditor.InspectorWindow is missing");
			if(_windowType.IsInstanceOfType(inspector)) return true;

			if(!exception) return false;
			throw new ArgumentException("The window isn't an Inspector.");

		}

		#endregion


		#region Methods

		/// <summary>
		/// Open new Inspector window locked to show specified objects.
		/// </summary>
		/// <returns>New Inspector window.</returns>
		/// <param name="targets">Targets.</param>
		public static EditorWindow Inspect(params Object[] targets){
			
			var _result = Create(targets);
			_result.Show();

			return _result;

		}

		/// <summary>
		/// Create new Inspector window locked to specified objects without showing.
		/// </summary>
		/// <returns>New Inspector window.</returns>
		/// <param name="targets">Targets.</param>
		public static EditorWindow Create(params Object[] targets){
			
			var _targets = (targets ?? new Object[0]).Where((target) => null != target).Distinct();
			if(!_targets.Any()) throw new ArgumentException("No target assigned.", "targets");

			if(null == _lockProp) throw new MissingMemberException("UnityEditor.InspectorWindow", "isLocked");
			var _result = ScriptableObject.CreateInstance(_windowType) as EditorWindow;

			var _objects = Selection.objects;
			Selection.objects = _targets.ToArray();

			SetLocked(_result, true);
			Selection.objects = _objects;

			return _result;

		}

		/// <summary>
		/// Get the inspector mode of specified Inspector.
		/// </summary>
		/// <returns>The mode.</returns>
		/// <param name="inspector">Inspector.</param>
		public static InspectorMode GetMode(EditorWindow inspector){

			CheckInspector(inspector);
			if(null == _modeField) throw new MissingFieldException("UnityEditor.InspectorWindow", "m_InspectorMode");

			return (InspectorMode)_modeField.GetValue(inspector);

		}

		/// <summary>
		/// Sets the mode.
		/// </summary>
		/// <param name="inspector">Inspector.</param>
		/// <param name="mode">Mode.</param>
		public static void SetMode(EditorWindow inspector, InspectorMode mode){

			CheckInspector(inspector);
			if(null == _modeMethod) throw new MissingMethodException("UnityEditor.InspectorWindow", "SetMode");

			_modeMethod.Invoke(inspector, new object[]{mode});

		}

		/// <summary>
		/// Get the lock state of specified Inspector.
		/// </summary>
		/// <returns><c>true</c>, if locked, <c>false</c> otherwise.</returns>
		/// <param name="inspector">Inspector.</param>
		public static bool GetLocked(EditorWindow inspector){

			CheckInspector(inspector);
			if(null == _lockProp) throw new MissingMemberException("UnityEditor.InspectorWindow", "isLocked");

			return (bool)_lockProp.GetValue(inspector, null);

		}

		/// <summary>
		/// Set the lock state of specified Inspector.
		/// </summary>
		/// <param name="inspector">Inspector.</param>
		/// <param name="locked">If set to <c>true</c> locked.</param>
		public static void SetLocked(EditorWindow inspector, bool locked){

			CheckInspector(inspector);
			if(null == _lockProp) throw new MissingMemberException("UnityEditor.InspectorWindow", "isLocked");

			_lockProp.SetValue(inspector, locked, null);
			inspector.Repaint();

		}

		#endregion

	}

}
