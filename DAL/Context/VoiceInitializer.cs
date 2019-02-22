using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace DAL.Context
{
    public class VoiceInitializer : CreateDatabaseIfNotExists<VoiceContext>
    {
        protected override void Seed(VoiceContext context)
        {
            DataTable table = null;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string pathToData = $@"{baseDirectory}\Content\WordAccentsSer.xml";

            XmlSerializer ser = new XmlSerializer(typeof(DataTable));
            using (FileStream stream = new FileStream(pathToData, FileMode.Open))
            {
                table = (DataTable)ser.Deserialize(stream);
            }

            using (SqlConnection connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("copyTransaction");

                try
                {        
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.BatchSize = 4000;
                        sqlBulkCopy.DestinationTableName = "WordAccents";
                        sqlBulkCopy.WriteToServer(table);

                        transaction.Commit();
                    }
                }  
                catch (Exception ex)
                {
                    transaction.Rollback("copyTransaction");
                }
                finally
                {
                    transaction.Dispose();
                }               
            }

            base.Seed(context);
        }
    }
}
