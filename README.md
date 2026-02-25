# Webalkalmazás PaaS köryezetben 1-2

Homework documentation for Felhőalapú elosztott rendszerek laboratórium - BMEVIIIMB12 2025/26/2 course.

## Technologies

- Database: MSSQL
- Backend: C#, .NET
- Frontend: Typescript, React, Vite
- Local: Docker Compose

- The project can be run and tested locally using Docker Compose, and Azurite (for azure blob storage emulation)
- This is a monorepo, /backend contains the backend project, and /frontend contains the frontend project

## Backend
- Basic three layered architecture, containing .API, .BLL and .DAL for clean separation of concerns
- There is an existing image of the backend in *docker hub*, which I used during the deployment of backend (https://hub.docker.com/repository/docker/adamchairly/fer-hf-backend/general)
- Azure gives you continous deployement, it can look for new versions of the image in the registry
  
## Deployment

- Database: Aure SQL database
- Backend: Azure App Service 
- Image storage: Azure Blob Storage
- Frontend: Vercel (free and gives seamless continous deployement experience straight from github)
- Created subdomains on my existing domain for api and for frontend

## Live instances

- Backend: https://api.szekelyadam.com
- Frontend: https://fer-hf.szekelyadam.com
