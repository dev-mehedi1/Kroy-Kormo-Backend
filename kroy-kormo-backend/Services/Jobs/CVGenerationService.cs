using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Sob_Need_Server.Context;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Models.Users;

namespace Sob_Need_Server.Services.Jobs
{
    public class CVGenerationService : ICVGenerationService
    {
        private readonly AppDbContext _context;

        public CVGenerationService(AppDbContext context)
        {
            _context = context;
            // QuestPDF License configuration - For community use
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GeneratePdfCVAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Educations.OrderByDescending(e => e.EndDate))
                .Include(u => u.Experiences.OrderByDescending(e => e.EndDate))
                .Include(u => u.Skills).ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Inch);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Grey.Darken3));

                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(user.FullName).SemiBold().FontSize(24).FontColor(Colors.Green.Darken2);
                            col.Item().Text($"{user.Email} | {user.Phone ?? "No Phone"}").FontSize(10);
                            if (!string.IsNullOrEmpty(user.Bio))
                            {
                                col.Item().PaddingTop(5).Text(user.Bio).Italic().FontSize(10);
                            }
                        });
                    });

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // About Me section
                        if (!string.IsNullOrEmpty(user.AboutMe))
                        {
                            col.Item().PaddingTop(10).Column(innerCol =>
                            {
                                innerCol.Item().Text("Professional Summary").SemiBold().FontSize(14).Underline();
                                innerCol.Item().PaddingTop(5).Text(user.AboutMe);
                            });
                        }

                        // Experience section
                        if (user.Experiences.Any())
                        {
                            col.Item().PaddingTop(20).Column(innerCol =>
                            {
                                innerCol.Item().Text("Work Experience").SemiBold().FontSize(14).Underline();
                                
                                foreach (var exp in user.Experiences)
                                {
                                    innerCol.Item().PaddingTop(10).Row(row =>
                                    {
                                        row.RelativeItem().Column(c =>
                                        {
                                            c.Item().Text(exp.JobTitle).Bold();
                                            c.Item().Text(exp.CompanyName);
                                        });
                                        row.ConstantItem(150).AlignRight().Text($"{exp.StartDate?.ToString("MMM yyyy") ?? "N/A"} - {(exp.IsCurrent ? "Present" : exp.EndDate?.ToString("MMM yyyy") ?? "N/A")}");
                                    });
                                    if (!string.IsNullOrEmpty(exp.Description))
                                    {
                                        innerCol.Item().PaddingTop(2).Text(exp.Description).FontSize(10);
                                    }
                                }
                            });
                        }

                        // Education section
                        if (user.Educations.Any())
                        {
                            col.Item().PaddingTop(20).Column(innerCol =>
                            {
                                innerCol.Item().Text("Education").SemiBold().FontSize(14).Underline();
                                
                                foreach (var edu in user.Educations)
                                {
                                    innerCol.Item().PaddingTop(10).Row(row =>
                                    {
                                        row.RelativeItem().Column(c =>
                                        {
                                            c.Item().Text($"{edu.Degree} in {edu.FieldOfStudy}").Bold();
                                            c.Item().Text(edu.Institution);
                                        });
                                        row.ConstantItem(150).AlignRight().Text($"{edu.StartDate?.ToString("yyyy") ?? "N/A"} - {(edu.IsCurrent ? "Present" : edu.EndDate?.ToString("yyyy") ?? "N/A")}");
                                    });
                                }
                            });
                        }

                        // Skills section
                        if (user.Skills.Any())
                        {
                            col.Item().PaddingTop(20).Column(innerCol =>
                            {
                                innerCol.Item().Text("Skills").SemiBold().FontSize(14).Underline();
                                innerCol.Item().PaddingTop(5).Text(string.Join(", ", user.Skills.Select(s => s.Skill?.SkillName).Where(n => n != null)));
                            });
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generated by ");
                        x.Span("Kroy-Kormo Job Portal").Bold().FontColor(Colors.Green.Darken2);
                        x.Span(" | Page ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
