# Build application
FROM maven:3-jdk-13 as builder
WORKDIR /repo
COPY . .
RUN mvn clean install

# Prepare application to start
FROM openjdk:13-alpine
WORKDIR /app
COPY --from=builder /repo/target/CamundaServer.jar .
EXPOSE 9221
CMD java -jar CamundaServer.jar
