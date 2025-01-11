namespace AdventureGameNamespace {
public partial class AdventureGame
{
    private static readonly int GRID_SIZE = 6; // Adjust as needed
    private readonly Room[,] grid;
    private readonly Random random;
    
    private Room currentRoom;
    private Room finalRoom;
    public int numRooms;
    
    private int numOfLives = 3;
    private readonly List<Key> keyList;
    private readonly List<Room> allRooms = new List<Room>();
    private static List<Bomb> bombs = new List<Bomb>();
    
    // colour codes (ANSI sequences). 
    private const string RESET = "\u001B[0m";   
    private const string BLACK = "\u001B[0;30m";   
    private const string RED = "\u001B[0;31m";     
    private const string GREEN = "\u001B[0;32m";   
    private const string YELLOW = "\u001B[0;33m";  
    private const string BLUE = "\u001B[0;34m";    
    private const string PURPLE = "\u001B[0;35m";  
    private const string CYAN = "\u001B[0;36m";    
    private const string WHITE = "\u001B[0;37m";   
    private const string GREEN_BACKGROUND = "\u001B[42m";
    private const string BLUE_BACKGROUND = "\u001B[44m";
    
    public Room CurrentRoom => currentRoom;
    public int NumOfLives { get; private set; } = 3;
    public List<Key> KeyList => keyList;
    public Room Finalroom => finalRoom;
    public List<Bomb> Bombs => bombs;

    public int Grid_Size => GRID_SIZE;
    
    public AdventureGame()
    {
        grid = new Room[GRID_SIZE, GRID_SIZE];
        keyList = new List<Key>();
        bombs = new List<Bomb>();
        random = new Random();
    
        DisplayAnimatedMessage("Setting up Room Grid randomly...");
        SetupRoomsRandomly();

        DisplayAnimatedMessage("Evaluating longest path for Start and End Rooms...");
        InitializeCurrentRoom();

        DisplayAnimatedMessage("Removing random cyclical paths from grid...");
        PruneCycles();
    
        DisplayAnimatedMessage("Collecting anagrams...");
        CollectJsonObjects();
        
        DisplayAnimatedMessage("Going on a date with your mama...");
    
        DisplayAnimatedMessage("Placing keys to avoid deadlocks...");
        AssignKeysToRoomsWithTimeout();
    
        DisplayAnimatedMessage("Placing bombs...");
        AssignBombsToRooms();
    }
    
    public void LoseLife()
    {
        NumOfLives--;
    }

    private void DisplayAnimatedMessage(string message, int interval = 100)
    {   
        Random random = new Random();
        int animationTime = random.Next(1000, 4001); // Random time between 1 and 4 seconds
        int cycles = animationTime / interval;
        string[] animationFrames = { "/", "-", "\\", "|" };
        Console.Write(message);

        for (int i = 0; i < cycles; i++)
        {
            Console.Write(animationFrames[i % animationFrames.Length]);
            Thread.Sleep(interval);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop); // Move cursor back
        }
        Console.WriteLine();
    }
    
    // If the user dies or game setup fails reinitialize
    public void ReinitializeProgram()
    {
        Console.WriteLine("Reinitializing the game setup...");
        // Reset all necessary game state components
        Array.Clear(grid, 0, grid.Length);
        keyList.Clear();
        bombs.Clear();
        allRooms.Clear();

        // Re-run setup logic
        SetupRoomsRandomly();
        InitializeCurrentRoom(); 
        PruneCycles();      
        CollectJsonObjects();

        try
        {
            AssignKeysToRoomsWithTimeout();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Critical failure during reinitialization: {ex.Message}");
            Environment.Exit(1);
        }

        AssignBombsToRooms();
    }
}
}
