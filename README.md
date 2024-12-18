# Assignment 2: Message-Driven Payment System  

This project implements a message-driven payment processing system using RabbitMQ and REST APIs as part of Assignment 2 for SE4458. The system simulates a three-step payment process:  

1. **Payment Queue**: Accepts payment requests and pushes them to a queue.  
2. **Process Payment**: Consumes messages from the Payment Queue, processes them, and pushes them to a Notification Queue.  
3. **Notification Service**: Consumes messages from the Notification Queue and sends notifications to users.

---

## **Project Structure**  

The project consists of the following components:  

### **1. RabbitMQHelper**  
A helper class that handles RabbitMQ connections and simplifies sending messages to queues.  

### **2. Controllers**  

#### **PaymentController**  
Accepts payment requests in JSON format via a REST API and pushes them to the `PaymentQueue`.

Example request body:
```json
{
    "user": "ali@gmail.com",
    "paymentType": "credit",
    "cardNo": "1234123412341234"
}

github link : https://github.com/AliAlpKUREL/Assignment2PaymentSystem.git.
youtube link: https://youtu.be/HqwjJzuEKas
