
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using System;
using System.Reflection;

using WanzyeeStudio.Extension;

namespace WanzyeeStudio.Editrix.Toolkit{
	
	/// <summary>
	/// Helper to undock and fix current Game view size in pixel unit absolutely.
	/// </summary>
	/// 
	/// <remarks>
	/// Apply by clicking menu "Window/View/Fix Game View Size".
	/// The target window found by the order below:
	/// 	1. Game view with mouse over.
	/// 	2. Current focused Game view.
	/// 	3. The main Game view.
	/// </remarks>
	/// 
	/// <remarks>
	/// It's useful to preview the real size in Game view, pixel by pixel, not ratio scaled.
	/// Set window to selected size on the aspect drop-down menu, only for "Fixed Resolution".
	/// Use this to easily set size and save presets with the built-in feature.
	/// It might be incorrect if the size is too big to close even over the monitor.
	/// </remarks>
	/// 
	/// <remarks>
	/// Note, this works by reflection to access internal classes.
	/// We'd try to keep it up-to-date, but can't guarantee.
	/// </remarks>
	/*
	 * http://answers.unity3d.com/questions/179775/
	 */
	public static class GameViewHelper{

		#region Menu
		
		/// <summary>
		/// Resize game view to selected fixed resolution.
		/// </summary>
		[MenuItem("Window/View/Fix Game View Size", false, 205)]
		public static void FixGameViewSize(){
			if(FixGameViewSizeValid()) FixSize();
			else throw new MissingMemberException("Some reflections invalid.");
		}
		
		/// <summary>
		/// Check if <c>FixGameViewSize()</c> valid, resizable window existing.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/View/Fix Game View Size", true)]
		private static bool FixGameViewSizeValid(){
			EditorWindow _window;
			object _size;
			return GetResizable(out _window, out _size);
		}

		#endregion


		#region Fields
		
		/// <summary>
		/// Type of Game view window.
		/// </summary>
		private static readonly Type _viewType = Type.GetType("UnityEditor.GameView, UnityEditor");
		
		/// <summary>
		/// Type of <c>UnityEditor.GameViewSize</c>.
		/// With custom setting info of game view resolution, not the displayed.
		/// </summary>
		private static readonly Type _sizeType = Type.GetType("UnityEditor.GameViewSize, UnityEditor");

		/// <summary>
		/// Type of <c>UnityEditor.GameViewSizeType</c>.
		/// Enum to set game view aspect ratio or fixed resolution.
		/// </summary>
		private static readonly Type _sizeTypeType = Type.GetType("UnityEditor.GameViewSizeType, UnityEditor");
		
		/// <summary>
		/// Method info to get main game view.
		/// </summary>
		private static readonly MethodInfo _mainMethod = _viewType.GetMethod("GetMainGameView", true, _viewType);
		
		/// <summary>
		/// Property info to get current setting of game view size.
		/// </summary>
		private static readonly PropertyInfo _sizeProp = _viewType.GetProperty("currentGameViewSize", false, _sizeType);
		
		/// <summary>
		/// Property info to get type of game view size, 0 as AspectRatio, 1 as FixedResolution.
		/// </summary>
		private static readonly PropertyInfo _sizeTypeProp = _sizeType.GetProperty("sizeType", false, _sizeTypeType);
		
		/// <summary>
		/// Property info to get width of game view size.
		/// </summary>
		private static readonly PropertyInfo _widthProp = _sizeType.GetProperty("width", false, typeof(int));
		
		/// <summary>
		/// Property info to get height of game view size.
		/// </summary>
		private static readonly PropertyInfo _heightProp = _sizeType.GetProperty("height", false, typeof(int));

		#endregion


		#region Methods
		
		/// <summary>
		/// Undock and fix current game view size in pixel unit.
		/// Set window size as Aspect Drop-Down menu, only for fixed resolution.
		/// </summary>
		/*
		 * Extend the height to contain the toolbar.
		 * Set the position twice to ensure to undock and resize.
		 */
		private static void FixSize(){
			
			EditorWindow _window;
			object _size;
			if(!GetResizable(out _window, out _size)) return;
			
			var _position = _window.position;
			
			_position.width = (int)_widthProp.GetValue(_size, null);
			_position.height = (int)_heightProp.GetValue(_size, null) + 17;
			
			_window.position = _position;
			_window.position = _position;
			
		}
		
		/// <summary>
		/// Get game view and check if able to fix size.
		/// Check reflections first, then try to get and check values used.
		/// Also valid for FixedResolution only.
		/// </summary>
		/// <returns><c>true</c>, if able to fix size.</returns>
		/// <param name="window">GameView Window.</param>
		/// <param name="size">GameViewSize.</param>
		private static bool GetResizable(out EditorWindow window, out object size){
			
			window = null;
			size = null;
			
			if(null == _mainMethod || null == _sizeProp) return false;
			if(null == _sizeTypeProp || null == _widthProp || null == _heightProp) return false;
			
			window = GetCurrent();
			if(null == window) return false;
			
			size = _sizeProp.GetValue(window, null);
			return (null != size) && (1 == (int)_sizeTypeProp.GetValue(size, null));
			
		}
		
		/// <summary>
		/// Get the current game view.
		/// Window with mouse over first, then focused, otherwise find main.
		/// </summary>
		/// <returns>The current window.</returns>
		private static EditorWindow GetCurrent(){

			if(_viewType.IsInstanceOfType(EditorWindow.mouseOverWindow)) return EditorWindow.mouseOverWindow;

			else if(_viewType.IsInstanceOfType(EditorWindow.focusedWindow)) return EditorWindow.focusedWindow;
			
			else return _mainMethod.Invoke(null, null) as EditorWindow;
			
		}
		
		#endregion

	}

}
