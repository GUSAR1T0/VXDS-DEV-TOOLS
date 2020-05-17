# Build application
FROM maven:3-jdk-14 as builder
WORKDIR /repo
COPY . .
RUN mvn clean install test

# Prepare application to start
FROM openjdk:14-alpine
WORKDIR /app
COPY --from=builder /repo/target/CamundaServer.jar .
EXPOSE 9221
CMD java -jar CamundaServer.jar
