using PlatformService.Models;
using System.Collections.Generic;

namespace FlatformService.Data
{
  public interface IPlatformRepo
  {
    bool SaveChanges();
    IEnumerable<Platform> GetAllPlatForms();
    Platform GetPlatformById(int id);
    void CreatePlatform(Platform plat);
  }
  
}