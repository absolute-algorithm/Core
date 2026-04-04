# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2026-03-28
### Added
- Initial release of **AbsoluteAlgorithm.Core**.
- Core utilities for API versioning, authentication, resilience, and storage.
- Support for Blazor, Unity, Avalonia, and ASP.NET Core frontends.
- Serialization utilities for JSON, XML, and compression.
- Security utilities for hashing, encryption, and privacy.
- Sanitization tools for file names and spreadsheet formulas.

## [1.0.1] - 2026-04-04
### Changed
- Updated `ApplicationConfiguration` to implement the Singleton design pattern.
- Added `ApplicationConfigurationBuilder` to configure and return the Singleton instance of `ApplicationConfiguration`.
- Updated `ApplicationConfigurationValidator` to replace early return statements.
- Improved validation logic for storage policies to ensure all required fields are checked and errors are reported appropriately.
