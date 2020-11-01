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
