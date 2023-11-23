using ConsoleTools;
using GXI86S_HFT_2023241.Models;
using System;
using System.Collections.Generic;

namespace GXI86S_HFT_2023241.Client
{
    internal class Program
    {
        static RestService rest;
        static void Create(string entity)
        {
            if (entity == "Customer")
            {
                Console.Write("Enter Customer Name: ");
                string name = Console.ReadLine();
                rest.Post(new Customer() { LastName = name }, "customer");
            }
        }
        static void List(string entity)
        {
            if (entity == "Customer")
            {
                List<Customer> actors = rest.Get<Customer>("customer");
                foreach (var item in actors)
                {
                    Console.WriteLine(item.Id + ": " + item.LastName + "  Balance: " + item.Email);
                }
            }
            Console.ReadLine();
        }
        static void Update(string entity)
        {
            if (entity == "Customer")
            {
                Console.Write("Enter Customer's id to update: ");
                int id = int.Parse(Console.ReadLine());
                Customer one = rest.Get<Customer>(id, "Customer");
                Console.Write($"New name [old: {one.Email}]: ");
                string name = Console.ReadLine();
                one.Email = name;
                rest.Put(one, "Customer");
            }
        }
        static void Delete(string entity)
        {
            if (entity == "Customer")
            {
                Console.Write("Enter Customer's id to delete: ");
                int id = int.Parse(Console.ReadLine());
                rest.Delete(id, "Customer");
            }
        }

        static void Main(string[] args)
        {
            rest = new RestService("http://localhost:53910/", "movie");

            var actorSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Customer"))
                .Add("Create", () => Create("Customer"))
                .Add("Delete", () => Delete("Customer"))
                .Add("Update", () => Update("Customer"))
                .Add("Exit", ConsoleMenu.Close);
            
            var roleSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Account"))
                .Add("Create", () => Create("Account"))
                .Add("Delete", () => Delete("Account"))
                .Add("Update", () => Update("Account"))
                .Add("Exit", ConsoleMenu.Close);
            
            var directorSubMenu = new ConsoleMenu(args, level: 1)
                .Add("List", () => List("Transaction"))
                .Add("Create", () => Create("Transaction"))
                .Add("Delete", () => Delete("Transaction"))
                .Add("Update", () => Update("Transaction"))
                .Add("Exit", ConsoleMenu.Close);

            var menu = new ConsoleMenu(args, level: 0)
                .Add("Customer", () => actorSubMenu.Show())
                .Add("Roles", () => roleSubMenu.Show())
                .Add("Directors", () => directorSubMenu.Show())
                .Add("Exit", ConsoleMenu.Close);

            menu.Show();

        }
    }
}