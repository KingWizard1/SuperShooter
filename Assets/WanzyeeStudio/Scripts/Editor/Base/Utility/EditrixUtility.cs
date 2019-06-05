
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Object = UnityEngine.Object;

namespace WanzyeeStudio.Editrix{
	
	/// <summary>
	/// Include some convenient methods for editor or asset operation.
	/// </summary>
	public static class EditrixUtility{
		
		#region Asset

		/// <summary>
		/// Get all main assets in the project folder.
		/// </summary>
		/// <returns>The all assets.</returns>
		/// <param name="progressBar">If set to <c>true</c> show progress bar while scanning.</param>
		public static Object[] GetAllAssets(bool progressBar = false){

			var _guids = AssetDatabase.FindAssets("");
			var _paths = _guids.Select((guid) => AssetDatabase.GUIDToAssetPath(guid)).OrderBy((path) => path).ToArray();

			var _title = "Scan Assets";
			var _assets = new List<Object>();

			for(int i = 0; i < _paths.Length; i++){
				if(progressBar) EditorUtility.DisplayProgressBar(_title, _paths[i], (float)i / (float)_paths.Length);
				_assets.Add(AssetDatabase.LoadMainAssetAtPath(_paths[i]));
			}

			if(progressBar) EditorUtility.ClearProgressBar();
			return _assets.Where((asset) => null != asset).Distinct().ToArray();

		}

		/// <summary>
		/// Get all asset labels used in project, or only find the ones used by assigned assets.
		/// </summary>
		/// <returns>The asset labels.</returns>
		/// <param name="assets">Assets.</param>
		public static string[] GetAllAssetLabels(params Object[] assets){

			if(null == assets || 0 == assets.Length) assets = GetAllAssets();

			var _assets = assets.Where((asset) => null != asset && AssetDatabase.Contains(asset));

			var _labels = _assets.SelectMany((asset) => AssetDatabase.GetLabels(asset));

			return _labels.Distinct().OrderBy((label) => label).ToArray();

		}

		/// <summary>
		/// Load all specified type assets with the search filter in the folders.
		/// </summary>
		/// <returns>The assets.</returns>
		/// <param name="filter">The filter string can contain search data.</param>
		/// <param name="searchInFolders">The folders where the search will start.</param>
		/// <typeparam name="T">The asset type.</typeparam>
		public static T[] LoadAssets<T>(string filter = null, params string[] searchInFolders) where T : Object{

			var _folders = (null != searchInFolders && searchInFolders.Any()) ? searchInFolders : new []{"Assets"};

			var _guids = AssetDatabase.FindAssets(filter + " t:" + typeof(T).Name, _folders);

			var _paths = _guids.Select((guid) => AssetDatabase.GUIDToAssetPath(guid));

			return _paths.Select((path) => AssetDatabase.LoadAssetAtPath<T>(path)).ToArray();

		}

		/// <summary>
		/// Common template method to open an asset.
		/// </summary>
		/// 
		/// <remarks>
		/// Basically for usage of <c>UnityEditor.Callbacks.OnOpenAssetAttribute</c>.
		/// </remarks>
		/// 
		/// <returns><c>true</c>, if handled the opening of the asset.</returns>
		/// <param name="instanceID">Instance ID.</param>
		/// <param name="handler">Callback to open the asset.</param>
		/// <typeparam name="T">Asset type.</typeparam>
		/// 
		public static bool OpenAsset<T>(int instanceID, Action<T> handler) where T : Object{

			var _asset = EditorUtility.InstanceIDToObject(instanceID) as T;
			if(null == _asset) return false;

			try{ handler.Invoke(_asset); }
			catch(Exception exception){ Debug.LogException(exception, _asset); }

			return true;

		}

		/// <summary>
		/// Get an order <c>string</c> of given object for sorting.
		/// </summary>
		/// 
		/// <remarks>
		/// It's asset path, append with sibling if relative to <c>UnityEngine.GameObject</c>.
		/// Optional to sort asset or hierarchy object first.
		/// </remarks>
		/// 
		/// <returns>The order.</returns>
		/// <param name="source">Source object.</param>
		/// <param name="assetFirst">If set to <c>true</c> asset first.</param>
		/// 
		public static string GetObjectOrder(Object source, bool assetFirst = true){

			var _path = AssetDatabase.GetAssetPath(source);

			if(!AssetDatabase.Contains(source)) _path = "h_" + _path;
			else _path = (assetFirst ? "a_" : "p_") + _path;

			var _object = (source is Component) ? (source as Component).gameObject : (source as GameObject);
			if(null == _object) return _path;

			var _sibling = "";
			for(var _transform = _object.transform; null != _transform; _transform = _transform.parent){
				_sibling = "." + _transform.GetSiblingIndex().ToString("D7") + _sibling;
			}

			if(source is Component) _sibling += "#" + Array.IndexOf(_object.GetComponents<Component>(), source);
			return _path + _sibling;

		}

		#endregion


		#region Other

