using System;
namespace Dc_Fileshifter.service
{
    public static class Constants
    {
        public static string DF_RootFolder = "DF_Forms_Files";
        public static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}

