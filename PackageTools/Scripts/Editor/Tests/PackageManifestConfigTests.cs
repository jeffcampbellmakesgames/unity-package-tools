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

		private const string ExpectedJson =
			"{\"name\":\"com.unity.package-4\",\"displayName\":\"Package Number 4\",\"version\":\"2.5.1\"," +
			"\"unity\":\"2018.1\",\"description\":\"This package provides X, Y, and Z. \n\nTo find out more," +
			" click the \"View Documentation\" link.\",\"keywords\":[\"key X\",\"key Y\",\"key Z\"]," +
			"\"category\":\"Controllers\",\"dependencies\":{\"com.unity.package-1\":\"1.0.0\",\"com.unity." +
			"package-2\":\"2.0.0\",\"com.unity.package-3\":\"3.0.0\"}}";

		[Test]
		public void AssertThatGeneratedJsonMatches()
		{
			var json = _packageManifest.GenerateJson();

			Assert.AreEqual(ExpectedJson, json);
		}
	}
}
