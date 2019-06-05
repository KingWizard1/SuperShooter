
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;

namespace WanzyeeStudio.Editrix.Extension{
	
	/// <summary>
	/// Extension methods for <c>UnityEditor.GenericMenu</c>.
	/// </summary>
	public static class MenuExtension{

		/// <summary>
		/// Add an item to the menu, it will be disabled if the function isn't assigned.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="content">Content.</param>
		/// <param name="func">The function to call when the menu item is selected.</param>
		/// <param name="param">The parameter to pass to the function.</param>
		/// <param name="check">Whether to show the item is currently activated.</param>
		/// <param name="enable">If set to <c>false</c> to enforce disabled.</param>
		/// <typeparam name="T">The parameter type.</typeparam>
		/*
		 * The reason of this extension method below:
		 * 		1. Use string instead of GUIContent which makes the code longer without other usage.
		 * 		2. Support strong type function and parameter.
		 * 		3. Reduce the "if" block to call AddItem() and AddDisabledItem().
		 */
		public static void AddItem<T>(
			this GenericMenu menu,
			string content,
			Action<T> func,
			T param,
			bool check = false,
			bool enable = true
		){
			menu.AddItem(content, () => func.Invoke(param), check, (null != func) && enable);
		}

		/// <summary>
		/// Add an item to the menu, it will be disabled if the function isn't assigned.
		/// </summary>
		/// <param name="menu">Menu.</param>
		/// <param name="content">Content.</param>
		/// <param name="func">The function to call when the menu item is selected.</param>
		/// <param name="check">Whether to show the item is currently activated.</param>
		/// <param name="enable">If set to <c>false</c> to enforce disabled.</param>
		public static void AddItem(
			this GenericMenu menu,
			string content,
			Action func = null,
			bool check = false,
			bool enable = true
		){
			
			if(null == menu) throw new ArgumentNullException("menu");
			if(string.IsNullOrEmpty(content)) throw new ArgumentNullException("content");
			 
			if(enable && null != func) menu.AddItem(new GUIContent(content), check, () => func.Invoke());
			else menu.AddDisabledItem(new GUIContent(content));

		}

	}

}
