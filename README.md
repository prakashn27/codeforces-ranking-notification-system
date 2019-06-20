# codeforces-ranking-notification-system
This is a sample project which is used to learn more about Asp.Net core 2.2. It helps us to
* Subscribe to one or more users(friends) for ranking changes
* Notify if there is a change in rank of our friends
* Out of scope: Authentication, UI

### Architecture Diagram
![Architecture Diagram](https://user-images.githubusercontent.com/3127498/59861206-5be55880-934e-11e9-917c-18ad333a12db.png)

## Overview of Architecture:
* Data resides in sql server instance of docker
* Message broker used is Rabbitmq instance of docker
* SMTP server is used to check the delivery of messages
* `UserManagementApi` and `FollowerManagementApi` are `dotnet webapi` projects used to add data to our DB.
* `ParseContestApi` is also a `dotnet core webapi` project which parses the ranking changes of a contest
* All the ranking changes are updated in queue `Rank`
* `CreateNotification` gets the each user in rank queue and creates a notification for all of its followers and updates it to `Notification` queue.
* `SendNotification` fetches the data from `Notification` queue and sends mail for each notification.

## Run
`./StartSolution.sh` starts all the projects currently from docker compose.

## Debug
1. To debug individual project, stop that particular project using `docker-compose stop {projectname}`. 
2. Run the project in development mode, using visual studio code.

## QuickLinks
* [RabbitMQ web](http://localhost:15672/#/)
* [MailDev](http://localhost:4000/#/)

## Docker Images (available in docker hub):
* [usermanagementapi](https://hub.docker.com/r/prakashnatarajan/usermanagementapi)
* [followermanagementapi](https://hub.docker.com/r/prakashnatarajan/followermanagementapi)

## Tutorials:
* [Intro to rabbitmq](https://www.youtube.com/watch?v=deG25y_r6OY)
* [Edwin Van Wijk talk on .net core Architecture](https://www.youtube.com/watch?v=-AfZxdXa7yc&t=734s)
