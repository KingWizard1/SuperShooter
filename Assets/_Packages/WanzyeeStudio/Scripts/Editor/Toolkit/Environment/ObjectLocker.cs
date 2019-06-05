
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix.Toolkit{
	
	/// <summary>
	/// Set <c>UnityEngine.Object</c> to be locked or editable.
	/// </summary>
	/// 
	/// <remarks>
	/// Set <c>UnityEngine.HideFlags</c> switch bit of <c>HideFlags.NotEditable</c> to avoid missing operation.
	/// Use context menu "Toggle Locked", or menu "Edit/Toggle Locked" with hotkey ALT-Shift-L for selections.
	/// </remarks>
	/// 
	public static class ObjectLocker{

		#region Menu

		/// <summary>
		/// Toggle the object locked or editable.
		/// </summary>
		/// <param name="command">Command.</param>
		[MenuItem("CONTEXT/Object/Toggle Locked", false, 10010)]
		private static void ObjectToggleLocked(MenuCommand command){
			SetLocked(!GetLocked(command.context), command.context);
		}

		/// <summary>
		/// Toggle the selected objects locked or editable to the opposite of the first one, with hotkey Alt-Shift-L.
		/// </summary>
		[MenuItem("Edit/Toggle Locked &#l", false, 200)]
		public static void EditToggleLocked(){

			if(!EditToggleLockedValid()) throw new InvalidOperationException("Nothing is selected.");

			SetLocked(!GetLocked(Selection.objects.First((obj) => null != obj)), Selection.objects);

		}

		/// <summary>
		/// Check if <c>EditToggleLocked()</c> valid, any <c>UnityEngine.Object</c> selected.
		/// </summary>
		/// <returns><c>true</c>, if valid.</returns>
		[MenuItem("Edit/Toggle Locked &#l", true)]
		private static bool EditToggleLockedValid(){
			return (null != Selection.activeObject);
		}

		#endregion


		#region Methods

		/// <summary>
		/// Determine if the object locked or editable.
		/// </summary>
		/// <returns><c>true</c>, if locked, <c>false</c> otherwise.</returns>
		/// <param name="target">Target.</param>
		public static bool GetLocked(Object target){

			if(null == target) throw new ArgumentNullException("obj");

			return HideFlags.NotEditable == (target.hideFlags & HideFlags.NotEditable);

		}

		/// <summary>
		/// Set the objects locked or editable.
		/// </summary>
		/// 
		/// <remarks>
		/// Only switch <c>HideFlags.NotEditable</c>, and keep other hideFlags bit.
		/// Note, to set <c>GameObjcet.hideFlags</c> will also set all components on it.
		/// </remarks>
		/// 
		/// <param name="locked">If set to <c>true</c> locked.</param>
		/// <param name="targets">Targets.</param>
		/*
		 * Use Undo.RegisterCompleteObjectUndo, since Undo.RecordObject doesn't work for HideFlags.
		 * Use Undo.RegisterFullObjectHierarchyUndo, since the above one doesn't work for Component on GameObject.
		 */
		public static void SetLocked(bool locked, params Object[] targets){
			
			if(null == targets) throw new ArgumentNullException("targets");
			var _targets = targets.Where((target) => null != target).Distinct();

			foreach(var _target in _targets){

				if(_target is GameObject) Undo.RegisterFullObjectHierarchyUndo(_target, "Set Locked");
				else Undo.RegisterCompleteObjectUndo(_target, "Set Locked");

				if(locked) _target.hideFlags |= HideFlags.NotEditable;
				else _target.hideFlags &= ~HideFlags.NotEditable;

				EditorUtility.SetDirty(_target);

			}

		}

		#endregion

	}

}
