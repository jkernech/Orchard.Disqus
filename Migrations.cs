using System;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Disqus.Models;

namespace Orchard.Disqus
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("DisqusThreadMappingRecord", 
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("ThreadId")                              
                    .Column<int>("ContentId"));

            SchemaBuilder.CreateTable("DisqusPostMappingRecord", 
                table => table
                    .ContentPartRecord()
                    .Column<string>("PostId"));

            SchemaBuilder.CreateTable("DisqusSettingsRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("ShortName")
                    .Column<string>("ApiKey")
                    .Column<DateTime>("SynchronizedAt", column => column.Nullable()));

            ContentDefinitionManager.AlterTypeDefinition("Comment",
                alteration => alteration.WithPart(typeof(DisqusPostMappingPart).Name));         

            return 1;
        }
    }
}