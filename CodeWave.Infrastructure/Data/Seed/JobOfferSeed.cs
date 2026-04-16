using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class JobOfferSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Get existing job offer IDs to avoid duplicates
            var existingJobIds = await context.JobOffers
                .Select(j => j.Id)
                .ToListAsync();
            
            // If all seed jobs already exist, skip seeding
            var seedJobIds = new List<Guid>
            {
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Guid.Parse("11111111-1111-1111-1111-111111111114"),
                Guid.Parse("22222222-2222-2222-2222-222222222221"),
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Guid.Parse("22222222-2222-2222-2222-222222222223"),
                Guid.Parse("22222222-2222-2222-2222-222222222224"),
                Guid.Parse("22222222-2222-2222-2222-222222222225"),
                Guid.Parse("33333333-3333-3333-3333-333333333331"),
                Guid.Parse("33333333-3333-3333-3333-333333333332"),
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Guid.Parse("33333333-3333-3333-3333-333333333334"),
                Guid.Parse("33333333-3333-3333-3333-333333333335"),
                Guid.Parse("44444444-4444-4444-4444-444444444441"),
                Guid.Parse("44444444-4444-4444-4444-444444444442"),
                Guid.Parse("44444444-4444-4444-4444-444444444443"),
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Guid.Parse("44444444-4444-4444-4444-444444444445")
            };
            
            var allExist = seedJobIds.All(id => existingJobIds.Contains(id));
            if (allExist)
            {
                Console.WriteLine("Job offers seed data already exists. Skipping...");
                return;
            }
            
            Console.WriteLine($"Seeding job offers. {existingJobIds.Count} existing jobs found.");

            var now = DateTime.UtcNow;
            var jobs = new List<JobOffer>
            {
                // ============= JUNIOR LEVEL JOBS =============
                
                new JobOffer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    JobTitle = "Junior Frontend Developer",
                    Company = "TechStart Solutions",
                    Description = "We are looking for an enthusiastic Junior Frontend Developer to join our growing team. You will be responsible for implementing visual elements that users see and interact with in web applications. This is a great opportunity to grow your skills in a supportive environment with mentorship from senior developers.\n\nResponsibilities:\n• Develop and maintain web applications using React and TypeScript\n• Collaborate with UI/UX designers to implement responsive designs\n• Write clean, maintainable code following best practices\n• Participate in code reviews and team meetings\n• Learn and adapt to new technologies and frameworks",
                    RequiredSkills = "HTML, CSS, JavaScript, React, TypeScript, Git",
                    PostedDate = now.AddDays(-5),
                    Deadline = now.AddMonths(1),
                    CreatedAt = now.AddDays(-5),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                    JobTitle = "Junior Python Developer",
                    Company = "DataFlow Inc",
                    Description = "Join our team as a Junior Python Developer and work on exciting data processing and automation projects. You'll work alongside experienced developers to build scalable backend systems and APIs.\n\nResponsibilities:\n• Develop and maintain Python-based web applications\n• Build RESTful APIs using Flask or Django\n• Work with databases (SQL and NoSQL)\n• Write unit tests and documentation\n• Collaborate with cross-functional teams",
                    RequiredSkills = "Python, Flask, Django, SQL, REST APIs, Git",
                    PostedDate = now.AddDays(-3),
                    Deadline = now.AddMonths(1).AddDays(5),
                    CreatedAt = now.AddDays(-3),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                    JobTitle = "Junior Full-Stack Developer",
                    Company = "InnovateLab",
                    Description = "We're seeking a Junior Full-Stack Developer to help build our next-generation web platform. This role offers exposure to both frontend and backend technologies with opportunities to work on diverse projects.\n\nResponsibilities:\n• Develop full-stack web applications\n• Work with React on the frontend and Node.js on the backend\n• Integrate with databases and third-party APIs\n• Participate in agile development practices\n• Contribute to architectural decisions",
                    RequiredSkills = "JavaScript, React, Node.js, Express, MongoDB, Git, HTML, CSS",
                    PostedDate = now.AddDays(-7),
                    Deadline = now.AddMonths(1).AddDays(10),
                    CreatedAt = now.AddDays(-7),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
                    JobTitle = "Junior Java Developer",
                    Company = "EnterpriseSoft",
                    Description = "Looking for a motivated Junior Java Developer to join our enterprise software development team. You'll work on building robust, scalable applications using Java and Spring Framework.\n\nResponsibilities:\n• Develop Java-based applications using Spring Boot\n• Write clean, testable code following SOLID principles\n• Work with relational databases\n• Participate in code reviews and sprint planning\n• Learn enterprise development best practices",
                    RequiredSkills = "Java, Spring Boot, Maven, SQL, REST APIs, Git",
                    PostedDate = now.AddDays(-4),
                    Deadline = now.AddMonths(1).AddDays(7),
                    CreatedAt = now.AddDays(-4),
                    isDeleted = false
                },

                // ============= MID-LEVEL JOBS =============

                new JobOffer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222221"),
                    JobTitle = "Mid-Level Frontend Developer",
                    Company = "Digital Innovations",
                    Description = "We're looking for an experienced Frontend Developer to lead frontend initiatives and mentor junior developers. You'll work on complex, high-performance web applications used by millions of users.\n\nResponsibilities:\n• Architect and develop scalable frontend solutions\n• Optimize application performance and user experience\n• Mentor junior developers and conduct code reviews\n• Collaborate with product managers and designers\n• Stay updated with latest frontend technologies and trends",
                    RequiredSkills = "React, TypeScript, Next.js, Redux, Tailwind CSS, Jest, GraphQL, Webpack",
                    PostedDate = now.AddDays(-6),
                    Deadline = now.AddMonths(1).AddDays(15),
                    CreatedAt = now.AddDays(-6),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    JobTitle = "Mid-Level Backend Developer (Python)",
                    Company = "CloudScale Technologies",
                    Description = "Join our backend team as a Mid-Level Python Developer. You'll design and implement robust backend systems, work with microservices architecture, and ensure high availability and performance.\n\nResponsibilities:\n• Design and develop scalable backend systems using Python\n• Build microservices using FastAPI or Django\n• Work with cloud platforms (AWS, Azure, or GCP)\n• Implement caching strategies and optimize database queries\n• Design and implement RESTful and GraphQL APIs",
                    RequiredSkills = "Python, FastAPI, Django, PostgreSQL, Redis, Docker, AWS, Kubernetes, Celery",
                    PostedDate = now.AddDays(-8),
                    Deadline = now.AddMonths(1).AddDays(20),
                    CreatedAt = now.AddDays(-8),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222223"),
                    JobTitle = "Full-Stack Developer",
                    Company = "NextGen Software",
                    Description = "We need a Full-Stack Developer with solid experience in both frontend and backend development. You'll work on end-to-end features and have ownership of complete product modules.\n\nResponsibilities:\n• Develop complete full-stack features from database to UI\n• Work with modern JavaScript frameworks (React, Vue, or Angular)\n• Build RESTful and GraphQL APIs\n• Design database schemas and optimize queries\n• Implement CI/CD pipelines and deployment strategies",
                    RequiredSkills = "JavaScript, TypeScript, React, Node.js, Express, PostgreSQL, MongoDB, Docker, Git, AWS",
                    PostedDate = now.AddDays(-10),
                    Deadline = now.AddMonths(1).AddDays(25),
                    CreatedAt = now.AddDays(-10),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222224"),
                    JobTitle = "Java Backend Developer",
                    Company = "Enterprise Solutions Group",
                    Description = "Looking for an experienced Java Developer to work on enterprise-grade applications. You'll work with Spring ecosystem, microservices, and cloud-native technologies.\n\nResponsibilities:\n• Develop microservices using Spring Boot and Spring Cloud\n• Design and implement distributed systems\n• Work with message queues (RabbitMQ, Kafka)\n• Optimize application performance and scalability\n• Ensure code quality through testing and code reviews",
                    RequiredSkills = "Java, Spring Boot, Spring Cloud, PostgreSQL, Redis, RabbitMQ, Docker, Kubernetes, AWS",
                    PostedDate = now.AddDays(-5),
                    Deadline = now.AddMonths(1).AddDays(18),
                    CreatedAt = now.AddDays(-5),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222225"),
                    JobTitle = "DevOps Engineer",
                    Company = "InfraTech Systems",
                    Description = "Join our DevOps team to help automate, scale, and maintain our infrastructure. You'll work with cutting-edge tools and practices to ensure reliable and scalable deployments.\n\nResponsibilities:\n• Design and maintain CI/CD pipelines\n• Manage cloud infrastructure (AWS, Azure, or GCP)\n• Implement Infrastructure as Code (Terraform, CloudFormation)\n• Monitor and optimize system performance\n• Automate deployment and scaling processes",
                    RequiredSkills = "Docker, Kubernetes, AWS, Terraform, Jenkins, GitLab CI, Linux, Bash, Python, Monitoring Tools",
                    PostedDate = now.AddDays(-9),
                    Deadline = now.AddMonths(1).AddDays(22),
                    CreatedAt = now.AddDays(-9),
                    isDeleted = false
                },

                // ============= SENIOR LEVEL JOBS =============

                new JobOffer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333331"),
                    JobTitle = "Senior Frontend Architect",
                    Company = "MegaTech Corporation",
                    Description = "We're seeking a Senior Frontend Architect to lead our frontend strategy and architecture. You'll design scalable frontend systems, establish best practices, and guide technical decisions.\n\nResponsibilities:\n• Architect and design scalable frontend systems\n• Establish coding standards and best practices\n• Lead technical architecture discussions\n• Mentor and grow the frontend team\n• Evaluate and adopt new technologies and frameworks\n• Optimize for performance, accessibility, and SEO",
                    RequiredSkills = "React, TypeScript, Next.js, Vue.js, Angular, Micro-frontends, Webpack, Vite, Performance Optimization, Design Systems",
                    PostedDate = now.AddDays(-12),
                    Deadline = now.AddMonths(2),
                    CreatedAt = now.AddDays(-12),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333332"),
                    JobTitle = "Senior Backend Engineer (Python)",
                    Company = "DataScience Labs",
                    Description = "Join our team as a Senior Backend Engineer to build next-generation data processing platforms. You'll work on complex distributed systems and collaborate with data scientists and ML engineers.\n\nResponsibilities:\n• Design and develop high-performance backend systems\n• Build distributed systems using Python and async frameworks\n• Work with big data technologies (Spark, Kafka)\n• Design database architectures for scale\n• Lead technical projects and mentor engineers",
                    RequiredSkills = "Python, FastAPI, asyncio, PostgreSQL, MongoDB, Redis, Kafka, Spark, Docker, Kubernetes, AWS, Machine Learning APIs",
                    PostedDate = now.AddDays(-14),
                    Deadline = now.AddMonths(2).AddDays(10),
                    CreatedAt = now.AddDays(-14),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    JobTitle = "Senior Full-Stack Developer",
                    Company = "InnovateHub",
                    Description = "We need a Senior Full-Stack Developer to lead end-to-end development initiatives. You'll architect complete solutions, make critical technical decisions, and work on innovative products.\n\nResponsibilities:\n• Architect complete full-stack solutions\n• Lead technical design and implementation\n• Work with modern frameworks and technologies\n• Optimize applications for performance and scalability\n• Mentor developers and conduct technical interviews",
                    RequiredSkills = "TypeScript, React, Next.js, Node.js, NestJS, PostgreSQL, MongoDB, Redis, GraphQL, Docker, Kubernetes, AWS, Microservices",
                    PostedDate = now.AddDays(-11),
                    Deadline = now.AddMonths(2).AddDays(5),
                    CreatedAt = now.AddDays(-11),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333334"),
                    JobTitle = "Senior Java Architect",
                    Company = "Enterprise Solutions Pro",
                    Description = "Looking for a Senior Java Architect to lead our enterprise software architecture. You'll design scalable systems, establish architectural patterns, and guide technical strategy.\n\nResponsibilities:\n• Design and architect enterprise-grade Java applications\n• Establish architectural patterns and best practices\n• Lead technical design reviews\n• Work with distributed systems and microservices\n• Mentor senior engineers and architects",
                    RequiredSkills = "Java, Spring Boot, Spring Cloud, Microservices, Event-Driven Architecture, PostgreSQL, Redis, Kafka, Docker, Kubernetes, AWS, Architecture Patterns",
                    PostedDate = now.AddDays(-13),
                    Deadline = now.AddMonths(2).AddDays(15),
                    CreatedAt = now.AddDays(-13),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333335"),
                    JobTitle = "Senior DevOps Architect",
                    Company = "CloudFirst Solutions",
                    Description = "Join us as a Senior DevOps Architect to lead our infrastructure strategy. You'll design cloud-native architectures, establish DevOps practices, and build scalable infrastructure platforms.\n\nResponsibilities:\n• Design and architect cloud-native infrastructure\n• Establish DevOps practices and CI/CD standards\n• Lead infrastructure automation initiatives\n• Design for high availability and disaster recovery\n• Mentor DevOps engineers and lead technical projects",
                    RequiredSkills = "Kubernetes, Docker, AWS, Azure, GCP, Terraform, Ansible, CI/CD, GitOps, Observability, Security, Infrastructure as Code, Scripting",
                    PostedDate = now.AddDays(-15),
                    Deadline = now.AddMonths(2).AddDays(20),
                    CreatedAt = now.AddDays(-15),
                    isDeleted = false
                },

                // ============= SPECIALIZED JOBS =============

                new JobOffer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444441"),
                    JobTitle = "React Native Developer",
                    Company = "MobileFirst Inc",
                    Description = "We're looking for a React Native Developer to build cross-platform mobile applications. You'll work on apps used by millions of users across iOS and Android platforms.\n\nResponsibilities:\n• Develop mobile applications using React Native\n• Build reusable components and libraries\n• Optimize app performance and user experience\n• Work with native modules when needed\n• Collaborate with designers and backend developers",
                    RequiredSkills = "React Native, JavaScript, TypeScript, Redux, React Navigation, iOS, Android, Git",
                    PostedDate = now.AddDays(-4),
                    Deadline = now.AddMonths(1).AddDays(12),
                    CreatedAt = now.AddDays(-4),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444442"),
                    JobTitle = "Machine Learning Engineer",
                    Company = "AIML Innovations",
                    Description = "Join our ML team to build and deploy machine learning models. You'll work on exciting AI projects, from data preprocessing to model deployment and monitoring.\n\nResponsibilities:\n• Develop and deploy machine learning models\n• Work with data scientists to implement ML solutions\n• Build MLOps pipelines for model deployment\n• Optimize models for production environments\n• Monitor and maintain ML systems in production",
                    RequiredSkills = "Python, TensorFlow, PyTorch, Scikit-learn, Pandas, NumPy, Docker, Kubernetes, AWS SageMaker, MLflow",
                    PostedDate = now.AddDays(-6),
                    Deadline = now.AddMonths(1).AddDays(28),
                    CreatedAt = now.AddDays(-6),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444443"),
                    JobTitle = "Blockchain Developer",
                    Company = "CryptoTech Solutions",
                    Description = "We're seeking a Blockchain Developer to build decentralized applications and smart contracts. You'll work on cutting-edge blockchain technologies and Web3 projects.\n\nResponsibilities:\n• Develop smart contracts using Solidity\n• Build decentralized applications (dApps)\n• Work with blockchain networks (Ethereum, Polygon)\n• Integrate Web3 technologies with traditional systems\n• Ensure security and best practices in blockchain development",
                    RequiredSkills = "Solidity, Ethereum, Web3.js, Truffle, Hardhat, Smart Contracts, Blockchain, TypeScript, Node.js",
                    PostedDate = now.AddDays(-8),
                    Deadline = now.AddMonths(1).AddDays(30),
                    CreatedAt = now.AddDays(-8),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    JobTitle = "Game Developer (Unity/C#)",
                    Company = "GameStudio Pro",
                    Description = "Join our game development team to create engaging mobile and web games. You'll work on game mechanics, physics, and multiplayer features.\n\nResponsibilities:\n• Develop games using Unity and C#\n• Implement game mechanics and features\n• Optimize games for performance\n• Work with 2D/3D graphics and animations\n• Collaborate with game designers and artists",
                    RequiredSkills = "C#, Unity, Game Development, 2D/3D Graphics, Physics, Git, OOP, Design Patterns",
                    PostedDate = now.AddDays(-7),
                    Deadline = now.AddMonths(1).AddDays(25),
                    CreatedAt = now.AddDays(-7),
                    isDeleted = false
                },

                new JobOffer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444445"),
                    JobTitle = "Security Engineer",
                    Company = "SecureTech Systems",
                    Description = "We need a Security Engineer to ensure our applications and infrastructure are secure. You'll perform security audits, implement security measures, and respond to security incidents.\n\nResponsibilities:\n• Perform security audits and penetration testing\n• Implement security best practices\n• Monitor and respond to security incidents\n• Conduct security code reviews\n• Educate teams on security practices",
                    RequiredSkills = "Security, Penetration Testing, OWASP, Encryption, Network Security, Python, Linux, Security Tools, Vulnerability Assessment",
                    PostedDate = now.AddDays(-9),
                    Deadline = now.AddMonths(1).AddDays(35),
                    CreatedAt = now.AddDays(-9),
                    isDeleted = false
                }
            };

            // Only add jobs that don't already exist
            var jobsToAdd = jobs.Where(j => !existingJobIds.Contains(j.Id)).ToList();
            
            if (jobsToAdd.Any())
            {
                try
                {
                    context.JobOffers.AddRange(jobsToAdd);
                    await context.SaveChangesAsync();
                    Console.WriteLine($"✅ Successfully seeded {jobsToAdd.Count} job offers.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error seeding job offers: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("ℹ️ All job offers already exist. No new jobs added.");
            }
        }
    }
}

