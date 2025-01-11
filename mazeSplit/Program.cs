using System;
using AdventureGameNamespace;
using System.Reflection;

namespace AdventureGameNamespace {
public class Program
{
    public static void Main(string[] args)
    {
        AdventureGame game = new AdventureGame();
        
    string RESET = "\u001B[0m";   
    string BLACK = "\u001B[0;30m";   
    string RED = "\u001B[0;31m";     
    string GREEN = "\u001B[0;32m";   
    string YELLOW = "\u001B[0;33m";  
    string BLUE = "\u001B[0;34m";    
    string PURPLE = "\u001B[0;35m";  
    string CYAN = "\u001B[0;36m";    
    string WHITE = "\u001B[0;37m";   
    string GREEN_BACKGROUND = "\u001B[42m";
    string BLUE_BACKGROUND = "\u001B[44m";

        while (true)
        {
            game.PrintGrid();
            Console.WriteLine(YELLOW + "\nYou are currently in : " + game.CurrentRoom.ToString());
            Console.WriteLine("There are " + game.numRooms + " Rooms");
            Console.WriteLine("You need to reach " + game.Finalroom + RESET);
            Console.WriteLine(BLUE + "\nRoom Navigator" + RESET);
            Console.WriteLine(GREEN + "1. Knock on all Walls");
            Console.WriteLine("2. Traverse Room");
            Console.WriteLine("3. Pick up objects");
            Console.WriteLine("4. Drop key");
            Console.WriteLine("5. Unlock exit");
            Console.WriteLine("6. Exit" + RESET);
            Console.Write(PURPLE + "Keys in your possession - " + RESET);
            foreach (Key key in game.KeyList)
            {   
                Console.Write(key.GetNumber() + " ");
            }
            Console.WriteLine();
            Console.WriteLine(CYAN + "Number of Flowers: " + game.Bombs.Count + RESET);
            Console.WriteLine(YELLOW + "Number of Lives: " + game.NumOfLives.ToString() + RESET);

            Console.WriteLine();
            
            if (game.CurrentRoom.IsFinal() && game.Bombs.Count == game.Grid_Size)
            {
                game.PrintGrid();
                Console.WriteLine("You made it to the final room with all your flowers");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                game.ReinitializeProgram();
                Console.Clear();
            } else if (game.CurrentRoom.IsFinal() && game.Bombs.Count < 6)
            {
                Console.WriteLine(RED + "You're in the final room but you don't have all your flowers, return with them" + RESET);
            }
            Console.Write("Choice:");
            
        string choice = Console.ReadLine();
        
            switch (choice)
            {
                case "1":
                    game.KnockWalls();
                    break;
                case "2":
                    game.TraverseRoom();
                    break;
                case "3":
                    bool gotTheObject = game.PickUpObject();
                    if (!gotTheObject && game.NumOfLives == 0){
                        game.PrintGrid();
                        Console.WriteLine("You died... Painfully");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        game.ReinitializeProgram();
                        break;
                    } else if (!gotTheObject && game.NumOfLives > 0)
                    {
                        game.LoseLife();
                        Console.WriteLine("You lost a life... Ouch!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        break;
                    }
                    else { break;
                    }
                case "4":
                    game.DropKey();
                    break;
                case "5":
                    game.UnlockExit();
                    break;
                case "6":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}

// Main adventure game class. Manages the grid, rooms, keys, and player actions.

    public partial class AdventureGame
{
// Prints the grid in an ASCII-based format, showing rooms, connections, and locked keys.

public void PrintGrid()
{
    // We print top row last to appear at "top" in console
    for (int i = GRID_SIZE - 1; i >= 0; i--)
    {
        // Print rooms and horizontal connections
        for (int j = 0; j < GRID_SIZE; j++)
        {
            if (grid[i, j] != null)
            {
                Room room = grid[i, j];
                string roomName = room.HasKey()
                    ? room.ToString() + "(" + room.GetKey().ToString() + ")"
                    : room.ToString();
                roomName = room.HasBomb() ? roomName + "(*)" : roomName;
                // If we need to reapply colours
                /*roomName = (room == currentRoom) ? BLUE_BACKGROUND + roomName + RESET : roomName;
                roomName = room.IsFinal() ? GREEN_BACKGROUND + roomName + RESET : roomName;*/
                Console.Write(string.Format("{0,-11}", roomName));
            }
            else
            {
                Console.Write("           ");
            }

            // Horizontal exit indicator
            if (j < GRID_SIZE - 1)
            {
                if (grid[i, j] != null && grid[i, j].GetExit("East") != null)
                {
                    // If locked, show key number
                    if (grid[i, j].IsExitLocked("East"))
                    {
                        string keyName = grid[i, j].GetExit("East").GetKey().ToString();
                        Console.Write(string.Format("{0,-11}", "    *(" + keyName + ")   "));
                    }
                    else
                    {
                        Console.Write("    -      ");
                    }
                }
                else
                {
                    Console.Write("           ");
                }
            }
        }

        Console.WriteLine();

        // Print vertical connections
        if (i > 0)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (grid[i, j] != null && grid[i, j].GetExit("South") != null)
                {
                    if (grid[i, j].IsExitLocked("South"))
                    {
                        string keyName = grid[i, j].GetExit("South").GetKey().ToString();
                        Console.Write(string.Format("{0,-11}", "    *(" + keyName + ")  "));
                    }
                    else
                    {
                        Console.Write("    |      ");
                    }
                }
                else
                {
                    Console.Write("           ");
                }

                if (j < GRID_SIZE - 1)
                {
                    Console.Write("           ");
                }
            }

            Console.WriteLine();
        }
    }
}

    // Shows which directions are available from the current room.

    public void KnockWalls()
    {
        List<string> directionList = new List<string> { "North", "East", "South", "West" };
        Console.WriteLine();
        foreach (string direction in directionList)
        {
            if (currentRoom.GetExit(direction) != null)
            {   
                string exitName = currentRoom.IsExitLocked(direction) ? direction + " requires key " + currentRoom.GetExit(direction).GetKey().ToString(): direction + " is unlocked";
                
                Console.Write(RED + exitName + "; " + RESET);
            }
        }
        Console.WriteLine();
    }

 
    // Moves the player to an adjacent room if not locked.
    public void TraverseRoom()
    {
        Console.Write("Direction to Traverse (N/S/W/E): ");
        string choice = (Console.ReadLine() ?? "").ToUpper();

        switch (choice)
        {
            case "N":
                Move("North");
                break;
            case "S":
                Move("South");
                break;
            case "W":
                Move("West");
                break;
            case "E":
                Move("East");
                break;
            default:
                Console.WriteLine("Invalid direction. Please enter N, S, W, or E.");
                break;
        }
    }

    // Help function for taversing
    private void Move(string direction)
    {
        Exit exit = currentRoom.GetExit(direction);
        if (exit != null)
        {
            if (exit.IsLocked())
            {
                Console.WriteLine("Room is locked");
            }
            else
            {
                currentRoom = exit.GetOtherRoom(currentRoom);
                Console.WriteLine("You moved to: " + currentRoom.ToString());
            }
        }
        else
        {
            Console.WriteLine("No exit in that direction");
        }
    }
    
    // Picks up the key in the current room if available and if the player has fewer than 3 keys.
    public bool PickUpObject()
    {
        if (!currentRoom.HasKey() && !currentRoom.HasKey())
        {Console.WriteLine("\nNo objects to pick up in this room");}
        
        if (currentRoom.HasKey())
        {
            if (keyList.Count < 3)
            {
                Console.WriteLine("\nYou picked up Key " + currentRoom.GetKey().ToString());
                keyList.Add(currentRoom.GetKey());
                currentRoom.SetKey(null);
            }
            else
                {
                Console.WriteLine("\nYou have too many keys");
            }
        }

        if (currentRoom.HasBomb())
        {
            Console.WriteLine("Oh no, you tiggered a bomb! You have 30s to disarm it");
            return SolveAnagram(currentRoom.GetBomb()) ? true : false;
        }
        
        return true;

    }
    
    // Drops a key from the player's inventory into the current room, if the room is empty.
    public void DropKey()
    {
        Console.WriteLine("Which key to drop:");
        string choice = Console.ReadLine();

        Key key = null;
        try
        {
            int number = int.Parse(choice);
            key = new Key(number);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid Selection.");
            return;
        }

        if (keyList.Contains(key) && !currentRoom.HasKey())
        {
            keyList.Remove(key);
            currentRoom.SetKey(key);
        }
        else if (currentRoom.HasKey() && keyList.Contains(key))
        {
            Console.WriteLine("Room already contains key");
        }
        else
        {
            Console.WriteLine("Invalid Selection.");
        }
    }


    // Attempts to unlock a locked exit using a key in the player's inventory.

    public void UnlockExit()
    {
        Console.Write("Direction to Unlock (N/S/W/E): ");
        string choice = (Console.ReadLine() ?? "").ToUpper();

        switch (choice)
        {
            case "N":
                Unlock("North");
                break;
            case "S":
                Unlock("South");
                break;
            case "W":
                Unlock("West");
                break;
            case "E":
                Unlock("East");
                break;
            default:
                Console.WriteLine("Invalid direction. Please enter N, S, W, or E.");
                break;
        }
    }

    void Unlock(string direction)
    {
        Exit exit = currentRoom.GetExit(direction);
        if (exit != null && currentRoom.IsExitLocked(direction) && keyList.Contains(exit.GetKey()))
        {
            exit.SetLocked(false);
            Console.WriteLine("\nDoor unlocked.");
            keyList.Remove(exit.GetKey());
        }
        else if (exit != null && !exit.IsLocked())
        {
            Console.WriteLine("\nThe exit is not locked.");
        }
        else
        {
            Console.WriteLine("\nCannot unlock door with current keys.");
        }
            }
        
    }
}
