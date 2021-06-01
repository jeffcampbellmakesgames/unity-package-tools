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

namespace JCMG.PackageTools.Editor
{
	/// <summary>
	/// Version info for this library.
	/// </summary>
	internal static class VersionConstants
	{
		/// <summary>
		/// The semantic version
		/// </summary>
		public const string VERSION = "1.5.1";

		/// <summary>
		/// The branch of GIT this package was published from.
		/// </summary>
		public const string GIT_BRANCH = "develop";

		/// <summary>
		/// The current GIT commit hash this package was published on.
		/// </summary>
		public const string GIT_COMMIT = "c816a46c484475246a4d27527a0a18ac5306c15f";

		/// <summary>
		/// The UTC human-readable date this package was published at.
		/// </summary>
		public const string PUBLISH_DATE = "Tuesday, June 1, 2021";

		/// <summary>
		/// The UTC time this package was published at.
		/// </summary>
		public const string PUBLISH_TIME = "06/01/2021 19:38:39";
	}
}