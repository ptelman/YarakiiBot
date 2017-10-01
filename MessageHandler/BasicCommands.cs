using System;
using System.Linq;
using YarakiiBot.Base;
using YarakiiBot.Model;
using YarakiiBot.Service;

namespace YarakiiBot.MessageHandler{
    public class BasicCommands : ICommandReceiver
    {
        public string HandleCommand(string username, string command)
        {
            if (command.StartsWith("!addCmd")){
                using (var context = new DatabaseContext()){
                    var commandSplit = command.Split();
                    if (commandSplit.Count() > 2){
                        var newCommand = commandSplit.Skip(1).First();
                        var response = String.Join(" ", commandSplit.Skip(2));
                        context.UpdateChatCommand(newCommand, response);
                        BasicCommandCache.Instance.Update();                        
                        return $"Added new basicCommand '{newCommand}'";
                    }
                }
            }
            if (command.StartsWith("!removeCmd")){
                using (var context = new DatabaseContext()){
                    var commandSplit = command.Split();
                    if (commandSplit.Count() > 1){
                        var cmdToRemove = commandSplit.Skip(1).First();
                        context.RemoveChatCommand(cmdToRemove);
                        BasicCommandCache.Instance.Update();
                        return $"Removed basicCommand '{cmdToRemove}'";
                    }
                }
            }

            foreach (var basicCommand in BasicCommandCache.Instance.Commands){
                if (command.StartsWith($"!{basicCommand.Command}")){
                    return basicCommand.Response;
                }
            }

            return null;
        }
    }
}