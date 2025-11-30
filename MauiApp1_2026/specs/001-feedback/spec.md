<!--
Spec: Feedback Submission (title + content)
Path: specs/001-feedback/spec.md
Created: 2025-11-29
-->
# Feedback Submission

**Summary**

Allow users to submit short feedback items consisting of a required title and optional content. Feedback is used for product improvement and support triage. The spec covers the UI flow, validation, storage contract, privacy constraints, and acceptance tests.

**Actors**
- User: an app user (authenticated or unauthenticated) who submits feedback.
- Support/Engineering: consumes feedback for triage.

**Scope**
- In-scope: UI for creating feedback (title + content), client-side validation, transient confirmation to user, persistence to the declared storage mechanism, and deletion/retention rules.
- Out-of-scope: advanced moderation workflows, analytics pipeline ingestion, or automated triage.

**User Scenarios**

1. Submit Feedback (Primary Flow)
   - User opens the Feedback screen, enters a title (required) and content (optional), taps Send. The app validates input, persists the feedback, shows a success confirmation, and navigates back.

2. Validation Failure
   - User taps Send with empty title. The UI shows a clear inline validation error and does not submit.

3. Submit While Offline
   - If persistence is unavailable, the user receives an informative error and may retry when connectivity/storage is available. (Implementation may queue submission; implementation detail must be declared in feature plan.)

**Functional Requirements**

FR-1: Title Required
- Users MUST provide a non-empty title with a maximum length of 200 characters.
  - Acceptance: If title is empty, show inline error: "Please enter a title." If title length > 200, show inline error: "Title must be 200 characters or fewer."

FR-2: Content Optional
- Users MAY provide additional content text up to 2000 characters.
  - Acceptance: If content length > 2000, show inline error: "Content must be 2000 characters or fewer."

FR-3: Submit Action
- On valid input, the app MUST persist the feedback record and show a non-blocking confirmation (toast or dialog) indicating success.
  - Acceptance: A persisted record exists and the UI shows success text: "Thanks for your feedback." If persisting fails, show a clear error and allow retry.

FR-4: Accessible UI
- The Feedback screen MUST meet accessibility requirements: semantic labels for inputs and buttons, proper focus order, and readable contrast.
  - Acceptance: Screen elements have accessible labels; keyboard or screen-reader users can navigate and submit feedback.

FR-5: Minimal PII Collection
- The feedback form MUST NOT require PII. If the implementation collects any identifying data (e.g., user id), the spec MUST declare purpose and retention.
  - Acceptance: Spec declares what identifying data (if any) will be stored and why.

FR-6: Persistence Contract
- The implementation MUST persist feedback to the project's declared storage mechanism (see Assumptions). Each feedback record contains: id, title, content, createdAt (ISO 8601), optional userId, and source platform.
  - Acceptance: Storage contains records matching the schema after submission.

FR-7: Retention & Deletion
- Feedback entries older than the retention window MUST be removable per the retention policy. Default retention: 90 days unless otherwise specified.
  - Acceptance: Spec and persistence plan document retention and deletion approach.

**Non-Functional Requirements (NFRs)**

- NFR-1: Performance — Submitting feedback completes within a reasonable user-perceived time (under 3 seconds for success confirmation) when storage is available.
- NFR-2: Security — The app MUST not log feedback content that could contain PII; any sensitive data must be encrypted at rest if stored.
- NFR-3: Testability — Business logic must be unit-testable. End-to-end submission flow must be automatable in CI for at least one happy path.

**Success Criteria**

- 95% of submitted feedback attempts with valid input result in a persisted record.
- Users receive success confirmation within 3 seconds for successful submissions.
- Accessibility checks report no missing labels or focus issues for primary controls.
- Retention policy is documented and enforced for stored records.

**Key Entities / Data Model**

- Feedback
  - id: string (GUID)
  - title: string (max 200)
  - content: string (max 2000, optional)
  - createdAt: string (ISO 8601)
  - userId: string (optional)
  - source: string (e.g., android, ios, windows)

**API / Persistence Contract (client-facing example)**

Note: This is technology-agnostic and focused on observable behavior.

- Persist Feedback (client -> storage/service):
  - Request record: fields as in Key Entities.
  - Response: 201 Created with stored id OR meaningful error (4xx/5xx).

- Read Feedback (admin/backend):
  - Query by date range or userId; response includes list of records.

Implementation-specific details (e.g., REST endpoint path or local store API) MUST be declared in the feature plan.

**Privacy & Security**

- Do not collect or store PII in feedback unless explicitly documented with purpose and retention.
- Avoid logging full feedback text in production logs. If logs are necessary for debugging, redact or sanitize.
- If userId or other identifiers are stored, restrict access and follow platform secure storage and access control guidance.

**Testing / Acceptance Tests**

- UT-1: Unit tests for validation rules (title required, max lengths).
- IT-1: Integration test that submits a valid feedback payload and asserts persistence.
- UI-1: Automated UI test (smoke) that enters title + content, taps Send, and verifies success confirmation.
- AX-1: Accessibility audit that verifies labels and focus order for the feedback controls.

**Assumptions**

- Storage mechanism: Use the project's declared persistence pattern (non-SQLite) — e.g., a remote API or file-backed structured store. The feature plan must specify the actual mechanism.
- Auth: Feedback may be submitted by unauthenticated users; if identifying data is included, the plan must document consent and purpose.
- Retention default: 90 days unless product/Legal requires longer retention.

**Open Questions [NEEDS CLARIFICATION]**

- None required: the spec uses reasonable defaults. If you require a different retention period or mandatory authentication, update the feature plan.

**Next Steps**

1. Add this spec to `specs/001-feedback/spec.md`.  
2. Create a short implementation plan referencing `plan-template.md` and declare chosen persistence.  
3. Implement persistence and add the integration tests from Testing section.  
4. Review retention and privacy with Legal if feedback may contain PII.


--
