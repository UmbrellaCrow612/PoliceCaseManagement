# Email Worker

The **Email Worker** is a background service that connects to RabbitMQ, listens for email-sending events, and processes them by dispatching actual emails through an email service implementation.
Other services simply publish email events containing the content to send, and the worker handles the delivery.

---

## Running Outside of Docker

When running outside of Docker, you can start this project directly (either as a standalone process or as part of a multi-launch configuration in Visual Studio/Rider/etc).

Make sure the configuration points to RabbitMQ. In most setups, RabbitMQ will be running inside Docker, so the host should remain `localhost`. Example configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "RabbitMqSettings": {
    "Host": "localhost",
    "Username": "user",
    "Password": "StrongPassword!"
  }
}
```

---

## Running Inside Docker or an Orchestrator

When running inside Docker (or Docker Compose, Kubernetes, etc.), you need to update the configuration so that the worker connects to RabbitMQ by its **service/container name**, not `localhost`.

If using Docker Compose, the host should match the RabbitMQ service name defined in the `docker-compose.yml`. Example configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "RabbitMqSettings": {
    "Host": "broker",
    "Username": "user",
    "Password": "StrongPassword!"
  }
}
```

---

✅ Summary:

* **Outside Docker:** use `localhost`
* **Inside Docker:** use the RabbitMQ container/service name (e.g., `broker`, `rabbitmq`, etc.)
