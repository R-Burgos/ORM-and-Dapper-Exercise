using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace ORM_Dapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string connString = config.GetConnectionString("DefaultConnection");
            IDbConnection conn = new MySqlConnection(connString);

            //FOR PART 1-----------------------------------------------------------------------------------------------------
            #region Part 1
            var repo = new DapperDepartmentRepository(conn);

            Console.WriteLine("*----------------------------------------------------*");
            //To List all departments to console
            Console.WriteLine(" Current Departments:");
            Console.WriteLine(" ---------------------");
            var depts = repo.GetAllDepartments();
            foreach (var dept in depts)
            {
                Console.WriteLine($" {dept.Name}");
            }
            Console.WriteLine(" ---------------------");


            Console.WriteLine(" Type a new Department name");
            var newDepartment = Console.ReadLine();
            repo.InsertDepartment(newDepartment);
            var departments = repo.GetAllDepartments();
            foreach (var dept in departments)
            {
                Console.WriteLine($" {dept.Name}");
            }
            #endregion

            //FOR PART 2-----------------------------------------------------------------------------------------------------
            #region Part 2
            //LIST ALL PRODUCTS TO CONSOLE
            var repoProducts = new DapperProductRepository(conn);
            Console.WriteLine("*----------------------------------------------------*");
            Console.WriteLine(" Current Products:");
            Console.WriteLine(" ---------------------");
            var prods = repoProducts.GetAllProducts();
            foreach (var p in prods)
            {
                Console.WriteLine($" ProductID:{p.ProductID}--------------------");
                Console.WriteLine($" * {p.Name} @ ${p.Price}");
                Console.WriteLine($" Stock Level: {p.StockLevel}");
                if (p.OnSale == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" ~!!! Currently on SALE !!!~");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.WriteLine(" No current sales promotion on this item.");
                }
                Console.WriteLine($" CategoryID: {p.CategoryID}");
                Console.WriteLine("");
            }

            //PRODUCT CREATION

            var repoCategories = new DapperCategoriesRepository(conn);
            var cats = repoCategories.GetAllCategories(); //TO CALL CATEGORIES IN PROGRAM

            //ASK USER IF THEY WANT TO ADD A PRODUCT TO THE DB
            Console.WriteLine("*----------------------------------------------------*");
            Console.WriteLine(" Add a product to the catalog?");
            Console.WriteLine(" Yes / No");
            var userInput = Console.ReadLine().ToLower();
            switch (userInput) //SWITCH CASE TO TAKE IN USER INPUT
            {
                case "y":
                case "yes":
                case "yup":
                case "okay":
                case "ok":
                case "sure":
                case "affirmative":
                case "yea":
                case "yeah":
                case "true":
                    Console.WriteLine(" Please provide the following information:\n"); //IF YES THEN CREATION TAKES PLACE HERE
                    Console.WriteLine(" What is the product name:");
                    var newProductName = Console.ReadLine();
                    Console.WriteLine(" What is the price of this item?");
                    var newProductPrice = double.Parse(Console.ReadLine());
                    Console.WriteLine(" Please review the following categories:");
                    foreach (var c in cats)       
                    {
                        Console.WriteLine($" CategoryID: {c.CategoryID} is {c.Name}"); //FOREACH LOOP TO LIST CATEGORIES
                    }
                    Console.WriteLine("");
                    Console.WriteLine(" Your product belongs to which category?");
                    var userCatID = Console.ReadLine().ToLower();
                    int newProductCat = 0;

                    switch (userCatID) //SWITCH CASE FOR CATEGORY ASSIGNING
                    {
                        case "1":
                        case "computers":
                            newProductCat = 1;
                            break;
                        case "2":
                        case "appliances":
                            newProductCat = 2;
                            break;
                        case "3":
                        case "phones":
                            newProductCat = 3;
                            break;
                        case "4":
                        case "audio":
                            newProductCat = 4;
                            break;
                        case "5":
                        case "home theater":
                            newProductCat = 5;
                            break;
                        case "6":
                        case "printers":
                            newProductCat = 6;
                            break;
                        case "7":
                        case "music":
                            newProductCat = 7;
                            break;
                        case "8":
                        case "games":
                            newProductCat = 8;
                            break;
                        case "9":
                        case "services":
                            newProductCat = 9;
                            break;
                        default:
                            newProductCat = 10;
                            Console.WriteLine(" CategoryID 10: Other assigned.");
                            break;
                    }
                    Console.WriteLine(" How many to stock");
                    var newProductStock = int.Parse(Console.ReadLine());
                    Console.WriteLine(" Thank you. Product is being added.");
                    repoProducts.CreateProduct(newProductName, newProductPrice, newProductCat, newProductStock); //PRODUCT IS CREATED HERE AFTER ALL USER INPUTS
                    break;
                default:
                    Console.WriteLine(" No products will be added. Thank you.");
                    break;
            }

            //LIST ALL PRODUCTS TO CONSOLE AGAIN
            prods = repoProducts.GetAllProducts();
            Console.WriteLine("*----------------------------------------------------*");
            foreach (var p in prods)
            {
                Console.WriteLine($" ProductID:{p.ProductID}--------------------");
                Console.WriteLine($" * {p.Name} @ ${p.Price}");
                Console.WriteLine($" Stock Level: {p.StockLevel}");
                if (p.OnSale == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" ~!!! Currently on SALE !!!~");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.WriteLine(" No current sales promotion on this item.");
                }
                Console.WriteLine($" CategoryID: {p.CategoryID}");
                Console.WriteLine("");
            }

            //ASKING USER IF THEY WANT TO UPDATE A PRODUCT
            Console.WriteLine("*----------------------------------------------------*");
            Console.WriteLine(" Do you want to update a product?");
            Console.WriteLine(" Yes / No");
            var userUpdateInput = Console.ReadLine().ToLower();
            switch (userUpdateInput)
            {
                case "y":
                case "yes":
                case "yup":
                case "okay":
                case "ok":
                case "sure":
                case "affirmative":
                case "yea":
                case "yeah":
                case "true":
                    Console.WriteLine(" Please provide the ProductID to update:"); //USER MUST PROVIDE PRODUCT ID
                    var productToUpdate = repoProducts.GetProductById(int.Parse(Console.ReadLine()));
                    Console.WriteLine(" Updated Name = ");
                    productToUpdate.Name = Console.ReadLine();
                    Console.WriteLine(" Updated Price = ");
                    productToUpdate.Price = double.Parse(Console.ReadLine());
                    Console.WriteLine(" Updated StockLevel = ");
                    productToUpdate.StockLevel = int.Parse(Console.ReadLine());
                    Console.WriteLine(" Please review the following categories:"); //LISTING CATEGORIES TO CONSOLE AGAIN
                    foreach (var c in cats)
                    {
                        Console.WriteLine($" CategoryID: {c.CategoryID} is {c.Name}");
                    }
                    Console.WriteLine("");
                    Console.WriteLine(" Updated CategoryID = ");
                    var updateCatID = Console.ReadLine().ToLower();
                    switch (updateCatID) //SWITCH TO UPDATE CATEGORY ID
                    {
                        case "1":
                        case "computers":
                            productToUpdate.CategoryID = 1;
                            break;
                        case "2":
                        case "appliances":
                            productToUpdate.CategoryID = 2;
                            break;
                        case "3":
                        case "phones":
                            productToUpdate.CategoryID = 3;
                            break;
                        case "4":
                        case "audio":
                            productToUpdate.CategoryID = 4;
                            break;
                        case "5":
                        case "home theater":
                            productToUpdate.CategoryID = 5;
                            break;
                        case "6":
                        case "printers":
                            productToUpdate.CategoryID = 6;
                            break;
                        case "7":
                        case "music":
                            productToUpdate.CategoryID = 7;
                            break;
                        case "8":
                        case "games":
                            productToUpdate.CategoryID = 8;
                            break;
                        case "9":
                        case "services":
                            productToUpdate.CategoryID = 9;
                            break;
                        default:
                            productToUpdate.CategoryID = 10;
                            Console.WriteLine(" CategoryID 10: Other assigned.");
                            break;
                    }
                    Console.WriteLine(" Updated On Sale =");
                    var userUpdateSale = Console.ReadLine().ToLower();
                    switch (userUpdateSale) //SWITCH CASE TO UPDATE SALE STATUS
                    {
                        case "y":
                        case "yes":
                        case "yup":
                        case "okay":
                        case "ok":
                        case "sure":
                        case "affirmative":
                        case "yea":
                        case "yeah":
                        case "true":
                        case "1":
                            productToUpdate.OnSale = 1;
                            break;
                        default:
                            productToUpdate.OnSale = 0;
                            break;
                    }
                    repoProducts.UpdateProduct(productToUpdate); //PRODUCT IS UPDATED
                    Console.WriteLine(" Product has been updated:");
                    break;
                default:
                    Console.WriteLine(" No products will be updated. Thank you.");
                    break;

            }
            prods = repoProducts.GetAllProducts(); //UPDATE PRODS TO CALL ALL PRODUCTS TO CONSOLE TO SEE UPDATED PRODUCT
            Console.WriteLine("*----------------------------------------------------*");
            foreach (var p in prods)
            {
                Console.WriteLine($" ProductID:{p.ProductID}--------------------");
                Console.WriteLine($" * {p.Name} @ ${p.Price}");
                Console.WriteLine($" Stock Level: {p.StockLevel}");
                if (p.OnSale == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" ~!!! Currently on SALE !!!~");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.WriteLine(" No current sales promotion on this item.");
                }
                Console.WriteLine($" CategoryID: {p.CategoryID}");
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("*----------------------------------------------------*");
            Console.WriteLine(" Do you want to delete a product?"); //ASKING USER IF THEY WANT TO DELETE A PRODUCT
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(" WARNING: THIS IS NOT REVERSABLE");
            Console.WriteLine(" The product will be deleted from all records.");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Yes / No");
            var userDeleteProduct = Console.ReadLine().ToLower();
            switch (userDeleteProduct) //SWITCH AGAIN 
            {
                case "y":
                case "yes":
                case "yup":
                case "okay":
                case "ok":
                case "sure":
                case "affirmative":
                case "yea":
                case "yeah":
                case "true":
                    Console.WriteLine(" Please enter the item's ProductID to be deleted:"); 
                    var itemTBD = int.Parse(Console.ReadLine());
                    var byeBye = repoProducts.GetProductById(itemTBD); 

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(" The following item will be deleted:");
                    Console.WriteLine($" ProductID: {byeBye.ProductID}");
                    Console.WriteLine($" {byeBye.Name}");
                    Console.WriteLine(" ..........");
                    Console.WriteLine(" Product has been deleted");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    repoProducts.DeleteProduct(byeBye.ProductID); //PRODUCT IS DELETED HERE
                    Console.WriteLine(" Please review the product list to ensure your item was deleted.\n");
                    Console.ReadLine();

                    prods = repoProducts.GetAllProducts(); //UPDATE PRODS TO CALL ALL PRODUCTS TO CONSOLE TO SEE UPDATED PRODUCT 
                    foreach (var p in prods)
                    {
                        Console.WriteLine($" ProductID:{p.ProductID}--------------------");
                        Console.WriteLine($" * {p.Name} @ ${p.Price}");
                        Console.WriteLine($" Stock Level: {p.StockLevel}");
                        if (p.OnSale == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(" ~!!! Currently on SALE !!!~");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            Console.WriteLine(" No current sales promotion on this item.");
                        }
                        Console.WriteLine($" CategoryID: {p.CategoryID}");
                        Console.WriteLine("");
                    }
                    break;
                default:
                    Console.WriteLine(" No items will be deleted.");
                    break;
            }

            Console.ReadLine();

            #endregion
        }
    }

}
