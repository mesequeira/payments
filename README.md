# Distributed Microservices Orchestration Project

Welcome to my **Distributed Microservices Orchestration** project! 🎉 This is a purely educational playground where I'm practicing **idempotency**, **microservices communication**, and **orchestration** patterns. Why? Well, I was a bit bored, and what better way to kill time than to create a complex microservices architecture for fun, right? 😎

---

## 🛠️ Project Overview

The project is still a work in progress and will evolve little by little. It leverages **Clean Architecture** and **Vertical Slice Architecture** to keep things tidy and organized. Here's a brief breakdown of the components and technologies used so far:

### **Architecture**

- **Minimal APIs** with **.NET 9**  
- **Vertical Slice Architecture**:  
  Each feature lives within its own folder. For example, in the **Orders** microservice (which is the orchestrator), there's an `Orders` folder containing:
  - `Commands`: Business commands to handle order processing.
  - `Repositories`: Data access logic.
  - `Sagas`: State machines for orchestration (handled with **MassTransit**).

### **Orchestration**

The core of this project is based on **Saga Orchestration** with **MassTransit** and **RabbitMQ** for message passing between microservices. The main workflow involves events such as order creation, stock reservation, shipping preparation, and payment processing.

To simulate a real-world system, I use **Docker Compose** to manage multiple microservices in a local environment.

### **Diagram Reference**

The overall flow of events and services is described in this **sequence diagram**:

[Order Processing Saga Sequence Diagram](./docs/OrderProcessingSagaSequenceDiagram.svg)

Feel free to check it out!

---

## 🚀 Running the Project

To get started, you'll need Docker installed. Then, you can spin up the entire system using:

```bash
docker-compose up
```

This will launch all services, including the orchestrator and RabbitMQ.

### **🔑 Environment Configuration**

Before running the project, make sure to create a .env file in the root directory with the following content:

```bash
ORCHESTATOR_CONNECTIONSTRING=Server=sqlserverdb,1433;Database=OrchestatorDb;User Id=sa;Password=TempDev123!;MultipleActiveResultSets=true;TrustServerCertificate=true
PAYMENT_CONNECTIONSTRING=Server=sqlserverdb,1433;Database=PaymentDb;User Id=sa;Password=TempDev123!;MultipleActiveResultSets=true;TrustServerCertificate=true
DATABASE_PASSWORD=TempDev123!
APPLICATION_API_KEY=123
RABBITMQ_USER=guest
RABBITMQ_PASSWORD=guest123
```

### **🌐 Docker Network Setup**

Additionally, you need to create a Docker network called `my_network`. This allows the microservices to communicate with each other. Run the following command:


```bash
docker network create my_network
```

This step is essential for the containers to be able to resolve each other's hostnames within the Docker Compose setup.

## 🤓 What’s Next?
I'm planning to continue improving the orchestration flow, enhancing fault-tolerance mechanisms (timeouts, retries, compensation), and adding more services to simulate a full distributed system. Who knows where this might end up—probably in a tech blog someday!
