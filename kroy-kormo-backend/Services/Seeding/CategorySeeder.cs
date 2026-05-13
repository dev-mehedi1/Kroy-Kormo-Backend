using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.Models.Categories;
using Sob_Need_Server.Utils.Helpers;

namespace Sob_Need_Server.Services.Seeding
{
    public class CategorySeeder
    {
        private readonly AppDbContext _context;

        public CategorySeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Only seed if we don't have these specific categories yet, or add missing ones.
            // Check for root categories
            await SeedCategoryTree("IT & Telecommunication", new[] 
            { 
                "Software Engineer", 
                "Web Developer", 
                "QA Engineer", 
                "UI/UX Designer",
                "Project Manager",
                "Data Scientist",
                "System Administrator",
                "DevOps Engineer"
            });

            await SeedCategoryTree("Human Resources", new[] 
            { 
                "HR Manager", 
                "Recruiter", 
                "Talent Acquisition",
                "HR Executive"
            });

            await SeedCategoryTree("Marketing & Sales", new[] 
            { 
                "Sales Executive", 
                "Digital Marketer", 
                "Content Writer", 
                "Brand Manager",
                "SEO Specialist"
            });

            await SeedCategoryTree("Customer Service", new[] 
            { 
                "Support Specialist", 
                "Call Center Agent",
                "Customer Success Manager"
            });
            
             await SeedCategoryTree("Accounting & Finance", new[] 
            { 
                "Accountant", 
                "Financial Analyst",
                "Auditor"
            });
             
             await SeedCategoryTree("Engineering & Architecture", new[] 
            { 
                "Civil Engineer", 
                "Architect",
                "Electrical Engineer"
            });

             await SeedCategoryTree("Medical & Healthcare", new[] 
            { 
                "Doctor", 
                "Nurse",
                "Pharmacist"
            });
             
             await SeedCategoryTree("Education & Training", new[] 
            { 
                "Teacher", 
                "Tutor",
                "Lecturer"
            });
        }

        private async Task SeedCategoryTree(string parentName, string[] childrenNames)
        {
            // 1. Find or Create Parent
            var parent = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == parentName && c.ParentCategoryId == null);
            if (parent == null)
            {
                parent = new Category
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryName = parentName,
                    Slug = StringUtilities.GenerateSlug(parentName) + "-" + Guid.NewGuid().ToString().Substring(0,4), // Ensure uniqueness
                    Type = CategoryType.Job, // Mark as Job category
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System_Seeder"
                };
                _context.Categories.Add(parent);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Created Parent Category: {parentName}");
            }

            // 2. Ensure Children Exist
            foreach (var childName in childrenNames)
            {
                var childSlugBase = StringUtilities.GenerateSlug(childName);
                
                // Check if child exists under this parent
                var child = await _context.Categories.FirstOrDefaultAsync(c => c.ParentCategoryId == parent.CategoryId && c.CategoryName == childName);
                
                if (child == null)
                {
                    child = new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        CategoryName = childName,
                        Slug = childSlugBase + "-" + Guid.NewGuid().ToString().Substring(0, 4),
                        ParentCategoryId = parent.CategoryId,
                        Type = CategoryType.Job, // Mark as Job category
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "System_Seeder"
                    };
                    _context.Categories.Add(child);
                    Console.WriteLine($"  - Created Child Category: {childName}");
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
