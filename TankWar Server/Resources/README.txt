Implemented by Ping Cheng,Chung and Michael Meadows for Cs 3500 Networking.cs
Date: 12/5/19 
Class: CS3500


private const string connectionString = 
		"server=atr.eng.utah.edu;" +
        "database=cs3500_u0954730;" +
        "uid=cs3500_u0954730;" +
        "password=u0954730";

--------------------------------------Tank Wars Game-----------------------------------------
Server Settings:
A simple server for receiving simple text messages from multiple clients that uses dictionairy
of long, socket state from the clients. Also a dictionairy for long, int for the socketID_tankID.
Set frame to 50 and start the server with an event loop of methods called NewClientConnected then
will call for the logic of the program in the TankWarBrain that uses the unityItem project to 
create the world to the client. 
Settings start based on projectileCounter beamCounter, World world, playerCounter, Random random,
UpdateTimer, round, PowerupCounter, SpawnPowerupTimer, MaximumPowerup = 2;
TankRespawnTime and the ConnectingString for the SQL DataBase are also within the TankWarBrain

 Resouce file contains the README.txt that you are reading as the previous assignments have 
 the same sprits but the README from this solution contains a more detailed README file then 
 the previous assignements.

 Start of World settings:
 world = new World(1200); TankRespawnTime = 250; playerCounter = 0; projectileCounter = 0; 
 beamCounter = 0; random = new Random(); UpdateTimer = 0; round = 1; PowerupCounter = 0;
 SpawnPowerupTimer = 0; InitializeWorld();

 Using a structure of the MVC- Model, View, Controller. Tank War Game with the world from PS8
 but with added functions such as collistion detection. 
 The view from PS8 will not change. 
 The Controller from the server is seperate from the clients gamecontroller. We used the server
 to update, keep track and interact with clients. Which is useful for the database to be updated
 once a player has left the game. 
 




 Motion of the tanks is one of the things that was rather easy compared to some of the other 
 features such as the collision of walls. 

 Challanges:
 The design of the tanks when spawning in when they would be on a wall or touching a wall 
 caused a bug in our program that we could not figure out for a while. However after realizing 
 that the tanks were not being able to be loaded on to the client without first having the tanks
 spawn anywhere other than were the walls were. 

 The projectiles that were used are from our PS8 that we thought would be interesting to put the 
 angry bird but if the client was used from a different client then the projectile would be 
 different form others. 

