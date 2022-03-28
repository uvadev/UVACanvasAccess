## UVACanvasAccess [![Build Status](https://github.com/uvadev/UVACanvasAccess/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://travis-ci.org/uvadev/UVACanvasAccess) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
A .NET wrapper for the Canvas LMS API.

### Installation
UVACanvasAccess can be installed from [GitHub Packages](https://github.com/uvadev/UVACanvasAccess/packages).
```bash
nuget sources Add -Name "GPR" \
     -Source "https://nuget.pkg.github.com/uvadev/index.json" \
     -UserName GH_USERNAME -Password GH_TOKEN
     
nuget install uvacanvasaccess
```
See [this](https://help.github.com/en/github/authenticating-to-github/creating-a-personal-access-token-for-the-command-line) to get a `GH_TOKEN`.

### Docs
Docs are generated and deployed automatically on push. They can be found here [here](https://uvadev.github.io/UVACanvasAccess/annotated.html). For quick reference, API endpoint functions can be found [here](https://uvadev.github.io/UVACanvasAccess/classUVACanvasAccess_1_1ApiParts_1_1Api.html).
