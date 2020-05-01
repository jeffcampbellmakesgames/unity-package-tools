# <img src="./Documentation/PackageManifestConfigIcon.png" alt="" width="35" height="35"/> Usage

## Overview
JCMG Package Tools is a library whose goal is ultimately aimed at making it easier for Unity tool, plugin, library, etc... developers at making it easier to create and update their content as a package usable in the native Unity package manager. To that end, JCMG Package Tools itself can be imported as a package (see the main readme for more information on this).

## Setting up your first package
Creating your first package is a simple process that can be made more complex because information available about Unity Package manager is inconsistent right now. This is largely due to it not being widely and officially supported for end-users of Unity yet, but starting with 2019.1 will gain more official support.

The first step is to create a `PackageManifestConfig` asset that we can use to define the properties and contents of our package. This can be done by right-clicking in the `Project` window and selecting "Create->JCMG->Package Tools->Package Manifest Config". This will create an asset named `PackageManifestConfig` in that folder.

![Package Manifest Config Inspector](./Documentation/Inspector.png)

A `PackageManifestConfig` is broken up into two sections:
* **Package Json:** This section contains fields relating to properties of the `package.json` file that we will need to have in our package root folder. It describes the minimum Unity version that it is compatible with and the version of the package itself (both in SemVer format of MAJOR.MINOR.PATCH), a description of the package, and any dependencies it has on other packages. This last part I have the least information about, but is supposed to gain more widespread support in 2019.1.
* **Package Content and Export:** This section focuses on defining the folders/files that make up the content (including code and assets) that make up the package itself and a location that the content will be published to when clicking on the button labeled `Export Package Source`. 

For convenience sake, I have added file and folder pickers as buttons to the right of these fields that make it easier to add relative file/folder paths from the Assets folder for package source and export location.

There are also options for excluding specific files and/or folders from the package source that might otherwise be included. This can be useful for example, if there are unit tests or example content located within your package source path that you would not want to include directly or indirectly. Folders and files can be excluded by adding them to the **ExcludePaths** list.

Recently I have found myself manually creating a `VersionConstants` static class containing versioning information about the package itself. This can be useful for displaying to a user or checking for updates outside of the native Unity PackageManager window.. Since updating this file by hand can be error-prone or easy to forget, this can now be configured on a `PackageManifestConfig` and generated using a new action `Generate VersionConstants.cs` on the config's inspector.

## Publishing Strategy

The location that you export your package source to is largely based on how you want to publish it for yourself or end-users. Packages can be referenced a variety of ways as either a local file or git URL; the link [here](https://forum.unity.com/threads/git-support-on-package-manager.573673/) contains some of the most up-to-date information I have found so far for publishing your own packages.

For example, my distribution strategy for packages of my open source tools and libraries has been to create orphaned branches on their git repository that contains only the package contents itself (no development tools, Unity projects, etc...). This allows me to separate how the tools are developed with how they are published and opens up a great deal of flexibility in how/when releases are made available via separate branches (for a stable, alpha, nightly, etc... versions of a package).

If using this type of approach, it is helpful to clone a second copy of the repository on the same computer you are developing on where it only pulls down the release branches. From there, you can create an individual `PackageManifestConfig` asset per release branch that can include the second clones repo folder as the publish location.



