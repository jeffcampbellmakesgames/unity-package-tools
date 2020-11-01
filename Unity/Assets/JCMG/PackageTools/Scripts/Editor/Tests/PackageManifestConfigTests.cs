/*
MIT License

Copyright (c) 2020 Jeff Campbell

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
using NUnit.Framework;
using UnityEngine;

namespace JCMG.PackageTools.Editor.Tests
{
	[TestFixture]
	internal class PackageManifestConfigTests
	{
		private PackageManifestConfig _packageManifest;

		[SetUp]
		public void SetUp()
		{
			_packageManifest = ScriptableObject.CreateInstance<PackageManifestConfig>();
			_packageManifest.packageName = "com.unity.package-4";
			_packageManifest.displayName = "Package Number 4";
			_packageManifest.packageVersion = "2.5.1";
			_packageManifest.unityVersion = "2018.1";
			_packageManifest.description =
				"This package provides X, Y, and Z. \n\nTo find out more, click the \"View Documentation\" link.";
			_packageManifest.category = "Controllers";
			_packageManifest.keywords = new[]
			{
				"key X",
				"key Y",
				"key Z"
			};
			_packageManifest.dependencies = new[]
			{
				new PackageManifestConfig.Dependency {packageName = "com.unity.package-1", packageVersion = "1.0.0"},
				new PackageManifestConfig.Dependency {packageName = "com.unity.package-2", packageVersion = "2.0.0"},
				new PackageManifestConfig.Dependency {packageName = "com.unity.package-3", packageVersion = "3.0.0"}
			};
		}

		private const string EXPECTED_JSON =
			"{\"name\":\"com.unity.package-4\",\"displayName\":\"Package Number 4\",\"version\":\"2.5.1\"," +
			"\"unity\":\"2018.1\",\"description\":\"This package provides X, Y, and Z. \n\nTo find out more," +
			" click the \"View Documentation\" link.\",\"keywords\":[\"key X\",\"key Y\",\"key Z\"]," +
			"\"category\":\"Controllers\",\"dependencies\":{\"com.unity.package-1\":\"1.0.0\",\"com.unity." +
			"package-2\":\"2.0.0\",\"com.unity.package-3\":\"3.0.0\"}}";

		[Test]
		public void AssertThatGeneratedJsonMatches()
		{
			var json = _packageManifest.GenerateJson();

			Assert.AreEqual(EXPECTED_JSON, json);
		}
	}
}
