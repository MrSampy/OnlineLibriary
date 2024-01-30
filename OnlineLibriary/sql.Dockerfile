# Use the official PostgreSQL image from Docker Hub
FROM postgres:latest

# Set environment variables for PostgreSQL
ENV POSTGRES_DB=OLDB
ENV POSTGRES_USER=sa
ENV POSTGRES_PASSWORD=P@ssw0rd123

# Expose PostgreSQL default port
EXPOSE 5432