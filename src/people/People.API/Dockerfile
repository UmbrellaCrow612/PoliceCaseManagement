# Stage 1: Build the Go binary
FROM golang:1.24-alpine AS builder

# Set working directory inside the container
WORKDIR /app

# Copy go.mod and go.sum first to cache dependencies
COPY go.mod go.sum ./ 
RUN go mod download

# Copy the rest of the source code
COPY . .

# Build the Go app
RUN go build -o main .

# Stage 2: Create a minimal runtime image
FROM alpine:latest

# Set working directory
WORKDIR /root/

# Copy the binary from the builder stage
COPY --from=builder /app/main .

# Copy the .env file from the build context
COPY .env ./

# Expose HTTP port (7321) and gRPC port (50051)
EXPOSE 7001
EXPOSE 50051

# Run the binary
CMD ["./main"]