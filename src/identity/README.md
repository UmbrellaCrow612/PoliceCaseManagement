# Identity

Contains all Identity-related domain logic and services, such as APIs, specialized UI portals, domain logic, custom CLI tools, and anything related to a user’s identity.

Identity encompasses **who a user is**, the process of **verifying that identity**, managing **roles and permissions**, and issuing **JWT tokens** for the user. These tokens enable access to identity-related sub-services and other services that require the user to be **identified, authenticated, and authorized**.

---

## Identity API

An API that exposes endpoints for web clients to **authenticate and authorize** a user.

---

## Identity Application

The **business implementation** of application-specific logic. This layer contains the interfaces and their implementations for handling Identity operations.

---

## Identity Core

Contains **core business contracts**, **interfaces**, and **value objects**.

* No implementations are included here.
* Designed to provide a clear and simple reference for the Identity Application, which implements the core logic.

---

## Identity CLI

A command-line interface that exposes Identity application logic via CLI commands.

* Uses the same implementation logic as the application; no code changes are required.
* Provides a convenient way to interact with Identity services through the terminal.

---

## Identity Infrastructure

Handles all **database-related concerns**:

* EF Core migrations
* Database configuration (e.g., indexes on columns)
* EF Core DbContext class

---

## Running

Identity is part of the **Police Case Management System (PCMS)**.

* In development, it can be run using the **multi-launch script**.
* Ensure **Docker** is running for required services like the database, Redis, and other supporting components.