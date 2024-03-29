# Roguelike Console Game C#

## Game Demo

![Последовательность 01 (1)](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/29167920-6348-4f87-a3d5-c02b02381b94)

---

 ## Components of the project
 
 <ul>
  <li>Maze Generator - A generator that generates a guaranteed passable maze that contains an entrance and an exit</li>
  <li>A player class containing drawing, movement, and health management methods</li>
  <li>An enemy class containing drawing, movement, and attack methods
     <ul>
      <li>A zombie that can attack the player when it gets close to him. It is indicated by the letter <b>Z</b>.</li>
      <li>A shooter who can shoot a projectile and thus cause damage to the player. It is indicated by the letter <b>S</b>.</li>
     </ul>
  </li>
 </ul>

 ---

 ## Examples of a maze
 
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/2a34f443-4388-400a-bd0b-24dbc514c5b2)
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/9a7b3783-836c-4869-9bf6-8ccf363171b4)
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/241eaea6-fea0-491e-a104-43cfe8e661ab)
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/103ad217-0c28-4c95-b3cc-15e37b79e03a)

---

## The algorithm for creating a maze

> A little bit about the algorithm for creating a maze:  
> First, the maze is filled with emptiness, after which  
> the boundaries of the maze are set. Then we go through  
> the "control points". A control point is some point in  
> the maze (not the border) with even coordinates, for  
> example [2,2], [2,4] and so on. At each such point,  
> it is checked whether there is already a wall there,  
> and if there is no wall, then we choose a random direction  
> {top, bottom, left, right} and start putting walls in this  
> direction to the nearest wall or to the maximum length of the wall.  
> And so we go through all the checkpoints.

---

 ## Ingame screenshots
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/59fe356f-ef97-4685-80ba-67b43bd0d851)  
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/81b54845-a1cd-48d2-b7a5-dd7b8dc29c07)  
 ![image](https://github.com/s11en1/RoguelikeConsoleGame/assets/132915375/3fe72d08-1992-44cd-b2ee-a9cbbca96876)





