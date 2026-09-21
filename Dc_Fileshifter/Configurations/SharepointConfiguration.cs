using System;
using Dc_Fileshifter.service;

namespace Dc_Fileshifter.Configurations
{
    public  class SharePointConfiguration
    {
        public static string Name => Constants.GetEnvironmentVariable("sp_name");//"educationgovuk";
        public static string Sites  => Constants.GetEnvironmentVariable("sp_sites");
        public static string Url => Constants.GetEnvironmentVariable("sp_url");
        public static string RootFolder => Constants.GetEnvironmentVariable("sp_rootfolder");
        public static string BaseAddress => Constants.GetEnvironmentVariable("sp_baseaddress");
        public static string AppId  => Constants.GetEnvironmentVariable("sp_appid");

        public static string TenantId => Constants.GetEnvironmentVariable("sp_tenantid");

        public static string SecretKey => Constants.GetEnvironmentVariable("sp_secretkey");
        public static string Site_RootFolder => Constants.GetEnvironmentVariable("dc_RootFolder");
    }
}

