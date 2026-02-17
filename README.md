# Webalkalmazás PaaS köryezetben 1-2

## Technologies

- Database: MSSQL
- Backend: C#, .NET
- Frontend: Typescript, React
- Local: Docker Compose

- The project can be run and tested locally using Docker Compose, and Azurite (for azure blob storage emulation)
- This is a monorepo, /backend contains the backend project, and /frontend contains the frontend project

## Backend
- Basic three layered architecture, containing .API, .BLL and .DAL for clean separation of concerns.
- There is an existing image of the backend in *docker hub*, which I used during the deployment of backend (https://hub.docker.com/repository/docker/adamchairly/fer-hf-backend/general)

## Deployment

- Database: Aure SQL database
- Backend: Azure App Service 
- Image storage: Azure Blob Storage
- Frontend: Vercel 
- Created subdomains on my existing domain for api and for frontend

## Live instances

- Backend: https://api.szekelyadam.com
- Frontend: https://fer-hf.szekelyadam.com