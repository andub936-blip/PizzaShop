# PizzaShop

A small online pizza store developed on ASP.NET Core MVC. The project demonstrates the implementation of a full user scenario: viewing the catalogue, viewing product details, adding products to the cart, placing an order and viewing the order history.

## Preview

![Catalog](screenshots/catalog.png)

## Features

* Browse the pizza catalogue
* Pizza Details Page
* Adding items to your cart
* Changing the quantity and deleting products
* Storing the cart in the Session
* Checkout with form validation
* Saving orders in the database
* View the created order
* A snapshot of the name and price of the product at the time of ordering
* Handling of missing items and incorrect data
* Responsive Interface

## Tech Stack
* C#
* ASP.NET Core MVC
* Entity Framework Core
* SQLite database
* Razor Views
* Session
* Data Annotations
* HTML / CSS
* xUnit

## Architecture

The project uses the division of responsibility between layers:

Controller -> Service -> DbContext -> Database

## How to Run

1. Clone the repository.
2. Open the solution in Visual Studio or Rider.
3. Launch the app.

## Screenshots

### Catalog

![Catalog](screenshots/catalog.png)


### Pizza Details

![Pizza Details](screenshots/details.png)

### Cart

![Cart](screenshots/cart2.png)

### Order

![Order](screenshots/success.png)

### Order (mobile version)

![Order mobile version](screenshots/success_mobile.png) 

## Project Status

Completed educational portfolio project.
Further improvements may include authentication, administration,
payment integration and expanded test coverage.
