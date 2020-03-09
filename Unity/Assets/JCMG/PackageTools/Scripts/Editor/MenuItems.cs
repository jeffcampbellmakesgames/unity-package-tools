using UnityEditor;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	internal static class MenuItems
	{
		[MenuItem("Tools/JCMG/PackageTools/Submit bug or feature request")]
		internal static void OpenURLToGitHubIssuesSection()
		{
			const string GITHUB_ISSUES_URL = "https://github.com/jeffcampbellmakesgames/unity-package-tools/issues";

			Application.OpenURL(GITHUB_ISSUES_URL);
		}

		[MenuItem("Tools/JCMG/PackageTools/Donate to support development")]
		internal static void OpenURLToKoFi()
		{
			const string KOFI_URL = "https://ko-fi.com/stampyturtle";

			Application.OpenURL(KOFI_URL);
		}

		[MenuItem("Tools/JCMG/PackageTools/About")]
		internal static void OpenAboutModalDialog()
		{
			AboutWindow.View();
		}
	}
}
