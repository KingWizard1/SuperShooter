
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

using WanzyeeStudio.Extension;

namespace WanzyeeStudio.Editrix.Toolkit{

	/// <summary>
	/// Utility to switch or close a tab or window.
	/// </summary>
	/// 
	/// <remarks>
	/// Menu "Window/View/Next Tab", with hotkey Ctrl-`, to focus the next of hovered or focused tab.
	/// Menu "Window/View/Previous Tab", with hotkey Ctrl-Shift-`, to focus the previous of hovered or focused tab.
	/// Menu "Window/View/Close Tab", with hotkey Ctrl-W, to close the hovered and focused window tab.
	/// Menu "Window/View/Close Window", with hotkey Ctrl-Shift-W, to close the hovered and focused window.
	/// Easy thing causes easy mistake, be careful to use this, since closing is not undoable.
	/// </remarks>
	/// 
	public static class TabNavigator{
		
		#region Menu

		/// <summary>
		/// Switch to the next window tab from the focused one, with hotkey Ctrl-`.
		/// </summary>
		[MenuItem("Window/View/Next Tab %`", false, 100)]
		public static void NextTab(){
			
			var _tab = GetTab();
			_tab.Value[(_tab.Value.IndexOf(_tab.Key) + 1) % _tab.Value.Count].Focus();

		}

		/// <summary>
		/// Switch to the previous window tab from the focused one, with hotkey Ctrl-Shift-`.
		/// </summary>
		[MenuItem("Window/View/Previous Tab %#`", false, 100)]
		public static void PreviousTab(){
			
			var _tab = GetTab();
			_tab.Value[(_tab.Value.IndexOf(_tab.Key) + _tab.Value.Count - 1) % _tab.Value.Count].Focus();

		}

		/// <summary>
		/// Check if <c>NextTab()</c> or <c>PreviousTab</c> valid, multiple tabs in the hovered or focused tab.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Next Tab %`", true)]
		[MenuItem("Window/View/Previous Tab %#`", true)]
		private static bool NextPreviousTabValid(){
			return null != GetTab(false).Key;
		}

		/// <summary>
		/// Close the focused window tab, with hotkey Ctrl-W.
		/// </summary>
		[MenuItem("Window/View/Close Tab %w", false, 100)]
		public static void CloseTab(){
			
			if(!CloseTabValid()) throw new InvalidOperationException("No window tab focused and hovered.");
			EditorWindow.focusedWindow.Close();

		}

		/// <summary>
		/// Check if <c>CloseTab()</c> valid, focused window is hovered and existing.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Close Tab %w", true)]
		private static bool CloseTabValid(){
			return EditorWindow.mouseOverWindow == EditorWindow.focusedWindow && null != EditorWindow.focusedWindow;
		}

		/// <summary>
		/// Close the whole window contains the focused tab, with hotkey Ctrl-Shift-W.
		/// </summary>
		[MenuItem("Window/View/Close Window %#w", false, 100)]
		public static void CloseWindow(){

			var _focused = EditorWindow.focusedWindow;
			var _window = (EditorWindow.mouseOverWindow == _focused) ? _focused : null;
			if(null == _window) throw new InvalidOperationException("No focused window tab hovered");

			try{ _closeMethod.Invoke(GetRoot(_window), null); }
			catch{ throw new MissingMemberException("Reflections failed."); }

		}

		/// <summary>
		/// Check if <c>CloseWindow()</c> valid, focused tab is hovered in a closable window.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Close Window %#w", true)]
		private static bool CloseWindowValid(){
			
			var _focused = EditorWindow.focusedWindow;
			return EditorWindow.mouseOverWindow == _focused && null != GetRoot(_focused);

		}

		#endregion


		#region Type Fields

		/// <summary>
		/// Type of dock area.
		/// </summary>
		private static readonly Type _dockType = Type.GetType("UnityEditor.DockArea, UnityEditor");

