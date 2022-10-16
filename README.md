# Clamify API 

## Introduction

The Clamify API is a personal project of @cdconn00. This project serves as the backend for a personal finance application used primarily for budgeting and cost insights

## Roadmap
The API will progress in 2 week sprints to deliver incremental functionality.

## Setup
* Clone the repository.
* Verify you can run a .NET 6 project.
* Verify Clamify.Web is the default startup project.
* Run the project and view the Swagger page.

## General Code Structure
* Each endpoint should be added to a Controller within Clamify.Web, being mindful to follow RESTful naming conventions.
* The endpoint in the controller communicates with outside services / the DB via interacting with writers/providers/services in the Clamify.Core project.
* Clamify.RequestHandling contains request and response objects and helpful classes for handling requests.

## Testing

### Integration Testing
* Integration tests run in a docker container with a containerized database. Docker Desktop / Rancher Desktop or an equivalent tool must be running for these tests to run.
* The database container schema is based on migrations. If changes were made to the Clamify.Entities project you will need to add a new migration with the command found in: `BaseHelpers/ClamifyContextFactory`.

## Deployments

TBD after deployments are configured.

## Code Standards

### Documentation
All public types/members should include appropriate XML documentation to facilitate a better understanding of the code. Warnings are configured for missing docs, see Linting.

### Linting
This project makes use of linting with the StyleCop.Analyzers NuGet package, in addition to the default Rosyln warnings. Rule configurations are defined in the .editorconfig file.
