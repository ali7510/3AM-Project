# 3AM-Project
3AM E-commerce .NET project

🙂Users are either Admins or Customers

🔒 Authentication endpoints
1- POST| /api/Auth/register
  💥Register a new user in the system.
  💥Rquest body ..
  {
  "name": "Ali Mustafa",
  "email": "ali@email.com", valid/unique
  "phone": "01012345678"    valid/unique
  }
  💥Returns status codes.

2- POST| /api/Auth/request-otp
  💥Start of the login process, user enters his registered email and receive otp on it.
  💥Returns status codes

3- POST| api/Auth/verify-otp
  💥User enters his email and the received otp to verify
  💥Returns access token, refresh token, refresh Token Expiry date time

4- POST| /api/Auth/refresh-token
  💥Request body ..
  {
    "refreshToken": "string"
  }
  receive the refresh token returned in the response of "verify-otp" endpoint.
  💥returns the same response as "verify-otp" but with different values.

5- POST| /api/Auth/logout
  💥Delete tokens from storage, Front-end must prevent user from doing furthur actions.
  💥 Returns status code


👨‍💼 Account endpoints
1- GET| /api/Account
  💥Response body
  {
  "name": "string",
  "email": "string",
  "phone": "string",
  "orders": [
    {
      "id": 0,
      "user_Id": 0,
      "user_Name": "string",
      "total_Price": 0,
      "status": "string",
      "payment_Status": "string",
      "cartItems": [
              {
                "id": 0,
                "product_Id": 0,
                "product_Name": "string",
                "product_Price": 0,
                "quantity": 0
              }
          ]
        }
      ]
    }

  💥Returns status code

2- DELETE| /api/Account/delete
  💥Returns status code
  💥switch the user account to inactive permanently


🛒Cart endpoints
🔷 User has only one cart that get cleared after checkout and payment to be reused, it is automatically created 
    once the user selects item to be added to his cart
1- GET| /api/Cart/mycart
  💥views the current cart items.
  💥Response body ..
    {
  "id": 0,
  "user_Name": "string",
  "cartItems": [
        {
          "id": 0,
          "product_Id": 0,
          "product_Name": "string",
          "product_Price": 0,
          "quantity": 0
        }
      ]
    }

2- POST| /api/Cart/items
  💥Adding a new item to the cart
  💥Request body ..
    {
      "product_Id": 0,
      "quantity": 0
    }
  💥Returns status code
      
3- DELETE| /api/Cart/items/{cartItemId}
  💥Removes item from User's cart, using the cartItem id

4- DELETE| /api/Cart/items/clear
  💥Clear all items in the cart

5- GET| /api/Cart/checkout
  💥Views the current cart items details as a single order
  💥Response body ..
    {
      "id": 0,
      "user_Id": 0,
      "user_Name": "string",
      "total_Price": 0,
      "status": "string",
      "payment_Status": "string",
      "cartItems": [
        {
          "id": 0,
          "product_Id": 0,
          "product_Name": "string",
          "product_Price": 0,
          "quantity": 0
        }
      ]
    }

📊 Dashboard endpoint
🔷Only admin users who can access this endpoint and get the response
1- GET| /api/Dashboard
  💥the responce contains ..
    1- TotalProducts
    2- TotalOrders
    3- TotalRevenue
    4- totalCustomers
    5- PendingOrders
    6- ProcessedOrders
    7- CancelledOrders


📦 Order endpoint
  1- GET| /api/Order/vieworder
  💥 Response body ..
    {
      "id": 0,
      "user_Id": 0,
      "user_Name": "string",
      "total_Price": 0,
      "status": "string",
      "payment_Status": "string",
      "cartItems": [
        {
          "id": 0,
          "product_Id": 0,
          "product_Name": "string",
          "product_Price": 0,
          "quantity": 0
        }
      ]
    }
    🎆The same response as checkout endpoint !


💰Payment endpoint
1- POST| /api/payment/confirm
  💥it confirms payment process, ..
    => method : 1 for myfatoorah payment method
    => method : 2 for cash on delivery method
  💥Response body ..
  {
      "success": true,
      "paymentUrl": "string",
      "externalPaymentId": "string",
      "requiresRedirect": true,
      "message": "string"
  }



🚗Product endpoints
  1- GET| /api/Product
    💥 views all existing products in the system (Cars & Accessories)
    💥 Response body ..
      [
        {
          "id": 0,
          "name": "string",
          "description": "string",
          "price": 0,
          "quantity": 0,
          "imageUrl": "string",
          "brand": "string",
          "specsJson": "string",
          "categoryName": "string"
        }
      ]

  2- GET| /api/Product/{id}
    💥 view details for specific product by id
    💥 Response body ..
      {
        "id": 0,
        "name": "string",
        "description": "string",
        "price": 0,
        "quantity": 0,
        "imageUrl": "string",
        "brand": "string",
        "specsJson": "string",
        "categoryName": "string"
      }

  3- GET| /api/Product/categories
    💥 view all categories in the system
    💥 Response body ..
      [
        {
          "id": 0,
          "name": "string",
          "parent_Category_Id": 0
        }
      ]

  4- GET| /api/Product/category/{categoryId}
    💥views all products that belong to specific category, we pass the category id to get those products
    💥 Response body ..
      [
        {
          "id": 0,
          "name": "string",
          "description": "string",
          "price": 0,
          "quantity": 0,
          "imageUrl": "string",
          "brand": "string",
          "specsJson": "string",
          "categoryName": "string"
        }
      ]

    🔷 Cars has Id = 1, Accessories = 2 those are the main categories so thry have subcategories that has many products belong to it


  5- GET| /api/Product/vehicles
    💥 Views all products that are vehicles (belong to Cars category)
    💥 Response body ..
      [
        {
          "id": 0,
          "name": "string",
          "description": "string",
          "price": 0,
          "quantity": 0,
          "imageUrl": "string",
          "brand": "string",
          "specsJson": "string",
          "categoryName": "string"
        }
      ]


  6- GET| /api/Product/accessories
    💥 Views all products that are accessories (belong to Accessories category)
    💥 Response body ..
      [
        {
          "id": 0,
          "name": "string",
          "description": "string",
          "price": 0,
          "quantity": 0,
          "imageUrl": "string",
          "brand": "string",
          "specsJson": "string",
          "categoryName": "string"
        }
      ]


