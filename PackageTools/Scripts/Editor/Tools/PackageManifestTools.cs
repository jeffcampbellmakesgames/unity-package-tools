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
using System.Text;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for the Package Manifest Tools
	/// </summary>
	internal static class PackageManifestTools
	{
		private static readonly StringBuilder _jsonStringBuilder
			= new StringBuilder(8192);

		// Package Json Properties
		private const string OpenBraces = "{";
		private const string OpenBracket = "[";
		private const string ClosedBraces = "}";
		private const string ClosedBracket = "]";
		private const string Comma = ",";

		private const string Name = @"""name"":""{0}""";
		private const string DisplayName = @"""displayName"":""{0}""";
		private const string PackageVersion = @"""version"":""{0}""";
		private const string UnityVersion = @"""unity"":""{0}""";
		private const string Description = @"""description"":""{0}""";
		private const string Keywords = @"""keywords"":";
		private const string KeywordChild = @"""{0}""";
		private const string Dependencies = @"""dependencies"":";
		private const string DependencyChildFormat = @"""{0}"":""{1}""";
		private const string Category = @"""category"":""{0}""";

		/// <summary>
		/// Returns a Json <see cref="string"/> representation of the <see cref="PackageManifestConfig"/>
		/// <paramref name="packageManifest"/>.
		/// </summary>
		/// <param name="packageManifest"></param>
		/// <returns></returns>
		public static string GenerateJson(PackageManifestConfig packageManifest)
		{
			// Clear string builder
			_jsonStringBuilder.Length = 0;

			_jsonStringBuilder.Append(OpenBraces);
			_jsonStringBuilder.AppendFormat(Name, packageManifest.packageName);
			_jsonStringBuilder.Append(Comma);
			_jsonStringBuilder.AppendFormat(DisplayName, packageManifest.displayName);
			_jsonStringBuilder.Append(Comma);
			_jsonStringBuilder.AppendFormat(PackageVersion, packageManifest.packageVersion);
			_jsonStringBuilder.Append(Comma);
			_jsonStringBuilder.AppendFormat(UnityVersion, packageManifest.unityVersion);
			_jsonStringBuilder.Append(Comma);
			_jsonStringBuilder.AppendFormat(Description, packageManifest.description);
			_jsonStringBuilder.Append(Comma);

			// Add the keywords if any exist.
			if (packageManifest.keywords != null &&
			    packageManifest.keywords.Length > 0)
			{
				_jsonStringBuilder.Append(Keywords);
				_jsonStringBuilder.Append(OpenBracket);

				for (var i = 0; i < packageManifest.keywords.Length; i++)
				{
					var keyword = packageManifest.keywords[i];

					_jsonStringBuilder.AppendFormat(KeywordChild, keyword);

					if (i != packageManifest.keywords.Length - 1)
					{
						_jsonStringBuilder.Append(Comma);
					}
				}

				_jsonStringBuilder.Append(ClosedBracket);
				_jsonStringBuilder.Append(Comma);
			}

			_jsonStringBuilder.AppendFormat(Category, packageManifest.category);

			// Add the dependencies block if any exist.
			if (packageManifest.dependencies != null &&
			    packageManifest.dependencies.Length > 0)
			{
				_jsonStringBuilder.Append(Comma);
				_jsonStringBuilder.Append(Dependencies);
				_jsonStringBuilder.Append(OpenBraces);

				for (var i = 0; i < packageManifest.dependencies.Length; i++)
				{
					var dependency = packageManifest.dependencies[i];
					if (string.IsNullOrEmpty(dependency.packageName) ||
					    string.IsNullOrEmpty(dependency.packageVersion))
					{
						continue;
					}

					_jsonStringBuilder.AppendFormat(
						DependencyChildFormat,
						dependency.packageName,
						dependency.packageVersion);

					if (i != packageManifest.dependencies.Length - 1)
					{
						_jsonStringBuilder.Append(Comma);
					}
				}

				_jsonStringBuilder.Append(ClosedBraces);
			}

			_jsonStringBuilder.Append(ClosedBraces);

			return _jsonStringBuilder.ToString();
		}
	}
}
