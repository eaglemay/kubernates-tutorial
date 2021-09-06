using CommandService.Models;
using System.Collections.Generic;

namespace CommandService.Data
{
  public interface ICommandRepo
  {
        bool SaveChanges();

        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int PlatformId);
        bool ExternalPlatformExists(int externalPlatformId);

        //Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
  
}