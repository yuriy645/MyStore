# Security systems online store

The purpose of the project is to gain experience using the knowledge gained during the course C#/.NET DEVELOPER.
I conceived a large-scale extensible project in which the basic features are immediately implemented, and there is the possibility of adding a lot of interesting features.
Web: http://vline.somee.com

## Notable points of the project 

1. The project is made according to Database First Approach (3NF).

2. The normalized database with pre-filling with products and settings for publishing products is created by the MyStoreDB.sql script (located in the current directory). 

4. The database schema is contained in the file MyStoreDB.pdf (created in the dbForge Studio for SQL Server Enterprise). 

3. The idea of a convenient quick filling of goods from different categories with specific characteristics has been implemented. The bottom line is that the table of product categories corresponds to a table of characteristics of categories (in which for each category there is a list of characteristics only for the selected category). And the table of characteristics of categories corresponds to the table of values of characteristics (which respectively contains a set of possible values for each possible characteristic of each category). More details are contained in the database description file DescriptionDB.docx (located in the current directory).
The application provides a full-fledged editor of possible characteristic values right on the product editing page. That is, a store employee who fills in the product fields immediately receives a ready list of characteristics, and for each characteristic - a small drop-down list of the most probable values. Moreover, if the desired value was not in the drop-down list, then it can be added to the database right here.

4. To account for the colors of goods, the table of general characteristics of goods is divided into two parts: 1st with characteristics dependent on color, 2nd - in which characteristics do not depend on color 

5. Access to administrative functions. Users: Store customers and employees are stored in separate tables. The authorization window on the site is common for customers and employees. If an employee is authorized, then the main menu contains additional items required by the administrator.

6. The site allows the order of goods by an unregistered client. In this case, an account is automatically created with a minimum filling to display in the order manager.

7. The site provides a nice look: with the help of CSS, the elements are arranged consistently in an ergonomic order. Provides the necessary optimization of the arrangement of elements when resizing the browser window. Simple JS functions are used for convenient user interaction. 

8. The application keeps a log of calls to all action methods. When you click the "Today's visit log" button in the admin menu, records with today's date are displayed on the page.

## Technology used 

C#, ASP.NET (MVC), SQL, Entity Framework, HTML, CSS, JavaScript

## Hosting 

Especially for compatibility with somee.com hosting, the version of ASP.Net was changed to .Net Core 3.1.
