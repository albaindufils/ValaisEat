# Valais Eats - Software development project
#### Cours: 623-1 Software Development
#### Students: Louise Bretz / Albain Dufils

### Concept:
Some restaurants in Valais would like to manage their food delivery in the region. They ask you to create this platform with the help of N-tier and a database. 
### Constraints:
* A table with the information of the staff responsible for the delivery
* A link to the orders table that each staff is responsible
* Customers must have a login/password
* Each order is identified by a number. This number in addition to firstname lastname will be used to cancel the order at least 3 hours before delivery.

### User stories:
#### Login
A customer must create an account with his/her address before using the website
#### Order
A logged customer can choose dishes from a list given by each restaurant available on the website to form an order. He/she will add delivery time (every 15 min) for his/her order. At the end of the order the price that the customer has to pay to the courier will be displayed
#### Delivery management
The system will assign the delivery of an order to one courier who is available in the same city as the restaurant where the order is made. One courier cannot have more than 5 orders to deliver every 30 minutes.
#### Delivery interface
each courier can log in the system to see his/her upcoming deliveries. Once one delivery is made the delivery person will archive it by pressing a button on the delivery interface.
