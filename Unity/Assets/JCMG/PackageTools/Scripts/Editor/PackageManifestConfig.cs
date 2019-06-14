/*
MIT License

Copyright (c) 2019 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// <see cref="PackageManifestConfig"/> is an asset config representing a Unity Package. It allows for
	/// defining the data used on the package json file, the locations of source files/assets that make up the
	/// package, and a destination for th package source to be distributed at.
	/// </summary>
	[CreateAssetMenu(fileName = "PackageManifestConfig", menuName = "JCMG/PackageTools/PackageManifestConfig")]
	public sealed class PackageManifestConfig : ScriptableObject
	{
		/// <summary>
		/// Describes a dependency that this package requires.
		/// </summary>
		[Serializable]
		public sealed class Dependency
		{
			/// <summary>
			/// The name of the dependent package.
			/// </summary>
			public string packageName;

			/// <summary>
			/// The semantic version of the dependent package in MAJOR.MINOR.PATCH format.
			/// </summary>
			public string packageVersion;
		}

		/// <summary>
		/// A unique id for this <see cref="PackageManifestConfig"/> instance.
		/// </summary>
		public string Id
		{
			get { return _id; }
		}

		/// <summary>
		/// A collection of paths to folders and files for the source for the package.
		/// </summary>
		public string[] packageSourcePaths;

		/// <summary>
		/// A collection of file/folder paths to exclude from the package.
		/// </summary>
		public string[] packageIgnorePaths;

		/// <summary>
		/// A path to the package source distribution contents.
		/// </summary>
		public string packageDestinationPath;

		/// <summary>
		/// The relative path to the folder where the legacy package will be exported to.
		/// </summary>
		public string legacyPackageDestinationPath;

		/// <summary>
		/// The fully-qualified package name.
		/// </summary>
		public string packageName;

		/// <summary>
		/// The package name as it appears in the Package Manager window.
		/// </summary>
		public string displayName;

		/// <summary>
		/// The semantic version of the package in MAJOR.MINOR.PATCH format.
		/// </summary>
		public string packageVersion;

		/// <summary>
		/// The version of Unity in semantic version format like 2018.1.
		/// </summary>
		public string unityVersion;

		/// <summary>
		/// A description of the package.
		/// </summary>
		public string description;

		/// <summary>
		/// The category the package belongs in.
		/// </summary>
		public string category;

		/// <summary>
		/// A collection of keywords that describe the package.
		/// </summary>
		public string[] keywords;

		/// <summary>
		/// A collection of packages that this package depends on.
		/// </summary>
		public Dependency[] dependencies;

		[SerializeField]
		private string _id;

		/// <summary>
		/// Returns a Json <see cref="string"/> representation.
		/// </summary>
		/// <returns></returns>
		public string GenerateJson()
		{
			return PackageManifestTools.GenerateJson(this);
		}

		private void Reset()
		{
			packageVersion = EditorConstants.DefaultPackageVersion;
			unityVersion = EditorConstants.DefaultUnityVersion;
			_id = Guid.NewGuid().ToString();
		}
	}
}
