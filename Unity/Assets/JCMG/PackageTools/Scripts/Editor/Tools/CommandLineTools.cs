/*
MIT License

Copyright (c) Jeff Campbell

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
using System.Collections.Generic;

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Helper methods for command-line usage
	/// </summary>
	public static class CommandLineTools
	{
		// Command-Line Delimiters
		private const string ARGUMENT_DELIMITER_STR = "=";
		private const char ARGUMENT_DELIMITER_CHAR = '=';

		/// <summary>
		/// Returns a more easily-searchable <see cref="Dictionary{TKey,TValue}"/> of command-line arguments.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string, object> GetKVPCommandLineArguments()
		{
			var dict = new Dictionary<string, object>();
			var arguments = Environment.GetCommandLineArgs();
			foreach (var argument in arguments)
			{
				// If the commandline argument contains a value, parse that and add it
				if (argument.Contains(ARGUMENT_DELIMITER_STR))
				{
					var array = argument.Split(ARGUMENT_DELIMITER_CHAR);
					var key = array[0].ToLower();
					var value = array[1];

					dict.Add(key, value);
				}
				// Otherwise add the command line argument as a key without a value.
				else if (!dict.ContainsKey(argument))
				{
					dict.Add(argument, null);
				}
			}

			return dict;
		}
	}
}
