using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CMS;
using CMS.DataEngine;
using CMS.IO;

// Registers the custom module into the system
[assembly: RegisterModule(typeof(AzureStorageModule))]

public class AzureStorageModule : Module
{
    // Module class constructor, the system registers the module under the name "CustomInit"
    public AzureStorageModule()
        : base("AzureStorageModuleInit")
    {
    }

    // Contains initialization code that is executed when the application starts
    protected override void OnInit()
    {
        base.OnInit();

        var azureContainer = ConfigurationManager.AppSettings["AzureContainer"];


        if (!string.IsNullOrEmpty(azureContainer))
        {
            // Creates a new StorageProvider instance for Azure
            var mediaProvider = StorageProvider.CreateAzureStorageProvider();

            // Specifies the target container
            mediaProvider.CustomRootPath = ConfigurationManager.AppSettings["AzureContainer"];

            // Makes the container publicly accessible
            mediaProvider.PublicExternalFolderObject = true;

            // Maps the local media library directory to the provider
            StorageHelper.MapStoragePath("~/ETG", mediaProvider);

        }
    }
}