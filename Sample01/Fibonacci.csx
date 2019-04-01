public long Fibonacci(int n) {
    long a = 1, b = 1;
    long result = 0;

    if (n <= 0) {
        throw new ArgumentOutOfRangeException(nameof(n));
    }

    if (n == 1 || n == 2) {
        return 1;
    }

    for (int i = 3; i <= n; i++) {
        result = a + b;
        a = b;
        b = result;
    }

    return result;
}

// you can define class as usual
public class FibonacciNumber
{
    public int N { get; set; }

    public long Number { get; set; }
}

// Fibonacci(-5);