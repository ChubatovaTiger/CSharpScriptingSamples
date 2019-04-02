using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;
using Newtonsoft.Json;

namespace Sample05
{
	class Program
	{
		static async Task Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("Type 1, 2 or 3. Press Ctrl + C to exit.");
				var input = Console.ReadLine();
				switch (input)
				{
					case "1":
						await Run();
						break;
					case "2":
						Console.WriteLine("Type temperature in °F: ");
						if (double.TryParse(Console.ReadLine(), out var tempInF))
						{
							await RunWithHost(tempInF);
						}
						else
						{
							Console.WriteLine("Sorry, I didn't understand that.");
						}
						break;
					case "3":
						await DynamicLambda();
						break;
					default:
						Console.WriteLine("What is so difficult about 1, 2 or 3???");
						break;
				}
			}
		}

		private static async Task Run()
		{
			var sourceCode = File.ReadAllText("script01.csx");
			await CSharpScript.RunAsync(sourceCode);
		}

		private static async Task RunWithHost(double input)
		{
			var code = File.ReadAllText("script02.csx");

			var result = await CSharpScript.EvaluateAsync<double>(code,
				globals: new ScriptHost { Number = input });
			Console.WriteLine($@"{input}°F is {result:0.00}°C");
		}

		private static List<Product> AllProducts()
		{
			return new List<Product>() {
				new Product{ Id = 1, ClientId = 1, Price = 123.50M, Name = "Product1" },
				new Product { Id = 2, ClientId = 1, Price = 130.99M, Name = "Product2" },
				new Product { Id = 3, ClientId = 2, Price = 1500, Name = "Product3" },
				new Product { Id = 4, ClientId = 2, Price = 10.99M, Name = "Product4" },
				new Product { Id = 5, ClientId = 2, Price = 54, Name = "Product5" },
				new Product { Id = 6, ClientId = 3, Price = 13400, Name = "Product6" },
				new Product { Id = 7, ClientId = 3, Price = 12.99M, Name = "Product7" },
				new Product { Id = 8, ClientId = 4, Price = 987, Name = "Product8" },
				new Product { Id = 9, ClientId = 4, Price = 44, Name = "Product9" },
				new Product { Id = 10, ClientId = 4, Price = 434, Name = "Product10" },
				new Product { Id = 11, ClientId = 11, Price = 4325.99M, Name = "Product11" },
				new Product { Id = 12, ClientId = 11, Price = 2, Name = "Product12" },
				new Product { Id = 13, ClientId = 11, Price = 4.4M, Name = "Product13" },
				new Product { Id = 14, ClientId = 11, Price = 4.2M, Name = "Product14" },
				new Product { Id = 15, ClientId = 11, Price = 100.25M, Name = "Product15" }
			};
		}

		private static async Task DynamicLambda()
		{
			var productFilter = "product => (product.ClientId == 11 && product.Price > 100)";
			var scriptOptions = ScriptOptions.Default.AddReferences(typeof(Product).Assembly);
			var expression = await CSharpScript.EvaluateAsync<Func<Product, bool>>(productFilter, scriptOptions);

			var myProducts = AllProducts().Where(expression);
			var output = JsonConvert.SerializeObject(myProducts.ToArray(), Formatting.Indented);
			Console.WriteLine(output);
		}
	}
}