		/// <summary>
		/// Type of main window.
		/// </summary>
		private static readonly Type _mainType = Type.GetType("UnityEditor.MainWindow, UnityEditor");

		/// <summary>
		/// Type of window host view.
		/// </summary>
		private static readonly Type _hostType = Type.GetType("UnityEditor.HostView, UnityEditor");

		/// <summary>
		/// Type of window view.
		/// </summary>
		private static readonly Type _viewType = Type.GetType("UnityEditor.View, UnityEditor");

		/// <summary>
		/// Type of container window.
		/// </summary>
		private static readonly Type _containerType = Type.GetType("UnityEditor.ContainerWindow, UnityEditor");

		#endregion


		#region Fields

		/// <summary>
		/// Field to get the editor windows of a dock area.
		/// </summary>
		private static readonly FieldInfo _paneField = _dockType.GetField("m_Panes", false, typeof(List<EditorWindow>));

		/// <summary>
		/// Field to get the host view of a window.
		/// </summary>
		private static readonly FieldInfo _parentField = typeof(EditorWindow).GetField("m_Parent", false, _hostType);

		/// <summary>
		/// Property to get the parent of a view.
		/// </summary>
		private static readonly PropertyInfo _parentProp = _viewType.GetProperty("parent", false, _viewType);

		/// <summary>
		/// Property to get the window container of a view.
		/// </summary>
		private static readonly PropertyInfo _windowProp = _viewType.GetProperty("window", false, _containerType);

		/// <summary>
		/// Method to close a window container.
		/// </summary>
		private static readonly MethodInfo _closeMethod = _containerType.GetMethod("Close", false, typeof(void));

		#endregion


		#region Methods

		/// <summary>
		/// Get the switchable tab, i.e., hovered or focused tab with multiple siblings.
		/// Optional to throw an exception or return nothing.
		/// </summary>
		/// <returns>The tab.</returns>
		/// <param name="exception">Flag to throw an exception or return nothing.</param>
		private static KeyValuePair<EditorWindow, List<EditorWindow>> GetTab(bool exception = true){
			try{
				var _window = EditorWindow.mouseOverWindow ?? EditorWindow.focusedWindow;
				return new KeyValuePair<EditorWindow, List<EditorWindow>>(_window, GetSiblings(_window));
			}catch{
				if(exception) throw;
				else return new KeyValuePair<EditorWindow, List<EditorWindow>>();
			}
		}

		/// <summary>
		/// Get the sibling tabs of specified window tab, include itself.
		/// </summary>
		/// <returns>The siblings.</returns>
		/// <param name="window">Window.</param>
		private static List<EditorWindow> GetSiblings(EditorWindow window){

			if(null == window) throw new ArgumentNullException("window");
			if(null == _dockType) throw new TypeLoadException("Could not load type 'UnityEditor.DockArea'.");

			if(null == _paneField) throw new MissingFieldException("UnityEditor.DockArea", "m_Panes");
			if(null == _parentField) throw new MissingFieldException("UnityEditor.EditorWindow", "m_Parent");

			var _dock = _parentField.GetValue(window);
			if(!_dockType.IsInstanceOfType(_dock)) throw new InvalidOperationException("The window isn't docked.");

			return _paneField.GetValue(_dock) as List<EditorWindow>;

		}

		/// <summary>
		/// Get the root container of specified window, exclude editor main window which isn't closable.
		/// </summary>
		/// <returns>The root.</returns>
		/// <param name="window">Window.</param>
		private static object GetRoot(EditorWindow window){
			
			if(null == _mainType || null == _parentField || null == _parentProp || null == _windowProp) return null;
			if(null == _closeMethod || null == window) return null;

			var _root = _parentField.GetValue(window);
			for(var _parent = _root; null != _parent; _parent = _parentProp.GetValue(_parent, null)) _root = _parent;

			return _mainType.IsInstanceOfType(_root) ? null : _windowProp.GetValue(_root, null);

		}

		#endregion

	}

}
