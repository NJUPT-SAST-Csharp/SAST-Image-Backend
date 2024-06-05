﻿# SAST-Image-Backend

It used to be the backend of SastImg.

It applies .NET8, EFcore, Dapper, Pgsql. Aspire is also included.

Moreover, It is also a bold attempt at distributed application, DDD and microservice technology, including Redis, MQ, OpenTelemetry and so on.

However, for applications with less payloads and requirement(such as SastImg), microservices have proven to be useless and even wasteful.

A continuation version of SAST-Image-Backend can be seen at [SAST-Image-RE](https://github.com/NJUPT-SAST-Csharp/SAST-Image-Re), which is refactored in Modular Monolith Architecture.
