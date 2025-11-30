<!--
Implementation Plan: Feedback Submission
Path: specs/001-feedback/plan.md
Created: 2025-11-30
-->
# Implementation Plan — Feedback Submission (001-feedback)

**Chosen persistence**: Remote API (HTTPS) with a local resilient enqueue-and-retry queue for offline/failed attempts.

Rationale: Centralized storage simplifies triage and analysis, avoids platform-specific file collection, and aligns with product needs for searchable feedback. A local queue improves UX when network is intermittent.

## Architecture Overview

- Client: .NET MAUI app (existing codebase). Implements a `IFeedbackService` interface used by `FeedbackPageModel`.
- Persistence: Remote HTTP API with endpoints described below. Client sends JSON over HTTPS.
- Local resilience: lightweight on-device queue persisted to file (or secure local storage) to buffer failed submissions. Queue items are retried in background with exponential backoff.
- Security: Transport via TLS; no plaintext logging of feedback content; optional userId if authenticated (see Privacy).

## API Contract (recommended example)

- POST /api/feedback
  - Request (application/json):
    {
      "title": "string (<=200)",
      "content": "string (<=2000, optional)",
      "createdAt": "ISO-8601 string",
      "userId": "string (optional)",
      "source": "string (android|ios|windows)"
    }
  - Response:
    - 201 Created: { "id": "guid" }
    - 4xx: validation error with message
    - 5xx: server error

- GET /api/feedback  (admin / internal)
  - Query params: from, to, userId
  - Response: 200 OK: [ { feedback } ]

## Client Implementation Details

- Interface: `public interface IFeedbackService { Task<Result<Guid>> SendAsync(FeedbackDto feedback, CancellationToken ct); }`
- Default implementation: `RemoteFeedbackService` calls POST endpoint.
- Resilience layer: `QueuedFeedbackService` wraps `RemoteFeedbackService` and provides:
  - Persistent local queue (simple file-based JSON queue or secure local storage). Queue writes must be atomic and small.
  - Background worker that processes queued items with exponential backoff and jitter.
  - API surface to enqueue immediately when network failure or server 5xx occurs.
- Wiring: Register `IFeedbackService` in DI as `QueuedFeedbackService` which composes `RemoteFeedbackService`.

## UI Integration

- `FeedbackPageModel` uses `IFeedbackService.SendAsync` for submit action.
- During Submit: validate input; show spinner; call SendAsync; on success show "Thanks for your feedback"; on transient failure show error and offer Retry; when queuing, show "Saved for delivery — will retry".

## Error Handling & Offline

- Validation errors (4xx): show inline error messages returned by server where relevant.
- Transient errors (network, 5xx): enqueue locally and return success state to user (or show "Saved for delivery") depending on UX decision. Default: enqueue and show a non-blocking confirmation with note: "We'll deliver this when connection is available." Provide a retry button in the Feedback screen for queued items.

## Retention & Deletion

- Client: queued items persist no longer than retention window (90 days) and are deleted after successful delivery.
- Server: server-side retention policy enforces deletion after 90 days by default; the server must expose an admin deletion or lifecycle policy.

## Privacy & Security

- Do NOT collect PII in feedback content. If `userId` is included, document purpose and limit retention.
- Transport: HTTPS only. Use certificate pinning if required by security policy.
- Logging: redact or avoid logging feedback content; log only non-sensitive metadata (status, size, timestamps).

## Testing Plan

- Unit tests: validation for title/content lengths; queuing behavior; `QueuedFeedbackService` enqueue/dequeue logic.
- Integration tests: test POST to a mock API server (in CI using test doubles) and assert persisted id returned and queue processed.
- UI tests: automated smoke test that fills title + content, taps Send, and verifies success toast or queued confirmation.

## Rollout & Migration

- Feature toggle: release behind a config flag to enable server endpoint and queue behavior.
- Monitoring: track send success rate, queue length, and delivery latency; alert if queue backlog grows.

## Acceptance Criteria Mapping

- FR-1/FR-2: validation unit tests pass.
- FR-3: integration test shows POST returns 201 and server stores record, UI shows confirmation.
- FR-4: accessibility checklist validated in AX tests.
- FR-6/FR-7: server stores metadata and retention enforced; client respects retention for queued items.

## Implementation Tasks (ordered)

1. Add `IFeedbackService` interface and DTOs.
2. Implement `RemoteFeedbackService` (HTTP client) and simple unit tests.
3. Implement `QueuedFeedbackService` with persistent queue and background worker.
4. Wire DI and update `FeedbackPageModel` to call `IFeedbackService`.
5. Add unit + integration tests; add UI smoke test.
6. Add feature flag and rollout plan; push telemetry for queue metrics.

## Open Items / Decisions

- Confirm server API path and contract (team/backend request).  
- Confirm whether unauthenticated submissions are allowed (default: allowed).  
- Confirm retention override if Legal requests different policy (default: 90 days).

---

**Prepared by:** developer pairing session — commit to branch `001-feedback`.
