// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyFilesPostProcessBuild.cs" company="Supyrb">
//   Copyright (c)  Supyrb. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Supyrb
{
	/// <summary>
	/// Copies files and folder to the data folder of a build
	/// </summary>
	public class CopyFilesPostProcessBuild : ScriptableObject
	{
		private static string fileName = "CopyFilesPostProcessBuild.asset";
		
		[SerializeField]
		private bool active = true;

		[SerializeField]
		private string buildBasePath = "StreamingAssets";

		[SerializeField]
		#if ODIN_INSPECTOR
		[FilePath(AbsolutePath = false)]
		#endif
		private List<string> assetFilesToCopy = null;

		[SerializeField]
		[Tooltip("Folders that should be copied, relative path. e.g. 'Assets/_Project/Meshes'")]
		#if ODIN_INSPECTOR
		[FolderPath(AbsolutePath = false)]
		#endif
		private List<string> assetFoldersToCopy = null;

		[SerializeField]
		[Tooltip("File extensions that are ignored when traversing folders. Extensions start with a point, e.g. '.meta'")]
		private List<string> ignoredExtensions = new List<string>() {".meta"};

		[SerializeField]
		private bool maintainFolderHierarchy = true;

		[SerializeField]
		private List<BuildTarget> targets = new List<BuildTarget>() {
				BuildTarget.StandaloneWindows64, 
				BuildTarget.StandaloneWindows,
				BuildTarget.StandaloneOSX, 
				BuildTarget.StandaloneLinux,
				BuildTarget.StandaloneLinux64,
				BuildTarget.StandaloneLinuxUniversal
		};
		
		public static CopyFilesPostProcessBuild Instance { get; private set; }
		
		[PostProcessBuild(100)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
		{
			if (Instance == null)
			{
				Debug.Log("Searching for CopyFilesPostProcessBuild.");
				Instance = EditorGUIUtility.Load(fileName) as CopyFilesPostProcessBuild;
			}

			if (Instance == null)
			{
				CreateInstance();
				// Will be skipped, since there are no settings by now
				return;
			}

			if (!Instance.active)
			{
				Debug.LogWarning("Skipping CopyFilesPostProcessBuild since it is not active.");
				return;
			}

			if (!Instance.targets.Contains(target))
			{
				Debug.LogWarning("Skipping CopyFilesPostProcessBuild, build target not in target list");
				return;
			}

			var dataPath = pathToBuiltProject.Substring(0, pathToBuiltProject.Length - ".exe".Length) + "_Data";
			Instance.CopyData(dataPath);
		}

		#if ODIN_INSPECTOR
		[Button]
		#endif
		private void CopyData(string builtProjectDataPath)
		{
			var builtProjectFolderPath = Path.Combine(builtProjectDataPath, buildBasePath);
			Debug.Log("Starting copy process to folder " + builtProjectFolderPath);
			
			for (int i = 0; i < assetFilesToCopy.Count; i++)
			{
				var assetFilePath = assetFilesToCopy[i];
				CopyFile(builtProjectFolderPath, assetFilePath, maintainFolderHierarchy);
			}

			for (int i = 0; i < assetFoldersToCopy.Count; i++)
			{
				var assetFolderPath = assetFoldersToCopy[i];
				CopyFolder(builtProjectFolderPath, assetFolderPath, ignoredExtensions, maintainFolderHierarchy);
			}
		}

		private static void CreateInstance()
		{
			Instance = ScriptableObject.CreateInstance<CopyFilesPostProcessBuild>();
			var assetRelativeFolderPath = "Assets/Editor Default Resources/";
			// Create folders if not set
			Directory.CreateDirectory(Path.GetFullPath(assetRelativeFolderPath));
			var assetRelativeFilePath = Path.Combine(assetRelativeFolderPath, fileName);
			AssetDatabase.CreateAsset(Instance, assetRelativeFilePath);
		}
		
		private static void CopyFile(string builtProjectFolderPath, string assetPath, bool maintainFolderHierarchy)
		{
			var absoluteAssetPath = Path.GetFullPath(assetPath);
			bool fileExists = File.Exists(absoluteAssetPath);

			if (!fileExists)
			{
				Debug.Log("File " + absoluteAssetPath + " doesn't exist.");
				return;
			}

			var destFilePath = Path.Combine(builtProjectFolderPath, maintainFolderHierarchy ? assetPath : Path.GetFileName(assetPath));
			var destFolder = destFilePath.Substring(0, destFilePath.Length - Path.GetFileName(destFilePath).Length);
			Directory.CreateDirectory(destFolder);
			File.Copy(absoluteAssetPath, destFilePath, true);
			Debug.Log("File copied: " + destFilePath);
		}
		
		private static void CopyFolder(string builtProjectFolderPath, string assetPath, List<string> ignoredExtensions, bool maintainFolderHierarchy)
		{
			var absoluteAssetPath = Path.GetFullPath(assetPath);
			bool folderExists = Directory.Exists(absoluteAssetPath);

			if (!folderExists)
			{
				Debug.Log("Folder " + absoluteAssetPath + " doesn't exist.");
				return;
			}

			var destFolderPath = Path.Combine(builtProjectFolderPath, maintainFolderHierarchy ? assetPath : Path.GetDirectoryName(assetPath));
			Directory.CreateDirectory(destFolderPath);
			CopyDirectory(absoluteAssetPath, destFolderPath, ignoredExtensions,true);
			Debug.Log("Folder copied: " + destFolderPath);
		}

		// From https://stackoverflow.com/a/3822913/3319358
		private static void CopyDirectory(string sourcePath, string destinationPath, List<string> ignoredExtensions, bool overwrite)
		{
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", 
				SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));
			}

			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", 
				SearchOption.AllDirectories))
			{
				var extension = Path.GetExtension(newPath);
				if (ignoredExtensions.Contains(extension))
				{
					continue;
				}
				File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), overwrite);
			}
		}
	}
}