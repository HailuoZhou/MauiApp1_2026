<!--
Sync Impact Report

- Version change: 1.0.0 -> 1.1.0
- Change: added Code Principles, Security, Architecture, Documentation sections (minor addition)
- Modified principles:
  - I. UserFirst Mobile Experience (unchanged)
  - II. Testable & Observable (unchanged)
  - III. Secure & PrivacyRespecting (unchanged)
  - IV. Maintainable Architecture & Versioning (unchanged)
- Templates requiring updates:
  - .specify/templates/plan-template.md  review recommended
  - .specify/templates/spec-template.md  reviewed
  - .specify/templates/tasks-template.md  reviewed
  - .specify/templates/commands/*  pending (directory absent; referenced by templates)
- Follow-up TODOs:
  - TODO(RATIFICATION_DATE): Original ratification date unknown  user to provide or confirm.
  - Verify any plan gates that referenced removed offline-specific guidance.

Version bump rationale: Adding new governance sections (code, security,
architecture, documentation) is a compatible, non-breaking addition to
governance guidance; per constitution rules this is a MINOR change. Therefore
version moved from 1.0.0 to 1.1.0.

--
-- End Sync Impact Report
-->

# MauiApp1_2026 Constitution

## Core Principles

### I. UserFirst Mobile Experience
All user-facing features MUST prioritize clarity, responsiveness, and accessibility.
- UI elements MUST follow platform conventions (navigation, gestures) and behave
  predictably across device sizes and orientations.
- Accessibility requirements are NONNEGOTIABLE: controls MUST include semantic
  labels, proper focus order, and meet contrast guidelines. Any deviation MUST be
  documented and justified in the feature plan.
- Animations MUST be performant and not degrade responsiveness; long-running
  work MUST be offloaded to background tasks with appropriate indicators.

Rationale: Mobile users expect immediate, accessible interactions; enforcing
these rules reduces regressions and support burden.

### II. Testable & Observable
Every feature MUST be designed to be testable and observable.
- Business logic MUST have unit tests that can run in CI without device emulators.
- Critical user journeys (login, data mutation, sync) MUST have integration or
  UI tests and be included in CI gating for releases.
- Logging and telemetry MUST avoid PII; collection and retention policies MUST
  be documented and optout options provided when required by privacy rules.

Rationale: Tests and safe observability reduce regressions and make diagnosis
practical for mobile releases.

### III. Secure & PrivacyRespecting
Security and user privacy MUST be enforced by default.
- Secrets and credentials MUST NOT be stored in source control. Use platform
  secure storage APIs and environment/secret management for CI.
- Data at rest and in transit that contains sensitive information MUST be
  encrypted using platformrecommended mechanisms.
- PII usage MUST be minimized; when collected, purpose, retention, and consent
  MUST be declared in the spec and storage design.

Rationale: Mobile apps handle personal devices and sensitive data; explicit
rules protect users and reduce compliance risk.

### IV. Maintainable Architecture & Versioning
Code and releases MUST be organized for maintainability and safe evolution.
- Prefer clear separation of concerns (UI / ViewModel / Services / Data) and
  dependency injection to make features testable and modular.
- Follow semantic versioning for public APIs and document migration steps for
  breaking changes. MAJOR releases denote incompatible changes and REQUIRE a
  migration plan; MINOR adds features compatibly; PATCH fixes and clarifications.
- Pull requests MUST include a description of intent, tests, and a short
  compatibility statement when relevant.

Rationale: A consistent architecture and versioning policy reduce accidental
breakage and simplify future feature work.

## Constraints & Standards
This project targets .NET MAUI for crossplatform mobile UI. Standards to follow:
- Language & Runtime: C# targeting the project's configured .NET target(s).
- Architecture: MVVM (or platformappropriate equivalent) and dependency
  injection via the configured DI container.
- Storage: For upcoming features, do NOT rely on SQLite. Use the project's
  chosen persistence mechanism (for example: secure platform storage, file-based
  structured storage, or a remote sync provider). Any chosen persistence MUST be
  declared in the feature plan, and schema versioning and migration procedures
  MUST be defined where applicable.
- Code Style & Quality: Enforce formatting and linting in CI (e.g., dotnet format).

## Code Principles
- MUST follow MVVM pattern
- MUST use async/await (never .Result or .Wait())
- MUST handle exceptions gracefully
- MUST write unit tests for business logic

## Security
- MUST validate all user input
- MUST use secure storage for credentials
- MUST NOT log sensitive data

## Architecture
- MUST separate concerns (View/ViewModel/Model)
- MUST use dependency injection
- MUST NOT reference platform-specific code in shared logic

## Documentation
- MUST document public APIs
- MUST update specs when requirements change

## Development Workflow
- Branching: Use main for releases; feature branches for work. Keep PRs small
  and focused on a single intent.
- Reviews: Every PR MUST have at least one approving reviewer and MUST pass CI
  (build + tests) before merge.
- CI: Automated builds, unit tests, and configured integration tests MUST run
  on PRs; release pipelines SHOULD include signing and distribution steps.
- Releases: Document compatibility impact and include a migration guide when
  data or API changes are introduced.

## Governance
Amendments to this constitution MUST be proposed in a PR that includes:
- A plain summary of the change
- The governance version bump rationale (MAJOR/MINOR/PATCH)
- Migration steps or checklist for affected repositories and templates

Versioning policy:
- MAJOR: Backwardsincompatible governance or principle removals/renames.
- MINOR: New principle/section added or material expansion of guidance.
- PATCH: Clarifications, wording fixes, and nonsemantic refinements.

**Version**: 1.1.0 | **Ratified**: TODO(RATIFICATION_DATE): original adoption date required | **Last Amended**: 2025-11-29
