# Implementation Plan: Constitution Update

**Branch**: `[con/constitution-update]` | **Date**: 2025-11-29 | **Spec**: /specs/constitution-update/spec.md
**Input**: Update project constitution to reflect code principles, security, architecture, and documentation guidance; bump governance version to 1.1.0.

## Summary

Formalize the project's governance by adding short, actionable rules for code, security, architecture, and documentation. Ensure templates and plan gates align with the revised constitution.

## Technical Context

**Language/Version**: C# / .NET MAUI
**Target Platform**: iOS, Android, Windows, MacCatalyst (via MAUI)
**Testing**: Unit tests via `dotnet test`; CI to validate builds and tests
**Persistence**: Project-level chosen mechanism (declared per feature)

## Constitution Check

- Accessibility: UI flows include accessibility labels and contrast checks where applicable.
- Storage & Migrations: Feature plans MUST declare persistence mechanism and migration strategy when required.
- Testing & CI: Unit tests for business logic; CI runs unit/integration tests for PR gating.
- Security: Input validation, secure storage for credentials, no sensitive logs.
- Documentation: Public APIs documented; feature specs updated on requirement changes.

## Project Structure / Files to Update

- `.specify/memory/constitution.md` (done)
- `.specify/templates/plan-template.md` (review for offline-related gates)
- `.specify/templates/spec-template.md` (verify references)
- Any feature specs that reference the removed offline-first principle

## Tasks

1. Review `plan-template.md` and remove or rephrase offline-first gates if inconsistent. (P1)
2. Search repository for references to the removed principle and update text. (P1)
3. Confirm persistence approach for upcoming features; add guidance to feature plan template if needed. (P2)
4. Add a small checklist to PR template reminding authors to: include tests, document API changes, and add migration notes when relevant. (P2)
5. Publish constitution v1.1.0 in repo and open PR with Sync Impact Report in description. (P1)

## Acceptance Criteria

- `constitution.md` updated and present in `.specify/memory/` with version 1.1.0
- `plan-template.md` reviewed and no leftover offline-first assumptions remain
- CI builds pass; PR includes tests for any modified code

## Risks & Mitigations

- Risk: Some templates reference removed principle; Mitigation: run repository-wide search and update matches.
- Risk: Future features assume SQLite; Mitigation: call out migration plan requirement and declare persistence per-feature.

## Next Steps

- Review this plan and suggest edits.
- If approved, I can open a branch and create a PR with the constitution change and related template updates.

