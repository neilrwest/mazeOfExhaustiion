# mazeOfExhaustiion
Adventure text-based game written in C# Gameplay is faily mundane but the use of recursive graph algorthims BFS and DFS used to setup the maze are fairly advanced.

- At the start of the game a random maze of rooms is set up that you can traverse. You will need to build a mental map of the grid or draw it on a piece of paper as you proceed. Knock on walls to feel your way around in the dark.
- You are trying to navigate from a starting room to a finishing room but you will need all six flowers when entering the final room to win the game.
- Some of the exits between the rooms are locked. You can find the corresponding key by picking up objects in any room. One key per room and three keys in your inventory maximum. You can also drop keys.
- Instead of keys some rooms contain bombs which will be triggered when you attempt to pick up objects. You will have 30 seconds to disarm the bomb by solving an anagram.
- If the bomb explodes you will lose a life. Disarming a bomb converts it into a flower that automatically goes into your inventory.
- You will need to revisit exploded bombs (don't ask me how the physics work) to solve them as you need to convert all 6 bombs into flowers to solve the maze.
- Once you lose all lives or return all flowers to the final room, a graphical representation of the maze will be displayed for you in daylight.
