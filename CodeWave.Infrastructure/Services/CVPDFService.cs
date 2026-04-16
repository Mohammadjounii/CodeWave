using CodeWave.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Text.Json;
using System.Linq;

namespace CodeWave.Infrastructure.Services
{
    public interface ICVPDFService
    {
        Task<string> GenerateCVPDFAsync(CV cv, string outputPath);
    }

    public class CVPDFService : ICVPDFService
    {
        public CVPDFService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private string? GetAcademyLogoPath()
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            // Try PNG first (QuestPDF supports PNG)
            var logoPath = Path.Combine(wwwrootPath, "images", "codewave-logo.png");
            if (System.IO.File.Exists(logoPath))
                return logoPath;
            
            // Try JPG as fallback
            logoPath = Path.Combine(wwwrootPath, "images", "codewave-logo.jpg");
            if (System.IO.File.Exists(logoPath))
                return logoPath;
            
            return null;
        }

        private string FormatExperienceForPDF(string? experienceJson)
        {
            if (string.IsNullOrEmpty(experienceJson))
                return string.Empty;

            try
            {
                var experiences = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(experienceJson);
                if (experiences == null || !experiences.Any())
                    return experienceJson; // Fallback to original if parsing fails

                var formatted = new System.Text.StringBuilder();
                foreach (var exp in experiences)
                {
                    var title = exp.ContainsKey("title") ? exp["title"].GetString() : "";
                    var company = exp.ContainsKey("company") ? exp["company"].GetString() : "";
                    var startDate = exp.ContainsKey("startDate") ? exp["startDate"].GetString() : "";
                    var endDate = exp.ContainsKey("endDate") ? exp["endDate"].GetString() : "";
                    var description = exp.ContainsKey("description") ? exp["description"].GetString() : "";
                    var skills = new List<string>();
                    
                    if (exp.ContainsKey("skills") && exp["skills"].ValueKind == JsonValueKind.Array)
                    {
                        skills.AddRange(exp["skills"].EnumerateArray().Select(s => s.GetString()).Where(s => !string.IsNullOrEmpty(s)));
                    }

                    if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(company))
                    {
                        formatted.Append($"{title}");
                        if (!string.IsNullOrEmpty(company))
                            formatted.Append($" - {company}");
                        formatted.AppendLine();

                        if (!string.IsNullOrEmpty(startDate) || !string.IsNullOrEmpty(endDate))
                        {
                            var start = !string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate + "-01", out var sd) 
                                ? sd.ToString("MMM yyyy") : startDate;
                            var end = string.IsNullOrEmpty(endDate) ? "Present" 
                                : (DateTime.TryParse(endDate + "-01", out var ed) ? ed.ToString("MMM yyyy") : endDate);
                            formatted.AppendLine($"{start} - {end}");
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            formatted.AppendLine(description);
                        }

                        if (skills.Any())
                        {
                            formatted.AppendLine($"Technologies: {string.Join(", ", skills)}");
                        }

                        formatted.AppendLine();
                    }
                }
                return formatted.ToString().Trim();
            }
            catch
            {
                // If JSON parsing fails, return original text
                return experienceJson;
            }
        }

        private string FormatCertificationsForPDF(string? certificationsJson)
        {
            if (string.IsNullOrEmpty(certificationsJson))
                return string.Empty;

            try
            {
                var certifications = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(certificationsJson);
                if (certifications == null || !certifications.Any())
                    return certificationsJson; // Fallback to original if parsing fails

                var formatted = new System.Text.StringBuilder();
                foreach (var cert in certifications)
                {
                    var name = cert.ContainsKey("name") ? cert["name"].GetString() : "";
                    var institution = cert.ContainsKey("institution") ? cert["institution"].GetString() : "";
                    var date = cert.ContainsKey("date") ? cert["date"].GetString() : "";
                    var expiry = cert.ContainsKey("expiry") ? cert["expiry"].GetString() : "";

                    if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(institution))
                    {
                        formatted.Append($"• {name}");
                        if (!string.IsNullOrEmpty(institution))
                            formatted.Append($" - {institution}");
                        if (!string.IsNullOrEmpty(date))
                        {
                            var certDate = DateTime.TryParse(date + "-01", out var d) ? d.ToString("MMMM yyyy") : date;
                            formatted.Append($" - {certDate}");
                        }
                        if (!string.IsNullOrEmpty(expiry))
                        {
                            var expDate = DateTime.TryParse(expiry + "-01", out var e) ? e.ToString("MMMM yyyy") : expiry;
                            formatted.Append($" (Expires: {expDate})");
                        }
                        formatted.AppendLine();
                    }
                }
                return formatted.ToString().Trim();
            }
            catch
            {
                // If JSON parsing fails, return original text
                return certificationsJson;
            }
        }

        private string FormatProjectsForPDF(string? projectsJson)
        {
            if (string.IsNullOrEmpty(projectsJson))
                return string.Empty;

            try
            {
                var projects = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(projectsJson);
                if (projects == null || !projects.Any())
                    return projectsJson; // Fallback to original if parsing fails

                var formatted = new System.Text.StringBuilder();
                foreach (var proj in projects)
                {
                    var name = proj.ContainsKey("name") ? proj["name"].GetString() : "";
                    var description = proj.ContainsKey("description") ? proj["description"].GetString() : "";
                    var technologies = new List<string>();
                    var date = proj.ContainsKey("date") ? proj["date"].GetString() : "";
                    
                    if (proj.ContainsKey("technologies") && proj["technologies"].ValueKind == JsonValueKind.Array)
                    {
                        technologies.AddRange(proj["technologies"].EnumerateArray().Select(t => t.GetString()).Where(t => !string.IsNullOrEmpty(t)));
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        formatted.AppendLine(name);
                        if (!string.IsNullOrEmpty(description))
                        {
                            formatted.AppendLine(description);
                        }
                        if (technologies.Any())
                        {
                            formatted.AppendLine($"Technologies: {string.Join(", ", technologies)}");
                        }
                        if (!string.IsNullOrEmpty(date))
                        {
                            var projDate = DateTime.TryParse(date + "-01", out var d) ? d.ToString("MMMM yyyy") : date;
                            formatted.AppendLine($"Completed: {projDate}");
                        }
                        formatted.AppendLine();
                    }
                }
                return formatted.ToString().Trim();
            }
            catch
            {
                // If JSON parsing fails, return original text
                return projectsJson;
            }
        }

        public async Task<string> GenerateCVPDFAsync(CV cv, string outputPath)
        {
            // Get full path for CV picture if available
            string? cvPicturePath = null;
            if (!string.IsNullOrEmpty(cv.CVPictureUrl))
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var fullPicturePath = Path.Combine(wwwrootPath, cv.CVPictureUrl.TrimStart('/'));
                if (System.IO.File.Exists(fullPicturePath))
                {
                    cvPicturePath = fullPicturePath;
                }
            }

            var template = string.IsNullOrEmpty(cv.Template) ? "dublin" : cv.Template.ToLower();
            
            // Generate document structure (this is fast)
            var document = template switch
            {
                "professional" => GenerateProfessionalTemplate(cv, cvPicturePath),
                "chrono" => GenerateChronoTemplate(cv, cvPicturePath),
                "elegant" => GenerateElegantTemplate(cv, cvPicturePath),
                "circular" => GenerateCircularTemplate(cv, cvPicturePath),
                // New resume.io templates
                "dublin" => GenerateDublinTemplate(cv, cvPicturePath),
                "newyork" => GenerateNewYorkTemplate(cv, cvPicturePath),
                "sydney" => GenerateSydneyTemplate(cv, cvPicturePath),
                "milan" => GenerateMilanTemplate(cv, cvPicturePath),
                "lisbon" => GenerateLisbonTemplate(cv, cvPicturePath),
                // Keep old templates for backward compatibility
                "modern" => GenerateDublinTemplate(cv, cvPicturePath),
                "classic" => GenerateNewYorkTemplate(cv, cvPicturePath),
                "creative" => GenerateSydneyTemplate(cv, cvPicturePath),
                "minimalist" => GenerateMilanTemplate(cv, cvPicturePath),
                "executive" => GenerateLisbonTemplate(cv, cvPicturePath),
                _ => GenerateDublinTemplate(cv, cvPicturePath) // Default to Dublin
            };

            // Ensure directory exists
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Generate PDF on thread pool to avoid blocking (CPU-intensive operation)
            // This typically takes 1-3 seconds depending on content complexity
            try
            {
                await Task.Run(() => 
                {
                    document.GeneratePdf(outputPath);
                    
                    // Verify the PDF was actually created
                    if (!System.IO.File.Exists(outputPath))
                    {
                        throw new Exception($"PDF file was not created at: {outputPath}");
                    }
                    
                    var fileInfo = new FileInfo(outputPath);
                    if (fileInfo.Length == 0)
                    {
                        throw new Exception("Generated PDF file is empty (0 bytes).");
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Wrap and rethrow with more context
                throw new Exception($"Error generating PDF document: {ex.Message}", ex);
            }

            return outputPath;
        }

        private Document GenerateDublinTemplate(CV cv, string? cvPicturePath)
        {
            // Dublin-style: Clean two-column layout with sidebar
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    page.Content()
                        .Row(row =>
                        {
                            // Left sidebar - narrow, colored
                            row.RelativeItem(3).Background(Colors.Blue.Darken4)
                                .Padding(25)
                                .Column(sidebarCol =>
                                {
                                    sidebarCol.Spacing(20);

                                    // Photo
                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        sidebarCol.Item().AlignCenter().Height(100).Image(cvPicturePath).FitArea();
                                    }

                                    // Name
                                    sidebarCol.Item().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                        .FontSize(18)
                                        .Bold()
                                        .FontColor(Colors.White)
                                        .AlignCenter();

                                    // Contact section
                                    sidebarCol.Item().PaddingTop(15).Column(contactCol =>
                                    {
                                        contactCol.Item().PaddingBottom(5).Text("CONTACT")
                                            .FontSize(11)
                                            .Bold()
                                            .FontColor(Colors.White);

                                        if (!string.IsNullOrEmpty(cv.Email))
                                            contactCol.Item().Text(cv.Email).FontSize(9).FontColor(Colors.Grey.Lighten2);
                                        if (!string.IsNullOrEmpty(cv.Phone))
                                            contactCol.Item().PaddingTop(3).Text(cv.Phone).FontSize(9).FontColor(Colors.Grey.Lighten2);
                                        if (!string.IsNullOrEmpty(cv.Location))
                                            contactCol.Item().PaddingTop(3).Text(cv.Location).FontSize(9).FontColor(Colors.Grey.Lighten2);
                                    });

                                    // Skills
                                    if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    {
                                        sidebarCol.Item().Column(skillsCol =>
                                        {
                                            skillsCol.Item().PaddingBottom(5).Text("SKILLS")
                                                .FontSize(11)
                                                .Bold()
                                                .FontColor(Colors.White);
                                            skillsCol.Item().Text(cv.ProgrammingLanguages)
                                                .FontSize(9)
                                                .LineHeight(1.5f)
                                                .FontColor(Colors.Grey.Lighten2);
                                        });
                                    }
                                });

                            // Right content area
                            row.RelativeItem(7).Padding(30).Column(contentCol =>
                            {
                                contentCol.Spacing(20);

                                if (!string.IsNullOrEmpty(cv.Summary))
                                    contentCol.Item().ModernSection("PROFESSIONAL SUMMARY", cv.Summary, Colors.Blue.Darken4);

                                var formattedExperience = FormatExperienceForPDF(cv.Experience);
                                if (!string.IsNullOrEmpty(formattedExperience))
                                    contentCol.Item().ModernSection("WORK EXPERIENCE", formattedExperience, Colors.Blue.Darken4);

                                if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                    contentCol.Item().ModernSection("EDUCATION", 
                                        (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                        (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""), Colors.Blue.Darken4);

                                var formattedProjects = FormatProjectsForPDF(cv.Projects);
                                if (!string.IsNullOrEmpty(formattedProjects))
                                    contentCol.Item().ModernSection("PROJECTS", formattedProjects, Colors.Blue.Darken4);

                                var formattedCertifications = FormatCertificationsForPDF(cv.Certifications);
                                if (!string.IsNullOrEmpty(formattedCertifications))
                                    contentCol.Item().ModernSection("CERTIFICATIONS", formattedCertifications, Colors.Blue.Darken4);
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateNewYorkTemplate(CV cv, string? cvPicturePath)
        {
            // New York-style: Bold header, strong typography, traditional layout
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Bold header with dark background
                    page.Header()
                        .MinHeight(120)
                        .Background(Colors.Grey.Darken4)
                        .Padding(30)
                        .Row(headerRow =>
                        {
                            headerRow.RelativeItem().Column(nameCol =>
                            {
                                nameCol.Item().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                    .FontSize(28)
                                    .Bold()
                                    .FontColor(Colors.White)
                                    .LetterSpacing(1.5f);

                                nameCol.Item().PaddingTop(8).Row(contactRow =>
                                {
                                    if (!string.IsNullOrEmpty(cv.Email))
                                        contactRow.RelativeItem().Text(cv.Email).FontSize(10).FontColor(Colors.Grey.Lighten2);
                                    if (!string.IsNullOrEmpty(cv.Phone))
                                        contactRow.RelativeItem().Text($" | {cv.Phone}").FontSize(10).FontColor(Colors.Grey.Lighten2);
                                    if (!string.IsNullOrEmpty(cv.Location))
                                        contactRow.RelativeItem().Text($" | {cv.Location}").FontSize(10).FontColor(Colors.Grey.Lighten2);
                                });
                            });

                            if (!string.IsNullOrEmpty(cvPicturePath))
                            {
                                headerRow.ConstantItem(80).Height(80).Image(cvPicturePath).FitArea();
                            }
                        });

                    page.Content()
                        .Padding(35)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            if (!string.IsNullOrEmpty(cv.Summary))
                                column.Item().ClassicSection("PROFESSIONAL SUMMARY", cv.Summary);

                            var formattedExperienceNY = FormatExperienceForPDF(cv.Experience);
                            if (!string.IsNullOrEmpty(formattedExperienceNY))
                                column.Item().ClassicSection("PROFESSIONAL EXPERIENCE", formattedExperienceNY);

                            if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                column.Item().ClassicSection("EDUCATION", 
                                    (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                    (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""));

                            if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                column.Item().ClassicSection("TECHNICAL SKILLS", cv.ProgrammingLanguages);

                            var formattedProjectsNY = FormatProjectsForPDF(cv.Projects);
                            if (!string.IsNullOrEmpty(formattedProjectsNY))
                                column.Item().ClassicSection("PROJECTS", formattedProjectsNY);

                            var formattedCertificationsNY = FormatCertificationsForPDF(cv.Certifications);
                            if (!string.IsNullOrEmpty(formattedCertificationsNY))
                                column.Item().ClassicSection("CERTIFICATIONS", formattedCertificationsNY);
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateSydneyTemplate(CV cv, string? cvPicturePath)
        {
            // Sydney-style: Colorful, modern, unique layout with accent colors
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Colorful top header bar
                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Background(Colors.Teal.Darken2)
                                .Padding(25)
                                .Row(headerRow =>
                                {
                                    headerRow.RelativeItem().Column(nameCol =>
                                    {
                                        nameCol.Item().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                            .FontSize(24)
                                            .Bold()
                                            .FontColor(Colors.White);

                                        nameCol.Item().PaddingTop(5).Row(contactRow =>
                                        {
                                            if (!string.IsNullOrEmpty(cv.Email))
                                                contactRow.RelativeItem().Text(cv.Email).FontSize(9).FontColor(Colors.White);
                                            if (!string.IsNullOrEmpty(cv.Phone))
                                                contactRow.RelativeItem().Text($" | {cv.Phone}").FontSize(9).FontColor(Colors.White);
                                            if (!string.IsNullOrEmpty(cv.Location))
                                                contactRow.RelativeItem().Text($" | {cv.Location}").FontSize(9).FontColor(Colors.White);
                                        });
                                    });

                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        headerRow.ConstantItem(90).Height(90).Image(cvPicturePath).FitArea();
                                    }
                                });

                            // Two-column content with accent colors
                            column.Item().Padding(25).Row(contentRow =>
                            {
                                // Left column - narrow
                                contentRow.RelativeItem(3).Column(leftCol =>
                                {
                                    leftCol.Spacing(15);

                                    if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    {
                                        leftCol.Item().Background(Colors.Orange.Lighten1)
                                            .Padding(15)
                                            .Column(skillsCol =>
                                            {
                                                skillsCol.Item().Text("SKILLS")
                                                    .FontSize(12)
                                                    .Bold()
                                                    .FontColor(Colors.White);
                                                skillsCol.Item().PaddingTop(5).Text(cv.ProgrammingLanguages)
                                                    .FontSize(9)
                                                    .LineHeight(1.5f)
                                                    .FontColor(Colors.White);
                                            });
                                    }
                                });

                                // Right column - wide
                                contentRow.ConstantItem(15);
                                contentRow.RelativeItem(7).Column(rightCol =>
                                {
                                    rightCol.Spacing(18);

                                    if (!string.IsNullOrEmpty(cv.Summary))
                                        rightCol.Item().CreativeSection("ABOUT", cv.Summary, Colors.Teal.Darken2);

                                    var formattedExperienceSY = FormatExperienceForPDF(cv.Experience);
                                    if (!string.IsNullOrEmpty(formattedExperienceSY))
                                        rightCol.Item().CreativeSection("EXPERIENCE", formattedExperienceSY, Colors.Teal.Darken2);

                                    if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                        rightCol.Item().CreativeSection("EDUCATION", 
                                            (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                            (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""), Colors.Teal.Darken2);

                                    var formattedProjectsSY = FormatProjectsForPDF(cv.Projects);
                                    if (!string.IsNullOrEmpty(formattedProjectsSY))
                                        rightCol.Item().CreativeSection("PROJECTS", formattedProjectsSY, Colors.Teal.Darken2);

                                    var formattedCertificationsSY = FormatCertificationsForPDF(cv.Certifications);
                                    if (!string.IsNullOrEmpty(formattedCertificationsSY))
                                        rightCol.Item().CreativeSection("CERTIFICATIONS", formattedCertificationsSY, Colors.Teal.Darken2);
                                });
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateMilanTemplate(CV cv, string? cvPicturePath)
        {
            // Milan-style: Elegant, minimalist, lots of white space, sophisticated
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(60);
                    page.PageColor(Colors.White);

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(30);

                            // Minimalist header
                            column.Item().Column(headerCol =>
                            {
                                headerCol.Item().Row(nameRow =>
                                {
                                    nameRow.RelativeItem().Text(cv.FullName ?? "Your Name")
                                        .FontSize(36)
                                        .Bold()
                                        .FontColor(Colors.Black)
                                        .LetterSpacing(2f);

                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        nameRow.ConstantItem(80).Height(80).Image(cvPicturePath).FitArea();
                                    }
                                });

                                headerCol.Item().PaddingTop(10).Row(contactRow =>
                                {
                                    if (!string.IsNullOrEmpty(cv.Email))
                                        contactRow.RelativeItem().Text(cv.Email).FontSize(9).FontColor(Colors.Grey.Darken2);
                                    if (!string.IsNullOrEmpty(cv.Phone))
                                        contactRow.RelativeItem().Text($" • {cv.Phone}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                    if (!string.IsNullOrEmpty(cv.Location))
                                        contactRow.RelativeItem().Text($" • {cv.Location}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                });

                                headerCol.Item().PaddingTop(15).Height(1).Background(Colors.Grey.Lighten1);
                            });

                            // Content sections with lots of spacing
                            if (!string.IsNullOrEmpty(cv.Summary))
                                column.Item().MinimalistSection("Summary", cv.Summary);

                            var formattedExperienceMI = FormatExperienceForPDF(cv.Experience);
                            if (!string.IsNullOrEmpty(formattedExperienceMI))
                                column.Item().MinimalistSection("Experience", formattedExperienceMI);

                            if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                column.Item().MinimalistSection("Education", 
                                    (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                    (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""));

                            if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                column.Item().MinimalistSection("Skills", cv.ProgrammingLanguages);

                            var formattedProjectsMI = FormatProjectsForPDF(cv.Projects);
                            if (!string.IsNullOrEmpty(formattedProjectsMI))
                                column.Item().MinimalistSection("Projects", formattedProjectsMI);

                            var formattedCertificationsMI = FormatCertificationsForPDF(cv.Certifications);
                            if (!string.IsNullOrEmpty(formattedCertificationsMI))
                                column.Item().MinimalistSection("Certifications", formattedCertificationsMI);
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateLisbonTemplate(CV cv, string? cvPicturePath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Lisbon-style: Professional, structured, corporate style with clean lines
                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Padding(40).Column(headerCol =>
                            {
                                headerCol.Item().Row(nameRow =>
                                {
                                    nameRow.RelativeItem().Column(nameCol =>
                                    {
                                        nameCol.Item().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                            .FontSize(26)
                                            .Bold()
                                            .FontColor(Colors.Black)
                                            .LetterSpacing(1.2f);

                                        nameCol.Item().PaddingTop(5).Row(contactRow =>
                                        {
                                            if (!string.IsNullOrEmpty(cv.Email))
                                                contactRow.RelativeItem().Text(cv.Email).FontSize(9).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Phone))
                                                contactRow.RelativeItem().Text($" | {cv.Phone}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Location))
                                                contactRow.RelativeItem().Text($" | {cv.Location}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                        });
                                    });

                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        nameRow.ConstantItem(85).Height(85).Image(cvPicturePath).FitArea();
                                    }
                                });

                                // Accent line
                                headerCol.Item().PaddingTop(15).Height(3).Background(Colors.Blue.Darken3);
                            });

                            // Structured content
                            column.Item().PaddingHorizontal(40).PaddingBottom(30).Column(contentCol =>
                            {
                                contentCol.Spacing(20);

                                if (!string.IsNullOrEmpty(cv.Summary))
                                    contentCol.Item().ExecutiveSection("PROFESSIONAL SUMMARY", cv.Summary);

                                var formattedExperienceLI = FormatExperienceForPDF(cv.Experience);
                                if (!string.IsNullOrEmpty(formattedExperienceLI))
                                    contentCol.Item().ExecutiveSection("PROFESSIONAL EXPERIENCE", formattedExperienceLI);

                                if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                    contentCol.Item().ExecutiveSection("EDUCATION", 
                                        (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                        (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""));

                                if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    contentCol.Item().ExecutiveSection("CORE COMPETENCIES", cv.ProgrammingLanguages);

                                var formattedProjectsLI = FormatProjectsForPDF(cv.Projects);
                                if (!string.IsNullOrEmpty(formattedProjectsLI))
                                    contentCol.Item().ExecutiveSection("KEY PROJECTS", formattedProjectsLI);

                                var formattedCertificationsLI = FormatCertificationsForPDF(cv.Certifications);
                                if (!string.IsNullOrEmpty(formattedCertificationsLI))
                                    contentCol.Item().ExecutiveSection("CERTIFICATIONS & ACHIEVEMENTS", formattedCertificationsLI);
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        // ========== NEW TEMPLATES BASED ON IMAGE ==========

        private Document GenerateProfessionalTemplate(CV cv, string? cvPicturePath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Dark blue header with picture on left
                    page.Content()
                        .Column(column =>
                        {
                            // Header section
                            column.Item().Background(Colors.Blue.Darken3)
                                .Padding(20)
                                .Row(headerRow =>
                                {
                                    // Profile picture on left
                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        headerRow.ConstantItem(80).Height(80).Image(cvPicturePath).FitArea();
                                        headerRow.ConstantItem(15); // Spacing
                                    }

                                    // Name and title on right
                                    headerRow.RelativeItem().Column(nameCol =>
                                    {
                                        nameCol.Item().Text(cv.FullName ?? "Your Name")
                                            .FontSize(24)
                                            .Bold()
                                            .FontColor(Colors.White);

                                        // Title/Summary line if available
                                        if (!string.IsNullOrEmpty(cv.Summary))
                                        {
                                            var title = cv.Summary.Split('.')[0]; // First sentence as title
                                            nameCol.Item().PaddingTop(3).Text(title.Length > 60 ? title.Substring(0, 60) + "..." : title)
                                                .FontSize(11)
                                                .FontColor(Colors.White);
                                        }

                                        // Contact info
                                        nameCol.Item().PaddingTop(8).Row(contactRow =>
                                        {
                                            if (!string.IsNullOrEmpty(cv.Email))
                                                contactRow.RelativeItem().Text($"✉ {cv.Email}").FontSize(9).FontColor(Colors.White);
                                            if (!string.IsNullOrEmpty(cv.Phone))
                                                contactRow.RelativeItem().Text($"📞 {cv.Phone}").FontSize(9).FontColor(Colors.White);
                                            if (!string.IsNullOrEmpty(cv.Location))
                                                contactRow.RelativeItem().Text($"📍 {cv.Location}").FontSize(9).FontColor(Colors.White);
                                        });
                                    });
                                });

                            // Two-column content layout
                            column.Item().Padding(25).Row(contentRow =>
                            {
                                // Left column (narrow) - Skills and Personal Details
                                contentRow.RelativeItem(3).Column(leftCol =>
                                {
                                    leftCol.Spacing(15);

                                    // Personal Details
                                    if (!string.IsNullOrEmpty(cv.LinkedInUrl) || !string.IsNullOrEmpty(cv.GitHubUrl))
                                    {
                                        leftCol.Item().Column(detailsCol =>
                                        {
                                            detailsCol.Item().Text("Personal Details")
                                                .FontSize(12)
                                                .Bold()
                                                .FontColor(Colors.Blue.Darken3);

                                            if (!string.IsNullOrEmpty(cv.LinkedInUrl))
                                            {
                                                detailsCol.Item().PaddingTop(5).Text("LinkedIn")
                                                    .FontSize(9)
                                                    .FontColor(Colors.Grey.Darken2);
                                                detailsCol.Item().Text(cv.LinkedInUrl.Length > 30 ? cv.LinkedInUrl.Substring(0, 30) + "..." : cv.LinkedInUrl)
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);
                                            }

                                            if (!string.IsNullOrEmpty(cv.GitHubUrl))
                                            {
                                                detailsCol.Item().PaddingTop(5).Text("GitHub")
                                                    .FontSize(9)
                                                    .FontColor(Colors.Grey.Darken2);
                                                detailsCol.Item().Text(cv.GitHubUrl.Length > 30 ? cv.GitHubUrl.Substring(0, 30) + "..." : cv.GitHubUrl)
                                                    .FontSize(8)
                                                    .FontColor(Colors.Grey.Darken1);
                                            }
                                        });
                                    }

                                    // Skills
                                    if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    {
                                        leftCol.Item().Column(skillsCol =>
                                        {
                                            skillsCol.Item().Text("Skills")
                                                .FontSize(12)
                                                .Bold()
                                                .FontColor(Colors.Blue.Darken3);

                                            skillsCol.Item().PaddingTop(5).Text(cv.ProgrammingLanguages)
                                                .FontSize(9)
                                                .LineHeight(1.5f)
                                                .FontColor(Colors.Grey.Darken2);
                                        });
                                    }
                                });

                                // Right column (wide) - Main content
                                contentRow.ConstantItem(15); // Spacing
                                contentRow.RelativeItem(7).Column(rightCol =>
                                {
                                    rightCol.Spacing(18);

                                    if (!string.IsNullOrEmpty(cv.Summary))
                                        rightCol.Item().ProfessionalSection("Profile", cv.Summary, Colors.Blue.Darken3);

                                    var formattedExperiencePR = FormatExperienceForPDF(cv.Experience);
                                    if (!string.IsNullOrEmpty(formattedExperiencePR))
                                        rightCol.Item().ProfessionalSection("Experience", formattedExperiencePR, Colors.Blue.Darken3);

                                    if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                        rightCol.Item().ProfessionalSection("Education",
                                            (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                            (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""), Colors.Blue.Darken3);

                                    var formattedProjectsPR = FormatProjectsForPDF(cv.Projects);
                                    if (!string.IsNullOrEmpty(formattedProjectsPR))
                                        rightCol.Item().ProfessionalSection("Projects", formattedProjectsPR, Colors.Blue.Darken3);

                                    var formattedCertificationsPR = FormatCertificationsForPDF(cv.Certifications);
                                    if (!string.IsNullOrEmpty(formattedCertificationsPR))
                                        rightCol.Item().ProfessionalSection("Certifications", formattedCertificationsPR, Colors.Blue.Darken3);
                                });
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateChronoTemplate(CV cv, string? cvPicturePath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Light blue header with picture on right
                    page.Content()
                        .Column(column =>
                        {
                            // Header section
                            column.Item().Background(Colors.Blue.Lighten2)
                                .Padding(25)
                                .Row(headerRow =>
                                {
                                    // Name and contact on left
                                    headerRow.RelativeItem().Column(nameCol =>
                                    {
                                        nameCol.Item().Text(cv.FullName ?? "Your Name")
                                            .FontSize(28)
                                            .Bold()
                                            .FontColor(Colors.Blue.Darken4);

                                        nameCol.Item().PaddingTop(8).Row(contactRow =>
                                        {
                                            if (!string.IsNullOrEmpty(cv.Email))
                                                contactRow.RelativeItem().Text(cv.Email).FontSize(10).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Phone))
                                                contactRow.RelativeItem().Text($"| {cv.Phone}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Location))
                                                contactRow.RelativeItem().Text($"| {cv.Location}").FontSize(10).FontColor(Colors.Grey.Darken2);
                                        });
                                    });

                                    // Profile picture on right
                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        headerRow.ConstantItem(15); // Spacing
                                        headerRow.ConstantItem(90).Height(90).Image(cvPicturePath).FitArea();
                                    }
                                });

                            // Chronological content layout
                            column.Item().Padding(30).Column(contentCol =>
                            {
                                contentCol.Spacing(20);

                                if (!string.IsNullOrEmpty(cv.Summary))
                                    contentCol.Item().ChronoSection("Summary", cv.Summary);

                                var formattedExperienceCH = FormatExperienceForPDF(cv.Experience);
                                if (!string.IsNullOrEmpty(formattedExperienceCH))
                                    contentCol.Item().ChronoSection("Experience", formattedExperienceCH);

                                if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                    contentCol.Item().ChronoSection("Education",
                                        (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                        (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""));

                                if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    contentCol.Item().ChronoSection("Skills", cv.ProgrammingLanguages);

                                var formattedProjectsCH = FormatProjectsForPDF(cv.Projects);
                                if (!string.IsNullOrEmpty(formattedProjectsCH))
                                    contentCol.Item().ChronoSection("Projects", formattedProjectsCH);
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateElegantTemplate(CV cv, string? cvPicturePath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Two-column layout with dark sidebar
                    page.Content()
                        .Row(row =>
                        {
                            // Left sidebar - Dark grey
                            row.RelativeItem(3).Background(Colors.Grey.Darken4)
                                .Padding(25)
                                .Column(sidebarCol =>
                                {
                                    sidebarCol.Spacing(20);

                                    // Profile picture in sidebar
                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        sidebarCol.Item().AlignCenter().Height(120).Image(cvPicturePath).FitArea();
                                    }

                                    // Name in sidebar
                                    sidebarCol.Item().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                        .FontSize(18)
                                        .Bold()
                                        .FontColor(Colors.White)
                                        .AlignCenter();

                                    // Personal Details
                                    sidebarCol.Item().Column(detailsCol =>
                                    {
                                        detailsCol.Item().Text("Personal Details")
                                            .FontSize(11)
                                            .Bold()
                                            .FontColor(Colors.White);

                                        if (!string.IsNullOrEmpty(cv.Email))
                                            detailsCol.Item().PaddingTop(5).Text(cv.Email).FontSize(9).FontColor(Colors.Grey.Lighten2);
                                        if (!string.IsNullOrEmpty(cv.Phone))
                                            detailsCol.Item().PaddingTop(3).Text(cv.Phone).FontSize(9).FontColor(Colors.Grey.Lighten2);
                                        if (!string.IsNullOrEmpty(cv.Location))
                                            detailsCol.Item().PaddingTop(3).Text(cv.Location).FontSize(9).FontColor(Colors.Grey.Lighten2);

                                        if (!string.IsNullOrEmpty(cv.LinkedInUrl))
                                        {
                                            detailsCol.Item().PaddingTop(8).Text("LinkedIn").FontSize(9).FontColor(Colors.Grey.Lighten2);
                                            detailsCol.Item().Text(cv.LinkedInUrl.Length > 25 ? cv.LinkedInUrl.Substring(0, 25) + "..." : cv.LinkedInUrl)
                                                .FontSize(8).FontColor(Colors.Grey.Lighten3);
                                        }
                                    });

                                    // Skills in sidebar
                                    if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    {
                                        sidebarCol.Item().Column(skillsCol =>
                                        {
                                            skillsCol.Item().Text("Skills")
                                                .FontSize(11)
                                                .Bold()
                                                .FontColor(Colors.White);

                                            skillsCol.Item().PaddingTop(5).Text(cv.ProgrammingLanguages)
                                                .FontSize(9)
                                                .LineHeight(1.5f)
                                                .FontColor(Colors.Grey.Lighten2);
                                        });
                                    }
                                });

                            // Right content area - Light blue header bar
                            row.RelativeItem(7).Column(rightCol =>
                            {
                                // Light blue header bar
                                rightCol.Item().Background(Colors.Blue.Lighten2)
                                    .Padding(20)
                                    .Row(headerRow =>
                                    {
                                        headerRow.RelativeItem().Text(cv.FullName?.ToUpper() ?? "YOUR NAME")
                                            .FontSize(22)
                                            .Bold()
                                            .FontColor(Colors.White);
                                    });

                                // Main content
                                rightCol.Item().Padding(30).Column(contentCol =>
                                {
                                    contentCol.Spacing(20);

                                    if (!string.IsNullOrEmpty(cv.Summary))
                                        contentCol.Item().ElegantSection("About", cv.Summary, Colors.Grey.Darken4);

                                    var formattedExperienceEL = FormatExperienceForPDF(cv.Experience);
                                    if (!string.IsNullOrEmpty(formattedExperienceEL))
                                        contentCol.Item().ElegantSection("Experience", formattedExperienceEL, Colors.Grey.Darken4);

                                    if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                        contentCol.Item().ElegantSection("Education",
                                            (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                            (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""), Colors.Grey.Darken4);

                                    var formattedProjectsEL = FormatProjectsForPDF(cv.Projects);
                                    if (!string.IsNullOrEmpty(formattedProjectsEL))
                                        contentCol.Item().ElegantSection("Projects", formattedProjectsEL, Colors.Grey.Darken4);

                                    var formattedCertificationsEL = FormatCertificationsForPDF(cv.Certifications);
                                    if (!string.IsNullOrEmpty(formattedCertificationsEL))
                                        contentCol.Item().ElegantSection("Certifications", formattedCertificationsEL, Colors.Grey.Darken4);
                                });
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }

        private Document GenerateCircularTemplate(CV cv, string? cvPicturePath)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    // Light blue header with circular image style
                    page.Content()
                        .Column(column =>
                        {
                            // Header section
                            column.Item().Background(Colors.Blue.Lighten2)
                                .Padding(25)
                                .Row(headerRow =>
                                {
                                    // Profile picture on left (circular style - will be rendered as square but styled)
                                    if (!string.IsNullOrEmpty(cvPicturePath))
                                    {
                                        headerRow.ConstantItem(100).Height(100).Image(cvPicturePath).FitArea();
                                        headerRow.ConstantItem(20); // Spacing
                                    }

                                    // Name and contact on right
                                    headerRow.RelativeItem().Column(nameCol =>
                                    {
                                        nameCol.Item().Text(cv.FullName ?? "Your Name")
                                            .FontSize(26)
                                            .Bold()
                                            .FontColor(Colors.Blue.Darken4);

                                        nameCol.Item().PaddingTop(10).Row(contactRow =>
                                        {
                                            if (!string.IsNullOrEmpty(cv.Email))
                                                contactRow.RelativeItem().Text($"✉ {cv.Email}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Phone))
                                                contactRow.RelativeItem().Text($"📞 {cv.Phone}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                            if (!string.IsNullOrEmpty(cv.Location))
                                                contactRow.RelativeItem().Text($"📍 {cv.Location}").FontSize(9).FontColor(Colors.Grey.Darken2);
                                        });

                                        // Social links
                                        if (!string.IsNullOrEmpty(cv.LinkedInUrl) || !string.IsNullOrEmpty(cv.GitHubUrl))
                                        {
                                            nameCol.Item().PaddingTop(8).Row(socialRow =>
                                            {
                                                if (!string.IsNullOrEmpty(cv.LinkedInUrl))
                                                    socialRow.RelativeItem().Text($"🔗 LinkedIn").FontSize(8).FontColor(Colors.Blue.Darken3);
                                                if (!string.IsNullOrEmpty(cv.GitHubUrl))
                                                    socialRow.RelativeItem().Text($"💻 GitHub").FontSize(8).FontColor(Colors.Blue.Darken3);
                                            });
                                        }
                                    });
                                });

                            // Two-column content layout
                            column.Item().Padding(25).Row(contentRow =>
                            {
                                // Left column
                                contentRow.RelativeItem(3).Column(leftCol =>
                                {
                                    leftCol.Spacing(15);

                                    if (!string.IsNullOrEmpty(cv.ProgrammingLanguages))
                                    {
                                        leftCol.Item().Column(skillsCol =>
                                        {
                                            skillsCol.Item().Text("Skills")
                                                .FontSize(12)
                                                .Bold()
                                                .FontColor(Colors.Blue.Darken3);

                                            skillsCol.Item().PaddingTop(5).Text(cv.ProgrammingLanguages)
                                                .FontSize(9)
                                                .LineHeight(1.5f)
                                                .FontColor(Colors.Grey.Darken2);
                                        });
                                    }
                                });

                                // Right column
                                contentRow.ConstantItem(15); // Spacing
                                contentRow.RelativeItem(7).Column(rightCol =>
                                {
                                    rightCol.Spacing(18);

                                    if (!string.IsNullOrEmpty(cv.Summary))
                                        rightCol.Item().CircularSection("Summary", cv.Summary, Colors.Blue.Darken3);

                                    var formattedExperienceCI = FormatExperienceForPDF(cv.Experience);
                                    if (!string.IsNullOrEmpty(formattedExperienceCI))
                                        rightCol.Item().CircularSection("Experience", formattedExperienceCI, Colors.Blue.Darken3);

                                    if (!string.IsNullOrEmpty(cv.Education) || !string.IsNullOrEmpty(cv.EducationDetails))
                                        rightCol.Item().CircularSection("Education",
                                            (!string.IsNullOrEmpty(cv.Education) ? $"{cv.Education}\n" : "") +
                                            (!string.IsNullOrEmpty(cv.EducationDetails) ? cv.EducationDetails : ""), Colors.Blue.Darken3);

                                    var formattedProjectsCI = FormatProjectsForPDF(cv.Projects);
                                    if (!string.IsNullOrEmpty(formattedProjectsCI))
                                        rightCol.Item().CircularSection("Projects", formattedProjectsCI, Colors.Blue.Darken3);
                                });
                            });
                        });

                    page.Footer()
                        .MinHeight(35)
                        .Background(Colors.Grey.Lighten3)
                        .PaddingVertical(5)
                        .AlignCenter()
                        .Column(footerCol =>
                        {
                            // Academy logo as signature at the bottom
                            var academyLogoPath = GetAcademyLogoPath();
                            if (!string.IsNullOrEmpty(academyLogoPath))
                            {
                                try
                                {
                                    footerCol.Item().Height(18).Image(academyLogoPath).FitArea();
                                }
                                catch
                                {
                                    // If image fails, show text-based logo
                                    footerCol.Item().Text("{ }")
                                        .FontSize(16)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken2);
                                }
                            }
                            else
                            {
                                // Show text-based logo if PNG/JPG doesn't exist
                                footerCol.Item().Text("{ }")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(Colors.Grey.Darken2);
                            }
                            
                            footerCol.Item().PaddingTop(2).Text("Generated by CodeWave CV Builder")
                                .FontSize(7)
                                .FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }
    }

    public static class DocumentExtensions
    {
        public static void ModernSection(this IContainer container, string title, string content, QuestPDF.Infrastructure.Color accentColor)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.AutoItem().Text(title)
                        .FontSize(14)
                        .Bold()
                        .FontColor(accentColor);
                    row.RelativeItem().PaddingLeft(10).Height(1).Background(accentColor);
                });

                column.Item().PaddingTop(8).Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }

        public static void ClassicSection(this IContainer container, string title, string content)
        {
            container.Column(column =>
            {
                column.Item().Text(title)
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Black)
                    .Underline();

                column.Item().PaddingTop(8).PaddingLeft(10).Text(content)
                    .FontSize(10)
                    .LineHeight(1.5f)
                    .FontColor(Colors.Black);
            });
        }

        public static void CreativeSection(this IContainer container, string title, string content, QuestPDF.Infrastructure.Color accentColor)
        {
            container.Column(column =>
            {
                column.Item().PaddingBottom(5).Text(title)
                    .FontSize(16)
                    .Bold()
                    .FontColor(accentColor);

                column.Item().PaddingLeft(8).BorderLeft(3).BorderColor(accentColor)
                    .Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }

        public static void MinimalistSection(this IContainer container, string title, string content)
        {
            container.Column(column =>
            {
                column.Item().Text(title)
                    .FontSize(12)
                    .Bold()
                    .FontColor(Colors.Black)
                    .LetterSpacing(2);

                column.Item().PaddingTop(10).Text(content)
                    .FontSize(10)
                    .LineHeight(1.8f)
                    .FontColor(Colors.Grey.Darken1);
            });
        }

        public static void ExecutiveSection(this IContainer container, string title, string content)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.AutoItem().Text(title)
                        .FontSize(13)
                        .Bold()
                        .FontColor(Colors.Grey.Darken4)
                        .LetterSpacing(1);
                    row.RelativeItem().PaddingLeft(10).Height(1).Background(Colors.Grey.Darken2);
                });

                column.Item().PaddingTop(10).Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken3);
            });
        }

        // ========== NEW SECTION STYLES FOR NEW TEMPLATES ==========

        public static void ProfessionalSection(this IContainer container, string title, string content, QuestPDF.Infrastructure.Color accentColor)
        {
            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    row.AutoItem().Text(title)
                        .FontSize(14)
                        .Bold()
                        .FontColor(accentColor);
                    row.RelativeItem().PaddingLeft(10).Height(2).Background(accentColor);
                });

                column.Item().PaddingTop(8).Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }

        public static void ChronoSection(this IContainer container, string title, string content)
        {
            container.Column(column =>
            {
                column.Spacing(5);
                
                column.Item().Text(title)
                    .FontSize(16)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);

                column.Item().PaddingLeft(15).Text(content)
                    .FontSize(10)
                    .LineHeight(1.7f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }

        public static void ElegantSection(this IContainer container, string title, string content, QuestPDF.Infrastructure.Color accentColor)
        {
            container.Column(column =>
            {
                column.Spacing(8);
                
                column.Item().Text(title.ToUpper())
                    .FontSize(14)
                    .Bold()
                    .FontColor(accentColor)
                    .LetterSpacing(1.5f);

                column.Item().PaddingLeft(10).BorderLeft(2).BorderColor(accentColor)
                    .PaddingLeft(10)
                    .Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }

        public static void CircularSection(this IContainer container, string title, string content, QuestPDF.Infrastructure.Color accentColor)
        {
            container.Column(column =>
            {
                column.Spacing(5);
                
                column.Item().Text(title)
                    .FontSize(13)
                    .Bold()
                    .FontColor(accentColor);

                column.Item().Text(content)
                    .FontSize(10)
                    .LineHeight(1.6f)
                    .FontColor(Colors.Grey.Darken2);
            });
        }
    }
}
