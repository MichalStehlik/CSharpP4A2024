// See https://aka.ms/new-console-template for more information
// Vytvoření nového vlákna
Thread thread = new Thread(new ThreadStart(DoWork));

// Spuštění vlákna
thread.Start();

// Pokračování hlavního vlákna
for (int i = 0; i < 5; i++)
{
    Console.WriteLine("Hlavní vlákno: " + i);
    Thread.Sleep(1000); // Simulace práce ve vlákně
}
static void DoWork()
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine("Vedlejší vlákno: " + i);
        Thread.Sleep(1000); // Simulacetoho, že operace chvíli trvá
    }
}