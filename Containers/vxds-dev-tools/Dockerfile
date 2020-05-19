FROM ubuntu:16.04 as scripts
WORKDIR /scripts

RUN apt-get update && apt-get -y install curl

ENV DOCKERIZE_VERSION v0.6.1
ENV DOCKERIZE_RELEASE_LINK https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz
RUN curl -L $DOCKERIZE_RELEASE_LINK > dockerize.tar.gz \
    && tar -C /scripts -xzvf dockerize.tar.gz \
    && rm dockerize.tar.gz

# Build application
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.202-focal as builder
WORKDIR /solution
COPY . .

RUN curl -sL https://deb.nodesource.com/setup_12.x | bash -
RUN apt-get -y install nodejs

RUN npm install -C UnifiedPortal/Server/Static
RUN npm install -C Modules/SimpleNoteService/Server/Static
RUN dotnet build DevTools.sln

EXPOSE 9000 9001 9300 9301

COPY --from=scripts /scripts /scripts