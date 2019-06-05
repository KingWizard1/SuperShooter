
/*WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW*\     (   (     ) )
|/                                                      \|       )  )   _((_
||  (c) Wanzyee Studio  < wanzyeestudio.blogspot.com >  ||      ( (    |_ _ |=n
|\                                                      /|   _____))   | !  ] U
\.ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ./  (_(__(S)   |___*/

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace WanzyeeStudio{
	
	/// <summary>
	/// Include some convenient methods to extend IO operation.
	/// </summary>
	public static class IoUtility{
		
		/// <summary>
		/// Filter to get the paths of all files and files in directories except "*.meta".
		/// </summary>
		/// <returns>The file paths.</returns>
		/// <param name="paths">Paths.</param>
		public static string[] GetDeepFiles(params string[] paths){

			if(null == paths) throw new ArgumentNullException("paths");
			var _option = SearchOption.AllDirectories;

			return paths.AsEnumerable(
				
				).Where((path) => !string.IsNullOrEmpty(path)
				).GroupBy((path) => Path.GetFullPath(path)
				
				).Select((grp) => grp.First()
				).SelectMany((path) => Directory.Exists(path) ? Directory.GetFiles(path, "*.*", _option) : new []{path}
				
				).Where((path) => File.Exists(path) && !path.EndsWith(".meta")
				).Select((file) => file.Replace('\\', '/')
				
			).Distinct().ToArray();

		}

		/// <summary>
		/// Try to delete a file or directory at the specified path.
		/// </summary>
		/// 
		/// <remarks>
		/// This doesn't work in Web Player.
		/// Note, the operation is permanently and irreversibly.
		/// Optional to trace and delete ancestor directories if became empty.
		/// </remarks>
		/// 
		/// <param name="path">Path.</param>
		/// <param name="ancestor">If set to <c>true</c> delete ancestor directories if empty.</param>
		/// 
		public static void Delete(string path, bool ancestor = false){

			#if UNITY_WEBPLAYER
			throw new NotSupportedException("'Delete' is not supported in Web Player.");
			#endif

			try{

				var _file = new FileInfo(path);
				if(_file.Exists) _file.Delete();

				var _folder = new DirectoryInfo(path);
				if(_folder.Exists && null != _folder.Parent) _folder.Delete(true);

				if(!ancestor) return;

				for(var _parent = _folder.Parent; null != _parent; _parent = _parent.Parent){
					if(_parent.Exists && null != _parent.Parent) _parent.Delete(false);
				}

			}catch{}

		}

		/// <summary>
		/// Determine if the path can be used to create a file or directory.
		/// </summary>
		/// 
		/// <remarks>
		/// Optional to throw an exception message or just return <c>false</c> if invalid.
		/// A legal path might not be in good format, e.g., "C:dir\ //file" or "/\pc\share\\new.txt".
		/// But it's safe to pass to <c>Directory</c> or <c>FileInfo</c> to create.
		/// Path in situations below is invalid, even dangerous:
		/// 	1. Nothing but empty or white-spaces, nowhere to go.
		/// 	2. Starts with 3 slashes, this causes crash while system looking for parent directories.
		/// 	3. Includes invalid chars, can't name a file.
		/// 	4. A name in path starts or ends with space, we can't get the created file, even delete.
		/// </remarks>
		/// 
		/// <returns><c>true</c> if is creatable; otherwise, <c>false</c>.</returns>
		/// <param name="path">Path.</param>
		/// <param name="exception">Flag to throw an exception or return <c>false</c>.</param>
		/// 
		public static bool CheckCreatable(string path, bool exception = false){

			var _error = GetCreatableError(path);

			if(null != _error && exception) throw new ArgumentException(_error, "path");
			return null == _error;

		}

		/// <summary>
		/// Get the error for <c>CheckCreatable()</c>, <c>null</c> if passed.
		/// </summary>
		/// <returns>The error.</returns>
		/// <param name="path">Path.</param>
		private static string GetCreatableError(string path){
			
			if(null == path || "" == path.Trim()) return "Path cannot be null, empty, or white-spaces only.";
			if(Regex.IsMatch(path, @"\A[\\/]{3}")) return "Path cannot start with 3 slashes.";

			var _chars = Regex.IsMatch(path, @"\A\w:") ? path[0] + path.Substring(2) : path;
			_chars = Regex.Replace(_chars, @"[\\/]", "");
			if(-1 != _chars.IndexOfAny(Path.GetInvalidFileNameChars())) return "Illegal characters in path.";

			if(!path.Split('\\', '/').Any((name) => Regex.IsMatch(name, @"(^\s+\S|\S\s+$)"))) return null;
			return "Directory or file name cannot start or end with white-space.";

		}

	}

}
