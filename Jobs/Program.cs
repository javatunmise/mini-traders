using System;
using System.Collections.Generic;
using System.Linq;
using static Jobs.LoadCategories;

namespace Jobs
{
    class Program
    {
        static void Main(string[] args)
        {
            var categories = LoadCategories.GetCategories();

            foreach (var c in categories)
            {
                Console.WriteLine($"INSERT INTO Categories (Name, TempId, TempParentId) VALUES ('{c.Name}', {c.Id}, {c.Parent?.Id??0});");
            }

            Console.WriteLine();

            foreach (var c in categories.Where(c => c.Parent != null))
            {
                Console.WriteLine($"UPDATE Categories SET ParentId = (SELECT Id FROM Categories WHERE TempId = {c.Parent.Id}) WHERE tempId={c.Id};");
            }

            Console.ReadLine();
        }
    }
}
