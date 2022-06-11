# ChessGame
A simple Chess Game.

The coding of the project is written in C# language, and the graphical interface is implemented with WinForm (Windows Forms API).

The development environment is Visual Studio 2019, and the framework is .NET Core 3.1.

# How To Play
There are two camps, one is the big side with 3 chess pieces, and the other is the small side with 15 chess pieces.
The big side can play the adjacent vacancy, or capture the small side's pieces every other vacancy.
The small side can only take the adjacent vacancy.
Neither side can take an oblique position.
![`M10T%J$_ FNBWGDLZ04 EW](https://user-images.githubusercontent.com/93463576/172282818-600e11e3-15e7-41e6-a175-86b8c89cd94d.png)


# How To Win
The big side has no moveable pieces, and the small side wins.
The number of Small side is less than 4, there is no theoretical win condition, Big side wins.
Or one of the two sides surrenders.

# Features
surrender;
regret;
Reopen;
A simple Socket online implementation, the IP is set to the default local IP (127.0.0.1);
A simple GameAI algorithm.
