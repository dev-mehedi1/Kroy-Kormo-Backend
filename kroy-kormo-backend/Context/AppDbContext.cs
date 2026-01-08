using Microsoft.EntityFrameworkCore;
using Models.Products;
using AttachmentEntity = Models.Attachment.Attachment;
using Sob_Need_Server.Models.Authentication;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Models.Locations;
using Sob_Need_Server.Models.Notifications;
using Sob_Need_Server.Models.Products;
using Sob_Need_Server.Models.Users;
using Models.Locations;

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
        public DbSet<AttachmentEntity> Attachments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationGroup> NotificationGroup { get; set; }
        public DbSet<GroupMembers> GroupMembers { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        
        // New DbSets for classified ads platform
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
        public DbSet<ProductStatusHistory> ProductStatusHistories { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<SavedSearch> SavedSearches { get; set; }
        public DbSet<ProductView> ProductViews { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Models.Chat.Conversation> Conversations { get; set; }
        public DbSet<Models.Chat.Message> Messages { get; set; }
        
        // Job Listing DbSets
        public DbSet<Models.Jobs.JobListing> JobListings { get; set; }
        public DbSet<Models.Jobs.JobType> JobTypes { get; set; }
        public DbSet<Models.Jobs.ExperienceLevel> ExperienceLevels { get; set; }
        public DbSet<Models.Jobs.JobApplication> JobApplications { get; set; }
        public DbSet<Models.Jobs.ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<Models.Jobs.ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
        public DbSet<Models.Jobs.Skill> Skills { get; set; }
        public DbSet<Models.Jobs.JobSkill> JobSkills { get; set; }
        
        // User Profile DbSets
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<UserExperience> UserExperiences { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<UserResume> UserResumes { get; set; }
        
        // Recruitment DbSets
        public DbSet<Models.Jobs.CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<Models.Jobs.Interview> Interviews { get; set; }
        public DbSet<Models.Jobs.JobOffer> JobOffers { get; set; }
        public DbSet<Models.Jobs.SavedJob> SavedJobs { get; set; }
        public DbSet<Models.Jobs.JobAlert> JobAlerts { get; set; }
        public DbSet<Models.Jobs.JobView> JobViews { get; set; }


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
            
            // Configure Product -> Locations (One-to-Many)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Locations)
                .WithOne(l => l.Product)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
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
            
            // Configure ProductStatusHistory relationships
            modelBuilder.Entity<ProductStatusHistory>()
                .HasOne(psh => psh.Product)
                .WithMany(p => p.StatusHistories)
                .HasForeignKey(psh => psh.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ProductStatusHistory>()
                .HasOne(psh => psh.Status)
                .WithMany()
                .HasForeignKey(psh => psh.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
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
                
            // Seed data with fixed dates to avoid migration spam from DateTime.UtcNow
            var seedDate = new DateTime(2026, 1, 7, 0, 0, 0, DateTimeKind.Utc);

            // SEED INITIAL ADMIN
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    AdminId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    UserName = "superadmin",
                    Password = "$2a$11$0KjJ5q.ZgY.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx.Qx", // Dummy BCrypt hash
                    FullName = "Super Admin",
                    Email = "admin@kroykormo.com",
                    Role = "SuperAdmin",
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );

            // Seed JobTypes
            modelBuilder.Entity<Models.Jobs.JobType>().HasData(
                new Models.Jobs.JobType { JobTypeId = Guid.Parse("aa111111-1111-1111-1111-111111111111"), TypeName = "Full-time", Slug = "full-time", IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.JobType { JobTypeId = Guid.Parse("aa222222-2222-2222-2222-222222222222"), TypeName = "Part-time", Slug = "part-time", IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.JobType { JobTypeId = Guid.Parse("aa333333-3333-3333-3333-333333333333"), TypeName = "Contract", Slug = "contract", IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.JobType { JobTypeId = Guid.Parse("aa444444-4444-4444-4444-444444444444"), TypeName = "Internship", Slug = "internship", IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.JobType { JobTypeId = Guid.Parse("aa555555-5555-5555-5555-555555555555"), TypeName = "Remote", Slug = "remote", IsActive = true, CreatedAt = seedDate }
            );

            // Seed ExperienceLevels
            modelBuilder.Entity<Models.Jobs.ExperienceLevel>().HasData(
                new Models.Jobs.ExperienceLevel { ExperienceLevelId = Guid.Parse("bb111111-1111-1111-1111-111111111111"), LevelName = "Entry Level", Slug = "entry", MinYears = 0, MaxYears = 2, IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.ExperienceLevel { ExperienceLevelId = Guid.Parse("bb222222-2222-2222-2222-222222222222"), LevelName = "Mid Level", Slug = "mid", MinYears = 2, MaxYears = 5, IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.ExperienceLevel { ExperienceLevelId = Guid.Parse("bb333333-3333-3333-3333-333333333333"), LevelName = "Senior Level", Slug = "senior", MinYears = 5, MaxYears = 10, IsActive = true, CreatedAt = seedDate },
                new Models.Jobs.ExperienceLevel { ExperienceLevelId = Guid.Parse("bb444444-4444-4444-4444-444444444444"), LevelName = "Executive", Slug = "executive", MinYears = 10, IsActive = true, CreatedAt = seedDate }
            );

            // Seed ApplicationStatuses
            modelBuilder.Entity<Models.Jobs.ApplicationStatus>().HasData(
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc111111-1111-1111-1111-111111111111"), StatusName = "Applied", Slug = "applied", ColorCode = "#3498db", IsTerminal = false, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc222222-2222-2222-2222-222222222222"), StatusName = "Shortlisted", Slug = "shortlisted", ColorCode = "#f1c40f", IsTerminal = false, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc333333-3333-3333-3333-333333333333"), StatusName = "Interviewing", Slug = "interviewing", ColorCode = "#9b59b6", IsTerminal = false, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc444444-4444-4444-4444-444444444444"), StatusName = "Offered", Slug = "offered", ColorCode = "#2ecc71", IsTerminal = false, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc555555-5555-5555-5555-555555555555"), StatusName = "Rejected", Slug = "rejected", ColorCode = "#e74c3c", IsTerminal = true, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc666666-6666-6666-6666-666666666666"), StatusName = "Hired", Slug = "hired", ColorCode = "#27ae60", IsTerminal = true, CreatedAt = seedDate },
                new Models.Jobs.ApplicationStatus { ApplicationStatusId = Guid.Parse("cc777777-7777-7777-7777-777777777777"), StatusName = "Withdrawn", Slug = "withdrawn", ColorCode = "#95a5a6", IsTerminal = true, CreatedAt = seedDate }
            );

            
            // ======================
            // JOB PLATFORM CONFIGURATIONS
            // ======================
            
            // JobListing relationships
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasOne(j => j.Product)
                .WithOne()
                .HasForeignKey<Models.Jobs.JobListing>(j => j.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasOne(j => j.Employer)
                .WithMany(u => u.JobListings)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasOne(j => j.JobType)
                .WithMany(jt => jt.JobListings)
                .HasForeignKey(j => j.JobTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasOne(j => j.ExperienceLevel)
                .WithMany(el => el.JobListings)
                .HasForeignKey(j => j.ExperienceLevelId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasOne(j => j.Category)
                .WithMany()
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobListing>()

                .HasOne(j => j.CompanyProfile)
                .WithMany(cp => cp.JobListings)
                .HasForeignKey(j => j.CompanyProfileId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // JobApplication relationships
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasOne(ja => ja.Status)
                .WithMany(s => s.Applications)
                .HasForeignKey(ja => ja.ApplicationStatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasOne(ja => ja.Resume)
                .WithMany()
                .HasForeignKey(ja => ja.AttachmentId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // ApplicationStatusHistory relationships
            modelBuilder.Entity<Models.Jobs.ApplicationStatusHistory>()
                .HasOne(ash => ash.Application)
                .WithMany(ja => ja.StatusHistory)
                .HasForeignKey(ash => ash.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.ApplicationStatusHistory>()
                .HasOne(ash => ash.Status)
                .WithMany()
                .HasForeignKey(ash => ash.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // JobSkill relationships
            modelBuilder.Entity<Models.Jobs.JobSkill>()
                .HasOne(js => js.Job)
                .WithMany(j => j.RequiredSkills)
                .HasForeignKey(js => js.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobSkill>()
                .HasOne(js => js.Skill)
                .WithMany(s => s.JobSkills)
                .HasForeignKey(js => js.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserEducation relationships
            modelBuilder.Entity<UserEducation>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.Educations)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserExperience relationships
            modelBuilder.Entity<UserExperience>()
                .HasOne(ux => ux.User)
                .WithMany(u => u.Experiences)
                .HasForeignKey(ux => ux.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserSkill relationships
            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.User)
                .WithMany(u => u.Skills)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(us => us.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // UserResume relationships
            modelBuilder.Entity<UserResume>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.Resumes)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // CompanyProfile relationships
            modelBuilder.Entity<Models.Jobs.CompanyProfile>()
                .HasOne(cp => cp.Owner)
                .WithMany(u => u.CompanyProfiles)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Interview relationships
            modelBuilder.Entity<Models.Jobs.Interview>()
                .HasOne(i => i.Application)
                .WithMany(ja => ja.Interviews)
                .HasForeignKey(i => i.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.Interview>()
                .HasOne(i => i.Employer)
                .WithMany()
                .HasForeignKey(i => i.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.Interview>()
                .HasOne(i => i.Candidate)
                .WithMany()
                .HasForeignKey(i => i.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // JobOffer relationships
            modelBuilder.Entity<Models.Jobs.JobOffer>()
                .HasOne(jo => jo.Application)
                .WithMany(ja => ja.Offers)
                .HasForeignKey(jo => jo.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobOffer>()
                .HasOne(jo => jo.Employer)
                .WithMany()
                .HasForeignKey(jo => jo.EmployerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Models.Jobs.JobOffer>()
                .HasOne(jo => jo.Candidate)
                .WithMany()
                .HasForeignKey(jo => jo.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // SavedJob relationships
            modelBuilder.Entity<Models.Jobs.SavedJob>()
                .HasOne(sj => sj.User)
                .WithMany(u => u.SavedJobs)
                .HasForeignKey(sj => sj.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.SavedJob>()
                .HasOne(sj => sj.Job)
                .WithMany(j => j.SavedByUsers)
                .HasForeignKey(sj => sj.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // JobAlert relationships
            modelBuilder.Entity<Models.Jobs.JobAlert>()
                .HasOne(ja => ja.User)
                .WithMany(u => u.JobAlerts)
                .HasForeignKey(ja => ja.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobAlert>()
                .HasOne(ja => ja.Category)
                .WithMany()
                .HasForeignKey(ja => ja.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Models.Jobs.JobAlert>()
                .HasOne(ja => ja.JobType)
                .WithMany()
                .HasForeignKey(ja => ja.JobTypeId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Models.Jobs.JobAlert>()
                .HasOne(ja => ja.ExperienceLevel)
                .WithMany()
                .HasForeignKey(ja => ja.ExperienceLevelId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // JobView relationships
            modelBuilder.Entity<Models.Jobs.JobView>()
                .HasOne(jv => jv.Job)
                .WithMany(j => j.Views)
                .HasForeignKey(jv => jv.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Models.Jobs.JobView>()
                .HasOne(jv => jv.User)
                .WithMany()
                .HasForeignKey(jv => jv.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // ======================
            // JOB PLATFORM INDEXES
            // ======================
            
            // JobListing indexes
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasIndex(j => new { j.JobTypeId, j.ExperienceLevelId, j.IsRemote, j.IsActive })
                .HasDatabaseName("IX_JobListing_Type_Level_Remote_Active");
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasIndex(j => new { j.EmployerId, j.JobStatus, j.IsActive })
                .HasDatabaseName("IX_JobListing_Employer_Status_Active");
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasIndex(j => new { j.CategoryId, j.IsActive, j.CreatedAt })
                .HasDatabaseName("IX_JobListing_Category_Active_Date");
            
            modelBuilder.Entity<Models.Jobs.JobListing>()
                .HasIndex(j => j.ApplicationDeadline)
                .HasDatabaseName("IX_JobListing_Deadline");
            
            // JobApplication indexes
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasIndex(ja => new { ja.JobId, ja.ApplicationStatusId, ja.CreatedAt })
                .HasDatabaseName("IX_JobApplication_Job_Status_Date");
            
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasIndex(ja => new { ja.ApplicantId, ja.ApplicationStatusId, ja.IsActive })
                .HasDatabaseName("IX_JobApplication_Applicant_Status_Active");
            
            modelBuilder.Entity<Models.Jobs.JobApplication>()
                .HasIndex(ja => new { ja.JobId, ja.ApplicantId })
                .IsUnique()
                .HasDatabaseName("IX_JobApplication_Job_Applicant_Unique");
            
            // SavedJob indexes
            modelBuilder.Entity<Models.Jobs.SavedJob>()
                .HasIndex(sj => new { sj.UserId, sj.JobId })
                .IsUnique()
                .HasDatabaseName("IX_SavedJob_User_Job_Unique");
            
            // JobView indexes
            modelBuilder.Entity<Models.Jobs.JobView>()
                .HasIndex(jv => new { jv.JobId, jv.ViewedAt })
                .HasDatabaseName("IX_JobView_Job_Date");
            
            modelBuilder.Entity<Models.Jobs.JobView>()
                .HasIndex(jv => new { jv.UserId, jv.ViewedAt })
                .HasDatabaseName("IX_JobView_User_Date");
            
            // Interview indexes
            modelBuilder.Entity<Models.Jobs.Interview>()
                .HasIndex(i => new { i.CandidateId, i.ScheduledAt, i.Status })
                .HasDatabaseName("IX_Interview_Candidate_Schedule_Status");
            
            modelBuilder.Entity<Models.Jobs.Interview>()
                .HasIndex(i => new { i.EmployerId, i.ScheduledAt, i.Status })
                .HasDatabaseName("IX_Interview_Employer_Schedule_Status");
            
            // CompanyProfile indexes
            modelBuilder.Entity<Models.Jobs.CompanyProfile>()
                .HasIndex(cp => cp.Slug)
                .IsUnique()
                .HasDatabaseName("IX_CompanyProfile_Slug");
            
            modelBuilder.Entity<Models.Jobs.CompanyProfile>()
                .HasIndex(cp => new { cp.IsVerified, cp.IsActive })
                .HasDatabaseName("IX_CompanyProfile_Verified_Active");
            
            // Skill indexes
            modelBuilder.Entity<Models.Jobs.Skill>()
                .HasIndex(s => s.Slug)
                .IsUnique()
                .HasDatabaseName("IX_Skill_Slug");
            
            modelBuilder.Entity<Models.Jobs.Skill>()
                .HasIndex(s => s.SkillName)
                .HasDatabaseName("IX_Skill_Name");
        }
    }
}


