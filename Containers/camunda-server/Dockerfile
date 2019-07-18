# Build application
FROM maven:3-jdk-12 as builder
WORKDIR /repo
COPY . .
RUN mvn clean install

# Prepare application to start
FROM openjdk:12-alpine
WORKDIR /app
COPY --from=builder /repo/target/CamundaServer.jar .
EXPOSE 9201
CMD java -jar CamundaServer.jar
