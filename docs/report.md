---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 15
author:
	- # DONT REMOVE THIS
    - "Daniel Millard <dmil@itu.dk>"
    - "Frederik Rosenlund <frlr@itu.dk>"
    - "Jacob Pærregaard <jacp@itu.dk>"
    - "Mads Nørklit Jensen <macj@itu.dk>"
    - "Rasmus Nielsen <raln@itu.dk>"
numbersections: true
---

- [Design and Architecture of _Chirp!_](#design-and-architecture-of-chirp)
  - [Domain model](#domain-model)
  - [Architecture — In the small](#architecture--in-the-small)
  - [Architecture of deployed application](#architecture-of-deployed-application)
  - [User activities](#user-activities)
  - [Sequence of functionality/calls trough _Chirp!_](#sequence-of-functionalitycalls-trough-chirp)
- [Process](#process)
  - [Build, test, release, and deployment](#build-test-release-and-deployment)
  - [Team work](#team-work)
  - [How to make _Chirp!_ work locally](#how-to-make-chirp-work-locally)
  - [How to run test suite locally](#how-to-run-test-suite-locally)
- [Ethics](#ethics)
  - [License](#license)
  - [LLMs, ChatGPT, CoPilot, and others](#llms-chatgpt-copilot-and-others)

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.]()

## Architecture — In the small

## Architecture of deployed application
Illustrate the architecture of your deployed application. Remember, you developed a client-server application. Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

- Clients: 
  - Web browser
  - Mobile app
- Web server: 
  - ASP.NET Core
  - Docker
  - Azure
- Database: 
  - MSSQL
  - Docker
  - Azure

Clients -> Web server <-> Database

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work

- Get issue from teacher/customer/whoever
- Create issue with the format described in readme
- The issue is up for grabs by everyone in the "new column"
- A fitting member(s) will pickup the issue and start working on it.
- Issue moves to "In progress"
- Link pull request to issue, to track progress
- Pull request gets ready for review by members who worked on the issue.
- Pull review is requested to those who didn't work on the issue
- Pull request is reviewed/merged
- Issue is moved to "Done"

## How to make _Chirp!_ work locally
- Probably something about docker? The program cant run without docker I think?

To clone the project run the following command in the terminal: 

- git clone https://github.com/ITU-BDSA23-GROUP15/Chirp.git

From the root of the project run the following command to run the project locally:
- dotnet run --project src/Chirp.Razor/
- Open a browser and go to http://localhost:5273/
- Do we need to do any migrations?


You should now expect to see the public timeline, stating at the top of the site, that you need to login to cheep, a login button should be available at the top right.

## How to run test suite locally
To run the test suite locally, you need to have a local instance of the program running. This can be done from the root of the project by entering 
-  cd src/Chirp.Razor and dotnet run. This will start the program at http://localhost:5273/

The test suite can then be run by entering cd src/Chirp.Tests (maybe not, can just do dotnet test from root of project) and afterwards dotnet test. This will run the test suite and output the results in the terminal.




# Ethics

## License
We've chosen the MIT License, which is a permissive free software license, because of its limited restriction on reuse.

## LLMs, ChatGPT, CoPilot, and others
"State which LLM(s) were used during development of your project. In case you were not using any, just state so. In case you were using an LLM to support your development, briefly describe when and how it was applied. Reflect in writing to which degree the responses of the LLM were helpful. Discuss briefly if application of LLMs sped up your development or if the contrary was the case."

- Co-pilot: has been useful for auto completing code that has already been made previously, especially the repository database functions.
- Co-pilot: was not good at creating new code with new logic, almost always faulty and spent more time debugging autocompleted code than what it benefitted.
- ChatGPT: has been used a lot as a starting point in debugging, when everything seemed overwhelming.
- ChatGPT:General questions about project topics, that helped narrow down the scope of the task and therefore researching became a lot easier. 
