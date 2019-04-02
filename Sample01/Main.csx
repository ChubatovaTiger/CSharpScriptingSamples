// 1) no main class, no project, no main method

// 2) common usings are implicitly available

// 3) you can reference other libraries and files
#r "nuget:Newtonsoft.Json,12.0.1"
#load "Fibonacci.csx"

using Newtonsoft.Json;

// you can access the host object to pass in arguments
var args = Args.FirstOrDefault();
int n = 10;
if (!string.IsNullOrEmpty(args)) {
    n = int.Parse(args);
    if (n <= 0) {
        throw new ArgumentOutOfRangeException(nameof(Args));
    }
}

// some global variable
var numbers = new List<FibonacciNumber>();

// global statement
for (int i = 1; i <= n; i++)
{
   var nthNumber = new FibonacciNumber{N = i, Number = Fibonacci(i)};
   numbers.Add(nthNumber);
}

// you can call global method
var output = JsonConvert.SerializeObject(numbers.ToArray(), Formatting.Indented);
Console.WriteLine(output);