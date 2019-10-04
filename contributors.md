# <img src="./Documentation/PackageManifestConfigIcon.png" alt="" width="35" height="35"/> Contributing
Thanks for considering contributing to JCMG Package Tools! Read the guidelines below before you submit an issue or create a PR. 

## Project structure
The project structure is split between several branches
* **master:** This is the stable branch and all releases/packages are generated from this. 
* **develop:** This is the primary development branch which all PRs should be made to and is generally considered less-stable. This is occasionally merged into **master** and a new release tag/package is generated from this.
* **releases/stable:** This branch is orphaned and contains only the package contents for JCMG.PackageTools. This is updated in sync with tagged releases on **master** and each commit that changes these contents should result in the version in **package.json** being changed.

This structure allows for ease of development and quick testing via **master** or **develop**, but clear isolation and separation between the package distribution via **releases/stable**.

## Style Guide

### Language Usage
* Mark closed types as sealed to enable proper devirtualization (see [here](https://blogs.unity3d.com/2016/07/26/il2cpp-optimizations-devirtualization/) for more info).
* Avoid LINQ usage for runtime usage except where absolutely possible (`ToList` or `ToArray` for example) and avoid using `ForEach`. Using these methods creates easily avoidable garbage (in newer versions of Unity >= 5.6 this is situational to the Collection or if its being used via an interface, but easy to avoid by default avoidance). Editor usage is another story as performance is not as generally important.

### Layout
There is an `.editorconfig` at the root of the repository located [here](/.editorconfig) that can be used by most IDEs to help ensure these settings are automatically applied.
* **Indentation:** 1 tab = 4 spaces (tab character)
* **Desired width:** 120-130 characters max per line
* **Line Endings:** Unix (LF), with a new-line at the end of each file.
* **White Space:** Trim empty whitespace from the ends of lines.

### Naming and Formatting
| Object Name | Notation | Example |
| ----------- | -------- | ------- |
| Namespaces | PascalCase | `JCMG.PackageTools.Editor` |
| Classes | PascalCase | `SemVersion` |
| Methods | PascalCase | `ParseVersion` |
| Method arguments | camelCase | `oldValue` |
| Properties | PascalCase | `Value` |
| Public fields | camelCase | `value` |
| Private fields | _camelCase | `_value` |
| Constants | PascalCase | `DEFAULT_VERSION` |
| Inline variables | camelCase | `value` |

### Structure
* Follow good encapsulation principles and try to limit exposing fields directly as public; unless necessary everything should be marked as private/protected unless necessary. Where public access to a field is needed, use a public property instead.
* Always order access from most-accessible to least (i.e, `public` to `private`).
* Where classes or methods are not intended for use by a user, mark these as `internal`.
* Order class structure like so:
    * Namespace
        * Internal classes
        * Properties
        * Fields
        * Events
        * Unity Methods
        * Primary Methods
        * Helper Methods
* Lines of code that are generally longer than 120-130 characters should generally be broken out into multiple lines. For example, instead of:

`public bool SomeMethodWithManyParams(int param1, float param2, List<int> param3, out int param4, out int param5)...`

do

```
public bool SomeMethodWithManyParams(
     int param1,
     float param2,
     List<int> param3,
     out int param4,
     out int param5)...
 ```

### Example Formatting
```
using System;
using UnityEngine;

namespace Example
{
    public class Foo : MonoBehavior
    {
        public int SomeValue { get { return _someValue; } }

        [SerializeField]
        private int _someValue;

        private const string WARNING = "Somethings wrong!";

        private void Update()
        {
            // Do work
            Debug.Log(Warning);
        }
    }
}
```

## Pull requests
Pull requests should be made to the [develop branch](https://github.com/jeffcampbellmakesgames/unity-package-tools/tree/develop).