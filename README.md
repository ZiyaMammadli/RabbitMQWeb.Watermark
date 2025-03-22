# RabbitMQWeb.Watermark

## Description

RabbitMQWeb.Watermark is an **ASP.NET Core 8 MVC** application that allows users to upload product images and automatically adds a watermark using **RabbitMQ** and a **background service**.

## Features

- **Product Management** (CRUD operations)
- **Image Upload** during product creation
- **RabbitMQ Message Queue** for processing watermarking asynchronously
- **Background Service** to handle watermark addition
- **Entity Framework Core** for database interactions

## Technologies Used

- **ASP.NET Core 8 (MVC)**
- **RabbitMQ** (Message Broker for asynchronous processing)
- **Entity Framework Core** (Database Management)
- **Background Service** (For watermark processing)
- **Bootstrap** (For UI styling)
- **FluentValidation** (For input validation)

## Installation

1. Clone the repository:

```bash
git clone https://github.com/ZiyaMammadli/RabbitMQWeb.Watermark.git
cd RabbitMQWeb.Watermark
```

2. Configure **appsettings.json** with your database and RabbitMQ connection details.

3. Apply migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run RabbitMQ (Docker):

```bash
docker run -d --hostname rabbitmq-host --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
```

5. Start the application:

```bash
dotnet run
```

## Project Structure

- **Models/Product.cs**: Defines the Product entity.
- **Controllers/ProductController.cs**: Manages product CRUD operations.
- **Services/WatermarkBackgroundService.cs**: Background worker for adding watermarks.
- **RabbitMQ/MessageQueuePublisher.cs**: Publishes messages to RabbitMQ.
- **RabbitMQ/MessageQueueConsumer.cs**: Listens for messages and triggers watermarking.

## How It Works

1. **User Uploads a Product Image**: During product creation, an image is uploaded.
2. **Message Sent to RabbitMQ**: The image details are sent to RabbitMQ.
3. **Background Service Processes Image**: The background service listens to the queue, processes the image, and applies the watermark.
4. **Updated Image Stored**: The watermarked image is stored back into the system.

## API Endpoints

| Endpoint              | Method | Description         |
|----------------------|--------|---------------------|
| /Products            | GET    | Get all products   |
| /Products/Details/{id} | GET    | Get product details |
| /Products/Create     | POST   | Create new product |
| /Products/Edit/{id}  | PUT    | Edit product       |
| /Products/Delete/{id} | DELETE | Delete product     |

## Contact

For questions or issues, please reach out to:

- Email: [ziyam040@gmail.com](mailto:ziyam040@gmail.com)
- GitHub: [Profile](https://github.com/ZiyaMammadli)

## License

This project is licensed under the MIT License.

