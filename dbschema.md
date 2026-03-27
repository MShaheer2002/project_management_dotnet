# Database Schema Review: SaaS Project Management Tool

This document outlines the current database tables and provides a comprehensive list of recommended tables to reach production-grade SaaS maturity (similar to Linear, Jira, or ClickUp).

## Existing Tables (MVP)
The current implementation includes the following 7 core tables:

1. `Users`
2. `Organizations`
3. `OrganizationMembers`
4. `Projects`
5. `Issues`
6. `Teams`
7. `TeamMembers`

---

## Recommended Production-Grade Tables
To support advanced SaaS features, the following tables are essential:

### Core Management
8. `Workspaces` (Logical partitioning for larger organizations)
9. `Cycles` (Sprints or time-boxed iterations)
10. `Milestones` (High-level goals or roadmaps)
11. `Labels` (Custom tagging for issues)
12. `Statuses` (Customizable workflow states)
13. `Templates` (Reusable issue/project templates)

### Collaboration & Content
14. `Comments` (Threaded discussions on issues)
15. `Attachments` (File uploads and links)
16. `Reactions` (Emoji reactions for engagement)
17. `Documents` (Internal wiki or project specs, similar to Linear Docs)

### System & Logistics
18. `ActivityLogs` (Audit trail of changes across the system)
19. `Notifications` (User-specific alerts and read states)
20. `Webhooks` (Outgoing events for third-party systems)
21. `Integrations` (Connections to GitHub, Slack, etc.)
22. `ApiKeys` (Programmatic access for developers)

### SaaS Features
23. `Subscriptions` (Billing plans and tier management)
24. `Plans` (Feature flagging based on subscription tier)
25. `UsageQuotas` (Tracking limits for seats, storage, or projects)

### Advanced Productivity
26. `Views` (Saved custom filters and layouts)
27. `CustomFields` (User-defined metadata for issues)
28. `TimeEntries` (Detailed time tracking per issue/user)
29. `Relations` (Issue dependencies: "blocks", "is blocked by", "duplex")
