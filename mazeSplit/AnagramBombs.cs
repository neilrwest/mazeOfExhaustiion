using System.Text.Json;
using System.Reflection;
using System;
using System.Threading;

namespace AdventureGameNamespace
{
    public partial class AdventureGame
    {
        // Simple assignment of bombs to rooms with few preconditions
        // i.e. No Key, No Existing Bomb, nor start or finish rooms
        private void AssignBombsToRooms()
        {
            // Original random assignment logic
            var bombEnumerator = bombs.ToList();
            foreach (var bomb in bombEnumerator)
            {
                int roomI, roomJ;
                do
                {
                    roomI = random.Next(GRID_SIZE);
                    roomJ = random.Next(GRID_SIZE);
                }
                while (grid[roomI, roomJ] == null ||
                       grid[roomI, roomJ].HasKey() ||
                       grid[roomI, roomJ].HasBomb() ||
                       grid[roomI, roomJ].IsFinal() ||
                       grid[roomI, roomJ] == currentRoom);
                grid[roomI, roomJ].SetBomb(bomb);
                bombs.Remove(bomb);
            }
        }

        // Collect anagrams randomly from associated JSON file and places them in a bombs list
        private void CollectJsonObjects()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "mazeSplit.anagrams.json"; // Replace "YourNamespace" with your actual namespace
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonString = reader.ReadToEnd();
                List<Bomb> allBombs = JsonSerializer.Deserialize<List<Bomb>>(jsonString);
                bombs = GetRandomObjects(GRID_SIZE, allBombs);
                if (allBombs == null || allBombs.Count == 0)
                {
                    throw new Exception("No objects found in the JSON file.");
                }
            }
        }

        // Helper function to assign anagrams to bombs randomly
        public static List<Bomb> GetRandomObjects(int count, List<Bomb> allbombs)
        {
            if (count > allbombs.Count)
            {
                throw new ArgumentException("The number of objects to select exceeds the total available objects.");
            }
            // Create a copy of the list to avoid modifying the original
            List<Bomb> copy = new List<Bomb>(allbombs);
            // Shuffle the list
            Random random = new Random();
            for (int i = copy.Count - 1; i > 0; i--)
            {
                int swapIndex = random.Next(i + 1);
                (copy[i], copy[swapIndex]) = (copy[swapIndex], copy[i]);
            }
            // Return the first 'count' objects from the shuffled list
            return copy.GetRange(0, count);
        }

        public class CountdownTimer
        {
            private bool isCountdownComplete;
            private bool hasExploded;

            // Expose this read-only property to check if the bomb has exploded
            public bool HasExploded => hasExploded;

            public void StartCountdown(int countdownSeconds)
            {
                isCountdownComplete = false;
                hasExploded = false;
                Thread countdownThread = new Thread(() => ShowCountdown(countdownSeconds));
                countdownThread.Start();
            }

            public void StopCountdown()
            {
                isCountdownComplete = true;
            }

            private void ShowCountdown(int countdownSeconds)
            {
                int countdownLine = Console.CursorTop; // Remember the current line for writing the countdown
                for (int i = countdownSeconds; i >= 0; i--)
                {
                    // If the bomb is disarmed (StopCountdown called), break out of the loop
                    if (isCountdownComplete) return;
                    // Save the cursor position where user might be typing
                    int currentCursorLeft = Console.CursorLeft;
                    int currentCursorTop = Console.CursorTop;
                    // Move to the right side of the console to update countdown
                    int rightMargin = Console.WindowWidth - 30;
                    Console.SetCursorPosition(rightMargin, countdownLine);
                    Console.Write($"Time remaining: {i} seconds ");
                    // Restore the cursor to the user input position
                    Console.SetCursorPosition(currentCursorLeft, currentCursorTop);
                    Thread.Sleep(1000); // 1-second intervals
                }
                // If we got here, it means we did NOT break out due to disarming
                // => Time ran out, so the bomb "explodes"
                hasExploded = true;
                Console.SetCursorPosition(Console.WindowWidth - 30, countdownLine);
                Console.WriteLine("Bomb Exploded!");
            }
        }

        public bool SolveAnagram(Bomb bomb)
        {
            // Display anagram and context
            string display = $"Anagram: {bomb.Anagram}\nContext: {bomb.Context}";
            Console.WriteLine(display);
            // Initialize and start the timer
            CountdownTimer timer = new CountdownTimer();
            timer.StartCountdown(30);
            // Keep asking the user for the correct solution unless time runs out
            while (!timer.HasExploded) // Stop if the bomb explodes
            {
                Console.Write("Enter the correct solution to disarm the bomb: ");
                string userInput = Console.ReadLine();
                if (string.Equals(userInput, bomb.Solution, StringComparison.OrdinalIgnoreCase))
                {
                    // If correct solution, disarm by stopping the countdown
                    timer.StopCountdown();
                    Console.WriteLine("\nCorrect solution entered. Bomb disarmed. And for some magical reason it changed into a Flower!");
                    break;
                }
                else
                {
                    if (!timer.HasExploded) { Console.WriteLine("Incorrect answer. Try again."); }
                }
                // A small delay to avoid spamming the console
                Thread.Sleep(100);
            }
            // If the bomb hasn't exploded, we assume it’s disarmed
            // Clear the bomb from the current room
            if (!timer.HasExploded)
            {
                currentRoom.SetBomb(null);
                bombs.Add(bomb);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}