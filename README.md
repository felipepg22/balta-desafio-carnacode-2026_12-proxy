![ES-7](https://github.com/user-attachments/assets/61d1998c-69c4-484e-a6d8-7c84b03357b9)

## 🥁 CarnaCode 2026 - Challenge 12 - Proxy

Hi, I’m Felipe Parizzi Galli, and this is the place where I share my learning journey during the **CarnaCode 2026** challenge, organized by [balta.io](https://balta.io). 👻

Here you’ll find projects, exercises, and code I’m building throughout the challenge. The goal is to get hands-on, test ideas, and document my growth in the tech world.

### About this challenge
In the **Proxy** challenge, I had to solve a real-world problem by implementing the corresponding **Design Pattern**.
During this process, I learned:
* ✅ Software Best Practices
* ✅ Clean Code
* ✅ SOLID
* ✅ Design Patterns

## Problem
A corporate application needs to control access to sensitive documents, cache heavy documents, and log all operations.  
The current code mixes business logic with access control, caching, and logging.

## About CarnaCode 2026
The **CarnaCode 2026** challenge consists of implementing all 23 design patterns in real-world scenarios. Throughout the 23 challenges in this journey, participants are exposed to learning and practicing how to identify non-scalable code and solve problems using industry-standard patterns.

### eBook - Design Pattern Fundamentals
My main source of knowledge during the challenge was the free eBook [Design Pattern Fundamentals](https://lp.balta.io/ebook-fundamentos-design-patterns).

## What was implemented to apply the Proxy Pattern
In this codebase, the Proxy pattern was implemented by wrapping the real document service with multiple proxy layers, each adding one cross-cutting concern without changing the core business logic.

### 1) Common contract for real service and proxies
- `IDocumentService` defines the operations: `ViewDocument`, `EditDocument`, and `ShowAuditLog`.
- Both the real service and all proxies implement this same interface.

### 2) Real subject (core business logic)
- `DocumentService` is the real service responsible for retrieving and updating documents.
- It delegates persistence to `DocumentRepository`.
- The repository is created lazily (`Lazy<DocumentRepository>`), so initialization only happens when needed.

### 3) Base proxy abstraction
- `DocumentServiceProxyBase` receives an `IDocumentService` (`innerDocumentService`) and forwards calls by default.
- Specialized proxies inherit from this base class and override only the behavior they need.

### 4) Specialized proxies
- `AuthorizationDocumentServiceProxy`
  - Checks user clearance level against document security level.
  - Blocks unauthorized view/edit operations.
- `CachingDocumentServiceProxy`
  - Caches documents by ID on first read.
  - Returns cached results on subsequent reads.
  - Invalidates cache on edit.
- `AuditDocumentServiceProxy`
  - Records audit entries for access/edit attempts and outcomes.
  - Exposes a consolidated audit log via `ShowAuditLog()`.

### 5) Proxy composition (decorated chain)
In `Challenge.cs`, the service is composed as:

`Audit -> Authorization -> Caching -> DocumentService`

This allows stacking responsibilities (audit, authorization, caching) while keeping `DocumentService` focused on document operations.

### 6) Practical result
- Separation of concerns
- Easier extensibility (new behaviors can be added as new proxies)
- Less duplication of cross-cutting code
- Better maintainability and testability
