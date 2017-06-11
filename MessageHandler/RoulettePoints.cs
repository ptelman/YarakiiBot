using System;
using YarakiiBot.Base;
using YarakiiBot.Model;

namespace YarakiiBot.MessageHandler{
    public class RoulettePoints : ICommandReceiver
    {
        public string HandleCommand(string username, string command)
        {
            if (command.StartsWith("!roulette") && command.Split(' ').Length > 1){
                using(var context = new DatabaseContext()){
                    User user = context.GetUserByName(username);

                    if (user.Points <= 0){
                        return $"@{username} don't have any points FeelsBadMan";
                    }
                    var commandSplitted = command.Split(' ');

                    int pointsToRoulette;
                    if (commandSplitted.Length > 1 && commandSplitted[1] == "all"){
                        return Roulette(username, user.Points, context);
                    }
                    if(int.TryParse(commandSplitted[1], out pointsToRoulette)){
                        if(pointsToRoulette > user.Points)
                            return $"User {username} don't have enough points. Try smaller bet";
                        else
                            return Roulette(username, pointsToRoulette, context);
                    }
                }
            }
            return null;
        }

        private string Roulette(string username, int points, DatabaseContext context){
            if (new Random().Next() % 100 < 30){
                context.AddPointsToUser(username, points);
                User user = context.GetUserByName(username);
                return $"{username} won, now he has {user.Points} points";
            }
            else{
                context.AddPointsToUser(username, points * -1);
                User user = context.GetUserByName(username);
                return $"{username} lost, now he has {user.Points} points";
            }
        }
    }
}