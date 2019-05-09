# Build application
FROM maven:3.5-jdk-8-alpine as builder
WORKDIR /repo
COPY . .
RUN mvn clean install

# Prepare application to start
FROM openjdk:8-jre-alpine
WORKDIR /app
COPY --from=builder /repo/target/CamundaServer.jar .
EXPOSE 9201
CMD java -jar CamundaServer.jar
