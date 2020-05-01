using System.Diagnostics;
using UnityEngine;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for retrieving git information
	/// </summary>
	public static class GitTools
	{
		private const string GIT_APPLICATION = "git";

		public static string Run(string cmd)
		{
			// NOTE: This currently expects that you have git included in your PATH.
			var p = new Process();
			p.StartInfo.FileName = GIT_APPLICATION;
			p.StartInfo.Arguments = cmd;
			p.StartInfo.WorkingDirectory = Application.dataPath;
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;
			p.Start();

			var output = p.StandardOutput.ReadToEnd().Trim();

			p.WaitForExit();

			return output;
		}

		public static string GetBranch()
		{
			return Run("rev-parse --abbrev-ref HEAD");
		}

		public static string GetLongHeadHash()
		{
			return Run("rev-parse HEAD");
		}
	}
}
