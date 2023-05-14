using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;

namespace Web_InterViewTest.Models
{
    public class DataBaseManager
    {
       
        //private DataTable GetSqlDataTable(string SqlScript) 
        //{
        //    SqlConnection sqlConnection = new SqlConnection(DBConnectingStr);
        //    SqlCommand sqlCommand = new SqlCommand(SqlScript);
        //    sqlCommand.Connection = sqlConnection;
        //    sqlConnection.Open();

        //    DataTable dataTable = new DataTable();
        //    SqlDataAdapter Sdqpt = new SqlDataAdapter(sqlCommand);
        //    Sdqpt.Fill(dataTable);

        //    sqlConnection.Close();
        //    return dataTable;
        //}
        //public List<VoteRecord> GetVoteRecord()
        //{
        //    List<VoteRecord> VoteRecordList = new List<VoteRecord>();
        
        //    DataTable dataTable = GetSqlDataTable("SELECT Voter,VoteItem FROM VoteRecord");
        //    for (int i = 0; i < dataTable.Rows.Count; i++)
        //    {
        //        VoteRecord VoteRecordTemp = new VoteRecord();
        //        VoteRecordTemp.Voter = dataTable.Rows[i]["Voter"].ToString();
        //        VoteRecordTemp.VoteItem = int.Parse(dataTable.Rows[i]["VoteItem"].ToString());
        //        VoteRecordList.Add(VoteRecordTemp);
        //    }
           
        //    return VoteRecordList;
        //}

        public class ApplicationDbContext : DbContext
        {
            public virtual IEnumerable<VoteRecord> SP_GetVoteRecord()
            {
                return Set<VoteRecord>().FromSqlInterpolated($"EXEC GetVoteRecord").ToList();
            }

            public virtual IEnumerable<VoteRecord_ItemCount> SP_GetVoteRecordItemCount()
            {
                return Set<VoteRecord_ItemCount>().FromSqlInterpolated($"EXEC GetVoteRecordItemCount").ToList();
            }

            private readonly string _connectionString;
            public ApplicationDbContext(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("Web_InterViewTestContext");
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(_connectionString, options =>
                    {
                        options.EnableRetryOnFailure(); // 如果需要，可以添加重試選項
                        //options.TrustServerCertificate(true); // 啟用 SSL 驗證
                    });
                }
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<VoteRecord>().HasNoKey();
                modelBuilder.Entity<VoteRecord_ItemCount>().HasNoKey();
            }

            [NotMapped]
            public class VoteRecord_ItemCount
            {
                public string? Item { get; set; }
                public int VoteItem { get; set; }
            }
            [NotMapped]
            public class VoteRecord
            {
                public string? Voter { get; set; }
                public int VoteItem { get; set; }
            }
        }
    }

   

    




}
