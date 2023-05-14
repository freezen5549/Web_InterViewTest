using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Reflection.Metadata;
using static Web_InterViewTest.Models.DataBaseManager.ApplicationDbContext;

namespace Web_InterViewTest.Models
{
    public class DataBaseManager
    {
        public class ApplicationDbContext : DbContext
        {
            public virtual IEnumerable<VoteRecord> SP_GetVoteRecord()
            {
                return Set<VoteRecord>().FromSqlInterpolated($"EXEC GetVoteRecord").ToList();
            }
            public virtual IEnumerable<VoteItem> SP_GetVoteItem()
            {
                return Set<VoteItem>().FromSqlInterpolated($"EXEC GetVoteItem").ToList();
            }
            public virtual IEnumerable<VoteRecord_ItemCount> SP_GetVoteRecordItemCount()
            {
                return Set<VoteRecord_ItemCount>().FromSqlInterpolated($"EXEC GetVoteRecordItemCount").ToList();
            }

            public virtual IEnumerable<VoteItem_Sn> SP_GetVoteItemSnByItem(string Item)
            {
                var ItemParameter = new SqlParameter("@Item", Item);
                return Set<VoteItem_Sn>().FromSqlRaw($"EXEC GetVoteItemSnByItem @Item", ItemParameter).ToList();
            }

            public virtual IEnumerable<VoteItem> SP_GetVoteItemItemBySn(int Sn)
            {
                var SnParameter = new SqlParameter("@Sn", Sn);

                return Set<VoteItem>().FromSqlRaw($"EXEC GetVoteItemItemBySn @Sn", SnParameter).ToList();
            }

            public virtual IEnumerable<VoteRecordIsExist> SP_CheckVoteRecordExist(string Voter,int VoterItem)
            {
                var voterParameter = new SqlParameter("@Voter", Voter);
                var voterItemParameter = new SqlParameter("@VoterItem", VoterItem);

                return Set<VoteRecordIsExist>().FromSqlRaw($"EXEC CheckVoteRecordExist @Voter, @VoterItem", voterParameter, voterItemParameter).ToList();
            }

            public virtual IEnumerable<VoteItem_Sn> SP_CheckVoteItemExist(string Item)
            {
                var ItemParameter = new SqlParameter("@Item", Item);
                return Set<VoteItem_Sn>().FromSqlRaw($"EXEC CheckVoteItemExist @Item", ItemParameter).ToList();
            }

            private readonly string _connectionString;
            public ApplicationDbContext(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("Web_InterViewTestContext");
            }

            public void SP_SetVoteItem(string Item)
            {
                Database.ExecuteSqlRaw("EXEC SetVoteItem @Parameter1",
                                       new SqlParameter("@Parameter1", Item));
            }

            public void SP_UpdateVoteItem(int Sn, string Item)
            {
                Database.ExecuteSqlRaw("EXEC UpdateVoteItem @Parameter1, @Parameter2",
                    new SqlParameter("@Parameter1", Sn),
                    new SqlParameter("@Parameter2", Item));
            }

            public void SP_SetVoteRecord(string Voter, int VoteItem)
            {
                Database.ExecuteSqlRaw("EXEC SetVoteRecord @Parameter1, @Parameter2",
                    new SqlParameter("@Parameter1", Voter),
                    new SqlParameter("@Parameter2", VoteItem));
            }

            public void SP_DeleteVoteItem(int Sn)
            {
                Database.ExecuteSqlRaw("EXEC DeleteVoteItem @Parameter1",
                    new SqlParameter("@Parameter1", Sn));
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(_connectionString, options =>
                    {
                        options.EnableRetryOnFailure(); 
                    });
                }
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<VoteRecord>().HasNoKey();
                modelBuilder.Entity<VoteRecord_ItemCount>().HasNoKey();
                modelBuilder.Entity<VoteItem_Sn>().HasNoKey();
                modelBuilder.Entity<VoteItem>().HasNoKey();
                modelBuilder.Entity<VoteRecordIsExist>().HasNoKey();
            }

            [NotMapped]
            public class VoteRecord_ItemCount
            {
                public string? Item { get; set; }
                public int VoteItem { get; set; }
            }

            [NotMapped]
            public class VoteItem_Sn
            {
                public int Sn { get; set; }
            }

            [NotMapped]
            public class VoteRecord
            {
                public string? Voter { get; set; }
                public int VoteItem { get; set; }
            }

            [NotMapped]
            public class VoteRecordIsExist
            {
                 public int Counter { get; set; }
            }

            [NotMapped]
            public class VoteItem
            {
                public int Sn { get; set; }
                public string? Item { get; set; }
            }

        }
    }

   

    




}
