# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.0.4] / 2026-11-05
First release on nuget.org.
### Features
- One package for Revit 2019–2025: `net47` (Revit 2019 API), `net48` (Revit 2021 API) and `net8.0-windows` (Revit 2025 API).
- Package metadata: MIT license, README, icon, project and repository links.
- Symbol package (`.snupkg`) with portable PDBs.
- Publish to nuget.org from GitHub Actions with Trusted Publishing.
### Updated
- XML documentation for the whole public API.
- README with installation and usage examples.
- Build with `BIMPlugins.Nuke` 1.1.5; `net8.0-windows` is targeted as `net8.0-windows7.0` so the package builds with MSBuild.

[1.0.4]: https://github.com/Samveyil/BIMPlugins.UI/tree/v1.0.4
