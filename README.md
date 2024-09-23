
# AuthFlow Service

AuthFlow is an authentication and authorization service built using Azure Functions and .NET. It integrates with Azure AD B2C for user authentication, manages user data, and provides JWT token-based authorization. The service handles user registration and profile updates through secure API endpoints.

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [Tech Stack](#tech-stack)
4. [Architecture](#architecture)
5. [Usage](#usage)
6. [API Endpoints](#api-endpoints)
7. [Configuration](#configuration)

## Introduction

AuthFlow Service provides a secure and scalable solution for managing user authentication and authorization in applications. It uses Azure AD B2C for authentication and supports operations such as user registration, profile updates, and token management.

## Features

- **User Registration:** Handles user sign-up and account creation.
- **Profile Update:** Allows users to update their profile information.
- **Token Management:** Manages JWT tokens for authenticated sessions.
- **Integration with Azure AD B2C:** Utilizes Azure AD B2C for secure user authentication.

## Tech Stack

- **Backend:** .NET 8 (Isolated Worker), Azure Functions v4
- **Database:** SQL Server (via Entity Framework Core)
- **Authentication:** Azure AD B2C, JWT tokens
- **Configuration & Secrets Management:** Azure App Configuration, Azure Key Vault
- **Dependency Injection:** Used for service registrations and configurations

## Architecture

AuthFlow Service follows a serverless architecture using Azure Functions. It employs a clean architecture pattern, separating concerns across different layers, and uses dependency injection for managing service lifetimes. The service interacts with Azure AD B2C for user authentication and SQL Server for data persistence.

## Usage

1. **Deploy the functions to Azure Functions.**
2. **Configure Azure App Configuration and Key Vault with necessary secrets.**
3. **Ensure Azure AD B2C is correctly set up and integrated.**

## API Endpoints

- **PostRegister:** Handles user registration and redirects upon success.
    - **Route:** `GET /post-register`, `POST /post-register`
- **PostUpdate:** Handles user profile updates.
    - **Route:** `GET /post-update`, `POST /post-update`

## Configuration

- **DatabaseOption:** Configures the SQL Server database connection.
- **AuthServiceOptions:** Configures authentication service settings, including redirect URIs.
- **TokenServiceOptions:** Configures token-related settings, including endpoints and client credentials.
