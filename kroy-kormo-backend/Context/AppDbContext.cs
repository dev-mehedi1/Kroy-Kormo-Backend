using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Models.Attachment;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Notifications;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        // Existing DbSets
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationGroup> NotificationGroup { get; set; }
        public DbSet<GroupMembers> GroupMembers { get; set; }
        
        // New DbSets for classified ads platform
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<SavedSearch> SavedSearches { get; set; }
        public DbSet<ProductView> ProductViews { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Models.Chat.Conversation> Conversations { get; set; }
        public DbSet<Models.Chat.Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure Product relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Condition)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ConditionId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Status)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Location)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure ProductAttribute relationships
            modelBuilder.Entity<ProductAttribute>()
                .HasOne(pa => pa.Category)
                .WithMany()
                .HasForeignKey(pa => pa.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure ProductAttributeValue relationships
            modelBuilder.Entity<ProductAttributeValue>()
                .HasOne(pav => pav.Product)
                .WithMany(p => p.AttributeValues)
                .HasForeignKey(pav => pav.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ProductAttributeValue>()
                .HasOne(pav => pav.Attribute)
                .WithMany(pa => pa.AttributeValues)
                .HasForeignKey(pav => pav.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure Favorite relationships
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure SavedSearch relationships
            modelBuilder.Entity<SavedSearch>()
                .HasOne(ss => ss.User)
                .WithMany(u => u.SavedSearches)
                .HasForeignKey(ss => ss.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure ProductView relationships
            modelBuilder.Entity<ProductView>()
                .HasOne(pv => pv.Product)
                .WithMany(p => p.Views)
                .HasForeignKey(pv => pv.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ProductView>()
                .HasOne(pv => pv.User)
                .WithMany(u => u.ProductViews)
                .HasForeignKey(pv => pv.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure ProductPromotion relationships
            modelBuilder.Entity<ProductPromotion>()
                .HasOne(pp => pp.Product)
                .WithMany(p => p.Promotions)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure Review relationships
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Seller)
                .WithMany(u => u.ReviewsReceived)
                .HasForeignKey(r => r.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.ReviewsGiven)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Configure Location relationships
            modelBuilder.Entity<Location>()
                .HasOne(l => l.Country)
                .WithMany()
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Location>()
                .HasOne(l => l.State)
                .WithMany()
                .HasForeignKey(l => l.StateId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Location>()
                .HasOne(l => l.City)
                .WithMany()
                .HasForeignKey(l => l.CityId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Location>()
                .HasOne(l => l.Region)
                .WithMany()
                .HasForeignKey(l => l.RegionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Chat relationships
            modelBuilder.Entity<Models.Chat.Conversation>()
                .HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Chat.Conversation>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Chat.Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Models.Chat.Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes for optimized queries
            
            // Product indexes
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.CategoryId, p.StatusId, p.IsActive, p.CreatedAt })
                .HasDatabaseName("IX_Product_Category_Status_Active_Created");
            
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.UserId, p.StatusId, p.IsActive })
                .HasDatabaseName("IX_Product_User_Status_Active");
            
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Product_Slug");
            
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.Price, p.IsActive })
                .HasDatabaseName("IX_Product_Price_Active");
            
            // User indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_User_UserName");
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .HasDatabaseName("IX_User_Phone");
            
            // Favorite indexes
            modelBuilder.Entity<Favorite>()
                .HasIndex(f => new { f.UserId, f.ProductId })
                .IsUnique()
                .HasDatabaseName("IX_Favorite_User_Product");
            
            // Location indexes
            modelBuilder.Entity<Location>()
                .HasIndex(l => new { l.CountryId, l.StateId, l.CityId, l.RegionId })
                .HasDatabaseName("IX_Location_Hierarchy");
            
            // Category indexes
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Category_Slug");
            
            // ProductView indexes
            modelBuilder.Entity<ProductView>()
                .HasIndex(pv => new { pv.ProductId, pv.ViewedAt })
                .HasDatabaseName("IX_ProductView_Product_Date");

            // Chat indexes
            modelBuilder.Entity<Models.Chat.Conversation>()
                .HasIndex(c => new { c.User1Id, c.User2Id })
                .HasDatabaseName("IX_Conversation_Users");

            modelBuilder.Entity<Models.Chat.Message>()
                .HasIndex(m => new { m.ConversationId, m.CreatedAt })
                .HasDatabaseName("IX_Message_Conversation_Date");

            // Configure Notification models (changed from int to Guid)
            modelBuilder.Entity<Notification>()
                .Property(n => n.NotificationId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<NotificationGroup>()
                .Property(ng => ng.NotificationGroupId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GroupMembers>()
                .Property(gm => gm.GroupMembersId)
                .ValueGeneratedOnAdd();
        }
    }
}

