# 🕵️‍♂️ Anonymous Survey App

A cleanly architected, domain-driven survey platform built using [Ardalis Clean Architecture](https://github.com/ardalis/CleanArchitecture) and [FastEndpoints](https://fast-endpoints.com). This application enables organizations—such as universities or companies—to gather anonymous feedback, while providing structured moderation for administrators.

---

## 🧠 Project Purpose

This system allows anonymous users (contributors) to submit comments about subjects (e.g., teachers, teams), and administrators to manage and review this feedback. The app focuses on **anonymity**, **moderation**, and **clean software architecture** to ensure long-term maintainability and testability.

---

## 🔧 Architecture 

This solution follows **Clean Architecture**, emphasizing separation of concerns across four major layers:

- **Core**: Business rules, domain models, and enums.
- **UseCases**: CQRS-based handlers for application-specific logic.
- **Infrastructure**: Email, database, and service integrations.
- **Web**: FastEndpoints-based API, DTOs, auth, and validation.

> Designed for clarity, testability, and ease of future extension.

---

## ✅ Key Features

| Feature                         | Description                                                   |
|----------------------------------|---------------------------------------------------------------|
| 🔐 Admin Authentication         | JWT-based login, refresh tokens, role protection              |
| 💬 Anonymous Commenting         | Contributors can submit feedback without accounts             |
| 📄 Subject & Department Entities | Hierarchical organization of content                         |
| 🧪 Request Validation           | FluentValidation on all endpoints                             |
| 📦 Clean Domain Aggregates      | Rich models (e.g., `Comment`, `Subject`, `Admin`)             |
| 🔍 Query Flexibility            | Specification pattern for reusable, expressive filtering      |
| 💡 Minimal API Framework        | FastEndpoints used instead of MVC controllers                 |

---

## ⚙️ Tech Stack

| Concern          | Stack / Library                              |
|------------------|----------------------------------------------|
| API Framework     | FastEndpoints                                |
| ORM               | Entity Framework Core                        |
| Auth              | JWT + Refresh Tokens                         |
| Validation        | FluentValidation                             |
| Architecture      | Clean Architecture + CQRS                    |
| Patterns          | Specification, Aggregate Root, Repository    |

---

## 🧱 Patterns & Principles

- **Clean Architecture** – Independent business logic, infra and UI layers
- **CQRS** – Separation of commands and queries for clarity and scaling
- **Specification Pattern** – Reusable query filters with expressiveness
- **Domain-Driven Design** – Rich aggregates, enums, and invariants
- **Dependency Injection** – Modular service wiring and testing




