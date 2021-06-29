using System.Collections.Generic;

namespace TankWars
{
    /// <summary>
    /// Referance for the server to display the information on localhost
    /// </summary>
    public class PlayerModel
    {
        public readonly string Name;
        public readonly uint Score;
        public readonly uint Accuracy;
        public PlayerModel(string n, uint s, uint a)
        {
            Name = n;
            Score = s;
            Accuracy = a;
        }
    }

    /// <summary>
    /// A simple container class representing the information about one player's session in one game
    /// </summary>
    public class Record
    {
        public readonly string Name;
        public readonly string Date;
        public readonly string Score;
        public readonly string Accuracy;

        public Record(string name, string date, string score, string acc)
        {
            Name = name;
            Date = date;
            Score = score;
            Accuracy = acc;
        }
    }
}