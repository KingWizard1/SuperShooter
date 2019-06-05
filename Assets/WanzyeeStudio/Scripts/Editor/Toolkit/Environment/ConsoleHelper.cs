
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
	/// Helper to clear logs in Console window.
	/// </summary>
	/// 
	/// <remarks>
	/// Trigger from the menu "Window/Clear Console", or hotkey ALT-Shift-C.
	/// </remarks>
	/// 
	public static class ConsoleHelper{

		#region Menu

		/// <summary>
		/// Clear the console logs, with hotkey Alt-Shift-C.
		/// </summary>
		/*
		 * Invoke twice to avoid only focusing the Console window in case.
		 */
		[MenuItem("Window/Clear Console &#c", false, 22000)]
		public static void ClearConsole(){

			if(!ClearConsoleValid()) throw new MissingMethodException("LogEntries", "Clear");

			_method.Invoke(null, null);
			_method.Invoke(null, null);

		}
		
		/// <summary>
		/// Check if <c>ClearConsole()</c> valid, reflection existing.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Window/Clear Console &#c", true)]
		private static bool ClearConsoleValid(){
			return (null != _method);
		}

		#endregion


		#region Fields

		/// <summary>
		/// Method info to clear the console window.
		/// </summary>
		/*
		 * The namespace has been changed from UnityEditorInternal to UnityEditor after Unity 2017.
		 * http://answers.unity3d.com/questions/707636/
		 */
		private static readonly MethodInfo _method = (
			Type.GetType("UnityEditor.LogEntries, UnityEditor").GetMethod("Clear", true, typeof(void)) ??
			Type.GetType("UnityEditorInternal.LogEntries, UnityEditor").GetMethod("Clear", true, typeof(void))
		);

		#endregion

	}

}
