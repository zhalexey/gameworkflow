using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

	/// <summary>
	/// This tool helps to identify and remove empty folders from your Unity 3D project.
	///
	/// /// Why do I need this:
	/// Empty folders are not committed by git but the connected meta files are.
	/// So there will be a creation - deletion cycle between between person with and without such a folder.
	///
	/// /// Usage:
	/// The tool adds a new menu Tools->Empty Folder Tool.
	/// 1. If you "Toggle Auto Delete", every time you remove or move something in your project
	/// it will remove empty folders connected to the specific operation path. It will put the a Debug.Log for each removed folder.
	/// 2. "Show Empty Folder" will put a Debug.Log for each empty folder in your project.
	/// 3. "Delete Empty Folder" will delete all empty folders in your project and put a Debug.Log for each removed folder.
	///
	/// /// Acknowledgment:
	/// The basecode is partly from http://ideaplusplus.com/emptydirectoriesremover-cs/, https://gist.github.com/liortal53/780075ddb17f9306ae32
	/// </summary>

	[InitializeOnLoad]
	public class EmptyFolderTool : AssetPostprocessor
	{
		private static bool autoDelete;
		private const string MENU_NAME = "Tools/Empty Folder Tool/";
		private const string MENU_NAME_AUTO_DELETE = MENU_NAME + "Toggle Auto Delete";
		private const string ASSET_STRING = "Assets";

		/// Called on load thanks to the InitializeOnLoad attribute
		static EmptyFolderTool()
		{
			autoDelete = EditorPrefs.GetBool(MENU_NAME_AUTO_DELETE, false);

			/// Delaying until first editor tick so that the menu
			/// will be populated before setting check state, and
			/// re-apply correct action
			EditorApplication.delayCall += () =>
			{
				PerformAction(autoDelete);
			};
		}

		[MenuItem(MENU_NAME_AUTO_DELETE)]
		private static void ToggleAction()
		{
			/// Toggling action
			PerformAction(!autoDelete);
		}

		public static void PerformAction(bool enabled)
		{
			/// Set checkmark on menu item
			Menu.SetChecked(MENU_NAME_AUTO_DELETE, enabled);
			/// Saving editor state
			EditorPrefs.SetBool(MENU_NAME_AUTO_DELETE, enabled);

			autoDelete = enabled;
		}

		/// <summary>
		/// Remove Folders Automatically if auto delete is active
		/// </summary>
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			if (autoDelete)
			{
				DeleteEmptyDirectories(deletedAssets);
				DeleteEmptyDirectories(movedFromAssetPaths);
			}
		}

		private static void DeleteEmptyDirectories(string[] paths)
		{
			foreach (string path in paths)
				DeleteUpmostEmptyDirectory(path);
		}

		private static void DeleteUpmostEmptyDirectory(string assetPath)
		{
			try
			{
				string assetDir = Path.GetDirectoryName(assetPath);
				if (assetDir == ASSET_STRING)
					return;
				string absoluteDir = AssetPathToAbsolutePath(assetDir);
				string[] files = Directory.GetFiles(absoluteDir, "*.*", SearchOption.AllDirectories);
				if (files.Length == 0)
				{
					AssetDatabase.DeleteAsset(assetDir);
					Debug.Log("Deleting : " + assetDir);
					DeleteUpmostEmptyDirectory(Path.GetDirectoryName(assetPath));
				}
			}
			catch
			{
			}
		}

		private static string AssetPathToAbsolutePath(string assetPath)
		{
			if (assetPath == ASSET_STRING)
				return Application.dataPath;
			else
				return Path.Combine(Application.dataPath, assetPath.Substring(ASSET_STRING.Length + 1));
		}

		[MenuItem(MENU_NAME + "Show empty folders")]
		private static void ShowEmptyFoldersMenuItem()
		{
			RemoveEmptyFoldersFunc(true);
			Debug.Log("Show empty folders is done.");
		}

		[MenuItem("Tools/Empty Folder Tool/Remove empty folders")]
		private static void RemoveEmptyFoldersMenuItem()
		{
			RemoveEmptyFoldersFunc(false);
			Debug.Log("Remove empty folders is done.");
		}

		private static void RemoveEmptyFoldersFunc(bool dryRun)
		{
			var index = Application.dataPath.IndexOf("/Assets");
			var projectSubfolders = Directory.GetDirectories(Application.dataPath, "*", SearchOption.AllDirectories);

			// Create a list of all the empty subfolders under Assets.
			var emptyFolders = projectSubfolders.Where(path => IsEmptyRecursive(path)).ToArray();

			foreach (var folder in emptyFolders)
			{
				// Verify that the folder exists (may have been already removed).
				if (Directory.Exists(folder))
				{
					if (dryRun)
					{
						Debug.Log("Found Empty Folder : " + folder);
					}

					if (!dryRun)
					{
						Debug.Log("Deleting : " + folder);
						// Remove dir (recursively)
						Directory.Delete(folder, true);

						// Sync AssetDatabase with the delete operation.
						AssetDatabase.DeleteAsset(folder.Substring(index + 1));
					}
				}
			}

			// Refresh the asset database once we're done.
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// A helper method for determining if a folder is empty or not.
		/// </summary>
		private static bool IsEmptyRecursive(string path)
		{
			// A folder is empty if it (and all its subdirs) have no files (ignore .meta files)
			return Directory.GetFiles(path).Select(file => !file.EndsWith(".meta")).Count() == 0
				&& Directory.GetDirectories(path, string.Empty, SearchOption.AllDirectories).All(IsEmptyRecursive);
		}
	}