		/// <summary>
		/// Determine if the path can be used to create a file or directory.
		/// </summary>
		/// 
		/// <remarks>
		/// Optional to throw an exception message or just return <c>false</c> if invalid.
		/// Check <c>IoUtility.CheckCreatable()</c> at the first.
		/// Then return <c>true</c> if the file doesn't exist yet or force to <c>overwrite</c>.
		/// Otherwise popup a dialog for the user to make the decision.
		/// </remarks>
		/// 
		/// <returns><c>true</c> if is creatable; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		/// <param name="overwrite">Overwrite.</param>
		/// <param name="exception">Flag to throw an exception or return <c>false</c>.</param>
		/// 
		public static bool CheckIoCreatable(string path, bool overwrite = false, bool exception = false){

			if(!IoUtility.CheckCreatable(path, exception)) return false;
			if(overwrite || !File.Exists(path)) return true;

			var _message = Path.GetFileName(path) + " already exists.\nDo you want to replace it?";
			if(EditorUtility.DisplayDialog("Confirm Save As", _message, "Yes", "No")) return true;

			if(exception) throw new OperationCanceledException(path + " already exists.");
			else return false;

		}

		/// <summary>
		/// Get types appropriate to expose in the Inspector to select members, optional to include editor types.
		/// </summary>
		/// 
		/// <remarks>
		/// Include types from Unity and assemblies in the project folder.
		/// And what excluded is non-public, <c>interface</c>, <c>enum</c>, array or generic types.
		/// </remarks>
		/// 
		/// <returns>The types.</returns>
		/// <param name="editor">If set to <c>true</c> include editor types.</param>
		/// 
		public static IEnumerable<Type> GetExposingTypes(bool editor = false){

			var _types = GetCustomTypes(editor).Concat(GetUnityTypes(editor));

			_types = _types.Where((type) => type.IsPublic || type.IsNestedPublic);
			_types = _types.Where((type) => !type.IsInterface && !type.IsEnum && !type.IsArray && !type.IsGenericType);

			return _types.Distinct();

		}

		/// <summary>
		/// Get all custom types, optional to include editor types.
		/// </summary>
		/// <returns>The types.</returns>
		/// <param name="editor">If set to <c>true</c> include editor types.</param>
		private static IEnumerable<Type> GetCustomTypes(bool editor){

			var _folder = Directory.GetCurrentDirectory();

			var _dlls = AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();
			_dlls = _dlls.Where((dll) => dll.Location.StartsWith(_folder));

			var _regex = new Regex(@"\Weditor\W", RegexOptions.IgnoreCase);
			if(!editor) _dlls = _dlls.Where((dll) => !_regex.IsMatch(dll.Location, _folder.Length));

			return _dlls.SelectMany((dll) => dll.GetTypes());

		}

		/// <summary>
		/// Get all Unity types, optional to include editor types.
		/// </summary>
		/// <returns>The types.</returns>
		/// <param name="editor">If set to <c>true</c> include editor types.</param>
		private static IEnumerable<Type> GetUnityTypes(bool editor){

			var _dlls = AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();
			_dlls = _dlls.Where((dll) => dll.GetName().Name.StartsWith("Unity"));

			var _types = _dlls.SelectMany((dll) => dll.GetTypes());
			var _regex = new Regex(editor ? @"^(UnityEngine|UnityEditor)\." : @"^UnityEngine\.");

			return _types.Where((type) => _regex.IsMatch(type.FullName)).ToArray();

		}

		#endregion


		#region Patch

		/// <summary>
		/// Callback of play mode state changed event, for compatibility before Unity 2017.2.
		/// </summary>
		public static event EditorApplication.CallbackFunction playmodeChanged = () => {};

		/// <summary>
		/// Callback of project changed event, for compatibility before Unity 2018.1.
		/// </summary>
		public static event Action projectChanged = () => {};

		/// <summary>
		/// Callback of hierarchy changed event, for compatibility before Unity 2018.1.
		/// </summary>
		public static event Action hierarchyChanged = () => {};

		/// <summary>
		/// Register compatible callbacks.
		/// </summary>
		/*
		 * Used for compatibility until the APIs totally deprecated or the version becomes common.
		 */
		[InitializeOnLoadMethod]
		private static void RegisterCompatibleCallbacks(){

			#if UNITY_2017_2_OR_NEWER
			EditorApplication.playModeStateChanged += (playState) => playmodeChanged.Invoke();
			EditorApplication.pauseStateChanged += (pauseState) => playmodeChanged.Invoke();
			#else
			EditorApplication.playmodeStateChanged += playmodeChanged;
			#endif

			#if UNITY_2018_1_OR_NEWER
			EditorApplication.projectChanged += projectChanged;
			EditorApplication.hierarchyChanged += hierarchyChanged;
			#else
			EditorApplication.hierarchyWindowChanged += () => hierarchyChanged.Invoke();
			EditorApplication.projectWindowChanged += () => projectChanged.Invoke();
			#endif

		}

		#endregion

	}

}
